using System.Collections;
using System.Collections.Specialized;
using System.Dynamic;

namespace JinLei.Extensions;
public static partial class IEnumerableExtensions
{
    public static IEnumerable<TSource> GetSelfOrEmpty<TSource>(this IEnumerable<TSource> items) => items.GetValueOrDefault(Enumerable.Empty<TSource>());

    public static TEnumerable GetSelfOrEmpty<TEnumerable, TSource>(this TEnumerable items) where TEnumerable : IEnumerable<TSource>, new() => items.GetValueOrDefault([]);

    public static List<T> GetRange<T>(this IEnumerable<T> values, int index, int count) => values.GetSelfOrEmpty().Skip(index).Take(count).ToList();

    public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Any() == false;

    public static IEnumerable<KeyValuePair<int, T>> SelectIndexValue<T>(this IEnumerable<T> items) => items.GetSelfOrEmpty().Select((t, i) => new KeyValuePair<int, T>(i, t));

    public static int CountOrZero<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Count();

    public static bool CheckRange<TSource>(this IEnumerable<TSource> items, int index) => 0 <= index && index <= (items is ICollection<TSource> collection ? collection.Count : items.CountOrZero()) - 1;

    public static List<TSource> ToListOrEmpty<TSource>(this IEnumerable<TSource> items) => new LinkedList<TSource>(items.GetSelfOrEmpty()).ToList();

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static IEnumerable<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, TResult> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default) => ForEachDoDelegate<TSource, TResult>(items, @delegate, whilePredicate);

    #region ForEach
    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static IEnumerable<TResult> ForEachDoDelegate<TSource, TResult>(this IEnumerable<TSource> items, Delegate @delegate, Delegate wherePredicate = default, Delegate whilePredicate = default)
    {
        if(@delegate.IsNull())
        {
            yield break;
        }

        var itemFuncResult = @delegate switch
        {
            Func<TSource, int, TResult> d => d,
            Func<TSource, TResult> d => d.AddParam(1),
            Func<TResult> d => d.AddParam(default(TSource)).AddParam(1),
            Action<TSource, int> d => d.ToFunc(default(TResult)),
            Action<TSource> d => d.AddParam(1).ToFunc(default(TResult)),
            Action d => d.AddParam(default(TSource)).AddParam(1).ToFunc(default(TResult)),
            _ => throw new NotImplementedException(),
        };

        var wherePredicateResult = wherePredicate switch
        {
            Func<TSource, int, bool> d => d,
            Func<TSource, bool> d => d.AddParam(1),
            Func<bool> d => d.AddParam(default(TSource)).AddParam(1),
            null => (t, i) => true,
            _ => throw new NotImplementedException(),
        };

        var whilePredicateResult = whilePredicate switch
        {
            Func<TSource, int, bool> d => d,
            Func<TSource, bool> d => d.AddParam(1),
            null => (t, i) => true,
            _ => throw new NotImplementedException(),
        };

        var enumerator = items.GetSelfOrEmpty().GetEnumerator();
        for(var i = 0; enumerator.MoveNext(); i++)
        {
            if(whilePredicateResult(enumerator.Current, i) == false)
            {
                yield break;
            }

            if(wherePredicateResult(enumerator.Current, i))
            {
                yield return itemFuncResult(enumerator.Current, i);
            }
        }
    }

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static IEnumerable<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, TResult> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default) => ForEachDoDelegate<TSource, TResult>(items, @delegate, whilePredicate);

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static void ForEach<TSource>(this IEnumerable<TSource> items, Action<TSource, int> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default) => ForEachDoDelegate<TSource, object>(items, @delegate, whilePredicate);

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static void ForEach<TSource>(this IEnumerable<TSource> items, Action<TSource> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default) => ForEachDoDelegate<TSource, object>(items, @delegate, whilePredicate);
    #endregion
}

public static partial class ICollectionExtensions
{
    public static TCollection ChangeFrom<TCollection, TSource>(this TCollection items, IEnumerable<TSource> values = default, NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add) where TCollection : ICollection<TSource>
    {
        if(items.IsNull())
        {
        } else if(action == NotifyCollectionChangedAction.Reset)
        {
            items.Clear();
        } else if(values.IsNullOrEmpty())
        {
        } else if(action == NotifyCollectionChangedAction.Add)
        {
            values.ForEach(items.Add);
        } else if(action == NotifyCollectionChangedAction.Remove)
        {
            values.ForEach(items.Remove);
        }

        return items;
    }
}

public static partial class IListExtensions
{
    public static TList ChangeFrom<TList, TSource>(this TList items, IEnumerable<TSource> values = default, NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add, int index = -1) where TList : IList<TSource>
    {
        if(items.CheckRange(index))
        {
            if(action == NotifyCollectionChangedAction.Add)
            {
                values?.ForEach(v => items.Insert(index++, v));
            } else if(action == NotifyCollectionChangedAction.Remove)
            {
                values?.ForEach(v => items.RemoveAt(index), whilePredicate: v => items.CheckRange(index));
            } else if(action == NotifyCollectionChangedAction.Replace)
            {
                values?.ForEach(v => items.Set(index++, v));
            } else
            {
                ICollectionExtensions.ChangeFrom(items, values);
            }
        } else
        {
            ICollectionExtensions.ChangeFrom(items, values);
        }

        return items;
    }

    public static void Set<TSource>(this IList<TSource> sources, int index, TSource value)
    {
        if(index < 0)
        {
            return;
        }

        if(sources.CheckRange(index) == false)
        {
            if(sources is List<TSource> list)
            {
                list.Capacity = index + 1;
            } else
            {
                for(var currentIndex = sources.Count - 1; currentIndex < index; currentIndex++)
                {
                    sources.Add(default);
                }
            }
        }

        sources[index] = value;
    }

    public static void RemoveRange<T>(this IList<T> items, int index, int count)
    {
        for(var i = 1; i <= count && index < items.Count; i++)
        {
            items.RemoveAt(index);
        }
    }
}

public static partial class IDictionaryExtensions
{
    public static TDictionary ChangeFrom<TDictionary, TKey, TValue>(this TDictionary items, IEnumerable<KeyValuePair<TKey, TValue>> values = default, NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add) where TDictionary : IDictionary<TKey, TValue>
    {
        if(action is NotifyCollectionChangedAction.Add or NotifyCollectionChangedAction.Replace)
        {
            values?.ForEach(v => items[v.Key] = v.Value, v => action == NotifyCollectionChangedAction.Replace || items.ContainsKey(v.Key) == false);
        } else
        {
            ICollectionExtensions.ChangeFrom(items, values, action);
        }

        return items;
    }

    public static bool TryGetValueNoException<TKey, TValue>(this IDictionary<TKey, TValue> keyValues, TKey key, out TValue result)
    {
        if(key.IsNull() == false)
        {
            try
            {
                return keyValues.TryGetValue(key, out result);
            } catch { }
        }

        return (result = default).Return(false);
    }

    public static ExpandoObject ToExpandoObject(this IDictionary<string, object> keyValues) => new ExpandoObject().ChangeFrom(keyValues);

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<TKey> keys, IEnumerable<TValue> values) => values.ToListOrEmpty().Do(values => keys.SelectIndexValue().ToDictionary(iv => iv.Value, iv => values.ElementAtOrDefault(iv.Key)));

    public static KeyValuePair<TKey, TValue> ToKeyValuePair<TKey, TValue>(this DictionaryEntry dictionaryEntry) => new(dictionaryEntry.Key.AsOrDefault<TKey>(), dictionaryEntry.Value.AsOrDefault<TValue>());
}