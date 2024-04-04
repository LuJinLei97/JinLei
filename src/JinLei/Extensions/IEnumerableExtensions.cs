using System.Collections;
using System.Collections.Specialized;

namespace JinLei.Extensions;
public static partial class IEnumerableExtensions
{
    public static IEnumerable<TSource> GetSelfOrEmpty<TSource>(this IEnumerable<TSource> items) => items.GetValueOrDefault(Enumerable.Empty<TSource>());

    public static TEnumerable GetSelfOrEmpty<TEnumerable>(this TEnumerable items) where TEnumerable : IEnumerable, new() => items.GetValueOrDefault([]);

    public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Any() == false;

    public static IEnumerable<KeyValuePair<int, T>> SelectIndexValue<T>(this IEnumerable<T> items) => items.GetSelfOrEmpty().Select((t, i) => new KeyValuePair<int, T>(i, t));

    public static int CountOrZero<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Count();

    public static bool CheckRange<TSource>(this IEnumerable<TSource> items, int index) => 0 <= index && index <= items.CountOrZero() - 1;

    public static TCollection ToTCollection<TCollection, TSource>(this IEnumerable<TSource> items) where TCollection : ICollection<TSource>, new() => new TCollection().Do(t => t.Change(items.GetSelfOrEmpty()));

    public static List<TSource> ToListOrEmpty<TSource>(this IEnumerable<TSource> items) => new LinkedList<TSource>(items.GetSelfOrEmpty()).ToList();

    #region List Functions
    public static List<T> GetRange<T>(this IEnumerable<T> values, int index, int count) => values.GetSelfOrEmpty().Skip(index).Take(count).ToListOrEmpty();

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static IEnumerable<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, TResult> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default) => items.ForEachDoDelegate<TSource, TResult>(@delegate, whilePredicate);

    #region ForEach
    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static LinkedList<TResult> ForEachDoDelegate<TSource, TResult>(this IEnumerable<TSource> items, Delegate @delegate, Delegate wherePredicate = default, Delegate whilePredicate = default)
    {
        var result = new LinkedList<TResult>();

        if(@delegate.IsNull())
        {
            goto Result;
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
            null => (t, i) => true,
            Func<TSource, int, bool> d => d,
            Func<TSource, bool> d => d.AddParam(1),
            Func<bool> d => d.AddParam(default(TSource)).AddParam(1),
            _ => throw new NotImplementedException(),
        };

        var whilePredicateResult = whilePredicate switch
        {
            null => (t, i) => true,
            Func<TSource, int, bool> d => d,
            Func<TSource, bool> d => d.AddParam(1),
            Func<bool> d => d.AddParam(default(TSource)).AddParam(1),
            _ => throw new NotImplementedException(),
        };

        foreach(var item in items.SelectIndexValue())
        {
            if(whilePredicateResult(item.Value, item.Key) == false)
            {
                goto Result;
            }

            if(wherePredicateResult(item.Value, item.Key))
            {
                result.AddLast(itemFuncResult(item.Value, item.Key));
            }
        }

    Result:
        return result;
    }

    public static IEnumerable<object> ForEachDoDelegate<TSource>(this IEnumerable<TSource> items, Delegate @delegate, Delegate wherePredicate = default, Delegate whilePredicate = default) => items.ForEachDoDelegate<TSource, object>(@delegate, wherePredicate, whilePredicate);

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
    #endregion
}

public static partial class ICollectionExtensions
{
    public static void Change<TSource>(this ICollection<TSource> items, IEnumerable<TSource> values = default, NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add, int count = int.MaxValue)
    {
        if(items.IsNull() == false)
        {
            if(action == NotifyCollectionChangedAction.Add)
            {
                values?.ForEach((v, i) => items.Add(v), whilePredicate: (v, i) => i < count);
            } else if(action == NotifyCollectionChangedAction.Remove)
            {
                values?.ForEach((v, i) => items.Remove(v), whilePredicate: (v, i) => i < count);
            } else if(action == NotifyCollectionChangedAction.Reset)
            {
                items.Clear();
            }
        }
    }

    #region List Functions
    public static int RemoveAll<TSource>(this ICollection<TSource> items, Predicate<TSource> match)
    {
        var count1 = items.Count;
        items.Change(items.Where(t => match(t)), NotifyCollectionChangedAction.Remove);
        return items.Count - count1;
    }
    #endregion
}

public static partial class IListExtensions
{
    public static void Change<TSource>(this IList<TSource> items, IEnumerable<TSource> values = default, NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add, int count = int.MaxValue, int? startIndex = default)
    {
        if(items.IsNull() == false)
        {
            var index = startIndex.GetValueOrDefault(items.Count);
            if(action == NotifyCollectionChangedAction.Add)
            {
                if(items.CheckRange(index))
                {
                    values?.ForEach((v, i) => items.Insert(index++, v), whilePredicate: (v, i) => i < count);
                } else
                {
                    values?.ForEach((v, i) => items.Set(index++, v), whilePredicate: (v, i) => i < count);
                }
            } else if(action == NotifyCollectionChangedAction.Remove)
            {
                if(items.CheckRange(index))
                {
                    Enumerable.Range(index, count).ForEach(items.RemoveAt, whilePredicate: items.CheckRange);
                } else
                {
                    ICollectionExtensions.Change(items, values, action, count);
                }
            } else if(action == NotifyCollectionChangedAction.Replace)
            {
                values?.ForEach((v, i) => items.Set(index++, v), whilePredicate: (v, i) => i < count && items.CheckRange(index));
            } else
            {
                ICollectionExtensions.Change(items, values, action, count);
            }
        }
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
                list.Capacity = index + 8;
            } else
            {
                Enumerable.Range(sources.Count, index - sources.Count + 8).ForEach(t => sources.Add(default));
            }
        }

        sources[index] = value;
    }
}

public static partial class IDictionaryExtensions
{
    public static void Change<TKey, TValue>(this IDictionary<TKey, TValue> items, IEnumerable<KeyValuePair<TKey, TValue>> values = default, NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add, int count = int.MaxValue)
    {
        if(action is NotifyCollectionChangedAction.Add or NotifyCollectionChangedAction.Replace)
        {
            values?.ForEach((v, i) => items[v.Key] = v.Value, (v, i) => action == NotifyCollectionChangedAction.Replace || items.ContainsKey(v.Key) == false, (v, i) => i < count);
        } else
        {
            ICollectionExtensions.Change(items, values, action, count);
        }
    }

    public static bool TryGetValueNonException<TKey, TValue>(this IDictionary<TKey, TValue> keyValues, TKey key, out TValue result) => (key.IsNull() == false).Do(() => default, out result) && keyValues.TryGetValue(key, out result);

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<TKey> keys, IEnumerable<TValue> values)
    {
        var valuesEnumerator = values.GetSelfOrEmpty().GetEnumerator();
        return keys.GetSelfOrEmpty().ToDictionary(t => t, t => valuesEnumerator.MoveNext() ? valuesEnumerator.Current : default);
    }

    public static KeyValuePair<TKey, TValue> ToKeyValuePair<TKey, TValue>(this DictionaryEntry dictionaryEntry) => new(dictionaryEntry.Key.AsOrDefault<TKey>(), dictionaryEntry.Value.AsOrDefault<TValue>());
}