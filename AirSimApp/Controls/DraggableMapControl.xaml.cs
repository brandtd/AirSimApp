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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirSimApp.Controls
{
    /// <summary>Interaction logic for DraggableMapControl.xaml</summary>
    public partial class DraggableMapControl : UserControl
    {
        public static readonly DependencyProperty MouseLocationProperty =
            DependencyProperty.Register(
                nameof(MouseLocation),
                typeof(Position),
                typeof(DraggableMapControl),
                new PropertyMetadata(new Position()));

        public DraggableMapControl()
        {
            InitializeComponent();
        }

        public Position MouseLocation
        {
            get { return (Position)GetValue(MouseLocationProperty); }
            set { SetValue(MouseLocationProperty, value); }
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
            //mouseLocation.Text = string.Empty;
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
            Location location = map.ViewportPointToLocation(e.GetPosition(this));
            MouseLocation = new Position(new Latitude(location.Latitude), new Longitude(location.Longitude));
        }

        private void mapMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                //map.ZoomMap(e.GetPosition(map), Math.Ceiling(map.ZoomLevel - 1.5));
            }
        }
    }
}