using System.Windows;
using System.Windows.Controls;

namespace Db.Controls
{
    /// <summary>Interaction logic for Tape.xaml</summary>
    public partial class Tape : UserControl
    {
        /// <summary>Commanded value (for bug).</summary>
        public static readonly DependencyProperty CommandedValueProperty =
            DependencyProperty.Register(
                nameof(CommandedValue),
                typeof(double),
                typeof(Tape),
                new PropertyMetadata(double.NaN));

        /// <summary>Actual/current value.</summary>
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(
                nameof(CurrentValue),
                typeof(double),
                typeof(Tape),
                new PropertyMetadata(double.NaN));

        /// <summary>Number of divisions drawn per major tick.</summary>
        public static readonly DependencyProperty DivisionsPerTickProperty =
            DependencyProperty.Register(
                nameof(DivisionsPerTick),
                typeof(int),
                typeof(Tape),
                new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>How thick to make the major tick markings.</summary>
        public static readonly DependencyProperty MajorStrokeProperty =
            DependencyProperty.Register(
                nameof(MajorStroke),
                typeof(double),
                typeof(Tape),
                new FrameworkPropertyMetadata(1.5, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        ///     The size of a major tick described in the same units as the commanded/current value properties.
        /// </summary>
        public static readonly DependencyProperty MajorTickProperty =
            DependencyProperty.Register(
                nameof(MajorTick),
                typeof(double),
                typeof(Tape),
                new FrameworkPropertyMetadata(50.0, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>How thick to make the minor tick markings.</summary>
        public static readonly DependencyProperty MinorStrokeProperty =
            DependencyProperty.Register(
                nameof(MinorStroke),
                typeof(double),
                typeof(Tape),
                new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Resolution of the displayed odometer.</summary>
        public static readonly DependencyProperty OdometerResolutionProperty =
            DependencyProperty.Register(
                nameof(OdometerResolution),
                typeof(OdometerResolution),
                typeof(Tape),
                new PropertyMetadata(OdometerResolution.R1));

        /// <summary>The total range of the tape.</summary>
        public static readonly DependencyProperty RangeProperty =
            DependencyProperty.Register(
                nameof(Range),
                typeof(double),
                typeof(Tape),
                new FrameworkPropertyMetadata(200.0, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Whether to draw ticks on the left or right side of the control.</summary>
        public static readonly DependencyProperty RightOrLeftProperty =
            DependencyProperty.Register(
                nameof(RightOrLeft),
                typeof(HorizontalAlignment),
                typeof(Tape),
                new PropertyMetadata(HorizontalAlignment.Left));

        public Tape()
        {
            InitializeComponent();
        }

        /// <inheritdoc cref="CommandedAltitudeProperty" />
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

        /// <inheritdoc cref="OdometerResolutionProperty" />
        public OdometerResolution OdometerResolution
        {
            get => (OdometerResolution)GetValue(OdometerResolutionProperty);
            set => SetValue(OdometerResolutionProperty, value);
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
    }
}