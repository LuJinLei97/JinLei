using System.IO;

using JinLei.Extensions;

namespace JinLei.Utilities;
public static partial class ConsoleUtility
{
    public static string TipAndReadLine(string tip = "Tip:") => (tip ?? string.Empty).Do(Console.Write).Return(Console.ReadLine());

    public static string ReadPath(string tip = "Path:") => TipAndReadLine(tip).Trim('"');

    public static DirectoryInfo ReadDirectoryPath(string tip = "DirectoryPath:") => new(ReadPath(tip));

    public static FileInfo ReadFilePath(string tip = "FilePath:") => new(ReadPath(tip));
}
