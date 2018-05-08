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

using AirSimRpc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AirSimApp.Models
{
    /// <summary>Controls access to an AirSim RPC server.</summary>
    public class ProxyController : PropertyChangedBase, IDisposable
    {
        /// <summary />
        public ProxyController()
        {
            _proxy = new MultirotorProxy();
            _proxy.ConnectionClosed += onProxyConnectionClosed;
        }

        /// <summary>Address to use to connect to server.</summary>
        public IPAddress AddressToUse
        {
            get => _addressToUse;
            set
            {
                if (_addressToUse != value)
                {
                    _addressToUse = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>Whether proxy is connected with server.</summary>
        public bool Connected
        {
            get => _connected;
            set
            {
                if (_connected != value)
                {
                    _connected = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>Address of connected server.</summary>
        public IPAddress ConnectedAddress
        {
            get => _connectedAddress;
            set
            {
                if (_connectedAddress != value)
                {
                    _connectedAddress = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>Port of connected server.</summary>
        public ushort ConnectedPort
        {
            get => _connectedPort;
            set
            {
                if (_connectedPort != value)
                {
                    _connectedPort = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>Port number to use to connect to server.</summary>
        public ushort PortToUse
        {
            get => _portToUse;
            set
            {
                if (_portToUse != value)
                {
                    _portToUse = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>The RPC proxy, <c>null</c> if not connected.</summary>
        public IAirSimMultirotorProxy Proxy => Connected ? _proxy : null;

        /// <inheritdoc cref="IAirSimProxy.ConnectAsync(IPEndPoint)" />
        public async Task<bool> ConnectAsync(IPEndPoint endpoint)
        {
            if (await _proxy.ConnectAsync(endpoint, TimeSpan.FromSeconds(1)))
            {
                ConnectedAddress = endpoint.Address;
                ConnectedPort = (ushort)endpoint.Port;
                Connected = true;
            }
            else
            {
                Connected = false;
            }

            return Connected;
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _proxy.ConnectionClosed -= onProxyConnectionClosed;
                _proxy.Dispose();
            }
        }

        private readonly MultirotorProxy _proxy;

        private IPAddress _addressToUse = IPAddress.Parse("127.0.0.1");
        private bool _connected;
        private IPAddress _connectedAddress = IPAddress.Parse("127.0.0.1");
        private ushort _connectedPort = 41451;
        private bool _disposed = false;
        private ushort _portToUse = 41451;

        private void onProxyConnectionClosed(object sender, EventArgs e)
        {
            Connected = false;
        }
    }
}