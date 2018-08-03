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
    public class GraduatedTape : Control
    {
        /// <summary>The tape's current value.</summary>
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(
                nameof(CurrentValue),
                typeof(double),
                typeof(GraduatedTape),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Number of divisions drawn per major tick.</summary>
        public static readonly DependencyProperty DivisionsPerTickProperty =
            DependencyProperty.Register(
                nameof(DivisionsPerTick),
                typeof(int),
                typeof(GraduatedTape),
                new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>How thick to make the major tick markings.</summary>
        public static readonly DependencyProperty MajorStrokeProperty =
            DependencyProperty.Register(
                nameof(MajorStroke),
                typeof(double),
                typeof(GraduatedTape),
                new FrameworkPropertyMetadata(1.5, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        ///     The size of a major tick described in the same units as the commanded/current value properties.
        /// </summary>
        public static readonly DependencyProperty MajorTickProperty =
            DependencyProperty.Register(
                nameof(MajorTick),
                typeof(double),
                typeof(GraduatedTape),
                new FrameworkPropertyMetadata(50.0, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>How thick to make the minor tick markings.</summary>
        public static readonly DependencyProperty MinorStrokeProperty =
            DependencyProperty.Register(
                nameof(MinorStroke),
                typeof(double),
                typeof(GraduatedTape),
                new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>The total range of the tape.</summary>
        public static readonly DependencyProperty RangeProperty =
            DependencyProperty.Register(
                nameof(Range),
                typeof(double),
                typeof(GraduatedTape),
                new FrameworkPropertyMetadata(200.0, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Whether to draw ticks on the left or right side of the control.</summary>
        public static readonly DependencyProperty RightOrLeftProperty =
            DependencyProperty.Register(
                nameof(RightOrLeft),
                typeof(HorizontalAlignment),
                typeof(GraduatedTape),
                new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.AffectsRender));

        static GraduatedTape()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraduatedTape), new FrameworkPropertyMetadata(typeof(GraduatedTape)));
        }

        /// <inheritdoc cref="CurrentValueProperty" />
        public double CurrentValue
        {
            get => (double)GetValue(CurrentValueProperty);
            set => SetValue(CurrentValueProperty, value);
        }

        /// <inheritdoc cref="DivisionsPerTickProperty" />
        public int DivisionsPerTick
        {
            get => (int)GetValue(DivisionsPerTickProperty);
            set => SetValue(DivisionsPerTickProperty, value);
        }

        /// <inheritdoc cref="MajorStrokeProperty" />
        public double MajorStroke
        {
            get => (double)GetValue(MajorStrokeProperty);
            set => SetValue(MajorStrokeProperty, value);
        }

        /// <inheritdoc cref="MajorTickProperty" />
        public double MajorTick
        {
            get => (double)GetValue(MajorTickProperty);
            set => SetValue(MajorTickProperty, value);
        }

        /// <inheritdoc cref="MinorStrokeProperty" />
        public double MinorStroke
        {
            get => (double)GetValue(MinorStrokeProperty);
            set => SetValue(MinorStrokeProperty, value);
        }

        /// <inheritdoc cref="RangeProperty" />
        public double Range
        {
            get => (double)GetValue(RangeProperty);
            set => SetValue(RangeProperty, value);
        }

        /// <inheritdoc cref="RightOrLeftProperty" />
        public HorizontalAlignment RightOrLeft
        {
            get => (HorizontalAlignment)GetValue(RightOrLeftProperty);
            set => SetValue(RightOrLeftProperty, value);
        }

        /// <inheritdoc cref="Control.ArrangeOverride(Size)" />
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            FormattedText sampleText = buildFormattedText("-0000");
            return new Size(sampleText.Width * 1.6, arrangeBounds.Height);
        }

        /// <inheritdoc cref="Control.MeasureOverride(Size)" />
        protected override Size MeasureOverride(Size constraint)
        {
            FormattedText sampleText = buildFormattedText("-0000");
            return new Size(sampleText.Width * 1.6, constraint.Height);
        }

        /// <inheritdoc cref="UIElement.OnRender(DrawingContext)" />
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            Size size = new Size(ActualWidth, ActualHeight);
            dc.PushClip(new RectangleGeometry(new Rect(size)));

            dc.DrawRectangle(Background, null, new Rect(size));

            double x = RightOrLeft == HorizontalAlignment.Left ? 0.0 : size.Width;
            double xflip = RightOrLeft == HorizontalAlignment.Left ? 1.0 : -1.0;

            double center = double.IsNaN(CurrentValue) ? 0.0 : CurrentValue;
            double maximum = center + Range / 2;
            double minimum = center - Range / 2;
            double majorTickLength = xflip * size.Width / 3;
            double minorTickLength = xflip * size.Width / 4;

            double m = size.Height / (minimum - maximum);
            double b = size.Height * maximum / (maximum - minimum);

            Pen majorTickPen = new Pen(Foreground, MajorStroke);
            Pen minorTickPen = new Pen(Foreground, MinorStroke);

            double majorTickValue = MajorTick;
            double minorTickValue = MajorTick / DivisionsPerTick;
            double majorTickStart = minimum - Mod.CanonicalModulo(minimum, majorTickValue);

            for (double tickValue = majorTickStart; tickValue <= maximum; tickValue += majorTickValue)
            {
                Point lineStart = new Point(x, m * tickValue + b);
                drawLine(dc, majorTickPen, lineStart, new Point(lineStart.X + majorTickLength, lineStart.Y));

                drawText(dc, new Point(lineStart.X + majorTickLength + xflip * FontSize / 2, lineStart.Y), $"{tickValue:F0}");

                for (int minorTick = 1; minorTick < DivisionsPerTick; ++minorTick)
                {
                    lineStart = new Point(x, m * (tickValue + minorTickValue * minorTick) + b);
                    drawLine(dc, minorTickPen, lineStart, new Point(lineStart.X + minorTickLength, lineStart.Y));
                }
            }

            dc.Pop();
        }

        private FormattedText buildFormattedText(string text)
        {
            Typeface typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            return new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                FontSize,
                Foreground);
        }

        private void drawLine(DrawingContext dc, Pen pen, Point p0, Point p1)
        {
            if (p0.Y >= 0 && p1.Y >= 0 && p0.Y <= ActualHeight && p1.Y <= ActualHeight)
            {
                dc.DrawLine(pen, p0, p1);
            }
        }

        private void drawText(DrawingContext dc, Point p, string text)
        {
            FormattedText ft = buildFormattedText(text);
            Point adjusted;
            if (RightOrLeft == HorizontalAlignment.Left)
            {
                adjusted = new Point(p.X, p.Y - ft.Height / 2);
            }
            else
            {
                adjusted = new Point(p.X - ft.Width, p.Y - ft.Height / 2);
            }
            dc.DrawText(ft, adjusted);
        }
    }
}