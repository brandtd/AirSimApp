using System;
using System.Windows;
using System.Windows.Controls;

namespace AirSimApp.Controls
{
    /// <summary>
    /// Interaction logic for OdometerDigit.xaml
    /// </summary>
    public partial class OdometerDigit : UserControl
    {
        /// <summary>
        /// Value for this digit of an odometer.
        /// </summary>
        public static DependencyProperty DigitProperty =
            DependencyProperty.Register(
                nameof(Digit),
                typeof(double),
                typeof(OdometerDigit),
                new PropertyMetadata(0.0, onValueChanged));

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

        /// <inheritdoc cref="DigitProperty"/>
        public double Digit
        {
            get => (double)GetValue(DigitProperty);
            set => SetValue(DigitProperty, value);
        }

        /// <summary>
        /// Create control.
        /// </summary>
        public OdometerDigit()
        {
            InitializeComponent();
        }
    }
}