using System.Collections;

using JinLei.Extensions;

namespace JinLei.Classes;
public class LinkedListJL<T> : TreeNode<LinkedListJL<T>>, IList<T>
{
    public virtual LinkedList<T> Values { get; set; } = new();

    public virtual int BucketCapacity { get => bucketCapacity = Math.Max(1, bucketCapacity); set => bucketCapacity = Math.Max(1, value); }
    protected int bucketCapacity = 64;

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

    public virtual bool TryInsertChild(int index, LinkedListJL<T> treeNode)
    {
        if((Childs ??= []).CheckRange(index))
        {
            if(Childs.Count < BucketCapacity)
            {
                Childs.Insert(index, treeNode);
            } else
            {
                return Childs[index].TryInsertChild(0, treeNode);
            }
        } else if(index == Childs.Count)
        {
            if(Childs.Count < BucketCapacity)
            {
                Childs.Add(treeNode);
            } else
            {
                return Childs.Last().TryInsertChild(Childs.Last()?.Childs.Count ?? 0, treeNode);
            }
        } else
        {
            return false;
        }

        return true;
    }

    public virtual bool TryInsert(int index, T item)
    {
        var isInsert = TryGetLinkedListNode(index, out var node, out var treeNode);

        if(isInsert == false && index != Count)
        {
            if(index != Count)
            {
                return false;

            } else
            {
                treeNode = this;
            }
        }

        if(treeNode.Values.Count < BucketCapacity)
        {
            if(isInsert)
            {
                treeNode.Values.AddBefore(node, item);
            } else
            {
                treeNode.Values.AddLast(item);
            }
        } else
        {
            (var treeNode1, var values1) = (default(LinkedListJL<T>), default(LinkedList<T>));

            if(isInsert)
            {
                values1 = new LinkedList<T>().AddLast(treeNode.Values.Last.Value).List;
                treeNode1 = new() { Values = treeNode.Values.AddBefore(node, item).List.Do(t => t.RemoveLast()) };
            } else
            {
                values1 = new LinkedList<T>().AddLast(item).List;
                treeNode1 = new() { Values = treeNode.Values };
            }

            if(Childs.Count < BucketCapacity)
            {
                Childs.Add(treeNode);
                treeNode.Values = values1;
            } else
            {
                Childs.Last().TryInsertChild(Childs.Last()?.Childs.Count ?? 0, treeNode);
                treeNode.Values = values1;
            }
        }

        return true;
    }

    #region IList<T>
    public virtual T this[int index] { get { TryGetLinkedListNode(index, out var node, out var treeNode); return node.Value; } set { TryGetLinkedListNode(index, out var node, out var treeNode); node.Value = value; } }

    public virtual int Count => (Childs?.Count ?? 0) + Values.Count;

    public virtual bool IsReadOnly => false;

    public virtual void Add(T item) => Insert(Count, item);

    public virtual void Clear()
    {
        Childs.Clear();
        Values.Clear();
    }

    public virtual bool Contains(T item) => Enumerable.Contains(this, item);

    public virtual void CopyTo(T[] array, int arrayIndex) => this.ToListOrEmpty().CopyTo(array, arrayIndex);

    public virtual IEnumerator<T> GetEnumerator()
    {
        foreach(var child in Childs.GetSelfOrEmpty())
        {
            foreach(var item in child)
            {
                yield return item;
            }
        }

        foreach(var item in Values)
        {
            yield return item;
        }
    }

    public virtual int IndexOf(T item) => this.ToListOrEmpty().IndexOf(item);

    public virtual void Insert(int index, T item) => TryInsert(index, item);

    public virtual bool Remove(T item)
    {
        var index = IndexOf(item);
        if(this.CheckRange(index))
        {
            RemoveAt(index);
            return true;
        }

        return false;
    }

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