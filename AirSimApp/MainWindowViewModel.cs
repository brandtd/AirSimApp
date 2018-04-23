#region MIT License (c) 2018

// Copyright 2018 Dan Brandt
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#endregion

using AirSimApp.Commands;
using DotSpatial.Positioning;
using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;

namespace AirSimApp
{
    public class MainWindowViewModel : PropertyChangedBase, IDisposable
    {
        private readonly ProxyController _controller;
        private readonly ProxyViewModel _proxyViewModel;
        private readonly VehicleViewModel _vehicleViewModel;
        private readonly GoHomeCommand _goHomeCommand;
        private readonly HoverInPlaceCommand _hoverInPlaceCommand;
        private readonly LandNowCommand _landNowCommand;
        private readonly ResetCommand _resetCommand;
        private readonly EnableApiControlCommand _enableApiCommand;
        private readonly DisableApiControlCommand _disableApiCommand;

        private bool _disposed = false;

        public ICommand ConnectCommand => _proxyViewModel.Connect;
        public ICommand GoHomeCommand => _goHomeCommand;
        public ICommand HoverInPlaceCommand => _hoverInPlaceCommand;
        public ICommand LandNowCommand => _landNowCommand;
        public ICommand ResetCommand => _resetCommand;
        public ICommand EnableApiCommand => _enableApiCommand;
        public ICommand DisableApiCommand => _disableApiCommand;
        public IPAddress AddressToUse { get => _proxyViewModel.AddressToUse; set => _proxyViewModel.AddressToUse = value; }
        public ushort PortToUse { get => _proxyViewModel.PortToUse; set => _proxyViewModel.PortToUse = value; }
        public IPAddress ConnectedAddress { get => _proxyViewModel.ConnectedAddress; set => _proxyViewModel.ConnectedAddress = value; }
        public ushort ConnectedPort { get => _proxyViewModel.ConnectedPort; set => _proxyViewModel.ConnectedPort = value; }

        public Latitude GpsLatitude { get => _vehicleViewModel.GpsLatitude; set => _vehicleViewModel.GpsLatitude = value; }
        public Longitude GpsLongitude { get => _vehicleViewModel.GpsLongitude; set => _vehicleViewModel.GpsLongitude = value; }
        public Distance GpsAltitude { get => _vehicleViewModel.GpsAltitude; set => _vehicleViewModel.GpsAltitude = value; }
        public Latitude HomeLatitude { get => _vehicleViewModel.HomeLatitude; set => _vehicleViewModel.HomeLatitude = value; }
        public Longitude HomeLongitude { get => _vehicleViewModel.HomeLongitude; set => _vehicleViewModel.HomeLongitude = value; }
        public Distance HomeAltitude { get => _vehicleViewModel.HomeAltitude; set => _vehicleViewModel.HomeAltitude = value; }
        public Latitude VehicleLatitude { get => _vehicleViewModel.VehicleLatitude; set => _vehicleViewModel.VehicleLatitude = value; }
        public Longitude VehicleLongitude { get => _vehicleViewModel.VehicleLongitude; set => _vehicleViewModel.VehicleLongitude = value; }
        public Distance VehicleAltitude { get => _vehicleViewModel.VehicleAltitude; set => _vehicleViewModel.VehicleAltitude = value; }
        public Angle VehicleRoll { get => _vehicleViewModel.VehicleRoll; set => _vehicleViewModel.VehicleRoll = value; }
        public Angle VehiclePitch { get => _vehicleViewModel.VehiclePitch; set => _vehicleViewModel.VehiclePitch = value; }
        public Angle VehicleYaw { get => _vehicleViewModel.VehicleYaw; set => _vehicleViewModel.VehicleYaw = value; }

        public MainWindowViewModel()
        {
            _controller = new ProxyController();
            _disableApiCommand = new DisableApiControlCommand(_controller);
            _enableApiCommand = new EnableApiControlCommand(_controller);
            _goHomeCommand = new GoHomeCommand(_controller);
            _hoverInPlaceCommand = new HoverInPlaceCommand(_controller);
            _landNowCommand = new LandNowCommand(_controller);
            _resetCommand = new ResetCommand(_controller);
            _proxyViewModel = new ProxyViewModel(_controller);
            _vehicleViewModel = new VehicleViewModel(_controller);

            _proxyViewModel.PropertyChanged += onProxyViewModelPropertyChanged;
            _vehicleViewModel.PropertyChanged += onVehicleViewModelPropertyChanged;
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                _proxyViewModel.PropertyChanged -= onProxyViewModelPropertyChanged;
                _vehicleViewModel.PropertyChanged -= onVehicleViewModelPropertyChanged;

                _vehicleViewModel.Dispose();
                _proxyViewModel.Dispose();
                _resetCommand.Dispose();
                _landNowCommand.Dispose();
                _hoverInPlaceCommand.Dispose();
                _goHomeCommand.Dispose();
                _enableApiCommand.Dispose();
                _disableApiCommand.Dispose();
                _controller.Dispose();
            }
        }

        private void onVehicleViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
            case nameof(_vehicleViewModel.GpsLatitude):
                OnPropertyChanged(nameof(GpsLatitude));
                break;

            case nameof(_vehicleViewModel.GpsLongitude):
                OnPropertyChanged(nameof(GpsLongitude));
                break;

            case nameof(_vehicleViewModel.GpsAltitude):
                OnPropertyChanged(nameof(GpsAltitude));
                break;

            case nameof(_vehicleViewModel.HomeLatitude):
                OnPropertyChanged(nameof(HomeLatitude));
                break;

            case nameof(_vehicleViewModel.HomeLongitude):
                OnPropertyChanged(nameof(HomeLongitude));
                break;

            case nameof(_vehicleViewModel.HomeAltitude):
                OnPropertyChanged(nameof(HomeAltitude));
                break;

            case nameof(_vehicleViewModel.VehicleLatitude):
                OnPropertyChanged(nameof(VehicleLatitude));
                break;

            case nameof(_vehicleViewModel.VehicleLongitude):
                OnPropertyChanged(nameof(VehicleLongitude));
                break;

            case nameof(_vehicleViewModel.VehicleAltitude):
                OnPropertyChanged(nameof(VehicleAltitude));
                break;

            case nameof(_vehicleViewModel.VehicleRoll):
                OnPropertyChanged(nameof(VehicleRoll));
                break;

            case nameof(_vehicleViewModel.VehiclePitch):
                OnPropertyChanged(nameof(VehiclePitch));
                break;

            case nameof(_vehicleViewModel.VehicleYaw):
                OnPropertyChanged(nameof(VehicleYaw));
                break;
            }
        }

        private void onProxyViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
            case nameof(_proxyViewModel.AddressToUse):
                OnPropertyChanged(nameof(AddressToUse));
                break;

            case nameof(_proxyViewModel.PortToUse):
                OnPropertyChanged(nameof(PortToUse));
                break;

            case nameof(_proxyViewModel.ConnectedAddress):
                OnPropertyChanged(nameof(ConnectedAddress));
                break;

            case nameof(_proxyViewModel.ConnectedPort):
                OnPropertyChanged(nameof(ConnectedPort));
                break;
            }
        }
    }
}
