namespace JinLei.Extensions;

public static partial class ForEachExtensions
{
    public static IEnumerable<TResult> ForEachIterator<TSource, TResult>(this IEnumerable<TSource> items, Delegate @delegate, Delegate wherePredicate = default, Delegate whilePredicate = default, Delegate skipPredicate = default)
    {
        if(@delegate.IsNull())
        {
            goto Break;
        }

        var itemFuncResult = @delegate switch
        {
            Func<TSource, int, TResult> d => d,
            Func<TSource, TResult> d => d.AddParam(0),
            Action<TSource, int> d => d.ToFunc(default(TResult)),
            Action<TSource> d => d.AddParam(0).ToFunc(default(TResult)),
            _ => throw new NotImplementedException(),
        };

        var wherePredicateResult = wherePredicate switch
        {
            null => (t, i) => true,
            Func<TSource, int, bool> d => d,
            Func<TSource, bool> d => d.AddParam(0),
            Func<bool> d => d.AddParam(default(TSource)).AddParam(0),
            _ => throw new NotImplementedException(),
        };

        var whilePredicateResult = whilePredicate switch
        {
            null => (t, i) => true,
            Func<TSource, int, bool> d => d,
            Func<TSource, bool> d => d.AddParam(0),
            Func<bool> d => d.AddParam(default(TSource)).AddParam(0),
            _ => throw new NotImplementedException(),
        };

        var skipPredicateResult = skipPredicate switch
        {
            null => (t, i) => false,
            Func<TSource, int, bool> d => d,
            Func<TSource, bool> d => d.AddParam(0),
            Func<bool> d => d.AddParam(default(TSource)).AddParam(0),
            _ => throw new NotImplementedException(),
        };

        foreach(var item in items.SelectIndexValue())
        {
            if(whilePredicateResult(item.Value, item.Key) == false)
            {
                goto Break;
            }

            if(skipPredicateResult(item.Value, item.Key))
            {
                continue;
            }

            if(wherePredicateResult(item.Value, item.Key))
            {
                yield return itemFuncResult(item.Value, item.Key);
            }
        }

    Break:
        yield break;
    }

    public static IEnumerable<TResult> ForEachIterator<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, TResult> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default, Func<TSource, int, bool> skipPredicate = default) => items.ForEachIterator<TSource, TResult>(@delegate as Delegate, wherePredicate, whilePredicate, skipPredicate);

    public static IEnumerable<TResult> ForEachIterator<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, TResult> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default, Func<TSource, bool> skipPredicate = default) => items.ForEachIterator<TSource, TResult>(@delegate as Delegate, wherePredicate, whilePredicate, skipPredicate);

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static LinkedList<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, int, TResult> @delegate, Func<TSource, int, bool> wherePredicate = default, Func<TSource, int, bool> whilePredicate = default, Func<TSource, int, bool> skipPredicate = default) => items.ForEachIterator(@delegate, wherePredicate, whilePredicate, skipPredicate).ToTCollectionOrEmpty<LinkedList<TResult>, TResult>();

    /// <summary>
    /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
    /// </summary>
    public static LinkedList<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, TResult> @delegate, Func<TSource, bool> wherePredicate = default, Func<TSource, bool> whilePredicate = default, Func<TSource, bool> skipPredicate = default) => items.ForEachIterator(@delegate, wherePredicate, whilePredicate, skipPredicate).ToTCollectionOrEmpty<LinkedList<TResult>, TResult>();
}