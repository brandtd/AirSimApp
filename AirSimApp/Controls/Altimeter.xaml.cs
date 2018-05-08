using DotSpatial.Positioning;
using DotSpatialExtensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace AirSimApp.Controls
{
    /// <summary>Interaction logic for Altimeter.xaml</summary>
    public partial class Altimeter : UserControl
    {
        /// <summary>
        /// Units to use for altitude tape.
        /// </summary>
        public static readonly DependencyProperty UnitsProperty =
            DependencyProperty.Register(
                nameof(Units),
                typeof(DistanceUnit),
                typeof(Altimeter),
                new PropertyMetadata(DistanceUnit.Meters, onUnitsChanged));

        private static void onUnitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            setActualAltitude((Altimeter)d);
        }

        /// <inheritdoc cref="UnitsProperty"/>
        public DistanceUnit Units
        {
            get => (DistanceUnit)GetValue(UnitsProperty);
            set => SetValue(UnitsProperty, value);
        }

        /// <summary>Vehicle's actual altitude.</summary>
        public static readonly DependencyProperty ActualAltitudeProperty =
            DependencyProperty.Register(
                nameof(ActualAltitude),
                typeof(Distance),
                typeof(Altimeter),
                new PropertyMetadata(Distance.Invalid, onActualAltitudeChanged));

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

        private static void onActualAltitudeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            setActualAltitude((Altimeter)d);
        }

        private static void setActualAltitude(Altimeter control)
        {
            double altitude = control.ActualAltitude.In(control.Units);

            double digit0 = Mod.CanonicalModulo(altitude, 10);
            double digit1 = Mod.CanonicalModulo(altitude, 100) / 10;
            double digit2 = Mod.CanonicalModulo(altitude, 1000) / 100;
            double digit3 = Mod.CanonicalModulo(altitude, 10000) / 1000;
            double digit4 = Mod.CanonicalModulo(altitude, 100000) / 10000;

            control.Digit0.Digit = digit0;
            control.Digit1.Digit = digit1;
            control.Digit2.Digit = altitude > 100 ? digit2 : double.NaN;
            control.Digit3.Digit = altitude > 1000 ? digit3 : double.NaN;
            control.Digit4.Digit = altitude > 10000 ? digit4 : double.NaN;
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