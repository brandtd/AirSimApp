using System;
using System.Collections.Generic;
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
    public class GraduatedTapeBug : Control
    {
        /// <summary>
        ///     Describes where to draw the commanded value bug. If NaN, no bug will be drawn.
        /// </summary>
        public static readonly DependencyProperty CommandedValueProperty =
            DependencyProperty.Register(
                nameof(CommandedValue),
                typeof(double),
                typeof(GraduatedTapeBug),
                new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Describes the current value of this control.</summary>
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(
                nameof(CurrentValue),
                typeof(double),
                typeof(GraduatedTapeBug),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>The total range of the tape.</summary>
        public static readonly DependencyProperty RangeProperty =
            DependencyProperty.Register(
                nameof(Range),
                typeof(double),
                typeof(GraduatedTapeBug),
                new FrameworkPropertyMetadata(200.0, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Whether to draw ticks on the left or right side of the control.</summary>
        public static readonly DependencyProperty RightOrLeftProperty =
            DependencyProperty.Register(
                nameof(RightOrLeft),
                typeof(HorizontalAlignment),
                typeof(GraduatedTapeBug),
                new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.AffectsRender));

        static GraduatedTapeBug()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraduatedTapeBug), new FrameworkPropertyMetadata(typeof(GraduatedTapeBug)));
        }

        /// <inheritdoc cref="CommandedValueProperty" />
        public double CommandedValue
        {
            get => (double)GetValue(CommandedValueProperty);
            set => SetValue(CommandedValueProperty, value);
        }

        /// <inheritdoc cref="CurrentValueProperty" />
        public double CurrentValue
        {
            get => (double)GetValue(CurrentValueProperty);
            set => SetValue(CurrentValueProperty, value);
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

        /// <inheritdoc cref="UIElement.OnRender(DrawingContext)" />
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (!double.IsNaN(CommandedValue) && !double.IsNaN(CurrentValue))
            {
                drawBug(dc);
            }
        }

        private void drawBug(DrawingContext dc)
        {
            Size size = new Size(ActualWidth, ActualHeight);

            double yCenter = size.Height / 2;

            double drawnValue = Math.Min(Math.Max(CommandedValue, CurrentValue - Range / 2), CurrentValue + Range / 2);
            double yMarker = -size.Height / Range * (CommandedValue - CurrentValue) + yCenter;

            dc.DrawLine(new Pen(Brushes.Magenta, 0.5), new Point(0.0, yMarker), new Point(size.Width, yMarker - 1));
            dc.DrawLine(new Pen(Brushes.Magenta, 0.5), new Point(0.0, yMarker), new Point(size.Width, yMarker + 1));

            double xOffset = 0.1 * FontSize;
            if (RightOrLeft == HorizontalAlignment.Left)
            {
                Point startPoint = new Point(-xOffset, yMarker + FontSize);
                PointCollection points = new PointCollection
                {
                    new Point(-xOffset + FontSize, yMarker + FontSize),
                    new Point(-xOffset + FontSize, yMarker + FontSize / 2.0),
                    new Point(-xOffset + FontSize / 2.0, yMarker),
                    new Point(-xOffset + FontSize, yMarker - FontSize / 2.0),
                    new Point(-xOffset + FontSize, yMarker - FontSize),
                    new Point(-xOffset + 0.0, yMarker - FontSize),
                };
                StreamGeometry sg = new StreamGeometry();
                using (StreamGeometryContext sgc = sg.Open())
                {
                    sgc.BeginFigure(startPoint, true, true);
                    sgc.PolyLineTo(points, true, false);
                }
                sg.Freeze();
                dc.DrawGeometry(Brushes.Magenta, null, sg);
            }
            else
            {
                Point startPoint = new Point(xOffset + size.Width, yMarker + FontSize);
                PointCollection points = new PointCollection
                {
                    new Point(xOffset + size.Width - FontSize, yMarker + FontSize),
                    new Point(xOffset + size.Width - FontSize, yMarker + FontSize / 2.0),
                    new Point(xOffset + size.Width - FontSize / 2.0, yMarker),
                    new Point(xOffset + size.Width - FontSize, yMarker - FontSize / 2.0),
                    new Point(xOffset + size.Width - FontSize, yMarker - FontSize),
                    new Point(xOffset + size.Width - 0.0, yMarker - FontSize),
                };
                StreamGeometry sg = new StreamGeometry();
                using (StreamGeometryContext sgc = sg.Open())
                {
                    sgc.BeginFigure(startPoint, true, true);
                    sgc.PolyLineTo(points, true, false);
                }
                sg.Freeze();
                dc.DrawGeometry(Brushes.Magenta, null, sg);
            }
        }
    }
}