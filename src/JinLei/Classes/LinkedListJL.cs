using System.Collections;

using JinLei.Extensions;

namespace JinLei.Classes;
public class LinkedListJL<T> : TreeNode<LinkedListJL<T>>, IList<T>
{
    public virtual LinkedList<T> Values { get; set; } = new();

    public virtual int BucketCapacity
    {
        get => bucketCapacity = Math.Max(1, bucketCapacity);
        set => bucketCapacity = Math.Max(1, value);
    }
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
                return Childs.Last().TryInsertChild(Childs.Last()?.Childs?.Count ?? 0, treeNode);
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
            if(isInsert)
            {
                treeNode.Values.AddBefore(node, item);
            } else
            {
                treeNode.Values.AddLast(item);
            }

            var treeNode1 = new LinkedListJL<T>()
            {
                BucketCapacity = BucketCapacity,
                Values = new LinkedList<T>().AddLast(treeNode.Values.First.Value).List
            };

            if((treeNode.Childs ??= []).Count < BucketCapacity)
            {
                treeNode.TryInsertChild(treeNode.Childs.CountOrZero(), treeNode1);
            } else
            {
                treeNode.Childs.Last().TryInsertChild(treeNode.Childs.Last().Childs.CountOrZero(), treeNode1);
            }

            treeNode.Values.RemoveFirst();
        }

        return true;
    }

    #region IList<T>
    public virtual T this[int index]
    {
        get => TryGetLinkedListNode(index, out var node, out var treeNode).Return(node.Value);
        set => TryGetLinkedListNode(index, out var node, out var treeNode).Do(t => node.Value = value);
    }

    public virtual int Count => Childs.GetSelfOrEmpty().Select(t => t.Count).Append(Values.Count).Sum();

    public virtual bool IsReadOnly => false;

    public virtual void Add(T item) => Insert(Count, item);

    public virtual void Clear()
    {
        Childs.Clear();
        Values.Clear();
    }

    public virtual bool Contains(T item) => Enumerable.Contains(this, item);

    public virtual void CopyTo(T[] array, int arrayIndex)
    {
        foreach(var item in this)
        {
            if(arrayIndex < 0 || arrayIndex >= array.Length)
            {
                break;
            }

            array[arrayIndex++] = item;
        }
    }

    public virtual IEnumerator<T> GetEnumerator()
    {
        foreach(var item in Childs.GetSelfOrEmpty().SelectMany(t => t).Concat(Values))
        {
            yield return item;
        }
    }

    public virtual int IndexOf(T item)
    {
        foreach(var iv in this.SelectIndexValue())
        {
            if(iv.Value.Equals(item))
            {
                return iv.Key;
            }
        }

        return -1;
    }

    public virtual void Insert(int index, T item) => TryInsert(index, item);

    public virtual bool Remove(T item)
    {
        if(this.CheckRange(IndexOf(item).Out(out var index)))
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