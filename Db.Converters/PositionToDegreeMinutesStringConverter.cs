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
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Db.Converters
{
    /// <summary>Converts a <see cref="Position" /> to decimal degree, minutes.</summary>
    [ValueConversion(typeof(Position), typeof(string))]
    public class PositionToDegreeMinutesStringConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc cref="IValueConverter.Convert(object, Type, object, CultureInfo)" />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string format = parameter as string;
            if (value is Position position)
            {
                string text = null;
                if (position.IsInvalid)
                {
                    text = "---\n---";
                }
                else
                {
                    int latitude = (int)Math.Round((((position.Latitude.InDegrees() + 180) % 360) - 180) * 60000d);
                    int longitude = (int)Math.Round((((position.Longitude.InDegrees() + 180) % 360) - 180) * 60000d);
                    char latHemisphere = 'N';
                    char lonHemisphere = 'E';

                    if (latitude < 0)
                    {
                        latitude = -latitude;
                        latHemisphere = 'S';
                    }

                    if (longitude < 0)
                    {
                        longitude = -longitude;
                        lonHemisphere = 'W';
                    }

                    text = string.Format(culture,
                        "{0}  {1:00}° {2:00.000}'\n{3} {4:000}° {5:00.000}'",
                        latHemisphere, latitude / 60000, (latitude % 60000) / 1000d,
                        lonHemisphere, longitude / 60000, (longitude % 60000) / 1000d);
                }

                if (!string.IsNullOrEmpty(format))
                {
                    return string.Format(culture, format, text);
                }
                else
                {
                    return text;
                }
            }
            else
            {
                return $"{value}";
            }
        }

        /// <inheritdoc cref="IValueConverter.ConvertBack(object, Type, object, CultureInfo)" />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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