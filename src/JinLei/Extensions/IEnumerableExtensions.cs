using System.Collections;

namespace JinLei.Extensions;
public static partial class IEnumerableExtensions
{
    public static IEnumerable GetSelfOrEmpty(this IEnumerable items) => items.GetValueOrDefault(GetSelfOrEmpty<object>(default)).GetSelfOrEmpty();

    public static IEnumerable<TSource> GetSelfOrEmpty<TSource>(this IEnumerable<TSource> items) => items.GetValueOrDefault([]);

    public static TEnumerable GetSelfOrEmpty<TEnumerable>(this TEnumerable items) where TEnumerable : IEnumerable, new() => items.GetValueOrDefault([]);

    public static IEnumerable<object> ToObjects(this IEnumerable items) => items.GetSelfOrEmpty().Cast<object>();

    public static bool IsNullOrEmpty(this IEnumerable items) => items.ToObjects().Any() == false;

    public static IEnumerable<KeyValuePair<int, TSource>> SelectIndexValue<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Select((t, i) => new KeyValuePair<int, TSource>(i, t));

    public static int CountOrZero(this IEnumerable items) => items.GetSelfOrEmpty().Out(out var items2).Is<ICollection>(out var collection) ? collection.Count : items2.ToObjects().CountOrZero();

    public static int CountOrZero<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Count();

    public static bool CheckRange(this IEnumerable items, int index) => 0 <= index && index <= items.CountOrZero() - 1;

    public static TCollection ToTCollectionOrEmpty<TCollection, TSource>(this IEnumerable<TSource> items) where TCollection : ICollection<TSource>, new() => items is TCollection collection ? collection.GetSelfOrEmpty() : [.. items.GetSelfOrEmpty()];

    public static List<TSource> ToListOrEmpty<TSource>(this IEnumerable<TSource> items) => items.ToTCollectionOrEmpty<List<TSource>, TSource>();

    #region ICollection Functions
    public static void CopyTo<TSource>(this IEnumerable<TSource> items, TSource[] array, int arrayIndex) => items.ForEach(t => array[arrayIndex++] = t, t => array.CheckRange(arrayIndex));
    #endregion

    #region IList Functions
    public static int IndexOf<TSource>(this IEnumerable<TSource> items, TSource item) => items.ForEachIterator((t, i) => i, (t, i) => t.Equals(item)).Do(t => t.FirstOrDefault(), out var index).Any() ? index : -1;
    #endregion

    #region List Functions
    public static List<TSource> GetRange<TSource>(this IEnumerable<TSource> items, int index, int count) => items.GetSelfOrEmpty().Skip(index).Take(count).ToListOrEmpty();
    #endregion
}
