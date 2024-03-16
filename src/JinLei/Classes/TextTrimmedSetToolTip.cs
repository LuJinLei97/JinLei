using System.Windows;
using System.Windows.Controls;

using JinLei.Extensions;

namespace JinLei.Classes;
public static class TextTrimmedSetToolTip
{
    public static readonly DependencyProperty IsToolTipProperty = DependencyProperty.RegisterAttached(nameof(IsToolTipProperty).TrimEnd("Property"), typeof(bool), typeof(TextTrimmedSetToolTip), new PropertyMetadata(default(bool), RegisterSizeChangedCallback));

    public static bool GetIsToolTip(DependencyObject element) => element.GetValue(IsToolTipProperty).AsOrDefault<bool>();

    public static void SetIsToolTip(DependencyObject element, bool value) => element.SetValue(IsToolTipProperty, value);

    private static void RegisterSizeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        TextClippedSetToolTip(d, default);
        static void TextClippedSetToolTip(object o, SizeChangedEventArgs e)
        {
            if(o is not TextBlock textBlock)
            {
                return;
            }

            if(Utilities.Utilities.IsInDesignMode)
            {
                return;
            }

            textBlock.SizeChanged -= TextClippedSetToolTip;

            if(GetIsToolTip(textBlock).Out(out var isSizeChangedSetToolTip) && textBlock.IsInitialized && string.IsNullOrWhiteSpace(textBlock.Text) == false && textBlock.GetNeedsClipBounds())
            {
                textBlock.SetCurrentValue(FrameworkElement.ToolTipProperty, textBlock.Text);
            } else
            {
                textBlock.ClearValue(FrameworkElement.ToolTipProperty);
            }

            if(isSizeChangedSetToolTip)
            {
                textBlock.SizeChanged += TextClippedSetToolTip;
            }
        }
    }
}