using System.Collections;
using System.Collections.Specialized;

namespace JinLei.Extensions;

public static partial class IDictionaryExtensions
{
    public static void Change<TKey, TValue>(this IDictionary<TKey, TValue> items, IEnumerable<KeyValuePair<TKey, TValue>> values = default, NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add, int count = int.MaxValue / 2)
    {
        if(action is NotifyCollectionChangedAction.Add or NotifyCollectionChangedAction.Replace)
        {
            values?.ForEachDo((v, i) => items[v.Key] = v.Value, (v, i) => action == NotifyCollectionChangedAction.Replace || items.ContainsKey(v.Key) == false, (v, i) => i < count);
        } else
        {
            (items as ICollection<KeyValuePair<TKey, TValue>>).Change(values, action, count);
        }
    }

    public static bool TryGetValueEatException<TKey, TValue>(this IDictionary<TKey, TValue> keyValues, TKey key, out TValue result) => (key.IsNull() == false).Do(t => default, out result) && keyValues.TryGetValue(key, out result);

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<TKey> keys, IEnumerable<TValue> values)
    {
        var valuesEnumerator = values.GetSelfOrEmpty().GetEnumerator();
        return keys.GetSelfOrEmpty().ToDictionary(t => t, t => valuesEnumerator.MoveNext() ? valuesEnumerator.Current : default);
    }

    public static KeyValuePair<TKey, TValue> ToKeyValuePair<TKey, TValue>(this DictionaryEntry dictionaryEntry) => new(dictionaryEntry.Key.AsOrDefault<TKey>(), dictionaryEntry.Value.AsOrDefault<TValue>());
}