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
using System.Windows.Input;
using System.Windows.Media;

namespace Db.Controls
{
    /// <summary>Interaction logic for Tape.xaml</summary>
    public partial class Tape : UserControl
    {
        /// <summary>Brush to use when drawing the commanded value bug.</summary>
        public static readonly DependencyProperty CommandedBugBrushProperty =
            DependencyProperty.Register(
                nameof(CommandedBugBrush),
                typeof(Brush),
                typeof(Tape),
                new PropertyMetadata(GraduatedTapeBug.BugBrushProperty.DefaultMetadata.DefaultValue));

        /// <summary>Commanded value (for bug).</summary>
        public static readonly DependencyProperty CommandedValueProperty =
            DependencyProperty.Register(
                nameof(CommandedValue),
                typeof(double),
                typeof(Tape),
                new PropertyMetadata(double.NaN));

        /// <summary>Commits the pending command value.</summary>
        public static readonly DependencyProperty CommitPendingValueCommandProperty =
            DependencyProperty.Register(
                nameof(CommitPendingValueCommand),
                typeof(ICommand),
                typeof(Tape),
                new PropertyMetadata(null, onCommitPendingValueCommandChanged));

        /// <summary>Actual/current value.</summary>
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(
                nameof(CurrentValue),
                typeof(double),
                typeof(Tape),
                new PropertyMetadata(double.NaN, onCurrentValueChanged));

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

        /// <summary>Brush to use when drawing the pending value bug.</summary>
        public static readonly DependencyProperty PendingBugBrushProperty =
            DependencyProperty.Register(
                nameof(PendingBugBrush),
                typeof(Brush),
                typeof(Tape),
                new PropertyMetadata(GraduatedTapeBug.BugBrushProperty.DefaultMetadata.DefaultValue));

        /// <summary>
        ///     The pending command value (e.g., from the command bug being dragged around but not
        ///     yet released).
        /// </summary>
        public static readonly DependencyProperty PendingCommandValueProperty =
            DependencyProperty.Register(
                nameof(PendingCommandValue),
                typeof(double),
                typeof(Tape),
                new PropertyMetadata(double.NaN));

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

        /// <summary>
        ///     If set, defines the increment to be used if <see cref="SnapToIncrement" /> is <c>true</c>.
        /// </summary>
        public static readonly DependencyProperty SnapIncrementProperty =
            DependencyProperty.Register(
                nameof(SnapIncrement),
                typeof(double),
                typeof(Tape),
                new PropertyMetadata(double.NaN));

        /// <summary>
        ///     Whether command bug should snap to the nearest increment tick. If the SnapIncrement
        ///     is not set, the nearest major/minor tick will be used.
        /// </summary>
        public static readonly DependencyProperty SnapToIncrementProperty =
            DependencyProperty.Register(
                nameof(SnapToIncrement),
                typeof(bool),
                typeof(Tape),
                new PropertyMetadata(true));

        /// <summary>Whether pending value command can currently be executed.</summary>
        public static DependencyProperty CanExecutePendingValueCommandProperty =
            DependencyProperty.Register(
                nameof(CanExecutePendingValueCommand),
                typeof(bool),
                typeof(Tape),
                new PropertyMetadata(false));

        public Tape()
        {
            InitializeComponent();
        }

        /// <inheritdoc cref="CanExecutePendingValueCommandProperty" />
        public bool CanExecutePendingValueCommand
        {
            get => (bool)GetValue(CanExecutePendingValueCommandProperty);
            set => SetValue(CanExecutePendingValueCommandProperty, value);
        }

        /// <inheritdoc cref="CommandedBugBrushProperty" />
        public Brush CommandedBugBrush
        {
            get => (Brush)GetValue(CommandedBugBrushProperty);
            set => SetValue(CommandedBugBrushProperty, value);
        }

        /// <inheritdoc cref="CommandedValueProperty" />
        public double CommandedValue
        {
            get => (double)GetValue(CommandedValueProperty);
            set => SetValue(CommandedValueProperty, value);
        }

        /// <inheritdoc cref="CommitPendingValueCommandProperty" />
        public ICommand CommitPendingValueCommand
        {
            get => (ICommand)GetValue(CommitPendingValueCommandProperty);
            set => SetValue(CommitPendingValueCommandProperty, value);
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

        /// <inheritdoc cref="PendingBugBrushProperty" />
        public Brush PendingBugBrush
        {
            get => (Brush)GetValue(PendingBugBrushProperty);
            set => SetValue(PendingBugBrushProperty, value);
        }

        /// <inheritdoc cref="PendingCommandValueProperty" />
        public double PendingCommandValue
        {
            get => (double)GetValue(PendingCommandValueProperty);
            set => SetValue(PendingCommandValueProperty, value);
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

        /// <inheritdoc cref="SnapIncrementProperty" />
        public double SnapIncrement
        {
            get => (double)GetValue(SnapIncrementProperty);
            set => SetValue(SnapIncrementProperty, value);
        }

        /// <inheritdoc cref="SnapToIncrementProperty" />
        public bool SnapToIncrement
        {
            get => (bool)GetValue(SnapToIncrementProperty);
            set => SetValue(SnapToIncrementProperty, value);
        }

        private double _pendingCommandValueYPos = double.NaN;

        private double pendingCommandValueYPos
        {
            get => _pendingCommandValueYPos;
            set
            {
                if (_pendingCommandValueYPos != value)
                {
                    _pendingCommandValueYPos = value;
                    setPendingCommandValue();
                }
            }
        }

        private static void onCommitPendingValueCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Tape)d).commitPendingValueCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
        }

        private static void onCurrentValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Tape)d).setPendingCommandValue();
        }

        private void _this_MouseLeave(object sender, MouseEventArgs e)
        {
            cancelPendingValueCommand();
        }

        private void _this_MouseMove(object sender, MouseEventArgs e)
        {
            if (!double.IsNaN(_pendingCommandValueYPos))
            {
                pendingCommandValueYPos = e.GetPosition(this).Y;
            }
        }

        private void _this_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pendingCommandValueYPos = e.GetPosition(this).Y;

            e.Handled = true;
        }

        private void _this_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Escape))
            {
                cancelPendingValueCommand();
            }
            else if (!double.IsNaN(_pendingCommandValueYPos))
            {
                if (CommitPendingValueCommand?.CanExecute(null) == true)
                {
                    CommitPendingValueCommand.Execute(null);
                }
                pendingCommandValueYPos = double.NaN;
            }

            e.Handled = true;
        }

        private void cancelPendingValueCommand()
        {
            pendingCommandValueYPos = double.NaN;
        }

        private void commitPendingValueCommandChanged(ICommand oldCommand, ICommand newCommand)
        {
            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= onCommitCommandCanExecuteChanged;
            }

            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += onCommitCommandCanExecuteChanged;
                CanExecutePendingValueCommand = newCommand.CanExecute(null);
            }
            else
            {
                CanExecutePendingValueCommand = false;
            }
        }

        private void onCommitCommandCanExecuteChanged(object sender, EventArgs e)
        {
            CanExecutePendingValueCommand = CommitPendingValueCommand.CanExecute(null);
        }

        private void setPendingCommandValue()
        {
            if (!double.IsNaN(_pendingCommandValueYPos) && !double.IsNaN(CurrentValue))
            {
                double minValue = CurrentValue - Range / 2.0;
                double maxValue = CurrentValue + Range / 2.0;
                double b = maxValue;
                double m = (minValue - maxValue) / ActualHeight;

                double newPendingValue = m * _pendingCommandValueYPos + b;

                if (SnapToIncrement)
                {
                    double snapSize = !double.IsNaN(SnapIncrement) ? SnapIncrement : MajorTick / DivisionsPerTick;
                    newPendingValue = snapSize * Math.Round(newPendingValue / snapSize);
                }

                PendingCommandValue = newPendingValue;
            }
            else
            {
                PendingCommandValue = double.NaN;
            }
        }

        private void _this_LostFocus(object sender, RoutedEventArgs e)
        {
            cancelPendingValueCommand();
        }
    }
}