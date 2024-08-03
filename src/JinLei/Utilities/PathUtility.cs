using System.IO;

using JinLei.Extensions;

namespace JinLei.Utilities;

public partial class PathUtility
{
    public static bool TryGetFullPath(string path, out string fullPath)
    {
        try
        {
            return true.Do(t => Path.GetFullPath(path), out fullPath);
        } catch
        {
            return false.Do(t => string.Empty, out fullPath);
        }
    }

    public static string GetFullPath(string basePath, string path)
    {
        var b1 = TryGetFullPath(basePath, out var path1);
        var b2 = TryGetFullPath(path, out var path2);
        return b1 && b2 && TryGetFullPath(Path.Combine(basePath, path), out var path3) ? path3 : b2 ? path2 : path;
    }
}