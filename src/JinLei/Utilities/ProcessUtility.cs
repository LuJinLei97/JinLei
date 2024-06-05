using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

using JinLei.Extensions;

namespace JinLei.Utilities;
public static partial class ProcessUtility
{
    public static Collection<PSObject> InvokePowershell(string command)
    {
        using var powerShell = PowerShell.Create().AddScript(command);
        return powerShell.Invoke();
    }

    public static TaskCompletionSource<string> InvokeEXE(string fileName, string arguments)
    {
        Process.Start(new ProcessStartInfo(fileName, arguments)
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        }).Out(out var process).Do(t => process.WaitForExit());

        var result = new TaskCompletionSource<string>();
        result.TrySetResult(process.StandardOutput.Do(Trim));
        if(string.IsNullOrWhiteSpace(process.StandardError.Do(Trim).Out(out var error)))
        {
            result.TrySetException(new Exception(error));
        }

        return result;

        string Trim(StreamReader s) => s.ReadToEnd().Trim(Environment.NewLine);
    }

    public static TaskCompletionSource<string> InvokeCMD(string command) => InvokeEXE("cmd", $"/c {command}");

    public static TaskCompletionSource<string> MakeDirectoryLink(DirectoryInfo source, DirectoryInfo target) => InvokeCMD($"MkLink / D {target.FullName} {source.FullName}");

    public static TaskCompletionSource<string> JustCopyDirectory(DirectoryInfo source, DirectoryInfo target) => InvokeCMD($"XCopy {source.FullName} {target.FullName} /T /E");
}