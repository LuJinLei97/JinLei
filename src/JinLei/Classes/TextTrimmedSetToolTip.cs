using System.Windows;
using System.Windows.Controls;

using JinLei.Extensions;
using JinLei.Utilities;

namespace JinLei.Classes;

public static partial class TextTrimmedSetToolTip
{
    public static readonly DependencyProperty IsToolTipProperty = DependencyProperty.RegisterAttached(nameof(IsToolTipProperty).TrimEnd(DependencyPropertyUtility.Suffix), typeof(bool), typeof(TextTrimmedSetToolTip), new PropertyMetadata(RegisterSizeChangedCallback));

    public static bool GetIsToolTip(DependencyObject element) => element.GetValue(IsToolTipProperty).AsDynamicOrDefault();

    public static void SetIsToolTip(DependencyObject element, bool value) => element.SetValue(IsToolTipProperty, value);

    private static void RegisterSizeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        TextClippedSetToolTip(d, default);

        void TextClippedSetToolTip(object o, SizeChangedEventArgs e)
        {
            if(o is not TextBlock textBlock)
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