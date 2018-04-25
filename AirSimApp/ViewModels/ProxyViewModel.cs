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
using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;

namespace AirSimApp
{
    /// <summary>
    /// View model for an AirSim RPC proxy.
    /// </summary>
    public class ProxyViewModel : PropertyChangedBase, IDisposable
    {
        private readonly ConnectCommand _connectCommand;
        private readonly ProxyController _controller;

        private bool _disposed = false;

        /// <inheritdoc cref="ProxyModel.AddressToUse" />
        public IPAddress AddressToUse { get => _controller.AddressToUse; set => _controller.AddressToUse = value; }

        /// <inheritdoc cref="ProxyModel.PortToUse" />
        public ushort PortToUse { get => _controller.PortToUse; set => _controller.PortToUse = value; }

        /// <inheritdoc cref="ProxyModel.ConnectedAddress" />
        public IPAddress ConnectedAddress { get => _controller.ConnectedAddress; set => _controller.ConnectedAddress = value; }

        /// <inheritdoc cref="ProxyModel.ConnectedPort" />
        public ushort ConnectedPort { get => _controller.ConnectedPort; set => _controller.ConnectedPort = value; }

        /// <inheritdoc cref="ProxyModel.Connected" />
        public bool Connected { get => _controller.Connected; set => _controller.Connected = value; }

        /// <summary>
        /// Connect to RPC server.
        /// </summary>
        public ICommand Connect => _connectCommand;

        /// <summary>
        /// Wire up view model.
        /// </summary>
        public ProxyViewModel(ProxyController controller)
        {
            _controller = controller;
            _connectCommand = new ConnectCommand(_controller);

            _controller.PropertyChanged += onControllerPropertyChanged;
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                _controller.PropertyChanged -= onControllerPropertyChanged;

                _connectCommand.Dispose();
            }
        }

        private void onControllerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_controller.AddressToUse):
                    OnPropertyChanged(nameof(AddressToUse));
                    break;
                case nameof(_controller.PortToUse):
                    OnPropertyChanged(nameof(PortToUse));
                    break;
                case nameof(_controller.ConnectedAddress):
                    OnPropertyChanged(nameof(ConnectedAddress));
                    break;
                case nameof(_controller.ConnectedPort):
                    OnPropertyChanged(nameof(ConnectedPort));
                    break;
                case nameof(_controller.Connected):
                    OnPropertyChanged(nameof(Connected));
                    break;
            }
        }
    }
}
