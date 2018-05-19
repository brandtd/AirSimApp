#region MIT License (c) 2018 Dan Brandt

// Copyright 2018 Dan Brandt
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion MIT License (c) 2018 Dan Brandt

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Db.Converters
{
    /// <summary>
    ///     Takes two values, a boolean and a double. If the boolean is <c>true</c> and the double is
    ///     a valid number (is not NaN), returns <c>Visibility.Visibile</c>. Otherwise, returns <c>Visibility.Collapsed</c>
    /// </summary>
    public class IsTrueAndIsNumberToVisibilityConverter : MarkupExtension, IMultiValueConverter
    {
        /// <inheritdoc cref="IMultiValueConverter.Convert(object[], Type, object, CultureInfo)" />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool theBoolean = false;
            double theDouble = double.NaN;

            if (values[0] is bool)
            {
                theBoolean = (bool)values[0];
            }
            else if (values[0] is double)
            {
                theDouble = (double)values[0];
            }

            if (values[1] is bool)
            {
                theBoolean = (bool)values[1];
            }
            else if (values[1] is double)
            {
                theDouble = (double)values[1];
            }

            return (theBoolean && !double.IsNaN(theDouble)) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <inheritdoc cref="IMultiValueConverter.ConvertBack(object, Type[], object, CultureInfo)" />
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