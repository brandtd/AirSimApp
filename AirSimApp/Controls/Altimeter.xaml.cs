using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using DotSpatial.Positioning;
using DotSpatialExtensions;

namespace AirSimApp.Controls
{
    /// <summary>Interaction logic for Altimeter.xaml</summary>
    public partial class Altimeter : UserControl
    {
        /// <summary>Vehicle's actual altitude.</summary>
        public static readonly DependencyProperty ActualAltitudeProperty =
            DependencyProperty.Register(
                nameof(ActualAltitude),
                typeof(Distance),
                typeof(Altimeter),
                new PropertyMetadata(Distance.Invalid, actualAltitudeChanged));

        /// <summary>Vehicle's commanded altitude.</summary>
        public static readonly DependencyProperty CommandedAltitudeProperty =
            DependencyProperty.Register(
                nameof(CommandedAltitude),
                typeof(Distance),
                typeof(Altimeter),
                new PropertyMetadata(Distance.Invalid));

        /// <summary>Distances at which to draw the major tick marks.</summary>
        public static readonly DependencyProperty MajorTicksProperty =
                    DependencyProperty.Register(
                        nameof(MajorTicks),
                        typeof(IEnumerable<Distance>),
                        typeof(Altimeter),
                        new PropertyMetadata(null, majorTicksChanged));

        /// <summary>Maximum displayed altitude.</summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                nameof(Maximum),
                typeof(Distance),
                typeof(Altimeter),
                new PropertyMetadata(Distance.FromMeters(400)));

        /// <summary>Minimum displayed altitude.</summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                nameof(Minimum),
                typeof(Distance),
                typeof(Altimeter),
                new PropertyMetadata(Distance.FromMeters(0)));

        /// <summary>Distances at which to draw the minor tick marks.</summary>
        public static readonly DependencyProperty MinorTicksProperty =
                    DependencyProperty.Register(
                        nameof(MinorTicks),
                        typeof(IEnumerable<Distance>),
                        typeof(Altimeter),
                        new PropertyMetadata(null, minorTicksChanged));

        public Altimeter()
        {
            InitializeComponent();
        }

        /// <inheritdoc cref="ActualAltitudeProperty" />
        public Distance ActualAltitude
        {
            get => (Distance)GetValue(ActualAltitudeProperty);
            set => SetValue(ActualAltitudeProperty, value);
        }

        /// <inheritdoc cref="CommandedAltitudeProperty" />
        public Distance CommandedAltitude
        {
            get => (Distance)GetValue(CommandedAltitudeProperty);
            set => SetValue(CommandedAltitudeProperty, value);
        }

        /// <inheritdoc cref="MajorTicksProperty" />
        public IEnumerable<Distance> MajorTicks
        {
            get => (IEnumerable<Distance>)GetValue(MajorTicksProperty);
            set => SetValue(MajorTicksProperty, value);
        }

        /// <inheritdoc cref="MaximumProperty" />
        public Distance Maximum
        {
            get => (Distance)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        /// <inheritdoc cref="MinimumProperty" />
        public Distance Minimum
        {
            get => (Distance)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        /// <inheritdoc cref="MinorTicksProperty" />
        public IEnumerable<Distance> MinorTicks
        {
            get => (IEnumerable<Distance>)GetValue(MinorTicksProperty);
            set => SetValue(MinorTicksProperty, value);
        }

        private static void actualAltitudeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Altimeter control = (Altimeter)d;
            double altitude = ((Distance)e.NewValue).InMeters();
            int altitudeAsInt = (int)altitude;

            int digitValue = altitudeAsInt % 10;
            int decadeValue = ((altitudeAsInt - digitValue) % 100) / 10;
            int centuryValue = ((altitudeAsInt - decadeValue - digitValue) % 1000) / 100;

            control.Century.Content = $"{centuryValue}".PadLeft(3, ' ');

            control.DecadeAbove.Content = $"{canonicalModulo((decadeValue + 1), 10)}";
            control.DecadeCenter.Content = $"{decadeValue}";
            control.DecadeBelow.Content = $"{canonicalModulo((decadeValue - 1), 10)}";

            control.DigitAbove.Content = $"{canonicalModulo((digitValue + 1), 10)}";
            control.DigitCenter.Content = $"{digitValue}";
            control.DigitBelow.Content = $"{canonicalModulo((digitValue - 1), 10)}";
            double digitFraction = altitude % 1.0;
        }

        private static int canonicalModulo(int dividend, int divisor)
        {
            int temp = dividend % divisor;
            return temp < 0 ? temp + divisor : temp;
        }

        private static double canonicalModulo(double dividend, double divisor)
        {
            double temp = dividend % divisor;
            return temp < 0 ? temp + divisor : temp;
        }

        private static void majorTicksChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Altimeter control = (Altimeter)d;
            INotifyCollectionChanged oldCollection = e.OldValue as INotifyCollectionChanged;
            INotifyCollectionChanged newCollection = e.NewValue as INotifyCollectionChanged;

            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= control.majorTicksCollectionChanged;
            }
            if (newCollection != null)
            {
                newCollection.CollectionChanged += control.majorTicksCollectionChanged;
            }
        }

        private static void minorTicksChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Altimeter control = (Altimeter)d;
            INotifyCollectionChanged oldCollection = e.OldValue as INotifyCollectionChanged;
            INotifyCollectionChanged newCollection = e.NewValue as INotifyCollectionChanged;

            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= control.minorTicksCollectionChanged;
            }
            if (newCollection != null)
            {
                newCollection.CollectionChanged += control.minorTicksCollectionChanged;
            }
        }

        private void majorTicksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void minorTicksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }
    }
}