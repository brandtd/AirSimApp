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

using MsgPackRpc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AirSimRpc
{
    /// <summary>
    /// Implementation of <see cref="IAirSimProxy"/>.
    /// </summary>
    public class AirSimProxy : IAirSimProxy, IDisposable
    {
        private readonly RpcProxy _proxy;

        private bool _disposed = false;

        /// <inheritdoc cref="IAirSim.Connected" />
        public bool Connected => _proxy.Connected;

        /// <summary>
        /// Wire up proxy.
        /// </summary>
        public AirSimProxy()
        {
            _proxy = new RpcProxy();
        }

        /// <inheritdoc cref="IAirSim.ConnectAsync(IPEndPoint)" />
        public Task<bool> ConnectAsync(IPEndPoint endpoint)
        {
            return _proxy.ConnectAsync(endpoint);
        }

        /// <inheritdoc cref="IAirSim.GetCameraInfoAsync" />
        public Task<RpcResult<CameraInfo>> GetCameraInfoAsync()
        {
            return _proxy.CallAsync<CameraInfo>("getCameraInfo");
        }

        /// <inheritdoc cref="IAirSim.GetCollisionInfoAsync" />
        public Task<RpcResult<CollisionInfo>> GetCollisionInfoAsync()
        {
            return _proxy.CallAsync<CollisionInfo>("getCollisionInfo");
        }

        /// <inheritdoc cref="IAirSim.GetHomeGeoPointAsync" />
        public Task<RpcResult<GeoPoint>> GetHomeGeoPointAsync()
        {
            return _proxy.CallAsync<GeoPoint>("getHomeGeoPoint");
        }

        /// <inheritdoc cref="IAirSim.GetIsApiControlEnabledAsync" />
        public Task<RpcResult<bool>> GetIsApiControlEnabledAsync()
        {
            return _proxy.CallAsync<bool>("isApiControlEnabled");
        }

        /// <inheritdoc cref="IAirSim.GetLandedStateAsync" />
        public Task<RpcResult<LandedState>> GetLandedStateAsync()
        {
            return _proxy.CallAsync<LandedState>("getLandedState");
        }

        /// <inheritdoc cref="IAirSim.GetOrientationAsync" />
        public Task<RpcResult<QuaternionR>> GetOrientationAsync()
        {
            return _proxy.CallAsync<QuaternionR>("getOrientation");
        }

        /// <inheritdoc cref="IAirSim.GetPositionAsync" />
        public Task<RpcResult<Vector3R>> GetPositionAsync()
        {
            return _proxy.CallAsync<Vector3R>("getPosition");
        }

        /// <inheritdoc cref="IAirSim.GetVelocityAsync" />
        public Task<RpcResult<Vector3R>> GetVelocityAsync()
        {
            return _proxy.CallAsync<Vector3R>("getVelocity");
        }

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
