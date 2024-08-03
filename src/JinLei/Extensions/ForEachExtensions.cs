//#define DebugForEachIterator

using JinLei.Classes;

namespace JinLei.Extensions;

public static partial class ForEachExtensions
{
    public static IEnumerable<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, TResult> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default)
    {
        if(@delegate.IsNull())
        {
            goto End;
        }

        wherePredicate ??= (t, i) => true;
        whilePredicate ??= (t, i) => true;

        foreach(var iv in items.SelectIndexValue())
        {
            if(whilePredicate(iv.Value, iv.Key) == false)
            {
                goto End;
            }

            if(wherePredicate(iv.Value, iv.Key))
            {
                yield return @delegate(iv.Value, iv.Key);
            }
        }

    End:
        yield break;
    }

    public static IEnumerable<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, TResult> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default) => items.ForEach(@delegate.AddParam(0), wherePredicate.AddParam(0), whilePredicate.AddParam(0));

    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    public static LinkedList<TResult> ForEachDo<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, TResult> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default) => items.ForEach(@delegate, wherePredicate, whilePredicate).ToLinkedList();

    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    public static LinkedList<TResult> ForEachDo<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, TResult> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default) => items.ForEach(@delegate, wherePredicate, whilePredicate).ToLinkedList();

    #region ForEachThenByKey
    public static IEnumerable<TResult> ForEachThenByKey<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, KeyValuePair<bool, TResult>> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default, ConditionType conditionType = ConditionType.Where) => items.ForEach(@delegate, wherePredicate, whilePredicate).ForEach(t => t.Value, conditionType == ConditionType.Where ? t => t.Key : default, conditionType == ConditionType.While ? t => t.Key : default);

    public static IEnumerable<TResult> ForEachThenByKey<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, KeyValuePair<bool, TResult>> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default, ConditionType conditionType = ConditionType.Where) => items.ForEachThenByKey(@delegate.AddParam(0), wherePredicate.AddParam(0), whilePredicate.AddParam(0), conditionType);

    public static LinkedList<TResult> ForEachThenByKeyDo<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, KeyValuePair<bool, TResult>> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default, ConditionType conditionType = ConditionType.Where) => items.ForEachThenByKey(@delegate, wherePredicate, whilePredicate, conditionType).ToLinkedList();

    public static LinkedList<TResult> ForEachThenByKeyDo<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, KeyValuePair<bool, TResult>> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default, ConditionType conditionType = ConditionType.Where) => items.ForEachThenByKey(@delegate, wherePredicate, whilePredicate, conditionType).ToLinkedList();
    #endregion
}