using System.Collections;

using JinLei.Extensions;

namespace JinLei.Classes;
internal class LinkedListJL<T> : TreeNode<LinkedListJL<T>>, IList<T>
{
    public virtual LinkedList<T> Values { get; set; } = new();

    public virtual int BucketCapacity
    {
        get => bucketCapacity = Math.Max(1, bucketCapacity);
        set => bucketCapacity = Math.Max(1, value);
    }
    protected int bucketCapacity = 64;

    public virtual IEnumerable<LinkedList<T>> EnumerateLinkedLists() => Childs.GetSelfOrEmpty().SelectMany(t => (t?.EnumerateLinkedLists()).GetSelfOrEmpty()).Append(Values);

    public virtual IEnumerable<LinkedListNode<T>> EnumerateLinkedListNodes() => EnumerateLinkedLists().SelectMany(t => (t?.EnumerateLinkedListNodes()).GetSelfOrEmpty());

    public virtual bool TryGetLinkedListNode(int index, out LinkedListNode<T> linkedListNode, out LinkedListJL<T> treeNode)
    {
        (linkedListNode, treeNode) = (default, default);

        foreach(var child in Childs.GetSelfOrEmpty())
        {
            if(child.TryGetLinkedListNode(index, out linkedListNode, out treeNode))
            {
                return true;
            } else
            {
                index -= child.Count;
            }
        }

        if(Values.TryGetNode(index, out linkedListNode))
        {
            treeNode = this;
            return true;
        }

        return false;
    }

    public virtual bool TryInsert(int index, LinkedListJL<T> treeNode)
    {
        var isInsert = TryGetLinkedListNode(index, out var node, out var treeNode1);

        if(isInsert == false)
        {
            if(index == Count)
            {
                treeNode1 = this;
            } else
            {
                return false;
            }
        }

        if((Childs ??= []).CheckRange(index))
        {
            if(Childs.Count < BucketCapacity)
            {
                Childs.Insert(index, treeNode);
            } else
            {
                return Childs[index].TryInsert(0, treeNode);
            }
        } else if(index == Childs.Count)
        {
            if(Childs.Count < BucketCapacity)
            {
                Childs.Add(treeNode);
            } else
            {
                return Childs.Last().TryInsert(Childs.Last()?.Childs?.Count ?? 0, treeNode);
            }
        } else
        {
            return false;
        }

        return true;
    }

    public virtual bool TryInsert(int index, params T[] items)
    {
        if(items.IsNullOrEmpty())
        {
            return true;
        }

        var isInsert = TryGetLinkedListNode(index, out var node, out var treeNode);

        if(isInsert == false)
        {
            if(index == Count)
            {
                treeNode = this;
            } else
            {
                return false;
            }
        }

        while(true)
        {
            var count1 = items.ForEach(t => node = isInsert ? treeNode.Values.AddBefore(node, t).Next : treeNode.Values.AddLast(t), whilePredicate: t => treeNode.Values.Count < BucketCapacity).Count;
            if((items = items.Skip(count1).ToArray()).Length != 0)
            {
                var treeNode1 = new LinkedListJL<T>()
                {
                    BucketCapacity = treeNode.BucketCapacity,
                    Values = treeNode.Values
                };

            } else
            {
                break;
            }
        }

        return true;
    }

    #region IList<T>
    public virtual T this[int index]
    {
        get => TryGetLinkedListNode(index, out var node, out var treeNode).Return(node.Value);
        set => TryGetLinkedListNode(index, out var node, out var treeNode).Do(t => node.Value = value);
    }

    public virtual int Count => EnumerateLinkedLists().Sum(t => t.CountOrZero());

    public virtual bool IsReadOnly => false;

    public virtual void Add(T item) => Insert(Count, item);

    public virtual void Clear()
    {
        Childs.Clear();
        Values.Clear();
    }

    public virtual bool Contains(T item) => Enumerable.Contains(this, item);

    public virtual void CopyTo(T[] array, int arrayIndex) => IEnumerableExtensions.CopyTo(this, array, arrayIndex);

    public virtual IEnumerator<T> GetEnumerator() => EnumerateLinkedListNodes().Select(t => t.Value).GetEnumerator();

    public virtual int IndexOf(T item) => IEnumerableExtensions.IndexOf(this, item);

    public virtual void Insert(int index, T item) => TryInsert(index, item);

    public virtual bool Remove(T item) => EnumerateLinkedListNodes().FirstOrDefault(t => t.Value.Equals(item)).Out(out var node).Do(t => node.List.Remove(node), t => node.IsNull() == false).Return(node.IsNull() == false);

    public virtual void RemoveAt(int index)
    {
        if(TryGetLinkedListNode(index, out var node, out var treeNode))
        {
            node.List.Remove(node);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}