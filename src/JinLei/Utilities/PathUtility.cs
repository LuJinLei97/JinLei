using System.IO;

using JinLei.Extensions;

namespace JinLei.Utilities;
public static partial class PathUtility
{
    public static bool TryGetFullPath(string path, out string fullPath)
    {
        try
        {
            return Path.GetFullPath(path).Out(out fullPath).Return(true);
        } catch
        {
            return string.Empty.Out(out fullPath).Return(false);
        }
    }

    public static string GetFullPath(string basePath, string path)
    {
        var b1 = TryGetFullPath(basePath, out var path1);
        var b2 = TryGetFullPath(path, out var path2);
        return b1 && b2 && TryGetFullPath(Path.Combine(basePath, path), out var path3) ? path3 : b2 ? path2 : path;
    }
}