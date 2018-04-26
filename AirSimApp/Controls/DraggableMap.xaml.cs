using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MapControl;

namespace AirSimApp.Controls
{
    /// <summary>Interaction logic for DraggableMap.xaml</summary>
    public partial class DraggableMap : Map
    {
        public DraggableMap()
        {
            InitializeComponent();
        }

        private void MapItemTouchDown(object sender, TouchEventArgs e)
        {
            var mapItem = (MapItem)sender;
            mapItem.IsSelected = !mapItem.IsSelected;
            e.Handled = true;
        }

        private void MapManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            e.TranslationBehavior.DesiredDeceleration = 0.001;
        }

        private void MapMouseLeave(object sender, MouseEventArgs e)
        {
            //mouseLocation.Text = string.Empty;
        }

        private void MapMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                //map.ZoomMap(e.GetPosition(map), Math.Floor(map.ZoomLevel + 1.5));
                //map.ZoomToBounds(new BoundingBox(53, 7, 54, 9));
                TargetCenter = ViewportPointToLocation(e.GetPosition(this));
            }
        }

        private void MapMouseMove(object sender, MouseEventArgs e)
        {
            var location = ViewportPointToLocation(e.GetPosition(this));
            var latitude = (int)Math.Round(location.Latitude * 60000d);
            var longitude = (int)Math.Round(Location.NormalizeLongitude(location.Longitude) * 60000d);
            var latHemisphere = 'N';
            var lonHemisphere = 'E';

            if (latitude < 0)
            {
                latitude = -latitude;
                latHemisphere = 'S';
            }

            if (longitude < 0)
            {
                longitude = -longitude;
                lonHemisphere = 'W';
            }

            //mouseLocation.Text = string.Format(CultureInfo.InvariantCulture,
            //    "{0}  {1:00} {2:00.000}\n{3} {4:000} {5:00.000}",
            //    latHemisphere, latitude / 60000, (latitude % 60000) / 1000d,
            //    lonHemisphere, longitude / 60000, (longitude % 60000) / 1000d);
        }

        private void MapMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                //map.ZoomMap(e.GetPosition(map), Math.Ceiling(map.ZoomLevel - 1.5));
            }
        }
    }
}