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
    /// <summary>Interaction logic for ClickedOdometerDigit.xaml</summary>
    public partial class ClickedOdometerDigit : UserControl
    {
        /// <summary>Value for this digit of an odometer.</summary>
        public static DependencyProperty DigitProperty =
            DependencyProperty.Register(
                nameof(Digit),
                typeof(double),
                typeof(ClickedOdometerDigit),
                new PropertyMetadata(double.NaN, onValueChanged));

        /// <summary>Character used to fill label if digit is NaN.</summary>
        public static DependencyProperty FillCharacterProperty =
            DependencyProperty.Register(
                nameof(FillCharacter),
                typeof(char),
                typeof(ClickedOdometerDigit),
                new PropertyMetadata(' ', onValueChanged));

        /// <summary>Create control.</summary>
        public ClickedOdometerDigit()
        {
            InitializeComponent();
            setContent(this);
        }

        /// <inheritdoc cref="DigitProperty" />
        public double Digit
        {
            get => (double)GetValue(DigitProperty);
            set => SetValue(DigitProperty, value);
        }

        /// <inheritdoc cref="FillCharacterProperty" />
        public char FillCharacter
        {
            get => (char)GetValue(FillCharacterProperty);
            set => SetValue(FillCharacterProperty, value);
        }

        private static void onValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ClickedOdometerDigit control = (ClickedOdometerDigit)d;
            setContent(control);
        }

        private static void setContent(ClickedOdometerDigit control)
        {
            double value = control.Digit;
            if (double.IsNaN(value))
            {
                control.DigitLabel.Content = control.FillCharacter;
            }
            else
            {
                control.DigitLabel.Content = $"{Math.Floor(value)}";
            }
        }
    }
}