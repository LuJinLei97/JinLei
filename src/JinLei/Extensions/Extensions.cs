using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using JinLei.Utilities;

using Point = System.Windows.Point;

namespace JinLei.Extensions;
public static partial class CookieContainerExtensions
{
    private static Uri MyUri = new("http://MyTest.com");

    public static CookieContainer Parse(string cookies) => new CookieContainer().Do(t => t.SetCookies(MyUri, cookies));

    public static CookieCollection ParseToCookies(string cookies) => Parse(cookies).GetCookies(MyUri);

    public static string ParseToCookieHeader(string cookies) => Parse(cookies).GetCookieHeader(MyUri);
}

public static class DateTimeExtensions
{
    /// <inheritdoc cref="DateTimeOffset.Subtract(DateTimeOffset)"/>
    public static TimeSpan Subtract(this DateTimeOffset timeOffset, IEnumerable<DateTimeOffset> timeOffsetsToSubtract)
    {
        var result = timeOffset.Offset;
        timeOffsetsToSubtract?.ForEach(t =>
        {
            var t1 = t.UtcDateTime.GetValueInRange(timeOffset.UtcDateTime, timeOffset.DateTime);
            var t2 = t.DateTime.GetValueInRange(timeOffset.UtcDateTime, timeOffset.DateTime);
            result -= t1.GetDateTimeOffset(t2).Offset;
        });

        return result;
    }

    /// <inheritdoc cref="DateTimeOffset.DateTimeOffset(DateTime, TimeSpan)"/>
    public static DateTimeOffset GetDateTimeOffset(this DateTime utcDateTime, DateTime dateTime) => new(utcDateTime, dateTime - utcDateTime);
}

public static partial class EnumExtensions
{
    public static Dictionary<string, long> ToDictionary(this Enum tEnum)
    {
        var type = tEnum.GetType();
        var typeAsKey = $"EnumNameValues_{type.FullName}";

        if(MemoryCache.Default[typeAsKey].Is<Dictionary<string, long>>(out var result) == false)
        {
            result = Enum.GetValues(type).Cast<Enum>().ToDictionary(e => e.ToString(), Convert.ToInt64);

            MemoryCache.Default.Set(typeAsKey, result, new CacheItemPolicy() { SlidingExpiration = new(0, 5, 0) });
        }

        return result;
    }
}

public static partial class FielSystemInfoExtensions
{
    public static long GetLength(this FileSystemInfo fileSystemInfo) => (fileSystemInfo switch
    {
        FileInfo fileInfo => [fileInfo],
        DirectoryInfo directoryInfo => directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories),
        _ => throw new NotImplementedException()
    }).Sum(f => f.Length);

    public static string GetReadableSize(this FileSystemInfo fileSystemInfo) => ByteUtility.GetReadableSize(fileSystemInfo.GetLength());

    public static bool ToZip(this FileSystemInfo fileSystemInfo, out string savedZipPath, string zipFilePath = default, CompressionLevel compressionLevel = CompressionLevel.NoCompression, Encoding entryNameEncoding = default, bool includeBaseDirectory = false)
    {
        if(fileSystemInfo.Exists == false)
        {
            throw new FileNotFoundException();
        }

        savedZipPath = string.IsNullOrWhiteSpace(zipFilePath) ? Path.ChangeExtension(fileSystemInfo.FullName, "zip") : zipFilePath;

        try
        {
            if(fileSystemInfo is DirectoryInfo directoryInfo)
            {
                ZipFile.CreateFromDirectory(directoryInfo.FullName, savedZipPath, compressionLevel, includeBaseDirectory, entryNameEncoding);
            } else if(fileSystemInfo is FileInfo fileInfo)
            {
                using var zip = ZipFile.Open(savedZipPath, ZipArchiveMode.Create, entryNameEncoding);
                zip.CreateEntryFromFile(fileInfo.FullName, fileInfo.Name, compressionLevel);
            }

            return true;
        } catch
        {
            return false;
        }
    }

    public static DirectoryInfo GetParent(this FileSystemInfo fileSystemInfo)
    {
        return fileSystemInfo switch
        {
            DirectoryInfo f => f.Parent,
            FileInfo f => f.Directory,
            _ => throw new NotImplementedException()
        };
    }

    public static T GetPathNonAlreadyExists<T>(this T targetFileSystemInfo) where T : FileSystemInfo
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
    public static BitmapSource ToImage(this FrameworkElement frameworkElement)
    {
        var renderTargetBitmap = new RenderTargetBitmap((int)frameworkElement.ActualWidth, (int)frameworkElement.ActualHeight, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormats.Default);
        renderTargetBitmap.Render(frameworkElement);

        return renderTargetBitmap;
    }

    public static BitmapSource SaveToImage(this FrameworkElement frameworkElement, string imageFilePath)
    {
        var fileInfo = new FileInfo(imageFilePath);

        var bitmapSource = frameworkElement.ToImage();

        using var fileStream = fileInfo.Create();
        var bitmapEncoder = ImageExtensions.GetBitmapEncoderByExtension(fileInfo.Extension);
        bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
        bitmapEncoder.Save(fileStream);

        return bitmapSource;
    }

    public static bool GetNeedsClipBounds(this FrameworkElement frameworkElement) => getFrameworkElementNeedsClipBounds.Invoke(frameworkElement);
    private static Func<FrameworkElement, bool> getFrameworkElementNeedsClipBounds = frameworkElementNeedsClipBoundsPropertyInfo.GetGetMethod().CreateDelegate<Func<FrameworkElement, bool>>();
    private static PropertyInfo frameworkElementNeedsClipBoundsPropertyInfo = typeof(FrameworkElement).GetProperty("NeedsClipBounds", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
}

public static partial class ImageExtensions
{
    public static BitmapEncoder GetBitmapEncoderByExtension(string extension) => extension.Trim().ToLower() switch
    {
        ".png" => new PngBitmapEncoder(),
        ".jpg" => new JpegBitmapEncoder(),
        ".jpeg" => new JpegBitmapEncoder(),
        ".bmp" => new BmpBitmapEncoder(),
        ".gif" => new GifBitmapEncoder(),
        ".tiff" => new TiffBitmapEncoder(),
        ".wmp" => new WmpBitmapEncoder(),
        _ => new PngBitmapEncoder()
    };
}

public static class MD5Extensions
{
    public static MD5 DefaultMD5 { get; } = MD5.Create();

    public static string GetMD5(this byte[] buffer, int offset = 0, int? count = default)
    {
        using var stream = new MemoryStream(buffer, offset, count ?? (buffer.Length - offset));
        return stream.GetMD5();
    }

    public static string GetMD5(this Stream inputStream) => BitConverter.ToString(DefaultMD5.ComputeHash(inputStream)).Replace("-", "").ToLower();
}

public static partial class ResxExtensions
{
    public static List<KeyValuePair<string, T>> ToList<T>(this ResXResourceReader resXResourceReader) => resXResourceReader.Cast<DictionaryEntry>().Select(t => t.ToKeyValuePair<string, T>()).ToList();

    public static void AddResources<T>(this ResXResourceWriter resXResourceWriter, IEnumerable<KeyValuePair<string, T>> keyValues) => keyValues?.ForEach(keyValue => resXResourceWriter.AddResource(keyValue.Key, keyValue.Value));
}

public static partial class StreamExtensions
{
    /// <inheritdoc cref="MemoryStream.ToArray"/>
    public static byte[] ToArray(this Stream stream) => (stream is MemoryStream memoryStream ? memoryStream : new MemoryStream().Do(stream.CopyTo)).ToArray();

    /// <inheritdoc cref="StreamReader.ReadToEnd"/>
    public static string ReadToString(this Stream stream, Encoding encoding = default) => new StreamReader(stream, encoding ?? Encoding.UTF8).ReadToEnd();
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

        // Trim specified strings.
        if((trimType & TrimType.Head) != 0)
        {
            TrimByType(TrimType.Head);
        }

        if((trimType & TrimType.Tail) != 0)
        {
            TrimByType(TrimType.Tail);
        }

        var rect = new Rect(new Point(start, 0), new Point(end, 0));
        return s.SubstringEatException((int)rect.Left, (int)rect.Width + 1);

        void TrimByType(TrimType trimType)
        {
        Serach:
            foreach(var trimString in trimStrings)
            {
                if(s.SubstringEatException(trimType == TrimType.Head ? start : end, trimString.Length * (trimType == TrimType.Head ? 1 : -1)) == trimString)
                {
                    _ = trimType == TrimType.Head ? start += trimString.Length : end -= trimString.Length;
                    goto Serach;
                }
            }
        }
    }

    /// <inheritdoc cref="string.Trim()"/>
    public static string Trim(this string s, params string[] trimStrings) => s.TrimHelper(trimStrings, TrimType.Both);

    /// <inheritdoc cref="string.TrimStart(char[])"/>
    public static string TrimStart(this string s, params string[] trimStrings) => s.TrimHelper(trimStrings, TrimType.Head);

    /// <inheritdoc cref="string.TrimEnd(char[])"/>
    public static string TrimEnd(this string s, params string[] trimStrings) => s.TrimHelper(trimStrings, TrimType.Tail);

    /// <inheritdoc cref="Encoding.GetBytes(string)"/>
    public static byte[] GetBytes(this string s, Encoding encoding = default) => (encoding ?? Encoding.UTF8).GetBytes(s);

    public static string GetMD5(this string s) => s.GetBytes().GetMD5();

    /// <inheritdoc cref="File.ReadAllLines(string)"/>
    public static string[] ReadAllLines(this string s) => s.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);

    public static string SubstringEatException(this string s, int startIndex, int length)
    {
        var result = string.Empty;

        try
        {
            var rect = new Rect(new System.Windows.Point(startIndex, 0), new Vector(length += length >= 0 ? -1 : 1, 0));
            rect.Intersect(new Rect(new System.Windows.Point(0, 0), new Vector(s.Length - 1, 0)));
            if(rect.IsEmpty == false)
            {
                result = s.Substring((int)rect.Left, (int)rect.Width + 1);
            }
        } catch { }

        return result;
    }
}