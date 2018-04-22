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

using AirSimRpc;
using MsgPackRpc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AirSimApp
{
    /// <summary>
    /// Controls access to an AirSim RPC server.
    /// </summary>
    public class ProxyController : PropertyChangedBase, IDisposable
    {
        private readonly AirSimProxy _proxy;

        private bool _connected;
        private bool _disposed = false;
        private IPAddress _addressToUse = IPAddress.Parse("127.0.0.1");
        private IPAddress _connectedAddress = IPAddress.Parse("127.0.0.1");
        private ushort _connectedPort = 41451;
        private ushort _portToUse = 41451;

        /// <summary>
        /// Address to use to connect to server.
        /// </summary>
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

        /// <summary>
        /// Port number to use to connect to server.
        /// </summary>
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

        /// <summary>
        /// Address of connected server.
        /// </summary>
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

        /// <summary>
        /// Port of connected server.
        /// </summary>
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

        /// <summary>
        /// Whether proxy is connected with server.
        /// </summary>
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

        /// <summary />
        public ProxyController()
        {
            _proxy = new AirSimProxy();
        }

        /// <inheritdoc cref="IAirSimProxy.ConnectAsync(IPEndPoint)" />
        public async Task<bool> ConnectAsync(IPEndPoint endpoint)
        {
            if (await _proxy.ConnectAsync(endpoint))
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

        /// <inheritdoc cref="IAirSimProxy.GetCameraInfoAsync()" />
        public Task<RpcResult<CameraInfo>> GetCameraInfoAsync() => _proxy.GetCameraInfoAsync();

        /// <inheritdoc cref="IAirSimProxy.GetCollisionInfoAsync()" />
        public Task<RpcResult<CollisionInfo>> GetCollisionInfoAsync() => _proxy.GetCollisionInfoAsync();

        /// <inheritdoc cref="IAirSimProxy.GetHomeGeoPointAsync()" />
        public Task<RpcResult<GeoPoint>> GetHomeGeoPointAsync() => _proxy.GetHomeGeoPointAsync();

        /// <inheritdoc cref="IAirSimProxy.GetIsApiControlEnabledAsync()" />
        public Task<RpcResult<bool>> GetIsApiControlEnabledAsync() => _proxy.GetIsApiControlEnabledAsync();

        /// <inheritdoc cref="IAirSimProxy.GetLandedStateAsync()" />
        public Task<RpcResult<LandedState>> GetLandedStateAsync() => _proxy.GetLandedStateAsync();

        /// <inheritdoc cref="IAirSimProxy.GetOrientationAsync()" />
        public Task<RpcResult<QuaternionR>> GetOrientationAsync() => _proxy.GetOrientationAsync();

        /// <inheritdoc cref="IAirSimProxy.GetPositionAsync()" />
        public Task<RpcResult<Vector3R>> GetPositionAsync() => _proxy.GetPositionAsync();

        /// <inheritdoc cref="IAirSimProxy.GetVelocityAsync()" />
        public Task<RpcResult<Vector3R>> GetVelocityAsync() => _proxy.GetVelocityAsync();

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _proxy.Dispose();
            }
        }
    }
}
