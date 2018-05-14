﻿#region MIT License (c) 2018 Dan Brandt

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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;

namespace AirSimApp
{
    /// <summary>View model for an AirSim RPC proxy.</summary>
    public class ProxyViewModel : PropertyChangedBase, IDisposable
    {
        /// <summary>Wire up view model.</summary>
        public ProxyViewModel()
        {
            _controller = new ProxyController();
            _cameraProxyController = new ProxyController();

            _connectCommands = new List<ConnectCommand>
            {
                new ConnectCommand(_controller),
                new ConnectCommand(_cameraProxyController),
            };
            _connectCommand = new MultiCommand(_connectCommands);

            _cameraProxyController.PropertyChanged += onControllerPropertyChanged;
            _controller.PropertyChanged += onControllerPropertyChanged;
        }

        /// <inheritdoc cref="ProxyModel.AddressToUse" />
        public IPAddress AddressToUse
        {
            get => _controller.AddressToUse;
            set
            {
                _cameraProxyController.AddressToUse = value;
                _controller.AddressToUse = value;
            }
        }

        /// <summary>
        ///     Proxy controller intended for use by camera (because the camera's "get images" really
        ///     seams to mess with things).
        /// </summary>
        public ProxyController CameraController => _cameraProxyController;

        /// <summary>Connect to RPC server.</summary>
        public ICommand ConnectCommand => _connectCommand;

        /// <inheritdoc cref="ProxyModel.Connected" />
        public bool Connected
        {
            get => _controller.Connected;
            set
            {
                _cameraProxyController.Connected = value;
                _controller.Connected = value;
            }
        }

        /// <inheritdoc cref="ProxyModel.ConnectedAddress" />
        public IPAddress ConnectedAddress
        {
            get => _controller.ConnectedAddress;
            set
            {
                _cameraProxyController.ConnectedAddress = value;
                _controller.ConnectedAddress = value;
            }
        }

        /// <inheritdoc cref="ProxyModel.ConnectedPort" />
        public ushort ConnectedPort
        {
            get => _controller.ConnectedPort;
            set
            {
                _cameraProxyController.ConnectedPort = value;
                _controller.ConnectedPort = value;
            }
        }

        /// <summary>The general proxy controller.</summary>
        public ProxyController Controller => _controller;

        /// <inheritdoc cref="ProxyModel.PortToUse" />
        public ushort PortToUse
        {
            get => _controller.PortToUse;
            set
            {
                _cameraProxyController.PortToUse = value;
                _controller.PortToUse = value;
            }
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                _cameraProxyController.PropertyChanged -= onControllerPropertyChanged;
                _controller.PropertyChanged -= onControllerPropertyChanged;

                _connectCommands.ForEach(cmd => cmd.Dispose());
                _connectCommand.Dispose();

                _cameraProxyController.Dispose();
                _controller.Dispose();
            }
        }

        private readonly ProxyController _cameraProxyController;
        private readonly MultiCommand _connectCommand;
        private readonly List<ConnectCommand> _connectCommands;
        private readonly ProxyController _controller;

        private bool _disposed = false;

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