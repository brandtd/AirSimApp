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

using DotSpatial.Positioning;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Db.Converters
{
    [ValueConversion(typeof(IEnumerable<Distance>), typeof(DoubleCollection))]
    public class EnumerableDistanceToDoubleCollectionConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc cref="IValueConverter.Convert(object, Type, object, CultureInfo)" />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<Distance> distances = (IEnumerable<Distance>)value;

            if (distances != null)
            {
                DoubleCollection collection = new DoubleCollection();
                foreach (Distance d in distances)
                {
                    collection.Add(d.InMeters());
                }
                return collection;
            }

            return null;
        }

        /// <inheritdoc cref="IValueConverter.ConvertBack(object, Type, object, CultureInfo)" />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DoubleCollection collection = (DoubleCollection)value;

            if (collection != null)
            {
                List<Distance> distances = new List<Distance>();
                foreach (double d in collection)
                {
                    distances.Add(Distance.FromMeters(d));
                }
                return distances;
            }

            return null;
        }

        /// <inheritdoc cref="MarkupExtension.ProvideValue(IServiceProvider)" />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}