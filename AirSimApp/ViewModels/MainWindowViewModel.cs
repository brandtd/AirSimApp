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

using AirSimApp.Models;
using System;

namespace AirSimApp.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase, IDisposable
    {
        public MainWindowViewModel()
        {
            _controller = new ProxyController();
            _multirotorVehicle = new MultirotorVehicleModel(_controller);
            _rc = new RcModel(_controller);
            _camera = new CameraModel(_controller);

            Camera = new CameraViewModel(_camera);
            Map = new MapViewModel(_multirotorVehicle);
            Proxy = new ProxyViewModel(_controller);
            Rc = new RcViewModel(_rc);
            Vehicle = new VehicleViewModel(_multirotorVehicle);
        }

        public CameraViewModel Camera { get; }
        public MapViewModel Map { get; }
        public ProxyViewModel Proxy { get; }
        public RcViewModel Rc { get; }
        public VehicleViewModel Vehicle { get; }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Camera.Dispose();
                Map.Dispose();
                Proxy.Dispose();
                Rc.Dispose();
                Vehicle.Dispose();

                _camera.Dispose();
                _rc.Dispose();
                _multirotorVehicle.Dispose();
                _controller.Dispose();
            }
        }

        private readonly CameraModel _camera;
        private readonly ProxyController _controller;
        private readonly MultirotorVehicleModel _multirotorVehicle;
        private readonly RcModel _rc;

        private bool _disposed = false;
    }
}