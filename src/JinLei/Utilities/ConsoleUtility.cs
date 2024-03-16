using System.IO;

using JinLei.Extensions;

namespace JinLei.Utilities;
public static partial class ConsoleUtility
{
    public static string ReadPath(string tip = "Path:") => tip.Do(Console.Write).Return(Console.ReadLine().Trim('"'));

    public static DirectoryInfo ReadDirectoryPath(string tip = "Path:") => new(ReadPath(tip));

    public static FileInfo ReadFilePath(string tip = "Path:") => new(ReadPath(tip));
}
