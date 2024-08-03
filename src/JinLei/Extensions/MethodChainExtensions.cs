using JinLei.Classes;

namespace JinLei.Extensions;

public static partial class MethodChainExtensions
{
    public static TResult Return<TSource, TResult>(this TSource source, TResult result) => result;

    public static TSource Out<TSource>(this TSource source, out TSource result) => result = source;

    public static TSource Do<TSource>(this TSource source, Action @delegate, Func<bool> condition = default) => source.Do(@delegate.ToFunc(), out _, condition);

    #region Do
    public static TSource Do<TSource>(this TSource source, Action<TSource> @delegate, Func<TSource, bool> condition = default) => source.Do(@delegate.ToFunc(), out _, condition);

    public static TSource Do<TSource, TResult>(this TSource source, Func<TSource, TResult> @delegate, out TResult result, Func<TSource, bool> condition = default) => source.Do(@delegate, condition).Out(out result).Return(source);

    public static TSource Do<TSource, TResult>(this TSource source, Func<TResult> @delegate, out TResult result, Func<bool> condition = default) => source.Do(@delegate, condition).Out(out result).Return(source);

    public static TResult Do<TSource, TResult>(this TSource source, Func<TSource, TResult> @delegate, Func<TSource, bool> condition = default) => @delegate.IsNull() == false && (condition.IsNull() || condition.Invoke(source)) ? @delegate(source) : default;

    public static TResult Do<TSource, TResult>(this TSource source, Func<TResult> @delegate, Func<bool> condition = default) => source.Do(@delegate.AddParam(default(TSource)), condition.AddParam(default(TSource)));
    #endregion

    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    public static IEnumerable<TSource> ForEachDo<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, TResult> @delegate, out LinkedList<TResult> results, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default) => items.Do(t => t.ForEachDo(@delegate, wherePredicate, whilePredicate), out results);

    #region ForEach
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    public static IEnumerable<TSource> ForEachDo<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, TResult> @delegate, out LinkedList<TResult> results, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default) => items.Do(t => t.ForEachDo(@delegate, wherePredicate, whilePredicate), out results);

    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    public static IEnumerable<TSource> ForEachDo<TSource>(this IEnumerable<TSource> items, Action<TSource, int> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default) => items.ForEachDo(@delegate.ToFunc(), out _, wherePredicate, whilePredicate);

    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    public static IEnumerable<TSource> ForEachDo<TSource>(this IEnumerable<TSource> items, Action<TSource> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default) => items.ForEachDo(@delegate.ToFunc(), out _, wherePredicate, whilePredicate);
    #endregion

    #region ForEachThenByKey
    public static IEnumerable<TSource> ForEachThenByKeyDo<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, KeyValuePair<bool, TResult>> @delegate, out LinkedList<TResult> results, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default, ConditionType conditionType = ConditionType.Where) => items.Do(t => t.ForEachThenByKeyDo(@delegate, wherePredicate, whilePredicate, conditionType), out results);

    public static IEnumerable<TSource> ForEachThenByKeyDo<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, KeyValuePair<bool, TResult>> @delegate, out LinkedList<TResult> results, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default, ConditionType conditionType = ConditionType.Where) => items.Do(t => t.ForEachThenByKeyDo(@delegate, wherePredicate, whilePredicate, conditionType), out results);
    #endregion
}