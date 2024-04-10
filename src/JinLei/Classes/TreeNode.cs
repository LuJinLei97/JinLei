using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using JinLei.Extensions;

using Newtonsoft.Json;

namespace JinLei.Classes;
public class TreeNode<TNode> where TNode : TreeNode<TNode>
{
    [JsonIgnore]
    public virtual TNode Parent
    {
        get => parent;
        set
        {
            if(parent == value)
            {
                return;
            }

            if(this is TNode node)
            {
                parent?.Childs?.Remove(node);

                if((parent = value)?.Childs?.Contains(node) == false)
                {
                    parent?.Childs?.Add(node);
                }
            }
        }
    }
    protected TNode parent;

    [JsonIgnore]
    public virtual ObservableCollection<TNode> Childs
    {
        get => childs;
        set
        {
            if(childs == value)
            {
                return;
            }

            childs?.ForEach(t => t.Parent = null);

            if((childs = value).IsNull() == false)
            {
                if(this is TNode node)
                {
                    childs?.ForEach(t => t.Parent = node);
                }

                childs.CollectionChanged -= Childs_CollectionChanged;
                childs.CollectionChanged += Childs_CollectionChanged;
            }
        }
    }
    protected ObservableCollection<TNode> childs;

    protected virtual void Childs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if(e.Action is NotifyCollectionChangedAction.Remove or NotifyCollectionChangedAction.Replace or NotifyCollectionChangedAction.Reset)
        {
            SetChildsParent(e.OldItems, null);
        }

        if(e.Action is NotifyCollectionChangedAction.Add or NotifyCollectionChangedAction.Replace)
        {
            if(this is TNode node)
            {
                SetChildsParent(e.NewItems, node);
            }
        }

        void SetChildsParent(IList childs, TNode parent) => childs.OfType<TNode>().ForEach(t => t.Parent = parent);
    }
}

public static class TreeNodeExtensions
{
    public static TNode GetRoot<TNode>(this TreeNode<TNode> treeNode) where TNode : TreeNode<TNode> => treeNode?.Parent?.GetRoot() ?? treeNode.AsOrDefault<TNode>();
}

public class TValueTreeNode<TValue, TCollection> : TreeNode<TValueTreeNode<TValue, TCollection>> where TCollection : ICollection<TValue>, new()
{
    [JsonIgnore]
    public virtual TCollection Values { get; set; }
}

public class ValueTreeNode<TValue> : TValueTreeNode<TValue, ObservableCollection<TValue>>
{
}