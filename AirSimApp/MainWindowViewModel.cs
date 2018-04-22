#region MIT License

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

        private bool _disposed = false;

        public ICommand Connect => _proxyViewModel.Connect;
        public IPAddress AddressToUse { get => _proxyViewModel.AddressToUse; set => _proxyViewModel.AddressToUse = value; }
        public ushort PortToUse { get => _proxyViewModel.PortToUse; set => _proxyViewModel.PortToUse = value; }
        public IPAddress ConnectedAddress { get => _proxyViewModel.ConnectedAddress; set => _proxyViewModel.ConnectedAddress = value; }
        public ushort ConnectedPort { get => _proxyViewModel.ConnectedPort; set => _proxyViewModel.ConnectedPort = value; }

        public double HomeLatitude { get => _vehicleViewModel.HomeLatitude; set => _vehicleViewModel.HomeLatitude = value; }
        public double HomeLongitude { get => _vehicleViewModel.HomeLongitude; set => _vehicleViewModel.HomeLongitude = value; }
        public double HomeAltitude { get => _vehicleViewModel.HomeAltitude; set => _vehicleViewModel.HomeAltitude = value; }
        public double VehicleLatitude { get => _vehicleViewModel.VehicleLatitude; set => _vehicleViewModel.VehicleLatitude = value; }
        public double VehicleLongitude { get => _vehicleViewModel.VehicleLongitude; set => _vehicleViewModel.VehicleLongitude = value; }
        public double VehicleAltitude { get => _vehicleViewModel.VehicleAltitude; set => _vehicleViewModel.VehicleAltitude = value; }
        public double VehicleRoll { get => _vehicleViewModel.VehicleRoll; set => _vehicleViewModel.VehicleRoll = value; }
        public double VehiclePitch { get => _vehicleViewModel.VehiclePitch; set => _vehicleViewModel.VehiclePitch = value; }
        public double VehicleYaw { get => _vehicleViewModel.VehicleYaw; set => _vehicleViewModel.VehicleYaw = value; }

        public MainWindowViewModel()
        {
            _controller = new ProxyController();
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
            }
        }

        private void onVehicleViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
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
