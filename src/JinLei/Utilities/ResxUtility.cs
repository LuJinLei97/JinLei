using System.Collections.Specialized;
using System.IO;
using System.Resources;

using JinLei.Extensions;

namespace JinLei.Utilities;

public partial class ResxUtility
{
    public static List<KeyValuePair<string, T>> ReadToList<T>(StreamReader streamReader)
    {
        try
        {
            using var reader = new ResXResourceReader(streamReader);
            return reader.ToList<T>();
        } catch { }

        return [];
    }

    public static void WriteFromItems<T>(StreamWriter streamWriter, IEnumerable<KeyValuePair<string, T>> items)
    {
        try
        {
            using var writer = new ResXResourceWriter(streamWriter);
            writer.AddResources(items);
        } catch { }
    }

    public static Dictionary<string, T> ReadToDictionary<T>(StreamReader streamReader, bool samKeyOverwriteValue = false) => new Dictionary<string, T>().Do(t => t.Change(ReadToList<T>(streamReader), samKeyOverwriteValue ? NotifyCollectionChangedAction.Replace : NotifyCollectionChangedAction.Add));

    public static List<KeyValuePair<string, T>> ReadToList<T>(string path)
    {
        try
        {
            using var streamReader = new StreamReader(path);
            return ReadToList<T>(streamReader);
        } catch { }

        return [];
    }

    public static void WriteFromItems<T>(string path, IEnumerable<KeyValuePair<string, T>> items)
    {
        try
        {
            using var streamWriter = new StreamWriter(path);
            WriteFromItems<T>(streamWriter, items);
        } catch { }
    }

    public static Dictionary<string, T> ReadToDictionary<T>(string path, bool samKeyOverwriteValue = false)
    {
        try
        {
            using var streamReader = new StreamReader(path);
            return ReadToDictionary<T>(streamReader, samKeyOverwriteValue);
        } catch { }

        return [];
    }
}