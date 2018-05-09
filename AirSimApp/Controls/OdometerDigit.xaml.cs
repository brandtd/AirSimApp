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
using System.Windows;
using System.Windows.Controls;

namespace AirSimApp.Controls
{
    /// <summary>Interaction logic for OdometerDigit.xaml</summary>
    public partial class OdometerDigit : UserControl
    {
        /// <summary>Value for this digit of an odometer.</summary>
        public static DependencyProperty DigitProperty =
            DependencyProperty.Register(
                nameof(Digit),
                typeof(double),
                typeof(OdometerDigit),
                new PropertyMetadata(0.0, onValueChanged));

        /// <summary>Create control.</summary>
        public OdometerDigit()
        {
            InitializeComponent();
        }

        /// <inheritdoc cref="DigitProperty" />
        public double Digit
        {
            get => (double)GetValue(DigitProperty);
            set => SetValue(DigitProperty, value);
        }

        private static void onValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OdometerDigit control = (OdometerDigit)d;
            double value = (double)e.NewValue;

            int digitValue = (int)value;
            double blockHeight = control.DigitCenter.ActualHeight * 0.8;
            double translate = value - Math.Truncate(value);
            control.DigitFloater.Content = $"{Mod.CanonicalModulo((digitValue + 2), 10)}";
            control.DigitAbove.Content = $"{Mod.CanonicalModulo((digitValue + 1), 10)}";
            control.DigitCenter.Content = $"{digitValue}";
            control.DigitBelow.Content = $"{Mod.CanonicalModulo((digitValue - 1), 10)}";
            control.DigitFloaterY.Y = blockHeight * (-2 + translate);
            control.DigitAboveY.Y = blockHeight * (-1 + translate);
            control.DigitCenterY.Y = blockHeight * translate;
            control.DigitBelowY.Y = blockHeight * (1 + translate);
        }
    }
}