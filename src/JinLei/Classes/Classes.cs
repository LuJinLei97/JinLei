using System.Collections;

using JinLei.Extensions;

namespace JinLei.Classes;

public enum ProcessMode
{
    Undefined,
    SyncMode,
    AsyncMode
}

public enum DependencyPropertyType
{
    Default,
    Attached,
}

public enum ConditionType
{
    Where,
    While,
}

public partial interface IInitializable<TParma>
{
    void Initialize(TParma parma = default);
}

internal class LinkedListTree<T> : TreeNode<LinkedListTree<T>>, IList<T>
{
    public virtual LinkedList<T> Values { get; set; } = new();

    public virtual int BucketCapacity
    {
        get => bucketCapacity;
        set => bucketCapacity = Math.Max(1, value);
    }
    protected int bucketCapacity = 64;

    public virtual IEnumerable<LinkedList<T>> EnumerateLinkedLists() => Childs.GetSelfOrEmpty().SelectMany(t => (t?.EnumerateLinkedLists()).GetSelfOrEmpty()).Append(Values);

    public virtual IEnumerable<LinkedListNode<T>> EnumerateLinkedListNodes() => EnumerateLinkedLists().SelectMany(t => t.EnumerateLinkedListNodes());

    public virtual bool TryGetLinkedListNode(int index, out LinkedListNode<T> linkedListNode, out LinkedListTree<T> treeNode)
    {
        (linkedListNode, treeNode) = (default, default);

        if(index < 0)
        {
            return false;
        }

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

    public virtual bool TryInsert(int index, LinkedListTree<T> treeNode)
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
            var count1 = items.ForEachDo(t => node = isInsert ? treeNode.Values.AddBefore(node, t).Next : treeNode.Values.AddLast(t), whilePredicate: t => treeNode.Values.Count < BucketCapacity).Count;
            if((items = items.Skip(count1).ToArray()).Length != 0)
            {
                var treeNode1 = new LinkedListTree<T>()
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
        get => TryGetLinkedListNode(index, out var node, out var treeNode) ? node.Value : default;
        set => _ = TryGetLinkedListNode(index, out var node, out var treeNode) ? node.Value = value : default;
    }

    public virtual int Count => EnumerateLinkedLists().Sum(t => t.Count);

    public virtual bool IsReadOnly => false;

    public virtual void Add(T item) => Insert(Count, item);

    public virtual void Clear()
    {
        Childs.Clear();
        Values.Clear();
    }

    public virtual bool Contains(T item) => EnumerateLinkedLists().Any(t => t.Contains(item));

    public virtual void CopyTo(T[] array, int arrayIndex) => EnumerateLinkedLists().ForEachDo(t => { t.CopyTo(array, arrayIndex); arrayIndex += t.Count; });

    public virtual IEnumerator<T> GetEnumerator() => EnumerateLinkedListNodes().Select(t => t.Value).GetEnumerator();

    public virtual int IndexOf(T item) => EnumerateLinkedListNodes().ForEach((t, i) => i, (t, i) => item.Equals(t.Value)).FirstOrDefault(-1);

    public virtual void Insert(int index, T item) => TryInsert(index, item);

    public virtual bool Remove(T item) => EnumerateLinkedLists().FirstOrDefault(t => t.Remove(item)).IsNull();

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

internal class TreeList<T> : TreeNode<TreeList<T>>, IList<T>
{
    public virtual List<T> Values { get; set; } = [];

    public virtual int BucketCapacity
    {
        get => bucketCapacity;
        set => bucketCapacity = Math.Max(1, value);
    }
    protected int bucketCapacity = 64;

    public virtual IEnumerable<List<T>> EnumerateLists() => Childs.GetSelfOrEmpty().SelectMany(t => (t?.EnumerateLists()).GetSelfOrEmpty()).Append(Values);

    public virtual bool TryFindList(int index, out List<T> list, out int offset)
    {
        (list, offset) = (default, -1);

        if(index < 0)
        {
            return false;
        }

        var startIndex = 0;
        foreach(var values in EnumerateLists().Where(t => t.IsNullOrEmpty() == false))
        {
            offset = index - startIndex;
            if(values.CheckRange(offset))
            {
                list = values;
                return true;
            }

            startIndex += values.Count;
        }

        return false;
    }

    #region IList<T>
    public T this[int index]
    {
        get => TryFindList(index, out var list, out var offset) ? list[offset] : default;
        set => _ = TryFindList(index, out var list, out var offset) ? list[offset] = value : default;
    }

    public int Count => EnumerateLists().Sum(t => t.Count);

    public bool IsReadOnly => false;

    public void Add(T item) => Values.Add(item);

    public void Clear()
    {
        Childs = [];
        Values = [];
    }

    public bool Contains(T item) => EnumerateLists().Any(t => t.Contains(item));

    public void CopyTo(T[] array, int arrayIndex) => EnumerateLists().ForEachDo(t => { t.CopyTo(array, arrayIndex); arrayIndex += t.Count; });

    public int IndexOf(T item)
    {
        var startIndex = 0;
        foreach(var values in EnumerateLists().Where(t => t.IsNullOrEmpty() == false))
        {
            if(values.IndexOf(item).Out(out var result) != -1)
            {
                return startIndex + result;
            } else
            {
                startIndex += values.Count;
            }
        }

        return -1;
    }

    public void Insert(int index, T item)
    {
        if(TryFindList(index, out var list, out var offset) && list.Count < BucketCapacity)
        {
            list.Insert(offset, item);
        } else
        {

        }
    }

    public bool Remove(T item) => EnumerateLists().Any(t => t.Remove(item));

    public void RemoveAt(int index)
    {
        if(TryFindList(index, out var list, out var offset))
        {
            list.RemoveAt(offset);
        }
    }

    public IEnumerator<T> GetEnumerator() => EnumerateLists().SelectMany(t => t).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}