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

namespace AirSimApp.Controls
{
    /// <summary>Interaction logic for SpeedTape.xaml</summary>
    public partial class SpeedTape : UserControl
    {
        /// <summary>Vehicle's actual altitude.</summary>
        public static readonly DependencyProperty ActualSpeedProperty =
            DependencyProperty.Register(
                nameof(ActualSpeed),
                typeof(Speed),
                typeof(SpeedTape),
                new PropertyMetadata(Speed.Invalid, onActualSpeedChanged));

        /// <summary>Vehicle's commanded altitude.</summary>
        public static readonly DependencyProperty CommandedSpeedProperty =
            DependencyProperty.Register(
                nameof(CommandedSpeed),
                typeof(Speed),
                typeof(SpeedTape),
                new PropertyMetadata(Speed.Invalid));

        /// <summary>The size of a major tick.</summary>
        public static readonly DependencyProperty MajorTickProperty =
            DependencyProperty.Register(
                nameof(MajorTick),
                typeof(Speed),
                typeof(SpeedTape),
                new PropertyMetadata(Speed.FromStatuteMilesPerHour(5)));

        /// <summary>Resolution of the displayed odometer.</summary>
        public static readonly DependencyProperty OdometerResolutionProperty =
            DependencyProperty.Register(
                nameof(OdometerResolution),
                typeof(OdometerResolution),
                typeof(SpeedTape),
                new PropertyMetadata(OdometerResolution.R1));

        /// <summary>The total range of the tape.</summary>
        public static readonly DependencyProperty RangeProperty =
            DependencyProperty.Register(
                nameof(Range),
                typeof(Speed),
                typeof(SpeedTape),
                new PropertyMetadata(Speed.FromStatuteMilesPerHour(50)));

        /// <summary>Whether to draw ticks on the left or right side of the control.</summary>
        public static readonly DependencyProperty RightOrLeftProperty =
            DependencyProperty.Register(
                nameof(RightOrLeft),
                typeof(HorizontalAlignment),
                typeof(SpeedTape),
                new PropertyMetadata(HorizontalAlignment.Right));

        /// <summary>The units to use for the tape.</summary>
        public static readonly DependencyProperty SpeedUnitsProperty =
            DependencyProperty.Register(
                nameof(SpeedUnits),
                typeof(SpeedUnit),
                typeof(SpeedTape),
                new PropertyMetadata(SpeedUnit.MetersPerSecond));

        public SpeedTape()
        {
            InitializeComponent();
        }

        /// <inheritdoc cref="ActualSpeedProperty" />
        public Speed ActualSpeed
        {
            get => (Speed)GetValue(ActualSpeedProperty);
            set => SetValue(ActualSpeedProperty, value);
        }

        /// <inheritdoc cref="CommandedSpeedProperty" />
        public Speed CommandedSpeed
        {
            get => (Speed)GetValue(CommandedSpeedProperty);
            set => SetValue(CommandedSpeedProperty, value);
        }

        /// <inheritdoc cref="MajorTickProperty" />
        public Speed MajorTick
        {
            get => (Speed)GetValue(MajorTickProperty);
            set => SetValue(MajorTickProperty, value);
        }

        /// <inheritdoc cref="OdometerResolutionProperty" />
        public OdometerResolution OdometerResolution
        {
            get => (OdometerResolution)GetValue(OdometerResolutionProperty);
            set => SetValue(OdometerResolutionProperty, value);
        }

        /// <inheritdoc cref="RangeProperty" />
        public Speed Range
        {
            get => (Speed)GetValue(RangeProperty);
            set => SetValue(RangeProperty, value);
        }

        /// <inheritdoc cref="RightOrLeftProperty" />
        public HorizontalAlignment RightOrLeft
        {
            get => (HorizontalAlignment)GetValue(RightOrLeftProperty);
            set => SetValue(RightOrLeftProperty, value);
        }

        /// <inheritdoc cref="SpeedUnitsProperty" />
        public SpeedUnit SpeedUnits
        {
            get => (SpeedUnit)GetValue(SpeedUnitsProperty);
            set => SetValue(SpeedUnitsProperty, value);
        }

        private static void onActualSpeedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SpeedTape control = (SpeedTape)d;
            if (control.ActualSpeed.IsInvalid)
            {
                control.odometer.Value = 0;
            }
            else
            {
                control.odometer.Value = control.ActualSpeed.In(control.SpeedUnits);
            }
        }
    }
}