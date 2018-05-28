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
    /// <summary>Interaction logic for QuadCopterMapIcon.xaml</summary>
    public partial class QuadCopterMapIcon : UserControl
    {
        /// <summary>Whether a vehicle is connected.</summary>
        public static readonly DependencyProperty HaveVehicleProperty =
            DependencyProperty.Register(
                nameof(HaveVehicle),
                typeof(bool),
                typeof(QuadCopterMapIcon),
                new PropertyMetadata(false));

        /// <summary>Vehicle's current heading.</summary>
        public static readonly DependencyProperty VehicleHeadingProperty =
            DependencyProperty.Register(
                nameof(VehicleHeading),
                typeof(Angle),
                typeof(QuadCopterMapIcon),
                new PropertyMetadata(Angle.Invalid));

        /// <summary>Vehicle's current location.</summary>
        public static readonly DependencyProperty VehicleLocationProperty =
            DependencyProperty.Register(
                nameof(VehicleLocation),
                typeof(Position),
                typeof(QuadCopterMapIcon),
                new PropertyMetadata(Position.Invalid));

        public QuadCopterMapIcon()
        {
            InitializeComponent();
        }

        /// <inheritdoc cref="HaveVehicleProperty" />
        public bool HaveVehicle
        {
            get => (bool)GetValue(HaveVehicleProperty);
            set => SetValue(HaveVehicleProperty, value);
        }

        /// <inheritdoc cref="VehicleHeadingProperty" />
        public Angle VehicleHeading
        {
            get => (Angle)GetValue(VehicleHeadingProperty);
            set => SetValue(VehicleHeadingProperty, value);
        }

        /// <inheritdoc cref="VehicleLocationProperty" />
        public Position VehicleLocation
        {
            get => (Position)GetValue(VehicleLocationProperty);
            set => SetValue(VehicleLocationProperty, value);
        }
    }
}