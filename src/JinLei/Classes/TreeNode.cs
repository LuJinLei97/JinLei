using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

using JinLei.Extensions;

namespace JinLei.Classes;

public partial class TreeNode<TNode> where TNode : TreeNode<TNode>
{
    public virtual TNode Parent
    {
        get => parent;
        set
        {
            if(this is TNode node)
            {
                if(parent != value)
                {
                    parent?.Childs?.Remove(node);
                }

                if((parent = value)?.Childs?.Contains(node) == false)
                {
                    parent?.Childs?.Add(node);
                }
            }

            parent = value;
        }
    }
    protected TNode parent;

    public virtual ObservableCollection<TNode> Childs
    {
        get
        {
            childs ??= [];
            childs.CollectionChanged -= Childs_CollectionChanged;
            childs.CollectionChanged += Childs_CollectionChanged;
            return childs;
        }
        set
        {
            if(childs != value)
            {
                childs?.ForEachDo(t => t.Parent = null);
            }

            if(this is TNode node)
            {
                (childs = value)?.ForEachDo(t => t.Parent = node);
            }

            childs = value;

            _ = Childs;
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

        void SetChildsParent(IList childs, TNode parent) => childs.OfType<TNode>().ForEachDo(t => t.Parent = parent);
    }
}

public class ValueTreeNode<TValue, TCollection> : TreeNode<ValueTreeNode<TValue, TCollection>> where TCollection : ICollection<TValue>, new()
{
    public virtual TCollection Values { get; set; } = new();
}

public class XTreeElement<TValue> : XElement
{
    public XTreeElement(XName name) : base(name)
    {
    }

    public XTreeElement() : base(nameof(XTreeElement<TValue>))
    {
    }

    public virtual TValue AdditionalValue
    {
        get => this.Do(t => RefreshAdditionalValue()).Return(additionalValue);
        set => (additionalValue = value).Do(t => RefreshAdditionalValue(false));
    }
    private TValue additionalValue;

    protected virtual XmlSerializer TValueXmlSerializer { get; } = new(typeof(TValue));

    protected virtual void RefreshAdditionalValue(bool isGet = true)
    {
        using var stringWriter = new StringWriter();
        TValueXmlSerializer.Serialize(stringWriter, additionalValue);
        var tXE = XElement.Parse(stringWriter.ToString());

        if(isGet)
        {
            var vE = Element("XTreeElement").Element(nameof(AdditionalValue));
            if(vE.IsNull() == false)
            {
                tXE.Value = vE.Value;
                var stringReader = new StringReader(tXE.ToString());
                additionalValue = TValueXmlSerializer.Deserialize(stringReader).AsDynamicOrDefault();
            }
        } else
        {
            SetElementValue(nameof(AdditionalValue), additionalValue?.Return(tXE.Value));
        }
    }
}