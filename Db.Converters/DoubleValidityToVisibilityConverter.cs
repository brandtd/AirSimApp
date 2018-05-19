using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Db.Converters
{
    /// <summary>
    ///     Converts from a double to a UIElement visibility (switching on whether the double is NaN).
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class DoubleValidityToVisibilityConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc cref="IValueConverter.Convert(object, Type, object, CultureInfo)" />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = !double.IsNaN((double)value);
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <inheritdoc cref="IValueConverter.ConvertBack(object, Type, object, CultureInfo)" />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility isVisible = (Visibility)value;
            return (isVisible == Visibility.Visible) ? 0.0 : double.NaN;
        }

        /// <inheritdoc cref="MarkupExtension.ProvideValue(IServiceProvider)" />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}