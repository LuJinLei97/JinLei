namespace JinLei.Extensions;
public static partial class MethodChainExtensions
{
    public static TResult Return<TResult>(this object source, TResult result) => result;

    public static TSource Out<TSource>(this TSource source, out TSource result) => result = source;

    public static bool Is<TResult>(this object source, out TResult result) => (source is TResult value ? (Is: true, Value: value) : (false, default)).Do(t => t.Value, out result).Is;

    public static TSource Do<TSource, TResult>(this TSource source, Func<TSource, TResult> @delegate, out TResult result, Func<TSource, bool> condition = default) => new[] { source }.ForEach(@delegate, condition).FirstOrDefault().Out(out result).Return(source);

    #region Do
    public static TResult Do<TSource, TResult>(this TSource source, Func<TSource, TResult> @delegate, Func<TSource, bool> condition = default) => source.Do(@delegate, out var result, condition).Return(result);

    public static TSource Do<TSource>(this TSource source, Action<TSource> @delegate, Func<TSource, bool> condition = default) => source.Do(@delegate.ToFunc(0), out var result, condition);
    #endregion

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static IEnumerable<TSource> ForEach<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, TResult> @delegate, out LinkedList<TResult> results, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default, Func<TSource, int, bool> skipPredicate = default) => items.ForEach(@delegate, wherePredicate, whilePredicate, skipPredicate).Out(out results).Return(items);

    #region ForEach
    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static IEnumerable<TSource> ForEach<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, TResult> @delegate, out LinkedList<TResult> results, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default, Func<TSource, bool> skipPredicate = default) => items.ForEach(@delegate, wherePredicate, whilePredicate, skipPredicate).Out(out results).Return(items);

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> items, Action<TSource, int> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default, Func<TSource, int, bool> skipPredicate = default) => items.ForEach(@delegate.ToFunc(0), out var results, wherePredicate, whilePredicate, skipPredicate);

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> items, Action<TSource> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default, Func<TSource, bool> skipPredicate = default) => items.ForEach(@delegate.ToFunc(0), out var results, wherePredicate, whilePredicate, skipPredicate);
    #endregion

}