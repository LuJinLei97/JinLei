using System.Collections;
using System.Collections.Specialized;

namespace JinLei.Extensions;
public static partial class IEnumerableExtensions
{
    public static IEnumerable GetSelfOrEmpty(this IEnumerable items) => items.GetValueOrDefault(default(IEnumerable<object>).GetSelfOrEmpty());

    public static IEnumerable<TSource> GetSelfOrEmpty<TSource>(this IEnumerable<TSource> items) => items.GetValueOrDefault([]);

    public static TEnumerable GetSelfOrEmpty<TEnumerable>(this TEnumerable items) where TEnumerable : IEnumerable, new() => items.GetValueOrDefault([]);

    public static bool IsNullOrEmpty(this IEnumerable items) => items.GetSelfOrEmpty().GetEnumerator().MoveNext() == false;

    public static IEnumerable<KeyValuePair<int, object>> SelectIndexValue(this IEnumerable items) => items.GetSelfOrEmpty().OfType<object>().SelectIndexValue();

    public static IEnumerable<KeyValuePair<int, TSource>> SelectIndexValue<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Select((t, i) => new KeyValuePair<int, TSource>(i, t));

    public static int CountOrZero(this IEnumerable items) => items.GetSelfOrEmpty().Out(out var items2).Is<ICollection>(out var collection) ? collection.Count : items2.OfType<object>().CountOrZero();

    public static int CountOrZero<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Count();

    public static bool CheckRange(this IEnumerable items, int index) => index >= 0 && index < items.CountOrZero() - 1;

    public static TCollection ToTCollection<TCollection, TSource>(this IEnumerable<TSource> items) where TCollection : ICollection<TSource>, new() => new TCollection().Do(t => t.Change(items.GetSelfOrEmpty()));

    public static List<object> ToListOrEmpty(this IEnumerable items) => items.GetSelfOrEmpty().Out(out var items2).Is<ICollection>(out var collection) ? new List<object>(collection.Count).Do(t => t.AddRange(collection.OfType<object>())) : items2.ToListOrEmpty();

    public static List<TSource> ToListOrEmpty<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Out(out var items2).Is<ICollection<TSource>>(out var collection) ? new List<TSource>(collection) : new LinkedList<TSource>(items2).ToList();

    #region ICollection Functions
    public static void CopyTo(this IEnumerable items, object[] array, int arrayIndex) => items.GetSelfOrEmpty().OfType<object>().CopyTo(array, arrayIndex);

    public static void CopyTo<TSource>(this IEnumerable<TSource> items, TSource[] array, int arrayIndex) => items.ForEach(t => array[arrayIndex++] = t, whilePredicate: t => arrayIndex >= 0 && arrayIndex < array.Length - 1);
    #endregion

    #region IList Functions
    public static int IndexOf(this IEnumerable items, object item) => items.GetSelfOrEmpty().OfType<object>().IndexOf(item);

    public static int IndexOf<TSource>(this IEnumerable<TSource> items, TSource item) => items.SelectIndexValue().Where(t => t.Value.Equals(item)).Out(out var results).Any() ? results.First().Key : -1;
    #endregion

    #region List Functions
    public static List<TSource> GetRange<TSource>(this IEnumerable<TSource> items, int index, int count) => items.GetSelfOrEmpty().Skip(index).Take(count).ToListOrEmpty();

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static LinkedList<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, TResult> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default) => items.ForEachDoDelegate<TSource, TResult>(@delegate, whilePredicate);

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

    public static LinkedList<object> ForEachDoDelegate<TSource>(this IEnumerable<TSource> items, Delegate @delegate, Delegate wherePredicate = default, Delegate whilePredicate = default) => items.ForEachDoDelegate<TSource, object>(@delegate, wherePredicate, whilePredicate);

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static LinkedList<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, TResult> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default) => ForEachDoDelegate<TSource, TResult>(items, @delegate, whilePredicate);

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
    public static void Change<TSource>(this ICollection<TSource> items, IEnumerable<TSource> values = default, NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add, int maxCount = int.MaxValue)
    {
        if(items.IsNull() == false)
        {
            if(action == NotifyCollectionChangedAction.Add)
            {
                values?.ForEach((v, i) => items.Add(v), whilePredicate: (v, i) => i < maxCount);
            } else if(action == NotifyCollectionChangedAction.Remove)
            {
                values?.ForEach((v, i) => items.Remove(v), whilePredicate: (v, i) => i < maxCount);
            } else if(action == NotifyCollectionChangedAction.Reset)
            {
                items.Clear();
            }
        }
    }

    public static IEnumerable<LinkedListNode<T>> EnumerateLinkedListNodes<T>(this LinkedList<T> linkedList, bool isReverse = false)
    {
        if(linkedList.IsNullOrEmpty())
        {
            yield break;
        }

        for((var i, var node) = isReverse ? (linkedList.Count - 1, linkedList.Last) : (0, linkedList.First); linkedList.CheckRange(i); (i, node) = isReverse ? (i - 1, node.Previous) : (i + 1, node.Next))
        {
            yield return node;
        }
    }

    public static bool TryGetNode<T>(this LinkedList<T> linkedList, int index, out LinkedListNode<T> node)
    {
        node = default;

        if(linkedList.CheckRange(index))
        {
            var isFormFirst = index < linkedList.Count / 2;
            node = linkedList.EnumerateLinkedListNodes(!isFormFirst).SelectIndexValue().First(t => t.Key == (isFormFirst ? index : linkedList.Count - 1 - index)).Value;

            return true;
        }

        return false;
    }

    #region List Functions
    public static int RemoveAll<TSource>(this ICollection<TSource> items, Predicate<TSource> match)
    {
        var count1 = items?.Count;
        items?.Change(items?.Where(t => match(t)), NotifyCollectionChangedAction.Remove);
        return (items?.Count - count1).GetValueOrDefault();
    }
    #endregion
}

public static partial class IListExtensions
{
    public static void Change<TSource>(this IList<TSource> items, IEnumerable<TSource> values = default, NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add, int maxCount = int.MaxValue, int? startIndex = default)
    {
        if(items.IsNull() == false)
        {
            var index = startIndex.GetValueOrDefault(items.Count);
            if(action == NotifyCollectionChangedAction.Add)
            {
                if(items.CheckRange(index))
                {
                    values?.ForEach((v, i) => items.Insert(index++, v), whilePredicate: (v, i) => i < maxCount);
                } else
                {
                    values?.ForEach((v, i) => items.Set(index++, v), whilePredicate: (v, i) => i < maxCount);
                }
            } else if(action == NotifyCollectionChangedAction.Remove)
            {
                if(items.CheckRange(index))
                {
                    Enumerable.Range(1, maxCount).ForEach((v, i) => items.RemoveAt(index), whilePredicate: (v, i) => items.CheckRange(index));
                } else
                {
                    ICollectionExtensions.Change(items, values, action, maxCount);
                }
            } else if(action == NotifyCollectionChangedAction.Replace)
            {
                values?.ForEach((v, i) => items.Set(index++, v), whilePredicate: (v, i) => i < maxCount && items.CheckRange(index));
            } else
            {
                ICollectionExtensions.Change(items, values, action, maxCount);
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