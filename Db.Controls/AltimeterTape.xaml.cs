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
using System.Windows;
using System.Windows.Controls;

namespace Db.Controls
{
    /// <summary>Interaction logic for Altimeter.xaml</summary>
    public partial class AltimeterTape : UserControl
    {
        /// <summary>Vehicle's actual altitude.</summary>
        public static readonly DependencyProperty ActualAltitudeProperty =
            DependencyProperty.Register(
                nameof(ActualAltitude),
                typeof(Distance),
                typeof(AltimeterTape),
                new PropertyMetadata(Distance.Invalid, onActualAltitudeChanged));

        /// <summary>Vehicle's commanded altitude.</summary>
        public static readonly DependencyProperty CommandedAltitudeProperty =
            DependencyProperty.Register(
                nameof(CommandedAltitude),
                typeof(Distance),
                typeof(AltimeterTape),
                new PropertyMetadata(Distance.Invalid));

        /// <summary>The units to use for the tape.</summary>
        public static readonly DependencyProperty DistanceUnitsProperty =
            DependencyProperty.Register(
                nameof(DistanceUnits),
                typeof(DistanceUnit),
                typeof(AltimeterTape),
                new PropertyMetadata(DistanceUnit.Meters));

        /// <summary>Resolution of the displayed odometer.</summary>
        public static readonly DependencyProperty OdometerResolutionProperty =
            DependencyProperty.Register(
                nameof(OdometerResolution),
                typeof(OdometerResolution),
                typeof(AltimeterTape),
                new PropertyMetadata(OdometerResolution.R1));

        /// <summary>Whether to draw ticks on the left or right side of the control.</summary>
        public static readonly DependencyProperty RightOrLeftProperty =
            DependencyProperty.Register(
                nameof(RightOrLeft),
                typeof(HorizontalAlignment),
                typeof(AltimeterTape),
                new PropertyMetadata(HorizontalAlignment.Left));

        public AltimeterTape()
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

        /// <inheritdoc cref="DistanceUnitsProperty" />
        public DistanceUnit DistanceUnits
        {
            get => (DistanceUnit)GetValue(DistanceUnitsProperty);
            set => SetValue(DistanceUnitsProperty, value);
        }

        /// <inheritdoc cref="OdometerResolutionProperty" />
        public OdometerResolution OdometerResolution
        {
            get => (OdometerResolution)GetValue(OdometerResolutionProperty);
            set => SetValue(OdometerResolutionProperty, value);
        }

        /// <inheritdoc cref="RightOrLeftProperty" />
        public HorizontalAlignment RightOrLeft
        {
            get => (HorizontalAlignment)GetValue(RightOrLeftProperty);
            set => SetValue(RightOrLeftProperty, value);
        }

        private static void onActualAltitudeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AltimeterTape control = (AltimeterTape)d;
            if (control.ActualAltitude.IsInvalid)
            {
                control.odometer.Value = 0;
            }
            else
            {
                control.odometer.Value = control.ActualAltitude.In(control.DistanceUnits);
            }
        }
    }
}
