using System.Collections;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows;

using JinLei.Classes;
using JinLei.Utilities;

namespace JinLei.Extensions;

public static partial class DateTimeExtensions
{
    /// <inheritdoc cref="DateTimeOffset.Subtract(DateTimeOffset)"/>
    public static TimeSpan Subtract(this DateTimeOffset timeOffset, IEnumerable<DateTimeOffset> timeOffsetsToSubtract)
    {
        var result = timeOffset.Offset;
        timeOffsetsToSubtract?.ForEachDo(t =>
        {
            var t1 = RangeUtility.GetValueInRange(t.UtcDateTime, timeOffset.UtcDateTime, timeOffset.DateTime);
            var t2 = RangeUtility.GetValueInRange(t.DateTime, timeOffset.UtcDateTime, timeOffset.DateTime);
            result -= t1.GetDateTimeOffset(t2).Offset;
        });

        return result;
    }

    /// <inheritdoc cref="DateTimeOffset.DateTimeOffset(DateTime, TimeSpan)"/>
    public static DateTimeOffset GetDateTimeOffset(this DateTime utcDateTime, DateTime dateTime) => new(utcDateTime, dateTime - utcDateTime);
}

public static partial class FielSystemInfoExtensions
{
    public static long GetLength(this FileSystemInfo fileSystemInfo) => (fileSystemInfo switch
    {
        FileInfo fileInfo => [fileInfo],
        DirectoryInfo directoryInfo => directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories),
        _ => throw new NotImplementedException()
    }).Sum(f => f.Length);

    public static DirectoryInfo GetParent(this FileSystemInfo fileSystemInfo)
    {
        return fileSystemInfo switch
        {
            DirectoryInfo f => f.Parent,
            FileInfo f => f.Directory,
            _ => throw new NotImplementedException()
        };
    }

    public static T GetUnoccupiedPath<T>(this T targetFileSystemInfo) where T : FileSystemInfo
    {
        var originalName = Path.GetFileNameWithoutExtension(targetFileSystemInfo.Name);
        var parentPath = targetFileSystemInfo.GetParent().FullName;

        for(var i = 2; targetFileSystemInfo.Exists; i++)
        {
            var newPath = Path.Combine(parentPath, $"{originalName} ({i}){targetFileSystemInfo.Extension}");
            FileSystemInfo t = targetFileSystemInfo switch
            {
                DirectoryInfo => new DirectoryInfo(newPath),
                FileInfo => new FileInfo(newPath),
                _ => throw new NotImplementedException()
            };

            targetFileSystemInfo = (T)t;
        }

        return targetFileSystemInfo;
    }
}

public static partial class FrameworkElementExtensions
{
    public static bool GetNeedsClipBounds(this FrameworkElement frameworkElement) => getFrameworkElementNeedsClipBounds.Invoke(frameworkElement);
    private static Func<FrameworkElement, bool> getFrameworkElementNeedsClipBounds = frameworkElementNeedsClipBoundsPropertyInfo.GetGetMethod().CreateDelegate<Func<FrameworkElement, bool>>();
    private static PropertyInfo frameworkElementNeedsClipBoundsPropertyInfo = typeof(FrameworkElement).GetProperty("NeedsClipBounds", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
}

public static partial class ResxExtensions
{
    public static List<KeyValuePair<string, T>> ToList<T>(this ResXResourceReader resXResourceReader) => resXResourceReader.Cast<DictionaryEntry>().Select(t => t.ToKeyValuePair<string, T>()).ToList();

    public static void AddResources<T>(this ResXResourceWriter resXResourceWriter, IEnumerable<KeyValuePair<string, T>> keyValues) => keyValues?.ForEachDo(keyValue => resXResourceWriter.AddResource(keyValue.Key, keyValue.Value));
}

public static partial class StreamExtensions
{
    /// <inheritdoc cref="MemoryStream.ToArray"/>
    public static byte[] ToArray(this Stream stream) => (stream is MemoryStream memoryStream ? memoryStream : new MemoryStream().Do(stream.CopyTo)).ToArray();
}

public static partial class StringExtensions
{
    [Flags]
    public enum TrimType
    {
        /// <summary>
        /// Trim from the beginning of the string.
        /// </summary>
        Head = 1 << 0,

        /// <summary>
        /// Trim from the end of the string.
        /// </summary>
        Tail = 1 << 1,

        /// <summary>
        /// Trim from both the beginning and the end of the string.
        /// </summary>
        Both = Head | Tail
    }

    private static string TrimHelper(this string s, string[] trimStrings, TrimType trimType = TrimType.Both)
    {
        trimStrings = [.. trimStrings.GetSelfOrEmpty().OrderByDescending(t => t.Length)];

        // end will point to the first non-trimmed string on the right.
        // start will point to the first non-trimmed string on the left.
        var end = s.Length - 1;
        var start = 0;

        if(CheckRange() == false)
        {
            return string.Empty;
        }

        // Trim specified strings.
        if((trimType & TrimType.Head) != 0)
        {
            if(TrimByType(TrimType.Head) == false)
            {
                return string.Empty;
            }
        }

        if((trimType & TrimType.Tail) != 0)
        {
            if(TrimByType(TrimType.Tail) == false)
            {
                return string.Empty;
            }
        }

        var range = new RangeInfo(start) { End = end };
        return s.SubstringEatException(range.Start, range.Count);

        bool TrimByType(TrimType trimType)
        {
        Serach:
            foreach(var trimString in trimStrings)
            {
                var range = trimType == TrimType.Head ? new RangeInfo(start, trimString.Length) : new RangeInfo(end, -trimString.Length);
                if(s.SubstringEatException(range.Start, range.Count) == trimString)
                {
                    _ = trimType == TrimType.Head ? start += trimString.Length : end -= trimString.Length;
                    if(CheckRange() == false)
                    {
                        return false;
                    }

                    goto Serach;
                }
            }

            return true;
        }

        bool CheckRange() => start <= end;
    }

    /// <inheritdoc cref="string.Trim()"/>
    public static string Trim(this string s, params string[] trimStrings) => s.TrimHelper(trimStrings, TrimType.Both);

    /// <inheritdoc cref="string.TrimStart(char[])"/>
    public static string TrimStart(this string s, params string[] trimStrings) => s.TrimHelper(trimStrings, TrimType.Head);

    /// <inheritdoc cref="string.TrimEnd(char[])"/>
    public static string TrimEnd(this string s, params string[] trimStrings) => s.TrimHelper(trimStrings, TrimType.Tail);

    /// <inheritdoc cref="File.ReadAllLines(string)"/>
    public static string[] ReadAllLines(this string s)
    {
        using var stringReader = new StringReader(s);
        return EnumerableUtility.Repeat().ForEachThenByKey(t => KeyValuePair.Create(stringReader.ReadLine().Out(out var line).IsNull() == false, line), conditionType: ConditionType.While).ToArray();
    }

    public static string SubstringEatException(this string s, int startIndex, int length = int.MaxValue / 2)
    {
        var result = string.Empty;

        try
        {
            if(new RangeInfo(startIndex, length).TryIntersect(new RangeInfo(0, s.Length), out var resultRange))
            {
                result = s.Substring(resultRange.Left.Value, resultRange.AbsCount);
            }
        } catch { }

        return result;
    }
}