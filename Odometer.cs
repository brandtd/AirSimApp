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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Db.Controls
{
    public enum OdometerResolution
    {
        R1 = 1,
        R10 = 10,
        R100 = 100,
        R1000 = 1000,
    }

    public class Odometer : Control
    {
        /// <summary>Value of odometer.</summary>
        public static readonly DependencyProperty ResolutionProperty =
            DependencyProperty.Register(
                nameof(Resolution),
                typeof(OdometerResolution),
                typeof(Odometer),
                new FrameworkPropertyMetadata(OdometerResolution.R1,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>Whether to draw ticks on the left or right side of the control.</summary>
        public static readonly DependencyProperty RightOrLeftProperty =
            DependencyProperty.Register(
                nameof(RightOrLeft),
                typeof(HorizontalAlignment),
                typeof(Odometer),
                new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Value of odometer.</summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(double),
                typeof(Odometer),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        static Odometer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Odometer), new FrameworkPropertyMetadata(typeof(Odometer)));
        }

        /// <inheritdoc cref="ResolutionProperty" />
        public OdometerResolution Resolution
        {
            get => (OdometerResolution)GetValue(ResolutionProperty);
            set => SetValue(ResolutionProperty, value);
        }

        /// <inheritdoc cref="RightOrLeftProperty" />
        public HorizontalAlignment RightOrLeft
        {
            get => (HorizontalAlignment)GetValue(RightOrLeftProperty);
            set => SetValue(RightOrLeftProperty, value);
        }

        /// <inheritdoc cref="ValueProperty" />
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <inheritdoc cref="Control.MeasureOverride(Size)" />
        protected override Size MeasureOverride(Size constraint)
        {
            double height = buildFormattedText("0", FontSize).Height * 3;

            int bigChars = 5;
            int smallChars = 2;
            switch (Resolution)
            {
                case OdometerResolution.R1:
                    bigChars = 5;
                    smallChars = 2;
                    break;

                case OdometerResolution.R10:
                    bigChars = 4;
                    smallChars = 3;
                    break;

                case OdometerResolution.R100:
                    bigChars = 3;
                    smallChars = 4;
                    break;

                case OdometerResolution.R1000:
                    bigChars = 2;
                    smallChars = 5;
                    break;
            }
            double width = buildFormattedText("0", 1.5 * FontSize).Width * bigChars +
                           buildFormattedText("0", FontSize).Width * smallChars;

            return new Size(width, height);
        }

        /// <inheritdoc cref="UIElement.OnRender(DrawingContext)" />
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.PushClip(new RectangleGeometry(new Rect(new Size(ActualWidth, ActualHeight))));
            drawContainer(dc);

            if (!double.IsNaN(Value))
            {
                drawValueText(dc);
            }

            dc.Pop();
        }

        private FormattedText buildFormattedText(string text, double fontSize)
        {
            Typeface typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            return new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                fontSize,
                Foreground);
        }

        private void drawContainer(DrawingContext dc)
        {
            Size size = new Size(ActualWidth, ActualHeight);

            double yCenter = size.Height / 2.0;

            if (RightOrLeft == HorizontalAlignment.Left)
            {
                Point startPoint = new Point(0.0, yCenter);
                PointCollection points = new PointCollection
                {
                    new Point(FontSize / 2.0, yCenter + FontSize / 2.0),
                    new Point(FontSize / 2.0, size.Height),
                    new Point(size.Width, size.Height),
                    new Point(size.Width, 0.0),
                    new Point(FontSize / 2.0, 0.0),
                    new Point(FontSize / 2.0, yCenter - FontSize / 2.0),
                };
                StreamGeometry sg = new StreamGeometry();
                using (StreamGeometryContext sgc = sg.Open())
                {
                    sgc.BeginFigure(startPoint, true, true);
                    sgc.PolyLineTo(points, true, false);
                }
                sg.Freeze();
                dc.DrawGeometry(Background, new Pen(BorderBrush, BorderThickness.Left), sg);
            }
            else
            {
                Point startPoint = new Point(size.Width, yCenter);
                PointCollection points = new PointCollection
                {
                    new Point(size.Width - FontSize / 2.0, yCenter + FontSize / 2.0),
                    new Point(size.Width - FontSize / 2.0, size.Height),
                    new Point(size.Width - size.Width, size.Height),
                    new Point(size.Width - size.Width, 0.0),
                    new Point(size.Width - FontSize / 2.0, 0.0),
                    new Point(size.Width - FontSize / 2.0, yCenter - FontSize / 2.0),
                };
                StreamGeometry sg = new StreamGeometry();
                using (StreamGeometryContext sgc = sg.Open())
                {
                    sgc.BeginFigure(startPoint, true, true);
                    sgc.PolyLineTo(points, true, false);
                }
                sg.Freeze();
                dc.DrawGeometry(Background, new Pen(BorderBrush, BorderThickness.Left), sg);
            }
        }

        private void drawText(DrawingContext dc, Point p, string text, double fontSize)
        {
            FormattedText ft = buildFormattedText(text, fontSize);
            dc.DrawText(ft, p);
        }

        private void drawValueText(DrawingContext dc)
        {
            int sign = Math.Sign(Value);
            double steppedValue = Value / (double)Resolution;
            int upperValue = (int)Math.Truncate(Math.Round(steppedValue) / 10);
            double remainder = Math.Abs(steppedValue) % 10;
            int lowerValue = (int)Math.Truncate(remainder);
            double spin = remainder - lowerValue;

            int upperWidth = 0;
            int lowerWidth = 0;
            switch (Resolution)
            {
                case OdometerResolution.R1:
                    upperWidth = 5;
                    lowerWidth = 1;
                    break;

                case OdometerResolution.R10:
                    upperWidth = 4;
                    lowerWidth = 2;
                    break;

                case OdometerResolution.R100:
                    upperWidth = 3;
                    lowerWidth = 3;
                    break;

                case OdometerResolution.R1000:
                    upperWidth = 2;
                    lowerWidth = 4;
                    break;
            }

            string upperText = "";
            if (upperValue != 0)
            {
                upperText = $"{upperValue}";
            }
            else if (Value < 0)
            {
                upperText = "-";
            }
            string lowerTextFloat = $"{Mod.CanonicalModulo((lowerValue + 2), 10)}".PadRight(lowerWidth, '0');
            string lowerTextAbove = $"{Mod.CanonicalModulo((lowerValue + 1), 10)}".PadRight(lowerWidth, '0');
            string lowerTextMiddle = $"{lowerValue}".PadRight(lowerWidth, '0');
            string lowerTextBelow = $"{Mod.CanonicalModulo((lowerValue - 1), 10)}".PadRight(lowerWidth, '0');
            upperText = upperText.PadLeft(upperWidth);

            double x = RightOrLeft == HorizontalAlignment.Left ? FontSize / 2.0 : 0.0;
            double y = FontSize;
            FormattedText text = buildFormattedText(upperText, 1.5 * FontSize);
            dc.DrawText(text, new Point(x, 0.75 * FontSize));

            double textOffset = text.WidthIncludingTrailingWhitespace;
            drawText(dc, new Point(textOffset + x, 0 - y * (1 - spin)), lowerTextFloat, FontSize);
            drawText(dc, new Point(textOffset + x, 0 + y * spin), lowerTextAbove, FontSize);
            drawText(dc, new Point(textOffset + x, y + y * spin), lowerTextMiddle, FontSize);
            drawText(dc, new Point(textOffset + x, 2 * y + y * spin), lowerTextBelow, FontSize);
        }
    }
}