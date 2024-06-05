using System.Collections.Specialized;

namespace JinLei.Extensions;

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
