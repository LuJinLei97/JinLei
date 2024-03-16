namespace JinLei.Extensions;

public static partial class ForEachExtensions
{

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static LinkedList<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, TResult> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default) => items.ForEachIterator(@delegate, wherePredicate, whilePredicate).ToTCollectionOrEmpty<LinkedList<TResult>, TResult>();

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static LinkedList<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, TResult> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default) => items.ForEachIterator(@delegate, wherePredicate, whilePredicate).ToTCollectionOrEmpty<LinkedList<TResult>, TResult>();

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static void ForEach<TSource>(this IEnumerable<TSource> items, Action<TSource, int> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default) => items.ForEachIterator<TSource, object>(@delegate, wherePredicate, whilePredicate).ToTCollectionOrEmpty<LinkedList<object>, object>();

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static void ForEach<TSource>(this IEnumerable<TSource> items, Action<TSource> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default) => items.ForEachIterator<TSource, object>(@delegate, wherePredicate, whilePredicate).ToTCollectionOrEmpty<LinkedList<object>, object>();

    #region ForEachIterator
    public static IEnumerable<TResult> ForEachIterator<TSource, TResult>(this IEnumerable<TSource> items, Delegate @delegate, Delegate wherePredicate = default, Delegate whilePredicate = default)
    {
        if(@delegate.IsNull())
        {
            goto Break;
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
                goto Break;
            }

            if(wherePredicateResult(item.Value, item.Key))
            {
                yield return itemFuncResult(item.Value, item.Key);
            }
        }

    Break:
        yield break;
    }

    public static IEnumerable<TResult> ForEachIterator<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, TResult> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default) => items.ForEachIterator<TSource, TResult>(@delegate as Delegate, wherePredicate, whilePredicate);

    public static IEnumerable<TResult> ForEachIterator<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, TResult> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default) => items.ForEachIterator<TSource, TResult>(@delegate as Delegate, wherePredicate, whilePredicate);
    #endregion
}