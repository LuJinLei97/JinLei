using System.Collections;

namespace JinLei.Extensions;

public static partial class IEnumerableExtensions
{
    public static IEnumerable<TSource> GetSelfOrEmpty<TSource>(this IEnumerable<TSource> items) => items.GetValueOrDefault([]);

    public static TEnumerable GetSelfOrEmpty<TEnumerable>(this TEnumerable items) where TEnumerable : IEnumerable, new() => items.GetValueOrDefault([]);

    public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Any() == false;

#if NETFRAMEWORK
    public static bool TryGetNonEnumeratedCount<TSource>(this IEnumerable<TSource> items, out int count) => (items.Is<ICollection<TSource>>(out var collection) ? (true, collection.Count) : items.Is<ICollection>(out var collection1) ? (true, collection1.Count) : (default, default)).Do(t => t.Count, out count).Item1;
#endif

    public static int CountOrZero<TSource>(this IEnumerable<TSource> items, Func<TSource, bool> predicate = default) => items.GetSelfOrEmpty().Do(t => predicate.IsNull() ? t.Count() : t.Count(predicate));

    public static bool CheckRange<TSource>(this IEnumerable<TSource> items, int index) => 0 <= index && items.GetSelfOrEmpty().TryGetNonEnumeratedCount(out var count) ? index <= count - 1 : items.SelectIndexValue().Any(t => t.Key == index);

    public static IEnumerable<KeyValuePair<int, TSource>> SelectIndexValue<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Select((t, i) => KeyValuePair.Create(i, t));

    public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> items, TSource defaultValue = default, Func<TSource, bool> predicate = default) => items.WhereOrDefault(predicate.IsNull() ? (t, i) => true : predicate.AddParam(0), [defaultValue]).First();

    public static IEnumerable<TSource> WhereOrDefault<TSource>(this IEnumerable<TSource> items, Func<TSource, int, bool> predicate, IEnumerable<TSource> defaultValue = default) => items.GetSelfOrEmpty().Where(predicate).Do(t => t.IsNullOrEmpty() ? defaultValue : t);
}