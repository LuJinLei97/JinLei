﻿using System.IO;
using System.Management;

using JinLei.Extensions;

namespace JinLei.Utilities;

public partial class Win32_ShareUtility
{
    /// <returns>see <see href="https://learn.microsoft.com/windows/win32/cimwin32prov/delete-method-in-class-win32-share#return-value">Win32_Share Delete Return value</see></returns>
    public static int DeleteShare(string shareName)
    {
        try
        {
            foreach(var item in new ManagementObjectSearcher($"SELECT * FROM Win32_Share WHERE name = '{shareName}'").Get().Cast<ManagementObject>())
            {
                var result = (int)item.InvokeMethod("Delete", default);
                if(result != 0)
                {
                    return result;
                }
            }
        } catch(SystemException e)
        {
            Console.WriteLine("Error attempting to delete share {0}:", shareName);
            Console.WriteLine(e.Message);
            return 8;
        }

        return 0;
    }

    /// <returns>see <see href="https://learn.microsoft.com/windows/win32/cimwin32prov/create-method-in-class-win32-share#return-value">Win32_Share Create Return value</see></returns>
    public static int MakeShare(DirectoryInfo directory, string shareName = default)
    {
        try
        {
            shareName = string.IsNullOrWhiteSpace(shareName) ? directory.Name : shareName;

            DeleteShare(shareName);

            return (int)new ManagementClass("Win32_Share").InvokeMethod("Create", [Path.Combine(directory.GetParent().FullName, directory.Name), shareName, "0"]);
        } catch(SystemException e)
        {
            Console.WriteLine("Error attempting to create share {0}:", shareName);
            Console.WriteLine(e.Message);
            return 8;
        }
    }

    public static TaskCompletionSource<string> NetShareDelete(string shareName) => NetShareInvoke($@" ""{shareName}"" /delete");

    public static TaskCompletionSource<string> NetShareMake(DirectoryInfo directory, string shareName = default, string arguments = default)
    {
        shareName = string.IsNullOrWhiteSpace(shareName) ? directory.Name : shareName;

        if(string.IsNullOrWhiteSpace(NetShareInvoke($@" ""{shareName}"" ").Task.Result) == false)
        {
            NetShareDelete(shareName);
        }

        return NetShareInvoke($@" ""{shareName}""=""{directory.FullName}"" {arguments}");
    }

    public static TaskCompletionSource<string> NetShareInvoke(string arguments) => ProcessUtility.InvokeCMD($@"net share {arguments}");
}