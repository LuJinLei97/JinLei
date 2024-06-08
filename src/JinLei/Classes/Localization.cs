using System.Windows;
using System.Windows.Markup;

using JinLei.Extensions;
using JinLei.Utilities;

namespace JinLei.Classes;
public partial class Localization : DependencyObject
{
    public Localization(string key = default, string value = default)
    {
        Key = key;
        Value = value;
    }

    public virtual string Key
    {
        get => GetValue(KeyProperty).AsDynamicOrDefault();
        set => SetValue(KeyProperty, value);
    }
    public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(nameof(Key), ObjectExtensions.GetTypeFromCaller(nameof(Key)), ObjectExtensions.GetTypeFromCaller(string.Empty), new PropertyMetadata(ResourceChanged));

    public virtual string Value
    {
        get => GetValue(ValueProperty).AsDynamicOrDefault();
        set => SetValue(ValueProperty, value);
    }
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), ObjectExtensions.GetTypeFromCaller(nameof(Value)), ObjectExtensions.GetTypeFromCaller(string.Empty), new PropertyMetadata(ResourceChanged));

    public static void ResourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.AsOrDefault<Localization>()?.ResourceChangedImplementation(e);

    public virtual void ResourceChangedImplementation(DependencyPropertyChangedEventArgs e)
    {
        if(Utilities.Utilities.IsInDesignMode && DesignModeStringResources.TryGetValueEatException(Key, out var value))
        {
            if(Value.IsNull())
            {
                Value = value;
            } else
            {
                ResxWriter.Enabled = WriteResxOnResourceChanged;
            }
        }
    }

    public static System.Timers.Timer ResxWriter { get; } = new System.Timers.Timer(5000) { AutoReset = false }.Do(t => t.Elapsed += delegate { ResxUtility.WriteFromItems(new(DefaultLangResxPath), DesignModeStringResources); });

    #region AttachedProperty
    public static string DefaultLangResxPath { get; set; } = "StringResources.zh-CN.resx";

    public static void SetDefaultLangResxPath(DependencyObject target, string value)
    {
        if(DefaultLangResxPath != value)
        {
            DefaultLangResxPath = value;
            DesignModeStringResources = default;
        }
    }

    public static bool WriteResxOnResourceChanged { get; set; }

    public static void SetWriteResxOnDesignModeResourceChanged(DependencyObject target, bool value) => WriteResxOnResourceChanged = value;
    #endregion

    public static Dictionary<string, string> DesignModeStringResources
    {
        get => designModeAllStringResources[DefaultLangResxPath] ??= Utilities.Utilities.IsInDesignMode ? ResxUtility.ReadToDictionary<string>(new(DefaultLangResxPath)) : [];
        protected set => designModeAllStringResources[DefaultLangResxPath] = value;
    }
    protected static Dictionary<string, Dictionary<string, string>> designModeAllStringResources = [];
}

public partial class LocalizationExtension(string key = default, string defaultValue = default) : MarkupExtension
{
    #region override
    public override object ProvideValue(IServiceProvider serviceProvider) => Value;
    #endregion

    #region Property
    /// <summary>
    /// 语言资源Key
    /// </summary>
    public virtual string Key
    {
        get => Localization.Key;
        set => Localization.Key = value;
    }

    /// <summary>
    /// 语言资源Value
    /// </summary>
    public virtual string Value
    {
        get => Localization.Value;
        set => Localization.Value = value;
    }
    #endregion

    public virtual Localization Localization { get; protected set; } = new(key, defaultValue);
}