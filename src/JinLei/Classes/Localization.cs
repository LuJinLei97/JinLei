using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

using JinLei.Extensions;
using JinLei.Utilities;

namespace JinLei.Classes;

public partial class Localization : TextBlock
{
    // todo:未实现
    // Static constructor.
    static Localization() => TextProperty.OverrideMetadata(typeof(Localization), new FrameworkPropertyMetadata(OnLanguageResourceChanged));

    public Localization(string key = default, string text = default)
    {
        Key = key;
        Text = text;
    }

    protected override void OnInitialized(EventArgs e)
    {
        OnLanguageResxPathChanged(new(LanguageResxPathProperty, default, GetLanguageResxPath(this)));
        base.OnInitialized(e);
    }

    public virtual string Key
    {
        get => GetValue(KeyProperty).AsDynamicOrDefault();
        set => SetValue(KeyProperty, value);
    }
    public static readonly DependencyProperty KeyProperty = DependencyPropertyUtility.Register(nameof(Key), new(OnLanguageResourceChanged));

    protected static void OnLanguageResourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.AsOrDefault<Localization>()?.OnLanguageResourceChanged(e);

    protected virtual void OnLanguageResourceChanged(DependencyPropertyChangedEventArgs e)
    {
        //Debugger.Launch();

        if(Utility.IsInDesignMode == false)
        {
            return;
        }

        var languageResourceBindingMode = GetLanguageResourceBindingMode(this);
        if(languageResourceBindingMode == BindingMode.OneWay)
        {
            // todo: FileSystemWatcher监控源文件
            // Resx <= (Key, Value)
            return;
        } else if(languageResourceBindingMode == BindingMode.TwoWay)
        {
            // todo: FileSystemWatcher监控源文件
            // Resx <=> (Key, Value)
            return;
        }

        var languageResxPath = GetLanguageResxPath(this);

        if(StaticResources.TryGetValueEatException(languageResxPath, out var dictionary) == false)
        {
            StaticResources[languageResxPath] = dictionary = [];
        }

        var stringResources = dictionary[StringResourcesKey].AsOrDefault<Dictionary<string, string>>();

        if(languageResourceBindingMode == BindingMode.OneTime)
        {
            // Resx => (Key, Value) OneTime
            if(stringResources.TryGetValueEatException(Key, out var value))
            {
                Text = value;
            }
        } else if(languageResourceBindingMode == BindingMode.OneWayToSource)
        {
            // Resx <= (Key, Value)
            if(Key.IsNull() == false)
            {
                stringResources[Key] = Text;
                dictionary[ResxWriterKey].AsOrDefault<System.Timers.Timer>().Enabled = true;
            }
        }
    }

    #region AttachedProperty
    public static readonly DependencyProperty LanguageResxPathProperty = DependencyProperty.RegisterAttached(nameof(LanguageResxPathProperty).TrimEnd(DependencyPropertyUtility.Suffix), typeof(string), typeof(Localization), new FrameworkPropertyMetadata("StringResources.zh-CN.resx", FrameworkPropertyMetadataOptions.Inherits, OnLanguageResxPathChanged));

    public static string GetLanguageResxPath(DependencyObject target) => target.GetValue(LanguageResxPathProperty).AsDynamicOrDefault();

    public static void SetLanguageResxPath(DependencyObject target, string value) => target.SetCurrentValue(LanguageResxPathProperty, value);

    protected static void OnLanguageResxPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.AsOrDefault<Localization>()?.OnLanguageResxPathChanged(e);

    protected virtual void OnLanguageResxPathChanged(DependencyPropertyChangedEventArgs e)
    {
        //Debugger.Launch();

        if(Utility.IsInDesignMode == false)
        {
            return;
        }

        var languageResxPath = e.NewValue.AsOrDefault<string>();

        if(StaticResources.TryGetValueEatException(languageResxPath, out var dictionary) == false)
        {
            StaticResources[languageResxPath] = dictionary = [];
        }

        dictionary[StringResourcesKey] = ResxUtility.ReadToDictionary<string>(languageResxPath).Out(out var stringResources);
        dictionary[ResxWriterKey] = new System.Timers.Timer(5000) { AutoReset = false }.Do(t => t.Elapsed += delegate
        {
            ResxUtility.WriteFromItems(languageResxPath, stringResources);
        });
        // todo: FileSystemWatcher监控源文件
    }

    public static readonly DependencyProperty LanguageResourceBindingModeProperty = DependencyProperty.RegisterAttached(nameof(LanguageResourceBindingModeProperty).TrimEnd(DependencyPropertyUtility.Suffix), typeof(BindingMode), typeof(Localization), new FrameworkPropertyMetadata(BindingMode.OneTime, FrameworkPropertyMetadataOptions.Inherits));

    public static BindingMode GetLanguageResourceBindingMode(DependencyObject target) => target?.GetValue(LanguageResourceBindingModeProperty).AsDynamicOrDefault();

    public static void SetLanguageResourceBindingMode(DependencyObject target, BindingMode value) => target?.SetValue(LanguageResourceBindingModeProperty, value);
    #endregion

    protected static Dictionary<string, Dictionary<string, object>> StaticResources { get; set; } = [];

    protected const string StringResourcesKey = "StringResources";
    protected const string ResxWriterKey = "ResxWriter";
}

public partial class LocalizationExtension(string key = default, string defaultValue = default) : MarkupExtension
{
    #region override
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        //Debugger.Launch();

        var targetObject = serviceProvider.GetService(typeof(IProvideValueTarget)).AsOrDefault<IProvideValueTarget>().TargetObject.AsOrDefault<DependencyObject>();

        Localization.SetLanguageResxPath(Localization, Localization.GetLanguageResxPath(targetObject));
        Localization.SetLanguageResourceBindingMode(Localization, Localization.GetLanguageResourceBindingMode(targetObject));

        Localization.Key = Key;
        Localization.Text = Value;

        return Value = Localization.Text;
    }
    #endregion

    #region Property
    /// <summary>
    /// 语言资源Key
    /// </summary>
    public virtual string Key { get; set; }

    /// <summary>
    /// 语言资源Value
    /// </summary>
    public virtual string Value { get; set; }
    #endregion

    public virtual Localization Localization { get; protected set; } = new(key, defaultValue);
}