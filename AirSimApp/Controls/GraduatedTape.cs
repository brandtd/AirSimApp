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

namespace AirSimApp.Controls
{
    public class GraduatedTape : Control
    {
        public static readonly DependencyProperty CommandedValueProperty =
            DependencyProperty.Register(
                nameof(CommandedValue),
                typeof(double),
                typeof(GraduatedTape),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(
                nameof(CurrentValue),
                typeof(double),
                typeof(GraduatedTape),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DivisionsPerTickProperty =
            DependencyProperty.Register(
                nameof(DivisionsPerTick),
                typeof(int),
                typeof(GraduatedTape),
                new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty MajorTickProperty =
            DependencyProperty.Register(
                nameof(MajorTick),
                typeof(double),
                typeof(GraduatedTape),
                new FrameworkPropertyMetadata(50.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty RangeProperty =
            DependencyProperty.Register(
                nameof(Range),
                typeof(double),
                typeof(GraduatedTape),
                new FrameworkPropertyMetadata(200.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(
                nameof(Stroke),
                typeof(double),
                typeof(GraduatedTape),
                new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));

        static GraduatedTape()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraduatedTape), new FrameworkPropertyMetadata(typeof(GraduatedTape)));
        }

        public double CommandedValue
        {
            get => (double)GetValue(CommandedValueProperty);
            set => SetValue(CommandedValueProperty, value);
        }

        public double CurrentValue
        {
            get => (double)GetValue(CurrentValueProperty);
            set => SetValue(CurrentValueProperty, value);
        }

        public int DivisionsPerTick
        {
            get => (int)GetValue(DivisionsPerTickProperty);
            set => SetValue(DivisionsPerTickProperty, value);
        }

        public double MajorTick
        {
            get => (double)GetValue(MajorTickProperty);
            set => SetValue(MajorTickProperty, value);
        }

        public double Range
        {
            get => (double)GetValue(RangeProperty);
            set => SetValue(RangeProperty, value);
        }

        public double Stroke
        {
            get => (double)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Size size = new Size(ActualWidth, ActualHeight);
            double center = CurrentValue;
            double maximum = center + Range / 2;
            double minimum = center - Range / 2;
            double majorTickLength = size.Width / 3;
            double minorTickLength = size.Width / 4;

            double m = size.Height / (minimum - maximum);
            double b = size.Height * maximum / (maximum - minimum);

            Pen linePen = new Pen(Foreground, Stroke);

            double majorTickValue = MajorTick;
            double minorTickValue = MajorTick / DivisionsPerTick;
            double majorTickStart = minimum - Mod.CanonicalModulo(minimum, majorTickValue);
            Typeface tf = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            for (double tickValue = majorTickStart; tickValue <= maximum; tickValue += majorTickValue)
            {
                Point lineStart = new Point(0.0, m * tickValue + b);
                drawLine(drawingContext, linePen, lineStart, new Point(lineStart.X + majorTickLength, lineStart.Y));
                FormattedText ft = new FormattedText(
                    $"{tickValue:F0}",
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    tf,
                    FontSize,
                    Foreground);
                drawingContext.DrawText(ft, new Point(lineStart.X + majorTickLength + FontSize / 2, lineStart.Y - ft.Height / 2));

                for (int minorTick = 1; minorTick < DivisionsPerTick; ++minorTick)
                {
                    lineStart = new Point(0.0, m * (tickValue + minorTickValue * minorTick) + b);
                    drawLine(drawingContext, linePen, lineStart, new Point(lineStart.X + minorTickLength, lineStart.Y));
                }
            }
        }

        private Typeface _typeface;

        private void drawLine(DrawingContext dc, Pen pen, Point p0, Point p1)
        {
            if (p0.Y >= 0 && p1.Y >= 0 && p0.Y <= ActualHeight && p1.Y <= ActualHeight)
            {
                dc.DrawLine(pen, p0, p1);
            }
        }
    }
}