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

using AirSimApp.Commands;
using AirSimApp.Models;
using Db.Controls;
using DotSpatial.Positioning;
using MapControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace AirSimApp.ViewModels
{
    /// <summary>View model for map.</summary>
    public class MapViewModel : PropertyChangedBase, IDisposable
    {
        /// <summary>Wire up to model.</summary>
        public MapViewModel(MultirotorVehicleModel vehicle)
        {
            _vehicle = vehicle;
            _vehicle.PropertyChanged += onVehiclePropertyChanged;

            _altitudeCommand = new AltitudeCommand(_vehicle);
            _gotoCommand = new GoToCommand(_vehicle);
        }

        /// <summary>Commands vehicle to an altitude.</summary>
        public ICommand AltitudeCommand => _altitudeCommand;

        /// <summary>Altitude ticks to show on altitude slider.</summary>
        public IEnumerable<Distance> AltitudeTicks { get; } = new Distance[] {
            Distance.FromMeters(0),
            Distance.FromMeters(100),
            Distance.FromMeters(200),
            Distance.FromMeters(300),
            Distance.FromMeters(400),
            Distance.FromMeters(500),
            Distance.FromMeters(600),
            Distance.FromMeters(700),
            Distance.FromMeters(800),
            Distance.FromMeters(900),
            Distance.FromMeters(1000)
        };

        /// <summary>Vehicle's commanded altitude.</summary>
        public Distance CommandedAltitude { get => _commandedAltitude; set => SetProperty(ref _commandedAltitude, value); }

        /// <summary>Vehicle's commanded speed.</summary>
        public Speed CommandedSpeed { get => _commandedSpeed; set => SetProperty(ref _commandedSpeed, value); }

        /// <summary>Commands vehicle to a location.</summary>
        public ICommand GoToCommand => _gotoCommand;

        /// <summary>Whether we have a vehicle to display.</summary>
        public bool HaveVehicle { get => _haveVehicle; set => SetProperty(ref _haveVehicle, value); }

        /// <summary>Vehicle's home location.</summary>
        public Position Home { get => _home; set => SetProperty(ref _home, value); }

        /// <summary>Center location of map.</summary>
        public Position MapCenter { get => _center; set => SetProperty(ref _center, value); }

        /// <summary>The underlaying map layer.</summary>
        public UIElement MapLayer => _mapLayers[_mapLayerName];

        /// <summary>Underlaying map layer.</summary>
        public string MapLayerName
        {
            get => _mapLayerName; set
            {
                if (_mapLayerName != value && _mapLayers.ContainsKey(value))
                {
                    _mapLayerName = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(MapLayer));
                }
            }
        }

        /// <summary>All map layer names.</summary>
        public IEnumerable<string> MapLayerNames => _mapLayers.Keys;

        /// <summary>Max altitude to show on altitude slider.</summary>
        public Distance MaxAltitude { get; } = Distance.FromMeters(1000);

        /// <summary>Min altitude to show on altitude slider.</summary>
        public Distance MinAltitude { get; } = Distance.FromMeters(0);

        /// <summary>Projection type to use.</summary>
        public MapProjection Projection { get; } = new WebMercatorProjection();

        /// <summary>Vehicle's altitude.</summary>
        public Distance VehicleAltitude { get => _vehicleAltitude; set => SetProperty(ref _vehicleAltitude, value); }

        /// <summary>Vehicle's yaw/heading.</summary>
        public Angle VehicleHeading { get => _vehicleHeading; set => SetProperty(ref _vehicleHeading, value); }

        /// <summary>Vehicle location.</summary>
        public Position VehicleLocation { get => _vehicleLocation; set => SetProperty(ref _vehicleLocation, value); }

        /// <summary>Vehicle speed (in the X/Y or North/East plane).</summary>
        public Speed VehicleSpeed { get => _vehicleSpeed; set => SetProperty(ref _vehicleSpeed, value); }

        /// <summary>Map's zoom level.</summary>
        public double Zoom { get => _zoom; set => SetProperty(ref _zoom, value); }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!_disposed)
            {
                _vehicle.PropertyChanged -= onVehiclePropertyChanged;
                _altitudeCommand.Dispose();
                _gotoCommand.Dispose();
            }
        }

        private readonly Dictionary<string, UIElement> _mapLayers = new Dictionary<string, UIElement>
        {
            {
                "OpenStreetMap",
                MapTileLayer.OpenStreetMapTileLayer
            },
            {
                "OpenStreetMap German",
                new MapTileLayer
                {
                    SourceName = "OpenStreetMap German",
                    Description = "© [OpenStreetMap contributors](http://www.openstreetmap.org/copyright)",
                    TileSource = new TileSource { UriFormat = "http://{c}.tile.openstreetmap.de/tiles/osmde/{z}/{x}/{y}.png" },
                    MaxZoomLevel = 19
                }
            },
            {
                "Stamen Terrain",
                new MapTileLayer
                {
                    SourceName = "Stamen Terrain",
                    Description = "Map tiles by [Stamen Design](http://stamen.com/), under [CC BY 3.0](http://creativecommons.org/licenses/by/3.0)\nData by [OpenStreetMap](http://openstreetmap.org/), under [ODbL](http://www.openstreetmap.org/copyright)",
                    TileSource = new TileSource { UriFormat = "http://tile.stamen.com/terrain/{z}/{x}/{y}.png" },
                    MaxZoomLevel = 17
                }
            },
            {
                "Stamen Toner Light",
                new MapTileLayer
                {
                    SourceName = "Stamen Toner Light",
                    Description = "Map tiles by [Stamen Design](http://stamen.com/), under [CC BY 3.0](http://creativecommons.org/licenses/by/3.0)\nData by [OpenStreetMap](http://openstreetmap.org/), under [ODbL](http://www.openstreetmap.org/copyright)",
                    TileSource = new TileSource { UriFormat = "http://tile.stamen.com/toner-lite/{z}/{x}/{y}.png" },
                    MaxZoomLevel = 18
                }
            },
            {
                "Seamarks",
                new MapTileLayer
                {
                    SourceName = "OpenSeaMap",
                    TileSource = new TileSource { UriFormat = "http://tiles.openseamap.org/seamark/{z}/{x}/{y}.png" },
                    MinZoomLevel = 9,
                    MaxZoomLevel = 18
                }
            },
            {
                "OpenStreetMap WMS",
                new WmsImageLayer
                {
                    Description = "© [terrestris GmbH & Co. KG](http://ows.terrestris.de/)\nData © [OpenStreetMap contributors](http://www.openstreetmap.org/copyright)",
                    ServerUri = new Uri("http://ows.terrestris.de/osm/service"),
                    Layers = "OSM-WMS"
                }
            },
            {
                "OpenStreetMap TOPO WMS",
                new WmsImageLayer
                {
                    Description = "© [terrestris GmbH & Co. KG](http://ows.terrestris.de/)\nData © [OpenStreetMap contributors](http://www.openstreetmap.org/copyright)",
                    ServerUri = new Uri("http://ows.terrestris.de/osm/service"),
                    Layers = "TOPO-OSM-WMS"
                }
            },
            {
                "SevenCs ChartServer",
                new WmsImageLayer
                {
                    Description = "© [SevenCs GmbH](http://www.sevencs.com)",
                    ServerUri = new Uri("http://chartserver4.sevencs.com:8080"),
                    Layers = "ENC",
                    MaxBoundingBoxWidth = 360
                }
            }
        };

        private readonly MultirotorVehicleModel _vehicle;
        private AltitudeCommand _altitudeCommand;
        private Position _center = new Position(new Latitude(47.639666), new Longitude(-122.128245));
        private Distance _commandedAltitude = Distance.Invalid;
        private Speed _commandedSpeed = Speed.Invalid;
        private bool _disposed = false;
        private GoToCommand _gotoCommand;
        private bool _haveVehicle = false;
        private Position _home = Position.Invalid;
        private string _mapLayerName = "OpenStreetMap";
        private Distance _vehicleAltitude = Distance.Invalid;
        private Angle _vehicleHeading = Angle.Invalid;
        private Position _vehicleLocation = Position.Invalid;
        private Speed _vehicleSpeed = Speed.Invalid;
        private double _zoom = 15.0;

        private void onVehiclePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case (nameof(_vehicle.HomeLocation)):
                    Home = new Position(_vehicle.HomeLocation.Latitude, _vehicle.HomeLocation.Longitude);
                    break;

                case (nameof(_vehicle.VehicleLocation)):
                    MapCenter = new Position(_vehicle.VehicleLocation.Latitude, _vehicle.VehicleLocation.Longitude);
                    VehicleLocation = new Position(_vehicle.VehicleLocation.Latitude, _vehicle.VehicleLocation.Longitude);
                    VehicleAltitude = _vehicle.VehicleLocation.Altitude;
                    break;

                case (nameof(_vehicle.VehicleSpeed)):
                    VehicleSpeed = _vehicle.VehicleSpeed;
                    break;

                case (nameof(_vehicle.VehicleYaw)):
                    VehicleHeading = _vehicle.VehicleYaw;
                    break;

                case (nameof(_vehicle.Connected)):
                    HaveVehicle = _vehicle.Connected;
                    break;
            }
        }
    }
}