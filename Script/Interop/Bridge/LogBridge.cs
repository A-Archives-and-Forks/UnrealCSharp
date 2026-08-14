using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Interop;

public sealed class LogBridge : TextWriter
{
    private static unsafe delegate* unmanaged[Cdecl]<byte*, int, byte, void> LogFn;

    private static TextWriter? ConsoleOut;

    private static TextWriter? ConsoleError;

    private readonly bool bIsError;

    private readonly StringBuilder Buffer = new();

    private LogBridge(bool InIsError) => bIsError = InIsError;

    public override Encoding Encoding => Encoding.UTF8;

    // LeanCLR reverse channel (named P/Invoke, resolved by leanclr's PInvokes table under the managed
    // full name "Interop.LogBridge::LogLeanCLR" — the [DllImport] module name is ignored by leanclr).
    // LeanCLR cannot perform an unmanaged calli on a raw function pointer, so unlike the Mono/CoreCLR
    // path (SetLog + delegate* calli) the host registers FScriptLog::Log as a named P/Invoke and Flush
    // falls through to this extern on that backend. The DllImport is only ever CALLED there, so
    // Mono/CoreCLR never try to load the fake module (zero regression).
    [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
    private static unsafe extern void LogLeanCLR(byte* InBuffer, int InSize, byte InIsError);

    // Which of the two sinks Flush is allowed to use. Set once from InitializeLeanCLR — the leanclr-only
    // entry point — and deliberately never cleared, because "this process runs on leanclr" is a
    // structural fact for the lifetime of the process. Flush must NOT key off LogFn == null instead:
    // that null is merely temporal and occurs on every backend, both before SetLog and after
    // Deinitialize. The host resolves SetLog and Initialize through two independent lookups
    // (FScriptDomainImpl.inl RegisterLog) whose macros leave the pointer null and report nothing on
    // failure (GET_METHOD_AND_GET_FUNCTION_POINTER / LOAD_ASSEMBLY_AND_GET_FUNCTION_POINTER), so a
    // SetLog miss with Initialize still succeeding would have turned the first Console.WriteLine on
    // Mono/CoreCLR into a DllNotFoundException for a module that does not exist there. Keying off the
    // backend keeps the original behaviour in that case: no sink means the line is dropped.
    private static bool bUseNamedPInvoke;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe void SetLog(nint InLogFn)
    {
        LogFn = (delegate* unmanaged[Cdecl]<byte*, int, byte, void>)InLogFn;
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void Initialize()
    {
        ConsoleOut = Console.Out;

        ConsoleError = Console.Error;

        Console.SetOut(new LogBridge(InIsError: false));

        Console.SetError(new LogBridge(InIsError: true));
    }

    // LeanCLR variant of Initialize: redirect without capturing the previous writers. Reading
    // Console.Out/Console.Error materializes the default console StreamWriter, whose OSEncoding path
    // hits Kernel32 P/Invokes (GetConsoleCP/WideCharToMultiByte) that leanclr does not implement —
    // a fatal, not a catchable exception. SetOut/SetError themselves are pure managed field writes.
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void InitializeLeanCLR()
    {
        // Before the first SetOut: the writers created below are the earliest LogBridge instances that
        // can exist (the constructor is private), so none of them can ever observe this as false.
        bUseNamedPInvoke = true;

        Console.SetOut(new LogBridge(InIsError: false));

        Console.SetError(new LogBridge(InIsError: true));
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void Deinitialize()
    {
        if (ConsoleOut is not null)
        {
            Console.SetOut(ConsoleOut);

            ConsoleOut = null;
        }

        if (ConsoleError is not null)
        {
            Console.SetError(ConsoleError);

            ConsoleError = null;
        }

        unsafe
        {
            LogFn = null;
        }
    }

    public override void Write(char value)
    {
        if (value == '\n')
        {
            if (bIsError)
            {
                Buffer.Append(value);
            }
            else
            {
                Flush();
            }
        }
        else if (value != '\r')
        {
            Buffer.Append(value);
        }
    }

    public override void Write(string? value)
    {
        if (value != null)
        {
            foreach (var character in value)
            {
                Write(character);
            }
        }
    }

    public override void WriteLine(string? value)
    {
        Write(value);

        Flush();
    }

    public override void WriteLine()
    {
        Flush();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (Buffer.Length > 0)
            {
                Flush();
            }
        }

        base.Dispose(disposing);
    }

    public override void Flush()
    {
        if (Buffer.Length > 0)
        {
            Flush(Buffer.ToString().AsSpan(), bIsError);

            Buffer.Clear();
        }
    }

    private static unsafe void Flush(ReadOnlySpan<char> InBuffer, bool InIsError)
    {
        var MaxByteCount = Encoding.UTF8.GetMaxByteCount(InBuffer.Length) + 1;

        var UTF8 = MaxByteCount <= 512
            ? stackalloc byte[MaxByteCount]
            : new byte[MaxByteCount];

        var Size = Encoding.UTF8.GetBytes(InBuffer, UTF8);

        UTF8[Size] = 0;

        fixed (byte* Ptr = UTF8)
        {
            if (LogFn != null)
            {
                // Mono/CoreCLR: unmanaged function pointer handed over via SetLog.
                LogFn(Ptr, Size, (byte)(InIsError ? 1 : 0));
            }
            else if (bUseNamedPInvoke)
            {
                // LeanCLR: named P/Invoke (see LogLeanCLR's comment); SetLog is never called there.
                LogLeanCLR(Ptr, Size, (byte)(InIsError ? 1 : 0));
            }
        }
    }
}