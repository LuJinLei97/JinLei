﻿using System.Collections.Specialized;

namespace JinLei.Extensions;

public static partial class IListExtensions
{
    public static void Change<TSource>(this IList<TSource> items, IEnumerable<TSource> values = default, NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add, int maxCount = int.MaxValue / 2, int? startIndex = default)
    {
        if(items.IsNull() == false)
        {
            var index = startIndex.GetValueOrDefault(items.Count);
            if(action == NotifyCollectionChangedAction.Add)
            {
                if(items.CheckRange(index))
                {
                    values?.ForEachDo((v, i) => items.Insert(index++, v), whilePredicate: (v, i) => i < maxCount);
                } else
                {
                    values?.ForEachDo((v, i) => items.Set(index++, v), whilePredicate: (v, i) => i < maxCount);
                }
            } else if(action == NotifyCollectionChangedAction.Remove)
            {
                if(items.CheckRange(index))
                {
                    Enumerable.Range(1, maxCount).ForEachDo((v, i) => items.RemoveAt(index), whilePredicate: (v, i) => items.CheckRange(index));
                } else
                {
                    (items as ICollection<TSource>).Change(values, action, maxCount);
                }
            } else if(action == NotifyCollectionChangedAction.Replace)
            {
                values?.ForEachDo((v, i) => items.Set(index++, v), whilePredicate: (v, i) => i < maxCount && items.CheckRange(index));
            } else
            {
                (items as ICollection<TSource>).Change(values, action, maxCount);
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
                Enumerable.Range(sources.Count, index - sources.Count + 8).ForEachDo(t => sources.Add(default));
            }
        }

        sources[index] = value;
    }
}
