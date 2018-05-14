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
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace AirSimApp
{
    /// <summary>View model for a vehicle.</summary>
    public class VehicleViewModel : PropertyChangedBase, IDisposable
    {
        /// <summary>Wire up view model.</summary>
        public VehicleViewModel(MultirotorVehicleModel model)
        {
            _model = model;
            _model.PropertyChanged += onModelPropertyChanged;

            _armCommand = new ArmCommand(_model);
            _disableApiCommand = new DisableApiControlCommand(_model);
            _disarmCommand = new DisarmCommand(_model);
            _enableApiCommand = new EnableApiControlCommand(_model);
            _goHomeCommand = new GoHomeCommand(_model);
            _hoverInPlaceCommand = new HoverInPlaceCommand(_model);
            _landNowCommand = new LandNowCommand(_model);
            _resetCommand = new ResetCommand(_model);
            _takeoffCommand = new TakeoffCommand(_model);
        }

        /// <summary>Arm motors.</summary>
        public ICommand ArmCommand => _armCommand;

        /// <summary>Disable RPC API (API must be enabled for any commands to work).</summary>
        public ICommand DisableApiCommand => _disableApiCommand;

        /// <summary>Disarm motors.</summary>
        public ICommand DisarmCommand => _disarmCommand;

        /// <summary>Enable RPC API (API must be enabled for any commands to work).</summary>
        public ICommand EnableApiCommand => _enableApiCommand;

        /// <summary>Command vehicle to go home/recover.</summary>
        public ICommand GoHomeCommand => _goHomeCommand;

        /// <summary>Vehicle's GPS altitude.</summary>
        public Distance GpsAltitude => _model.VehicleLocationGps.Altitude;

        /// <summary>Vehicle's GPS latitude.</summary>
        public Latitude GpsLatitude => _model.VehicleLocationGps.Latitude;

        /// <summary>Vehicle's GPS longitude.</summary>
        public Longitude GpsLongitude => _model.VehicleLocationGps.Longitude;

        /// <summary>Home point altitude.</summary>
        public Distance HomeAltitude => _model.HomeLocation.Altitude;

        /// <summary>Home point latitude.</summary>
        public Latitude HomeLatitude => _model.HomeLocation.Latitude;

        /// <summary>Home point longitude.</summary>
        public Longitude HomeLongitude => _model.HomeLocation.Longitude;

        /// <summary>Command vehicle to hover in place.</summary>
        public ICommand HoverInPlaceCommand => _hoverInPlaceCommand;

        /// <summary>Are we able to control the vehicle via the API?</summary>
        public bool IsApiControlEnabled => _model.ApiEnabled;

        /// <summary>Command vehicle to land now.</summary>
        public ICommand LandNowCommand => _landNowCommand;

        /// <summary>Vehicle's current operating mode.</summary>
        public VehicleMode Mode => _model.Mode;

        /// <summary>Command simulator to reset.</summary>
        public ICommand ResetCommand => _resetCommand;

        /// <summary>Takeoff and hover.</summary>
        public ICommand TakeoffCommand => _takeoffCommand;

        /// <summary>Vehicle altitude.</summary>
        public Distance VehicleAltitude => _model.VehicleLocation.Altitude;

        /// <summary>Vehicle latitude.</summary>
        public Latitude VehicleLatitude => _model.VehicleLocation.Latitude;

        /// <summary>Vehicle longitude.</summary>
        public Longitude VehicleLongitude => _model.VehicleLocation.Longitude;

        /// <summary>Vehicle pitch.</summary>
        public Angle VehiclePitch => _model.VehiclePitch;

        /// <summary>Vehicle roll.</summary>
        public Angle VehicleRoll => _model.VehicleRoll;

        /// <summary>Vehicle yaw.</summary>
        public Angle VehicleYaw => _model.VehicleYaw;

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                _model.PropertyChanged -= onModelPropertyChanged;

                _takeoffCommand.Dispose();
                _resetCommand.Dispose();
                _landNowCommand.Dispose();
                _hoverInPlaceCommand.Dispose();
                _goHomeCommand.Dispose();
                _enableApiCommand.Dispose();
                _disarmCommand.Dispose();
                _disableApiCommand.Dispose();
                _armCommand.Dispose();
            }
        }

        private readonly ArmCommand _armCommand;
        private readonly DisableApiControlCommand _disableApiCommand;
        private readonly DisarmCommand _disarmCommand;
        private readonly EnableApiControlCommand _enableApiCommand;
        private readonly GoHomeCommand _goHomeCommand;
        private readonly HoverInPlaceCommand _hoverInPlaceCommand;
        private readonly LandNowCommand _landNowCommand;
        private readonly MultirotorVehicleModel _model;
        private readonly ResetCommand _resetCommand;
        private readonly TakeoffCommand _takeoffCommand;
        private bool _disposed = false;

        private void onModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_model.HomeLocation):
                    OnPropertyChanged(nameof(HomeLatitude));
                    OnPropertyChanged(nameof(HomeLongitude));
                    OnPropertyChanged(nameof(HomeAltitude));
                    break;

                case nameof(_model.VehicleLocation):
                    OnPropertyChanged(nameof(VehicleLatitude));
                    OnPropertyChanged(nameof(VehicleLongitude));
                    OnPropertyChanged(nameof(VehicleAltitude));
                    break;

                case nameof(_model.VehicleLocationGps):
                    OnPropertyChanged(nameof(GpsLatitude));
                    OnPropertyChanged(nameof(GpsLongitude));
                    OnPropertyChanged(nameof(GpsAltitude));
                    break;

                case nameof(_model.ApiEnabled):
                    OnPropertyChanged(nameof(IsApiControlEnabled));
                    break;

                case nameof(_model.VehicleRoll):
                    OnPropertyChanged(nameof(VehicleRoll));
                    break;

                case nameof(_model.VehiclePitch):
                    OnPropertyChanged(nameof(VehiclePitch));
                    break;

                case nameof(_model.VehicleYaw):
                    OnPropertyChanged(nameof(VehicleYaw));
                    break;

                case nameof(_model.Mode):
                    OnPropertyChanged(nameof(Mode));
                    break;
            }
        }
    }
}