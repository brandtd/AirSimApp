using System;
using System.Windows;
using System.Windows.Controls;

namespace AirSimApp.Controls
{
    /// <summary>
    /// Interaction logic for ClickedOdometerDigit.xaml
    /// </summary>
    public partial class ClickedOdometerDigit : UserControl
    {
        /// <summary>
        /// Value for this digit of an odometer.
        /// </summary>
        public static DependencyProperty DigitProperty =
            DependencyProperty.Register(
                nameof(Digit),
                typeof(double),
                typeof(ClickedOdometerDigit),
                new PropertyMetadata(0.0, onValueChanged));

        /// <summary>
        /// Character used to fill label if digit is NaN.
        /// </summary>
        public static DependencyProperty FillCharacterProperty =
            DependencyProperty.Register(
                nameof(FillCharacter),
                typeof(char),
                typeof(ClickedOdometerDigit),
                new PropertyMetadata(' ', onValueChanged));

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

        /// <inheritdoc cref="DigitProperty"/>
        public double Digit
        {
            get => (double)GetValue(DigitProperty);
            set => SetValue(DigitProperty, value);
        }

        /// <summary>
        /// Create control.
        /// </summary>
        public ClickedOdometerDigit()
        {
            InitializeComponent();
        }
    }
}