﻿using DotSpatial.Positioning;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace AirSimApp.Converters
{
    [ValueConversion(typeof(Latitude), typeof(string))]
    public class LatitudeToStringConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc cref="MarkupExtension.ProvideValue(IServiceProvider)" />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        /// <inheritdoc cref="IValueConverter.Convert(object, Type, object, CultureInfo)" />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string format = parameter as string;
            if (!string.IsNullOrEmpty(format))
            {
                return string.Format(culture, format, value);
            }
            else
            {
                return $"{value}";
            }
        }

        /// <inheritdoc cref="IValueConverter.ConvertBack(object, Type, object, CultureInfo)" />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valAsString = value as string;
            if (!string.IsNullOrEmpty(valAsString))
            {
                Latitude latitude = Latitude.Parse(valAsString);
                return latitude;
            }
            else
            {
                return null;
            }
        }
    }
}