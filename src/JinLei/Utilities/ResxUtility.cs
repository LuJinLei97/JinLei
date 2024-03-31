using System.Collections.Specialized;
using System.IO;
using System.Resources;

using JinLei.Extensions;

namespace JinLei.Utilities;
public static partial class ResxUtility
{
    public static Dictionary<string, T> ReadToDictionary<T>(StreamReader streamReader, bool samKeyOverwriteValue = false) => new Dictionary<string, T>().Change(ReadToList<T>(streamReader), samKeyOverwriteValue ? NotifyCollectionChangedAction.Replace : NotifyCollectionChangedAction.Add);

    public static List<KeyValuePair<string, T>> ReadToList<T>(StreamReader streamReader, bool samKeyOverwriteValue = false)
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
}