using System.Collections.Specialized;

namespace JinLei.Extensions;

public static partial class ICollectionExtensions
{
    public static void Change<TSource>(this ICollection<TSource> items, IEnumerable<TSource> values = default, NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add, int maxCount = int.MaxValue / 2)
    {
        if(items.IsNull() == false)
        {
            if(action == NotifyCollectionChangedAction.Add)
            {
                values?.ForEachDo((v, i) => items.Add(v), whilePredicate: (v, i) => i < maxCount);
            } else if(action == NotifyCollectionChangedAction.Remove)
            {
                values?.ForEachDo((v, i) => items.Remove(v), whilePredicate: (v, i) => i < maxCount);
            } else if(action == NotifyCollectionChangedAction.Reset)
            {
                items.Clear();
            }
        }
    }

    public static TCollection ToTCollection<TCollection, TSource>(this IEnumerable<TSource> items) where TCollection : ICollection<TSource>, new() => items.Is<TCollection>(out var collection) ? collection : [.. items.GetSelfOrEmpty()];

    public static LinkedList<TSource> ToLinkedList<TSource>(this IEnumerable<TSource> items) => items.ToTCollection<LinkedList<TSource>, TSource>();

    public static IEnumerable<LinkedListNode<T>> EnumerateLinkedListNodes<T>(this LinkedList<T> linkedList, bool isReverse = false)
    {
        if(linkedList.IsNullOrEmpty())
        {
            yield break;
        }

        for((var i, var node) = (0, isReverse ? linkedList.Last : linkedList.First); linkedList.CheckRange(i); (i, node) = (i + 1, isReverse ? node.Previous : node.Next))
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
}
