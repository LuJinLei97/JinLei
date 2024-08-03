using System.IO;
using System.Security.Cryptography;

using JinLei.Classes;

namespace JinLei.Utilities;

public partial class MD5Utility
{
    public static MD5 DefaultMD5 { get; } = MD5.Create();

    public static string GetMD5(byte[] buffer, int offset = 0, int count = int.MaxValue / 2) => new RangeInfo(offset, count).TryIntersect(new RangeInfo(0, buffer.Length), out var resultRange) == false ? default : BitConverter.ToString(DefaultMD5.ComputeHash(buffer, resultRange.Left.Value, resultRange.AbsCount)).Replace("-", "").ToLower();

    public static string GetMD5(Stream inputStream) => BitConverter.ToString(DefaultMD5.ComputeHash(inputStream)).Replace("-", "").ToLower();
}