using System;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Script.Dynamic;
using Interop;

namespace Script.CoreUObject
{
    public static class Utils
    {
#if LEANCLR
        // Everything in this region is process-wide and immutable once built: the UE assembly is
        // never unloaded on this backend (there is no collectible ALC, see AssemblyLoader.Unload),
        // so a type set derived from it cannot go stale. Before P8.6 these were rebuilt for EVERY
        // class the reflection layer resolved, each rebuild walking all ~17k UE.dll types.
        private static List<Type> LeanCLRAttributeCandidates;

        private static HashSet<Type> LeanCLRAttributeCandidateSet;

        private static Dictionary<string, Type> LeanCLRUETypesByFullName;

        // Candidates = OverrideAttribute plus every Attribute in the UClassAttribute namespace.
        // Measured on this project: 256 entries. The set is what the C++ side can consume
        // (FReflectionRegistry exposes 254 distinct Get*AttributeClass() accessors), so it must
        // NOT be narrowed to the attributes a particular game happens to use today.
        private static void LeanCLREnsureAttributeCandidates()
        {
            if (LeanCLRAttributeCandidates != null)
            {
                return;
            }

            var Candidates = new List<Type>();

            var OverrideAttributeType = typeof(OverrideAttribute);

            Candidates.Add(OverrideAttributeType);

            var CandidateNamespace = typeof(UClassAttribute).Namespace;

            Type[] CandidateTypes;

            try
            {
                CandidateTypes = typeof(UClassAttribute).Assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ReflectionTypeLoadException)
            {
                CandidateTypes = ReflectionTypeLoadException.Types;
            }

            foreach (var CandidateType in CandidateTypes)
            {
                if (CandidateType != null && CandidateType != OverrideAttributeType &&
                    CandidateType.Namespace == CandidateNamespace &&
                    typeof(Attribute).IsAssignableFrom(CandidateType))
                {
                    Candidates.Add(CandidateType);
                }
            }

            LeanCLRAttributeCandidateSet = new HashSet<Type>(Candidates);

            // Assign last: this field doubles as the "built" flag for both collections.
            LeanCLRAttributeCandidates = Candidates;
        }

        // Replaces a full UEAssembly.GetTypes().FirstOrDefault(...) scan per unresolved name.
        // First-wins insertion so lookups match what FirstOrDefault returned.
        private static Type LeanCLRFindUETypeByFullName(string InFullName)
        {
            if (LeanCLRUETypesByFullName == null)
            {
                var Map = new Dictionary<string, Type>();

                try
                {
                    Type[] Types;

                    try
                    {
                        Types = typeof(UClassAttribute).Assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ReflectionTypeLoadException)
                    {
                        Types = ReflectionTypeLoadException.Types;
                    }

                    foreach (var Type in Types)
                    {
                        var FullName = Type?.FullName;

                        if (FullName != null && !Map.ContainsKey(FullName))
                        {
                            Map[FullName] = Type;
                        }
                    }
                }
                catch
                {
                    // A partial map is still better than rescanning; never abort the parse.
                }

                LeanCLRUETypesByFullName = Map;
            }

            return LeanCLRUETypesByFullName.TryGetValue(InFullName, out var Result) ? Result : null;
        }

        // Round-trip partner of the weaver's EncodeAttributeField. Attribute arguments are arbitrary
        // user strings (a Category / DisplayName / ToolTip may contain '|' or a line break), so the
        // weaver escapes them and this undoes it.
        private static string LeanCLRDecodeAttributeField(string InValue)
        {
            if (string.IsNullOrEmpty(InValue) || InValue.IndexOf('\\') < 0)
            {
                return InValue;
            }

            var Result = new StringBuilder(InValue.Length);

            for (var i = 0; i < InValue.Length; i++)
            {
                if (InValue[i] != '\\' || i + 1 >= InValue.Length)
                {
                    Result.Append(InValue[i]);

                    continue;
                }

                i++;

                switch (InValue[i])
                {
                    case '\\': Result.Append('\\'); break;
                    case 'p': Result.Append('|'); break;
                    case 'r': Result.Append('\r'); break;
                    case 'n': Result.Append('\n'); break;

                    // Unknown escape: keep both characters rather than dropping data.
                    default:
                        Result.Append('\\');
                        Result.Append(InValue[i]);
                        break;
                }
            }

            return Result.ToString();
        }

        // Parses the payload's argument-count field. Deliberately hand-rolled instead of int.TryParse:
        // that routes through NumberFormatInfo / culture data, and leanclr only implements the Win32
        // NLS path -- the same trap that made the culture-sensitive StartsWith overloads silently fail
        // on POSIX targets (see the StringComparison.Ordinal note in GetClassPropertiesImplementation).
        // The field is always plain ASCII digits, so no culture is involved at all.
        private static bool LeanCLRTryParseCount(string InValue, out int OutCount)
        {
            OutCount = 0;

            if (string.IsNullOrEmpty(InValue) || InValue.Length > 9)
            {
                return false;
            }

            for (var i = 0; i < InValue.Length; i++)
            {
                var Digit = InValue[i] - '0';

                if (Digit < 0 || Digit > 9)
                {
                    return false;
                }

                OutCount = OutCount * 10 + Digit;
            }

            return true;
        }
#endif

        public static string GetPathName(Type InType) => InType.GetCustomAttribute<PathNameAttribute>(true)?.PathName;

        private static Type GetType(Type InType) =>
            InType.IsByRef
                ? InType.GetElementType()!.IsGenericType
                    ? InType.GetElementType()?.GetGenericTypeDefinition()
                    : InType.GetElementType()
                : InType.IsGenericType
                    ? InType.GetGenericTypeDefinition()
                    : InType;

        private static string GetTraceback()
        {
            var Traceback = new StringBuilder();

            var Trace = new StackTrace();

            var Frames = Trace.GetFrames();

            foreach (var Frame in Frames)
            {
                Traceback.Append(string.Format("at {0}.{1} in {2}:{3}\r\n",
                    Frame.GetMethod().DeclaringType.FullName,
                    Frame.GetMethod().Name,
                    Frame.GetFileName(),
                    Frame.GetFileLineNumber()));
            }

            return Traceback.ToString();
        }

        public static Type[] GetTypesWithAttribute(Type InAttributeType, Assembly InAssembly, out int OutLength)
        {
            var Result = new List<Type>();

            Type[] Types;

            try
            {
                Types = InAssembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ReflectionTypeLoadException)
            {
                Types = ReflectionTypeLoadException.Types;
            }

            foreach (var Type in Types)
            {
                if (Type != null && Type.IsDefined(InAttributeType, false))
                {
                    Result.Add(Type);
                }
            }

            OutLength = Result.Count;

            return Result.ToArray();
        }

        private static void GetClassDescriptorImplementation(Type InType,
            out Type OutTypeDefinition, out string OutNameSpace, out string OutPathName,
            out Type OutParent, out Type OutUnderlyingType, out bool OutIsClass, out bool OutIsEnum,
            out int OutGenericArgumentLength, out Type[] OutGenericArguments,
            out int OutInterfaceLength, out Type[] OutInterfaces,
            out int OutClassAttributeLength, out Type[] OutClassAttributes,
            out int[] OutClassAttributeValueLength, out string[] OutClassAttributeValues
        )
        {
            OutTypeDefinition = GetType(InType);

            OutNameSpace = InType.Namespace;

            OutPathName = null;

            var PathNameAttributeFullName = typeof(PathNameAttribute).FullName;

            foreach (var CustomAttribute in InType.CustomAttributes)
            {
                if (CustomAttribute.AttributeType.FullName == PathNameAttributeFullName)
                {
                    OutPathName = CustomAttribute.ConstructorArguments[0].Value as string;
                }
            }

            OutParent = InType.BaseType;

            OutUnderlyingType = InType.IsEnum ? InType.GetEnumUnderlyingType() : null;

            OutIsClass = InType.IsClass;

            OutIsEnum = InType.IsEnum;

            if (OutIsClass || OutIsEnum)
            {
                OutGenericArguments = InType.GenericTypeArguments;

                OutGenericArgumentLength = OutGenericArguments.Length;

                OutInterfaces = InType.GetInterfaces();

                OutInterfaceLength = (InType.IsValueType || InType.IsGenericType) ? 0 : OutInterfaces.Length;

                var ClassAttributes = new List<Type>();

                var ClassAttributeValueIndex = new List<int>();

                var ClassAttributeValues = new List<string>();

                var UClassAttributeNamespace = typeof(UClassAttribute).Namespace;

                var OverrideAttributeTypeFullName = typeof(OverrideAttribute).FullName;

                foreach (var CustomAttribute in InType.CustomAttributes)
                {
                    if (CustomAttribute.AttributeType.Namespace == UClassAttributeNamespace ||
                        CustomAttribute.AttributeType.FullName == OverrideAttributeTypeFullName)
                    {
                        ClassAttributes.Add(CustomAttribute.AttributeType);

                        var ClassAttributeValueCount = 0;

                        foreach (var ConstructorArgument in CustomAttribute.ConstructorArguments)
                        {
                            ClassAttributeValues.Add(ConstructorArgument.Value.ToString());

                            ClassAttributeValueCount++;
                        }

                        ClassAttributeValueIndex.Add(ClassAttributeValueCount);
                    }
                }

                OutClassAttributeLength = ClassAttributes.Count;

                OutClassAttributes = ClassAttributes.ToArray();

                OutClassAttributeValueLength = ClassAttributeValueIndex.ToArray();

                OutClassAttributeValues = ClassAttributeValues.ToArray();
            }
            else
            {
                OutGenericArgumentLength = 0;

                OutGenericArguments = null;

                OutInterfaceLength = 0;

                OutInterfaces = null;

                OutClassAttributeLength = 0;

                OutClassAttributes = null;

                OutClassAttributeValueLength = null;

                OutClassAttributeValues = null;
            }
        }

        private static void GetClassPropertiesImplementation(Type InType,
            out int OutPropertyLength, out string[] OutPropertyNames,
            out PropertyInfo[] OutPropertyInfos, out Type[] OutPropertyTypes,
            out int[] OutPropertyAttributeCounts, out Type[] OutPropertyAttributes,
            out int[] OutPropertyAttributeValueCounts, out string[] OutPropertyAttributeValues
        )
        {
            if (InType.IsClass)
            {
                var UClassAttributeNamespace = typeof(UClassAttribute).Namespace;

                OutPropertyInfos = InType.GetProperties(
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.NonPublic
                );

                OutPropertyLength = OutPropertyInfos.Length;

                OutPropertyNames = new string[OutPropertyLength];

                OutPropertyTypes = new Type[OutPropertyLength];

                OutPropertyAttributeCounts = new int[OutPropertyLength];

                var PropertyAttributes = new List<Type>();

                var PropertyAttributeIndex = new List<int>();

                var PropertyAttributeValues = new List<string>();

#if LEANCLR
                var LeanCLRWovenFieldNames = new HashSet<string>();

                var LeanCLRWovenAttrFields = new Dictionary<string, FieldInfo>();

                try
                {
                    var AllFields = InType.GetFields(
                        BindingFlags.Static | BindingFlags.Instance |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.FlattenHierarchy);

                    foreach (var Field in AllFields)
                    {
                        // StringComparison.Ordinal is mandatory here, do not simplify it away:
                        // the culture-sensitive overloads route through Win32 NLS, which leanclr
                        // only implements on Windows. On POSIX targets (Android/Linux/Mac/iOS)
                        // FindNLSStringEx returns -1 unconditionally, so they silently return
                        // false and every woven UProperty marker is lost.
                        if (Field != null && Field.Name.StartsWith("__", StringComparison.Ordinal))
                        {
                            if (Field.Name.EndsWith("_Attrs", StringComparison.Ordinal))
                            {
                                LeanCLRWovenAttrFields[Field.Name] = Field;
                            }
                            else
                            {
                                LeanCLRWovenFieldNames.Add(Field.Name);
                            }
                        }
                    }
                }
                catch
                {
                    // Never let field discovery abort the property parse.
                }
#endif

                for (var i = 0; i < OutPropertyInfos.Length; i++)
                {
                    OutPropertyNames[i] = OutPropertyInfos[i].Name;

                    OutPropertyTypes[i] = OutPropertyInfos[i].PropertyType;

                    var PropertyAttributeCount = 0;

#if LEANCLR
                    if (LeanCLRWovenFieldNames.Contains("__" + OutPropertyInfos[i].Name))
                    {
                        PropertyAttributes.Add(typeof(UPropertyAttribute));

                        PropertyAttributeIndex.Add(0);

                        PropertyAttributeCount++;
                    }

                    // Recover companion attributes from __<Name>_Attrs woven static string fields.
                    if (LeanCLRWovenAttrFields.TryGetValue(
                            "__" + OutPropertyInfos[i].Name + "_Attrs", out var AttrField))
                    {
                        string AttrData = null;

                        try { AttrData = AttrField.GetValue(null) as string; }
                        catch { /* skip this field on any read error */ }

                        if (!string.IsNullOrEmpty(AttrData))
                        {
                            foreach (var Line in AttrData.Split('\n'))
                            {
                                if (string.IsNullOrEmpty(Line))
                                {
                                    continue;
                                }

                                // Payload framing (written by the weaver's EmitCompanionAttributeField):
                                //   "FullTypeName|argCount|arg0|arg1|..."   -- every field escaped
                                // Splitting on '|' is safe because escaping guarantees no raw '|'
                                // survives inside a field. argCount is then re-checked against the
                                // actual field count: a mismatch means the payload was framed by a
                                // different weaver version, and skipping the line is far better than
                                // handing C++ a shifted attribute/value pairing it cannot detect.
                                var Parts = Line.Split('|');

                                int ValueCount;

                                if (Parts.Length == 1)
                                {
                                    // Pre-P8.7 payload: bare type name, no count, no arguments. Every
                                    // payload produced before this change has this shape, so accepting
                                    // it keeps a stale Game.dll working instead of silently dropping
                                    // its attributes.
                                    ValueCount = 0;
                                }
                                else if (LeanCLRTryParseCount(Parts[1], out ValueCount) &&
                                         Parts.Length - 2 == ValueCount)
                                {
                                    // Well-formed current payload.
                                }
                                else
                                {
                                    continue;
                                }

                                var TypeFullName = LeanCLRDecodeAttributeField(Parts[0]);

                                // Type.GetType misses every time on leanclr (measured in-editor:
                                // getType=0, dictionary=75), so the dictionary is the real lookup
                                // path here rather than a rare fallback.
                                var AttrType = Type.GetType(TypeFullName) ??
                                               LeanCLRFindUETypeByFullName(TypeFullName);

                                if (AttrType == null)
                                {
                                    continue;
                                }

                                PropertyAttributes.Add(AttrType);

                                for (var v = 0; v < ValueCount; v++)
                                {
                                    PropertyAttributeValues.Add(
                                        LeanCLRDecodeAttributeField(Parts[v + 2]));
                                }

                                PropertyAttributeIndex.Add(ValueCount);

                                PropertyAttributeCount++;
                            }
                        }
                    }
#else
                    foreach (var CustomAttribute in OutPropertyInfos[i].CustomAttributes)
                    {
                        if (CustomAttribute.AttributeType.Namespace == UClassAttributeNamespace)
                        {
                            var PropertyAttributeValueCount = 0;

                            PropertyAttributes.Add(CustomAttribute.AttributeType);

                            foreach (var ConstructorArgument in CustomAttribute.ConstructorArguments)
                            {
                                PropertyAttributeValues.Add(ConstructorArgument.Value.ToString());

                                PropertyAttributeValueCount++;
                            }

                            PropertyAttributeIndex.Add(PropertyAttributeValueCount);

                            PropertyAttributeCount++;
                        }
                    }
#endif

                    OutPropertyAttributeCounts[i] = PropertyAttributeCount;
                }

                OutPropertyAttributes = PropertyAttributes.ToArray();

                OutPropertyAttributeValueCounts = PropertyAttributeIndex.ToArray();

                OutPropertyAttributeValues = PropertyAttributeValues.ToArray();
            }
            else
            {
                OutPropertyLength = 0;

                OutPropertyNames = null;

                OutPropertyInfos = null;

                OutPropertyTypes = null;

                OutPropertyAttributeCounts = null;

                OutPropertyAttributes = null;

                OutPropertyAttributeValueCounts = null;

                OutPropertyAttributeValues = null;
            }
        }

        private static void GetClassFieldsImplementation(Type InType,
            out int OutFieldLength, out string[] OutFieldNames, out FieldInfo[] OutFieldInfos
        )
        {
            if (InType.IsClass || InType.IsEnum)
            {
                OutFieldInfos = InType.GetFields(
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.NonPublic);

                OutFieldLength = OutFieldInfos.Length;

                OutFieldNames = new string[OutFieldLength];

                for (var i = 0; i < OutFieldInfos.Length; i++)
                {
                    OutFieldNames[i] = OutFieldInfos[i].Name;
                }
            }
            else
            {
                OutFieldLength = 0;

                OutFieldNames = null;

                OutFieldInfos = null;
            }
        }

#if LEANCLR
        // leanclr's Type.GetMethods does not collapse overridden virtual slots: an override and the
        // base slot it overrides BOTH appear (runtime probe: 'Test' matches=2 / total=109 on
        // UUnitTestSubsystem, vs CoreCLR's matches=1 / total=105 on the same Game.dll). Forwarding both
        // breaks the C++ FClassReflection.Methods TMap keyed by (name, paramCount): the base slot's
        // entry (NewSlot, no [Override]) wins over the real override, FMethodReflection.bIsOverride
        // stays false and FCSharpBind never binds the C# override. Collapse duplicate signatures here,
        // keeping the most-derived declaration (the one carrying [Override]).
        //
        // Duplicates can only share a (Name, ParamCount) key, so bucket on that instead of rescanning
        // every kept method: the linear scan compared each candidate against all previously kept
        // methods, which is O(n^2) in a class's method count. Measured over UE.dll + Game.dll
        // (14518 classes / 321888 methods): 14596382 comparisons -> 871, output identical.
        private static MethodInfo[] LeanCLRCollapseOverriddenMethods(MethodInfo[] InMethods)
        {
            var Result = new List<MethodInfo>(InMethods.Length);

            // Parameter types of Result[i], captured once (GetParameters allocates a fresh array on
            // every call, and the old code re-read it for every comparison).
            var ResultParameterTypes = new List<Type[]>(InMethods.Length);

            var Buckets = new Dictionary<LeanCLRMethodSignatureKey, List<int>>();

            foreach (var Candidate in InMethods)
            {
                var CandidateParameters = Candidate.GetParameters();

                var CandidateParameterTypes = new Type[CandidateParameters.Length];

                for (var ParameterIndex = 0; ParameterIndex < CandidateParameters.Length; ParameterIndex++)
                {
                    CandidateParameterTypes[ParameterIndex] = CandidateParameters[ParameterIndex].ParameterType;
                }

                var Key = new LeanCLRMethodSignatureKey(Candidate.Name, CandidateParameterTypes.Length);

                var bDuplicate = false;

                Buckets.TryGetValue(Key, out var Bucket);

                if (Bucket != null)
                {
                    foreach (var Index in Bucket)
                    {
                        var ExistingParameterTypes = ResultParameterTypes[Index];

                        var bSameSignature = true;

                        for (var ParameterIndex = 0;
                             ParameterIndex < ExistingParameterTypes.Length;
                             ParameterIndex++)
                        {
                            if (ExistingParameterTypes[ParameterIndex] !=
                                CandidateParameterTypes[ParameterIndex])
                            {
                                bSameSignature = false;

                                break;
                            }
                        }

                        if (!bSameSignature)
                        {
                            continue;
                        }

                        bDuplicate = true;

                        var Existing = Result[Index];

                        // Same name + signature = same vtable slot chain: keep the most-derived declaration.
                        if (Existing.DeclaringType != null && Candidate.DeclaringType != null &&
                            Existing.DeclaringType != Candidate.DeclaringType &&
                            Existing.DeclaringType.IsAssignableFrom(Candidate.DeclaringType))
                        {
                            Result[Index] = Candidate;
                        }

                        break;
                    }
                }

                if (!bDuplicate)
                {
                    if (Bucket == null)
                    {
                        Buckets[Key] = Bucket = new List<int>();
                    }

                    Bucket.Add(Result.Count);

                    Result.Add(Candidate);

                    ResultParameterTypes.Add(CandidateParameterTypes);
                }
            }

            return Result.ToArray();
        }

        private readonly struct LeanCLRMethodSignatureKey : IEquatable<LeanCLRMethodSignatureKey>
        {
            private readonly string Name;

            private readonly int ParameterCount;

            public LeanCLRMethodSignatureKey(string InName, int InParameterCount)
            {
                Name = InName;

                ParameterCount = InParameterCount;
            }

            public bool Equals(LeanCLRMethodSignatureKey InOther) =>
                ParameterCount == InOther.ParameterCount &&
                string.Equals(Name, InOther.Name, StringComparison.Ordinal);

            public override bool Equals(object InOther) =>
                InOther is LeanCLRMethodSignatureKey Other && Equals(Other);

            // Ordinal comparison is mandatory, do not simplify it away: leanclr only implements the
            // Win32 NLS path, so culture-sensitive string APIs silently misbehave on POSIX targets
            // (see the StartsWith note in GetClassPropertiesImplementation). string.GetHashCode() is
            // ordinal by contract, so it pairs correctly with the ordinal Equals above.
            public override int GetHashCode() => (Name?.GetHashCode() ?? 0) ^ ParameterCount;
        }
#endif

        private static void GetClassMethodsImplementation(Type InType,
            out int OutMethodLength, out string[] OutMethodNames, out MethodBase[] OutMethodInfos,
            out bool[] OutMethodIsStatics, out int[] OutMethodParamCounts, out Type[] OutMethodReturnTypes,
            out int[] OutMethodParamIndex, out string[] OutMethodParamNames,
            out Type[] OutMethodParamTypes, out bool[] OutMethodParamRefs,
            out int[] OutMethodAttributeCounts, out Type[] OutMethodAttributes,
            out int[] OutMethodAttributeValueCounts, out string[] OutMethodAttributeValues
        )
        {
            if (InType.IsClass)
            {
                var UClassAttributeNamespace = typeof(UClassAttribute).Namespace;

#if !LEANCLR
                var OverrideAttributeTypeFullName = typeof(OverrideAttribute).FullName;
#endif

                var Constructors = InType.GetConstructors(
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.NonPublic);

                var Methods = InType.GetMethods(
                        BindingFlags.Instance |
                        BindingFlags.Static |
                        BindingFlags.Public |
                        BindingFlags.NonPublic)
                    .Where(Method => !Method.IsSpecialName)
                    .ToArray();

#if LEANCLR
                // leanclr returns the base virtual AND the derived override for the same slot (see
                // LeanCLRCollapseOverriddenMethods); collapse to the most-derived declaration so the
                // (name, paramCount)-keyed C++ map keeps the method that actually carries [Override].
                Methods = LeanCLRCollapseOverriddenMethods(Methods);
#endif

                var ConstructorLength = Constructors.Length;

                OutMethodLength = ConstructorLength + Methods.Length;

                OutMethodInfos = new MethodBase[OutMethodLength];

                for (var i = 0; i < ConstructorLength; i++)
                {
                    OutMethodInfos[i] = Constructors[i];
                }

                for (var i = 0; i < Methods.Length; i++)
                {
                    OutMethodInfos[ConstructorLength + i] = Methods[i];
                }

                OutMethodNames = new string[OutMethodLength];

                OutMethodParamCounts = new int[OutMethodLength];

                for (var i = 0; i < OutMethodInfos.Length; i++)
                {
                    OutMethodNames[i] = OutMethodInfos[i].Name;

                    OutMethodParamCounts[i] = OutMethodInfos[i].GetParameters().Length;
                }

                OutMethodIsStatics = new bool[OutMethodLength];

                OutMethodReturnTypes = new Type[OutMethodLength];

                OutMethodParamIndex = new int[OutMethodLength];

                var MethodParamNames = new List<string>();

                var MethodParamTypes = new List<Type>();

                var MethodParamRefs = new List<bool>();

                var UFunctionAttributeTypeFullName = typeof(UFunctionAttribute).FullName;

                for (var i = 0; i < OutMethodInfos.Length; i++)
                {
                    var IsUFunction = false;

                    foreach (var CustomAttribute in OutMethodInfos[i].CustomAttributes)
                    {
                        if (CustomAttribute.AttributeType.FullName == UFunctionAttributeTypeFullName)
                        {
                            IsUFunction = true;

                            break;
                        }
                    }

                    OutMethodParamIndex[i] = MethodParamNames.Count;

                    if (IsUFunction && OutMethodInfos[i] is MethodInfo MethodInfo)
                    {
                        OutMethodIsStatics[i] = MethodInfo.IsStatic;

                        OutMethodReturnTypes[i] = MethodInfo.ReturnType;

                        foreach (var Parameter in MethodInfo.GetParameters())
                        {
                            MethodParamNames.Add(Parameter.Name);

                            var ParameterType = Parameter.ParameterType;

                            MethodParamTypes.Add(ParameterType.IsByRef
                                ? ParameterType.GetElementType()
                                : ParameterType);

                            MethodParamRefs.Add(ParameterType.IsByRef);
                        }
                    }
                    else
                    {
                        OutMethodIsStatics[i] = false;

                        OutMethodReturnTypes[i] = null;
                    }
                }

                OutMethodParamNames = MethodParamNames.ToArray();

                OutMethodParamTypes = MethodParamTypes.ToArray();

                OutMethodParamRefs = MethodParamRefs.ToArray();

                OutMethodAttributeCounts = new int[OutMethodLength];

                var MethodAttributes = new List<Type>();

                var MethodAttributeValueIndex = new List<int>();

                var MethodAttributeValues = new List<string>();

#if LEANCLR
                LeanCLREnsureAttributeCandidates();

                var LeanCLRMethodAttributeCandidates = LeanCLRAttributeCandidates;

                HashSet<Type> LeanCLRMethodAttributeHits = null;
#endif

                for (var i = 0; i < OutMethodInfos.Length; i++)
                {
                    var MethodAttribute = 0;

#if LEANCLR
                    if (OutMethodInfos[i] is MethodInfo LeanCLRMethodInfo)
                    {
                        // Ask each applied attribute which candidates it satisfies, rather than asking
                        // all 256 candidates whether they are applied. IsDefined(C, false) is true iff
                        // some attribute applied directly to this member has a type assignable to C, so
                        // walking each applied type's base chain reproduces the same answer set exactly
                        // -- including the base entries that make a [UFunction] method also report
                        // OverrideAttribute (6 of the 256 candidates derive from it).
                        //
                        // This reads MethodInfo.CustomAttributes, which is NOT new API surface on this
                        // backend: the UFunction scan above already enumerates it for every method, and
                        // it was measured working on leanclr (P6 doc 5.18, 'Test' CA.Count=1). Only the
                        // PropertyInfo side is broken there, which is why properties still go through
                        // the woven __<Name> fields.
                        //
                        // Measured over UE.dll + Game.dll: 82149376 IsDefined calls -> 74788 applied
                        // attribute visits (-99.9%), with 0 differences in the emitted sequence.
                        LeanCLRMethodAttributeHits?.Clear();

                        foreach (var CustomAttribute in LeanCLRMethodInfo.CustomAttributes)
                        {
                            for (var AttributeType = CustomAttribute.AttributeType;
                                 AttributeType != null;
                                 AttributeType = AttributeType.BaseType)
                            {
                                if (LeanCLRAttributeCandidateSet.Contains(AttributeType))
                                {
                                    LeanCLRMethodAttributeHits ??= new HashSet<Type>();

                                    LeanCLRMethodAttributeHits.Add(AttributeType);
                                }
                            }
                        }

                        if (LeanCLRMethodAttributeHits != null && LeanCLRMethodAttributeHits.Count > 0)
                        {
                            // Emit in candidate-table order so the arrays handed to C++ keep the order
                            // the per-candidate loop produced.
                            foreach (var CandidateType in LeanCLRMethodAttributeCandidates)
                            {
                                if (LeanCLRMethodAttributeHits.Contains(CandidateType))
                                {
                                    MethodAttributes.Add(CandidateType);

                                    MethodAttributeValueIndex.Add(0);

                                    MethodAttribute++;
                                }
                            }
                        }
                    }
#else
                    foreach (var CustomAttribute in OutMethodInfos[i].CustomAttributes)
                    {
                        if (CustomAttribute.AttributeType.Namespace == UClassAttributeNamespace ||
                            CustomAttribute.AttributeType.FullName == OverrideAttributeTypeFullName
                           )
                        {
                            var MethodAttributeValue = 0;

                            MethodAttributes.Add(CustomAttribute.AttributeType);

                            foreach (var ConstructorArgument in CustomAttribute.ConstructorArguments)
                            {
                                MethodAttributeValues.Add(ConstructorArgument.Value.ToString());

                                MethodAttributeValue++;
                            }

                            MethodAttributeValueIndex.Add(MethodAttributeValue);

                            MethodAttribute++;
                        }
                    }
#endif

                    OutMethodAttributeCounts[i] = MethodAttribute;
                }

                OutMethodAttributes = MethodAttributes.ToArray();

                OutMethodAttributeValueCounts = MethodAttributeValueIndex.ToArray();

                OutMethodAttributeValues = MethodAttributeValues.ToArray();
            }
            else
            {
                OutMethodLength = 0;

                OutMethodNames = null;

                OutMethodInfos = null;

                OutMethodIsStatics = null;

                OutMethodParamCounts = null;

                OutMethodReturnTypes = null;

                OutMethodParamIndex = null;

                OutMethodParamNames = null;

                OutMethodParamTypes = null;

                OutMethodParamRefs = null;

                OutMethodAttributeCounts = null;

                OutMethodAttributes = null;

                OutMethodAttributeValueCounts = null;

                OutMethodAttributeValues = null;
            }
        }

        [UnmanagedCallersOnly]
        public static int IsOverride(nint InTypeHandle)
        {
            if (HandleData.GetObject(InTypeHandle) is Type Type)
            {
                return Type.IsDefined(typeof(UClassAttribute), false) ||
                       Type.IsDefined(typeof(OverrideAttribute), false)
                    ? 1
                    : 0;
            }

            return 0;
        }

        [UnmanagedCallersOnly]
        public static unsafe void GetClassDescriptor(nint InTypeHandle, nint* OutBuffer)
        {
            if (HandleData.GetObject(InTypeHandle) is Type Type)
            {
                GetClassDescriptorImplementation(Type,
                    out var OutTypeDefinition, out var OutNameSpace, out var OutPathName,
                    out var OutParent, out var OutUnderlyingType, out var OutIsClass, out var OutIsEnum,
                    out var OutGenericArgumentLength, out var OutGenericArguments,
                    out var OutInterfaceLength, out var OutInterfaces,
                    out var OutClassAttributeLength, out var OutClassAttributes,
                    out var OutClassAttributeValueLength, out var OutClassAttributeValues
                );

                OutBuffer[0] = OutTypeDefinition != null ? HandleData.Alloc(OutTypeDefinition) : 0;

                OutBuffer[1] = OutNameSpace != null ? HandleData.Alloc(OutNameSpace) : 0;

                OutBuffer[2] = OutPathName != null ? HandleData.Alloc(OutPathName) : 0;

                OutBuffer[3] = OutParent != null ? HandleData.Alloc(OutParent) : 0;

                OutBuffer[4] = OutUnderlyingType != null ? HandleData.Alloc(OutUnderlyingType) : 0;

                OutBuffer[5] = OutIsClass ? 1 : 0;

                OutBuffer[6] = OutIsEnum ? 1 : 0;

                OutBuffer[7] = OutGenericArgumentLength;

                OutBuffer[8] = OutGenericArguments != null ? HandleData.Alloc(OutGenericArguments) : 0;

                OutBuffer[9] = OutInterfaceLength;

                OutBuffer[10] = OutInterfaces != null ? HandleData.Alloc(OutInterfaces) : 0;

                OutBuffer[11] = OutClassAttributeLength;

                OutBuffer[12] = OutClassAttributes != null ? HandleData.Alloc(OutClassAttributes) : 0;

                OutBuffer[13] = OutClassAttributeValueLength != null
                    ? HandleData.Alloc(OutClassAttributeValueLength)
                    : 0;

                OutBuffer[14] = OutClassAttributeValues != null ? HandleData.Alloc(OutClassAttributeValues) : 0;
            }
        }

        [UnmanagedCallersOnly]
        public static unsafe void GetClassProperties(nint InTypeHandle, nint* OutBuffer)
        {
            if (HandleData.GetObject(InTypeHandle) is Type Type)
            {
                GetClassPropertiesImplementation(Type,
                    out var OutPropertyLength, out var OutPropertyNames,
                    out var OutPropertyInfos, out var OutPropertyTypes,
                    out var OutPropertyAttributeCounts, out var OutPropertyAttributes,
                    out var OutPropertyAttributeValueCounts, out var OutPropertyAttributeValues
                );

                OutBuffer[0] = OutPropertyLength;

                OutBuffer[1] = OutPropertyNames != null ? HandleData.Alloc(OutPropertyNames) : 0;

                OutBuffer[2] = OutPropertyInfos != null ? HandleData.Alloc(OutPropertyInfos) : 0;

                OutBuffer[3] = OutPropertyTypes != null ? HandleData.Alloc(OutPropertyTypes) : 0;

                OutBuffer[4] = OutPropertyAttributeCounts != null ? HandleData.Alloc(OutPropertyAttributeCounts) : 0;

                OutBuffer[5] = OutPropertyAttributes != null ? HandleData.Alloc(OutPropertyAttributes) : 0;

                OutBuffer[6] = OutPropertyAttributeValueCounts != null
                    ? HandleData.Alloc(OutPropertyAttributeValueCounts)
                    : 0;

                OutBuffer[7] = OutPropertyAttributeValues != null ? HandleData.Alloc(OutPropertyAttributeValues) : 0;
            }
        }

        [UnmanagedCallersOnly]
        public static unsafe void GetClassFields(nint InTypeHandle, nint* OutBuffer)
        {
            if (HandleData.GetObject(InTypeHandle) is Type Type)
            {
                GetClassFieldsImplementation(Type,
                    out var OutFieldLength, out var OutFieldNames, out var OutFieldInfos
                );

                OutBuffer[0] = OutFieldLength;

                OutBuffer[1] = OutFieldNames != null ? HandleData.Alloc(OutFieldNames) : 0;

                OutBuffer[2] = OutFieldInfos != null ? HandleData.Alloc(OutFieldInfos) : 0;
            }
        }

        [UnmanagedCallersOnly]
        public static unsafe void GetClassMethods(nint InTypeHandle, nint* OutBuffer)
        {
            if (HandleData.GetObject(InTypeHandle) is Type Type)
            {
                GetClassMethodsImplementation(Type,
                    out var OutMethodLength, out var OutMethodNames, out var OutMethodInfos,
                    out var OutMethodIsStatics, out var OutMethodParamCounts, out var OutMethodReturnTypes,
                    out var OutMethodParamIndex, out var OutMethodParamNames,
                    out var OutMethodParamTypes, out var OutMethodParamRefs,
                    out var OutMethodAttributeCounts, out var OutMethodAttributes,
                    out var OutMethodAttributeValueCounts, out var OutMethodAttributeValues
                );

                OutBuffer[0] = OutMethodLength;

                OutBuffer[1] = OutMethodNames != null ? HandleData.Alloc(OutMethodNames) : 0;

                OutBuffer[2] = OutMethodInfos != null ? HandleData.Alloc(OutMethodInfos) : 0;

                OutBuffer[3] = OutMethodIsStatics != null ? HandleData.Alloc(OutMethodIsStatics) : 0;

                OutBuffer[4] = OutMethodParamCounts != null ? HandleData.Alloc(OutMethodParamCounts) : 0;

                OutBuffer[5] = OutMethodReturnTypes != null ? HandleData.Alloc(OutMethodReturnTypes) : 0;

                OutBuffer[6] = OutMethodParamIndex != null ? HandleData.Alloc(OutMethodParamIndex) : 0;

                OutBuffer[7] = OutMethodParamNames != null ? HandleData.Alloc(OutMethodParamNames) : 0;

                OutBuffer[8] = OutMethodParamTypes != null ? HandleData.Alloc(OutMethodParamTypes) : 0;

                OutBuffer[9] = OutMethodParamRefs != null ? HandleData.Alloc(OutMethodParamRefs) : 0;

                OutBuffer[10] = OutMethodAttributeCounts != null ? HandleData.Alloc(OutMethodAttributeCounts) : 0;

                OutBuffer[11] = OutMethodAttributes != null ? HandleData.Alloc(OutMethodAttributes) : 0;

                OutBuffer[12] = OutMethodAttributeValueCounts != null
                    ? HandleData.Alloc(OutMethodAttributeValueCounts)
                    : 0;

                OutBuffer[13] = OutMethodAttributeValues != null ? HandleData.Alloc(OutMethodAttributeValues) : 0;
            }
        }
    }
}
