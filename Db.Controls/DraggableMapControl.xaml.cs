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
using MapControl;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Db.Controls
{
    /// <summary>Interaction logic for DraggableMapControl.xaml</summary>
    public partial class DraggableMapControl : UserControl
    {
        public static readonly DependencyProperty AltimeterCommandProperty =
            DependencyProperty.Register(
                nameof(AltimeterCommand),
                typeof(ICommand),
                typeof(DraggableMapControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty AltitudeTicksProperty =
                    DependencyProperty.Register(
                nameof(AltitudeTicks),
                typeof(IEnumerable<Distance>),
                typeof(DraggableMapControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ClickCommandProperty =
            DependencyProperty.Register(
                nameof(ClickCommand),
                typeof(ICommand),
                typeof(DraggableMapControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty CommandedAltitudeProperty =
            DependencyProperty.Register(
                nameof(CommandedAltitude),
                typeof(Distance),
                typeof(DraggableMapControl),
                new PropertyMetadata(Distance.Invalid,
                    (o, e) =>
                    {
                        ICommand command = ((DraggableMapControl)o).AltimeterCommand;
                        if (command != null && command.CanExecute(e.NewValue))
                        {
                            command.Execute(e.NewValue);
                        }
                    }));

        public static readonly DependencyProperty HaveVehicleProperty =
                    DependencyProperty.Register(
                nameof(HaveVehicle),
                typeof(bool),
                typeof(DraggableMapControl),
                new PropertyMetadata(false));

        public static readonly DependencyProperty LayerNameProperty =
            DependencyProperty.Register(
                nameof(LayerName),
                typeof(string),
                typeof(DraggableMapControl),
                new PropertyMetadata(""));

        public static readonly DependencyProperty LayerNamesProperty =
            DependencyProperty.Register(
                nameof(LayerNames),
                typeof(IEnumerable),
                typeof(DraggableMapControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MapCenterProperty =
            DependencyProperty.Register(
                nameof(MapCenter),
                typeof(Position),
                typeof(DraggableMapControl),
                new PropertyMetadata(Position.Invalid));

        public static readonly DependencyProperty MapLayerProperty =
            DependencyProperty.Register(
                nameof(MapLayer),
                typeof(UIElement),
                typeof(DraggableMapControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MaxAltitudeProperty =
            DependencyProperty.Register(
                nameof(MaxAltitude),
                typeof(Distance),
                typeof(DraggableMapControl),
                new PropertyMetadata(Distance.FromMeters(100)));

        public static readonly DependencyProperty MinAltitudeProperty =
            DependencyProperty.Register(
                nameof(MinAltitude),
                typeof(Distance),
                typeof(DraggableMapControl),
                new PropertyMetadata(Distance.FromMeters(0)));

        public static readonly DependencyProperty MouseLocationProperty =
            DependencyProperty.Register(
                nameof(MouseLocation),
                typeof(Position),
                typeof(DraggableMapControl),
                new PropertyMetadata(Position.Invalid));

        public static readonly DependencyProperty ShowDescriptionProperty =
            DependencyProperty.Register(
                nameof(ShowDescription),
                typeof(bool),
                typeof(DraggableMapControl),
                new PropertyMetadata(false));

        public static readonly DependencyProperty ShowExtrasProperty =
            DependencyProperty.Register(
                nameof(ShowExtras),
                typeof(bool),
                typeof(DraggableMapControl),
                new PropertyMetadata(false));

        public static readonly DependencyProperty VehicleAltitudeProperty =
            DependencyProperty.Register(
                nameof(VehicleAltitude),
                typeof(Distance),
                typeof(DraggableMapControl),
                new PropertyMetadata(Distance.Invalid));

        public static readonly DependencyProperty VehicleHeadingProperty =
            DependencyProperty.Register(
                nameof(VehicleHeading),
                typeof(Angle),
                typeof(DraggableMapControl),
                new PropertyMetadata(Angle.Invalid));

        public static readonly DependencyProperty VehicleLocationProperty =
            DependencyProperty.Register(
                nameof(VehicleLocation),
                typeof(Position),
                typeof(DraggableMapControl),
                new PropertyMetadata(Position.Invalid));

        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register(
                nameof(ZoomLevel),
                typeof(double),
                typeof(DraggableMapControl),
                new PropertyMetadata(1.0));

        public DraggableMapControl()
        {
            InitializeComponent();
        }

        public ICommand AltimeterCommand
        {
            get => (ICommand)GetValue(AltimeterCommandProperty);
            set => SetValue(AltimeterCommandProperty, value);
        }

        public IEnumerable<Distance> AltitudeTicks
        {
            get => (IEnumerable<Distance>)GetValue(AltitudeTicksProperty);
            set => SetValue(AltitudeTicksProperty, value);
        }

        public ICommand ClickCommand
        {
            get => (ICommand)GetValue(ClickCommandProperty);
            set => SetValue(ClickCommandProperty, value);
        }

        public Distance CommandedAltitude
        {
            get => (Distance)GetValue(CommandedAltitudeProperty);
            set => SetValue(CommandedAltitudeProperty, value);
        }

        public bool HaveVehicle
        {
            get => (bool)GetValue(HaveVehicleProperty);
            set => SetValue(HaveVehicleProperty, value);
        }

        public string LayerName
        {
            get => (string)GetValue(LayerNameProperty);
            set => SetValue(LayerNameProperty, value);
        }

        public IEnumerable LayerNames
        {
            get => (IEnumerable)GetValue(LayerNamesProperty);
            set => SetValue(LayerNamesProperty, value);
        }

        public Position MapCenter
        {
            get => (Position)GetValue(MapCenterProperty);
            set => SetValue(MapCenterProperty, value);
        }

        public UIElement MapLayer
        {
            get => (UIElement)GetValue(MapLayerProperty);
            set => SetValue(MapLayerProperty, value);
        }

        public Distance MaxAltitude
        {
            get => (Distance)GetValue(MaxAltitudeProperty);
            set => SetValue(MaxAltitudeProperty, value);
        }

        public Distance MinAltitude
        {
            get => (Distance)GetValue(MinAltitudeProperty);
            set => SetValue(MinAltitudeProperty, value);
        }

        public Position MouseLocation
        {
            get => (Position)GetValue(MouseLocationProperty);
            set => SetValue(MouseLocationProperty, value);
        }

        public bool ShowDescription
        {
            get => (bool)GetValue(ShowDescriptionProperty);
            set => SetValue(ShowDescriptionProperty, value);
        }

        public bool ShowExtras
        {
            get => (bool)GetValue(ShowExtrasProperty);
            set => SetValue(ShowExtrasProperty, value);
        }

        public Distance VehicleAltitude
        {
            get => (Distance)GetValue(VehicleAltitudeProperty);
            set => SetValue(VehicleAltitudeProperty, value);
        }

        public Angle VehicleHeading
        {
            get => (Angle)GetValue(VehicleHeadingProperty);
            set => SetValue(VehicleHeadingProperty, value);
        }

        public Position VehicleLocation
        {
            get => (Position)GetValue(VehicleLocationProperty);
            set => SetValue(VehicleLocationProperty, value);
        }

        public double ZoomLevel
        {
            get => (double)GetValue(ZoomLevelProperty);
            set => SetValue(ZoomLevelProperty, value);
        }

        private void mapItemTouchDown(object sender, TouchEventArgs e)
        {
            if (sender is MapItem mapItem)
            {
                mapItem.IsSelected = !mapItem.IsSelected;
                e.Handled = true;
            }
        }

        private void mapManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            e.TranslationBehavior.DesiredDeceleration = 0.001;
        }

        private void mapMouseLeave(object sender, MouseEventArgs e)
        {
            MouseLocation = Position.Invalid;
        }

        private void mapMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                map.TargetCenter = map.ViewportPointToLocation(e.GetPosition(this));
            }
        }

        private void mapMouseMove(object sender, MouseEventArgs e)
        {
            Location location = map.ViewportPointToLocation(e.GetPosition(map));
            MouseLocation = new Position(new Latitude(location.Latitude), new Longitude(location.Longitude));
        }

        private void mapMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                //map.ZoomMap(e.GetPosition(map), Math.Ceiling(map.ZoomLevel - 1.5));
            }
        }

        private void mapMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1 && !e.Handled)
            {
                ICommand command = ClickCommand;
                if (command != null && command.CanExecute(MouseLocation))
                {
                    command.Execute(MouseLocation);
                }
            }
        }
    }
}
