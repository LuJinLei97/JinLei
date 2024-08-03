using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using JinLei.Classes;
using JinLei.Extensions;

namespace JinLei.Utilities;

public partial class Utility
{
    #region Properties
    /// <inheritdoc cref="Environment.SpecialFolder.Desktop"/>
    public static string DesktopPath => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    /// <inheritdoc cref="DesignerProperties.GetIsInDesignMode(DependencyObject)"/>
    public static bool IsInDesignMode { get; } = DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue.AsDynamicOrDefault();
    #endregion
}

public partial class RangeUtility
{
    public static TSource GetValueInRange<TSource>(TSource source, TSource range1, TSource range2) where TSource : IComparable => GetValueInRange(source, range1, range2, default);

    public static TSource GetValueInRange<TSource>(TSource source, TSource range1, TSource range2, IComparer<TSource> comparer = default) => new[] { range1, source, range2 }.OrderBy(t => t, comparer).ElementAt(1);

    public static bool IsInRange<TSource>(TSource source, TSource range1, TSource range2) => source.Equals(GetValueInRange(source, range1, range2));
}

public partial class TypeUtility
{
    public static Type GetCallingType([CallerMemberName] string memberName = default) => GetTypeFromStackFrame(memberName, 2);

    public static Type GetTypeFromStackFrame([CallerMemberName] string memberName = default, int skipFrames = 1) => GetType(new StackFrame(skipFrames, false).GetMethod().DeclaringType, memberName);

    public static Type GetType(Type type, string memberName = default) => type.GetRuntimeProperty(memberName ??= string.Empty)?.PropertyType ?? type.GetRuntimeField(memberName)?.FieldType ?? type;
}

public partial class ByteUtility
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
        foreach(var item in EnumUtility.ToDictionary<BytesUnit>())
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

public partial class EnumUtility
{
    public static Dictionary<string, long> ToDictionary<TEnum>(TEnum tEnum = default) where TEnum : Enum => Enum.GetValues(tEnum.GetType()).Cast<Enum>().ToDictionary(e => e.ToString(), Convert.ToInt64);
}

public partial class CookieUtility
{
    private static Uri MyUri = new("http://MyTest.com");

    public static CookieContainer Parse(string cookies) => new CookieContainer().Do(t => t.SetCookies(MyUri, cookies));

    public static CookieCollection ParseToCookies(string cookies) => Parse(cookies).GetCookies(MyUri);

    public static string ParseToCookieHeader(string cookies) => Parse(cookies).GetCookieHeader(MyUri);
}

public partial class ZipUtility
{
    public static bool ToZip(FileSystemInfo fileSystemInfo, out string savedZipPath, string zipFilePath = default, CompressionLevel compressionLevel = CompressionLevel.NoCompression, Encoding entryNameEncoding = default, bool includeBaseDirectory = false)
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
}

public partial class FlieUtility
{
    public static string GetReadableSize(FileSystemInfo fileSystemInfo) => ByteUtility.GetReadableSize(fileSystemInfo.GetLength());
}

public partial class ImageUtility
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

public partial class FrameworkElementUtility
{
    public static BitmapSource ToImage(FrameworkElement frameworkElement)
    {
        var renderTargetBitmap = new RenderTargetBitmap((int)frameworkElement.ActualWidth, (int)frameworkElement.ActualHeight, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormats.Default);
        renderTargetBitmap.Render(frameworkElement);

        return renderTargetBitmap;
    }

    public static BitmapSource SaveToImage(FrameworkElement frameworkElement, string imageFilePath)
    {
        var fileInfo = new FileInfo(imageFilePath);

        var bitmapSource = FrameworkElementUtility.ToImage(frameworkElement);

        using var fileStream = fileInfo.Create();
        var bitmapEncoder = ImageUtility.GetBitmapEncoderByExtension(fileInfo.Extension);
        bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
        bitmapEncoder.Save(fileStream);

        return bitmapSource;
    }
}

public partial class DependencyPropertyUtility
{
    /// <summary>
    /// DependencyProperty后缀
    /// </summary>
    public const string Suffix = "Property";

    public static DependencyProperty Register([CallerMemberName] string callerMemberName = default, PropertyMetadata typeMetadata = default, ValidateValueCallback validateValueCallback = default, DependencyPropertyType registerType = DependencyPropertyType.Default)
    {
        var ownerType = TypeUtility.GetTypeFromStackFrame(default, 2);
        var propertyType = TypeUtility.GetType(ownerType, callerMemberName);
        return registerType == DependencyPropertyType.Default
            ? DependencyProperty.Register(callerMemberName, propertyType, ownerType, typeMetadata, validateValueCallback)
            : DependencyProperty.RegisterAttached(callerMemberName, propertyType, ownerType, typeMetadata, validateValueCallback);
    }
}

public partial class EnumerableUtility
{
    public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count = -1)
    {
        for(var i = 0; i < 1 || count < 0; i++)
        {
            foreach(var item in Enumerable.Repeat(element, count < 0 ? int.MaxValue : count))
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<bool> Repeat() => Repeat(true);
}