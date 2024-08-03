using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

using JinLei.Classes;
using JinLei.Extensions;

namespace JinLei.Utilities;

public partial class ProcessUtility
{
    public static Collection<PSObject> InvokePowerShell(string command)
    {
        using var powerShell = PowerShell.Create().AddScript(command);
        return powerShell.Invoke();
    }

    public static TaskCompletionSource<string> Start(Process process, ProcessMode processMode = ProcessMode.SyncMode, ProcessMode outputStreamReadMode = ProcessMode.SyncMode, ProcessMode errorStreamReadMode = ProcessMode.SyncMode)
    {
        if(outputStreamReadMode != ProcessMode.Undefined)
        {

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
        }

        if(errorStreamReadMode != ProcessMode.Undefined)
        {
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
        }

        process.Start();
        process.Do(t => t.BeginOutputReadLine(), t => outputStreamReadMode == ProcessMode.AsyncMode);
        process.Do(t => t.BeginErrorReadLine(), t => errorStreamReadMode == ProcessMode.AsyncMode);
        process.Do(t => t.WaitForExit(), t => processMode == ProcessMode.SyncMode);

        var result = new TaskCompletionSource<string>();

        if(outputStreamReadMode == ProcessMode.SyncMode)
        {
            result.TrySetResult(process.StandardOutput.Do(Trim));
        }

        if(errorStreamReadMode == ProcessMode.SyncMode)
        {
            if(process.StandardError.EndOfStream == false && string.IsNullOrWhiteSpace(process.StandardError.Do(Trim).Out(out var error)) == false)
            {
                result.TrySetException(new Exception(error));
            }
        }

        return result;

        string Trim(StreamReader s) => s.ReadToEnd().Trim(Environment.NewLine);
    }

    public static TaskCompletionSource<string> Start(ProcessStartInfo processStartInfo, out Process process, ProcessMode processMode = ProcessMode.SyncMode, ProcessMode outputStreamReadMode = ProcessMode.SyncMode, ProcessMode errorStreamReadMode = ProcessMode.SyncMode) => new Process() { StartInfo = processStartInfo }.Out(out process).Do(t => Start(t, processMode, outputStreamReadMode, errorStreamReadMode));

    public static TaskCompletionSource<string> Start(string fileName, string arguments, out Process process, ProcessMode processMode = ProcessMode.SyncMode, ProcessMode outputStreamReadMode = ProcessMode.SyncMode, ProcessMode errorStreamReadMode = ProcessMode.SyncMode) => Start(new(fileName, arguments), out process, processMode, outputStreamReadMode, errorStreamReadMode);

    public static TaskCompletionSource<string> InvokeEXE(string fileName, string arguments) => Start(fileName, arguments, out _, ProcessMode.SyncMode, ProcessMode.SyncMode, ProcessMode.SyncMode);

    public static TaskCompletionSource<string> InvokeCMD(string command) => InvokeEXE("cmd", $"/c {command}");

    public partial class CMDUtility
    {
        public static TaskCompletionSource<string> MakeLink(FileSystemInfo source, FileSystemInfo target, string arguments = default)
        {
            if(source is DirectoryInfo && target is DirectoryInfo)
            {
                return ProcessUtility.InvokeCMD($"MkLink {arguments ?? "/D"} {target.FullName} {source.FullName}");
            } else if(source is FileInfo && target is FileInfo)
            {
                return ProcessUtility.InvokeCMD($"MkLink {arguments} {target.FullName} {source.FullName}");
            }

            return new TaskCompletionSource<string>();
        }

        public static TaskCompletionSource<string> JustCopyDirectory(DirectoryInfo source, DirectoryInfo target) => ProcessUtility.InvokeCMD($"XCopy {source.FullName} {target.FullName} /T /E");
    }
}