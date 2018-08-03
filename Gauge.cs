#region MIT License (c) 2018 Dan Brandt, Filip Skakun

// This code originates from http://github.com/xyzzer/WinRTXamlToolkit, a repository by Filip Skakun
// with an MIT license.
//
// Copyright 2018 Dan Brandt, Filip Skakun
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

#endregion MIT License (c) 2018 Dan Brandt, Filip Skakun

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfBBQWinRTXamlToolkit
{
    // Straight copied from the WinRTXAML toolkit, which unfortunately, only targets UWP.

    [TemplatePart(Name = NeedlePartName, Type = typeof(Path))]
    [TemplatePart(Name = ScalePartName, Type = typeof(Path))]
    [TemplatePart(Name = TrailPartName, Type = typeof(Path))]
    [TemplatePart(Name = ValueTextPartName, Type = typeof(TextBlock))]
    public class Gauge : Control
    {
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                nameof(Maximum),
                typeof(double),
                typeof(Gauge),
                new PropertyMetadata(100.0));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                nameof(Minimum),
                typeof(double),
                typeof(Gauge),
                new PropertyMetadata(0.0));

        public static readonly DependencyProperty NeedleBrushProperty =
            DependencyProperty.Register(
                nameof(NeedleBrush),
                typeof(Brush),
                typeof(Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.OrangeRed)));

        public static readonly DependencyProperty ScaleBrushProperty =
            DependencyProperty.Register(
                nameof(ScaleBrush),
                typeof(Brush),
                typeof(Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public static readonly DependencyProperty ScaleTickBrushProperty =
            DependencyProperty.Register(
                nameof(ScaleTickBrush),
                typeof(Brush),
                typeof(Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public static readonly DependencyProperty ScaleWidthProperty =
            DependencyProperty.Register(
                nameof(ScaleWidth),
                typeof(double),
                typeof(Gauge),
                new PropertyMetadata(26.0));

        public static readonly DependencyProperty TickBrushProperty =
            DependencyProperty.Register(
                nameof(TickBrush),
                typeof(Brush),
                typeof(Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.DimGray)));

        public static readonly DependencyProperty TicksProperty =
                    DependencyProperty.Register(
                nameof(Ticks),
                typeof(IEnumerable<double>),
                typeof(Gauge),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TrailBrushProperty =
                    DependencyProperty.Register(
                nameof(TrailBrush),
                typeof(Brush),
                typeof(Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        public static readonly DependencyProperty UnitBrushProperty =
            DependencyProperty.Register(
                nameof(UnitBrush),
                typeof(Brush),
                typeof(Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.DimGray)));

        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register(
                nameof(Unit),
                typeof(string),
                typeof(Gauge),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ValueAngleProperty =
            DependencyProperty.Register(
                nameof(ValueAngle),
                typeof(double),
                typeof(Gauge),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ValueBrushProperty =
                    DependencyProperty.Register(
                nameof(ValueBrush),
                typeof(Brush),
                typeof(Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.DimGray)));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(double),
                typeof(Gauge),
                new PropertyMetadata(0.0, onValueChanged));

        public static readonly DependencyProperty ValueStringFormatProperty =
            DependencyProperty.Register(
                nameof(ValueStringFormat),
                typeof(string),
                typeof(Gauge),
                new PropertyMetadata("N0"));

        static Gauge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Gauge), new FrameworkPropertyMetadata(typeof(Gauge)));
        }

        public Gauge()
        {
            this.DefaultStyleKey = typeof(Gauge);
            this.Ticks = this.getTicks();
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public Brush NeedleBrush
        {
            get { return (Brush)GetValue(NeedleBrushProperty); }
            set { SetValue(NeedleBrushProperty, value); }
        }

        public Brush ScaleBrush
        {
            get { return (Brush)GetValue(ScaleBrushProperty); }
            set { SetValue(ScaleBrushProperty, value); }
        }

        public Brush ScaleTickBrush
        {
            get { return (Brush)GetValue(ScaleTickBrushProperty); }
            set { SetValue(ScaleTickBrushProperty, value); }
        }

        public double ScaleWidth
        {
            get { return (double)GetValue(ScaleWidthProperty); }
            set { SetValue(ScaleWidthProperty, value); }
        }

        public Brush TickBrush
        {
            get { return (Brush)GetValue(TickBrushProperty); }
            set { SetValue(TickBrushProperty, value); }
        }

        public IEnumerable<double> Ticks
        {
            get { return (IEnumerable<double>)GetValue(TicksProperty); }
            set { SetValue(TicksProperty, value); }
        }

        public Brush TrailBrush
        {
            get { return (Brush)GetValue(TrailBrushProperty); }
            set { SetValue(TrailBrushProperty, value); }
        }

        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        public Brush UnitBrush
        {
            get { return (Brush)GetValue(UnitBrushProperty); }
            set { SetValue(UnitBrushProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double ValueAngle
        {
            get { return (double)GetValue(ValueAngleProperty); }
            set { SetValue(ValueAngleProperty, value); }
        }

        public Brush ValueBrush
        {
            get { return (Brush)GetValue(ValueBrushProperty); }
            set { SetValue(ValueBrushProperty, value); }
        }

        public string ValueStringFormat
        {
            get { return (string)GetValue(ValueStringFormatProperty); }
            set { SetValue(ValueStringFormatProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            // Draw Scale
            Path scale = this.GetTemplateChild(ScalePartName) as Path;

            if (scale != null)
            {
                PathGeometry pg = new PathGeometry();
                PathFigure pf = new PathFigure();
                pf.IsClosed = false;
                double middleOfScale = 77 - this.ScaleWidth / 2;
                pf.StartPoint = scalePoint(-150, middleOfScale);
                ArcSegment seg = new ArcSegment();
                seg.SweepDirection = SweepDirection.Clockwise;
                seg.IsLargeArc = true;
                seg.Size = new Size(middleOfScale, middleOfScale);
                seg.Point = scalePoint(150, middleOfScale);
                pf.Segments.Add(seg);
                pg.Figures.Add(pf);
                scale.Data = pg;
            }

            onValueChanged(this, default(DependencyPropertyChangedEventArgs));
            base.OnApplyTemplate();
        }

        private const double Degrees2Radians = Math.PI / 180;

        private const string NeedlePartName = "PART_Needle";

        private const string ScalePartName = "PART_Scale";

        private const string TrailPartName = "PART_Trail";

        private const string ValueTextPartName = "PART_ValueText";

        private static void onValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Gauge c = (Gauge)d;

            if (double.IsNaN(c.Value))
                return;

            double middleOfScale = 77 - c.ScaleWidth / 2;
            Path needle = c.GetTemplateChild(NeedlePartName) as Path;
            TextBlock valueText = c.GetTemplateChild(ValueTextPartName) as TextBlock;
            c.ValueAngle = c.valueToAngle(c.Value);

            // Needle
            if (needle != null)
            {
                needle.RenderTransform = new RotateTransform() { Angle = c.ValueAngle };
            }

            // Trail
            Path trail = c.GetTemplateChild(TrailPartName) as Path;

            if (trail != null)
            {
                if (c.ValueAngle > -146)
                {
                    trail.Visibility = Visibility.Visible;
                    PathGeometry pg = new PathGeometry();
                    PathFigure pf = new PathFigure();
                    pf.IsClosed = false;
                    pf.StartPoint = scalePoint(-150, middleOfScale);
                    ArcSegment seg = new ArcSegment();
                    seg.SweepDirection = SweepDirection.Clockwise;
                    // We start from -150, so +30 becomes a large arc.
                    seg.IsLargeArc = c.ValueAngle > 30;
                    seg.Size = new Size(middleOfScale, middleOfScale);
                    seg.Point = scalePoint(c.ValueAngle, middleOfScale);
                    pf.Segments.Add(seg);
                    pg.Figures.Add(pf);
                    trail.Data = pg;
                }
                else
                {
                    trail.Visibility = Visibility.Collapsed;
                }
            }

            // Value Text
            if (valueText != null)
            {
                valueText.Text = c.Value.ToString(c.ValueStringFormat);
            }
        }

        private static Point scalePoint(double angle, double middleOfScale)
        {
            return new Point(100 + Math.Sin(Degrees2Radians * angle) * middleOfScale, 100 - Math.Cos(Degrees2Radians * angle) * middleOfScale);
        }

        private IEnumerable<double> getTicks()
        {
            double tickSpacing = (this.Maximum - this.Minimum) / 10;

            for (double tick = this.Minimum; tick <= this.Maximum; tick += tickSpacing)
            {
                yield return valueToAngle(tick);
            }
        }

        private double valueToAngle(double value)
        {
            const double minAngle = -150;
            const double maxAngle = 150;

            // Off-scale to the left
            if (value < this.Minimum)
            {
                return minAngle - 7.5;
            }

            // Off-scale to the right
            if (value > this.Maximum)
            {
                return maxAngle + 7.5;
            }

            double angularRange = maxAngle - minAngle;

            return (value - this.Minimum) / (this.Maximum - this.Minimum) * angularRange + minAngle;
        }
    }
}