using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace AirSimApp.Converters
{
    /// <summary>
    ///     Switches the source of a binding based on third value. If third value is <c>true</c>,
    ///     returns the first value. If third value is <c>false</c>, returns the second value.
    /// </summary>
    public class SwitchSourceConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)values[2])
            {
                return values[0];
            }
            else
            {
                return values[1];
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="MarkupExtension.ProvideValue(IServiceProvider)" />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}