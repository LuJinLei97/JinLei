using System.ComponentModel;
using System.Windows;

using JinLei.Extensions;

namespace JinLei.Utilities;
public static partial class Utilities
{
    #region Properties
    /// <inheritdoc cref="Environment.SpecialFolder.Desktop"/>
    public static string DesktopPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    /// <inheritdoc cref="DesignerProperties.GetIsInDesignMode(DependencyObject)"/>
    public static bool IsInDesignMode { get; } = (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
    #endregion
}

public static partial class ByteUtility
{
    public enum BytesUnit : long
    {
        B = 1,
        KB = 1024 * B,
        MB = 1024 * KB,
        GB = 1024 * MB,
        TB = 1024 * GB,
    }

    public static string GetReadableSize(long length)
    {
        var result = string.Empty;
        foreach(var item in default(BytesUnit).ToDictionary())
        {
            var num = 1.0 * length / item.Value;
            result = $"{num:F1} {item.Key}";

            if(num < 1024)
            {
                return result;
            }
        }

        return result;
    }
}