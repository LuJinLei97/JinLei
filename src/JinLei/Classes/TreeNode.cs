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
                if(parent?.Childs.Contains(node) == true)
                {
                    parent?.Childs.Remove(node);
                }

                if((parent = value)?.Childs.Contains(node) == false)
                {
                    parent?.Childs.Add(node);
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

            if(childs.IsNull() == false)
            {
                foreach(var child in childs)
                {
                    child.Parent = null;
                }
            }

            if((childs = value).IsNull() == false)
            {
                if(this is TNode node)
                {
                    foreach(var child in childs)
                    {
                        child.Parent = node;
                    }
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

        void SetChildsParent(IList childs, TNode parent)
        {
            foreach(var child in childs.OfType<TNode>())
            {
                child.Parent = parent;
            }
        }
    }
}