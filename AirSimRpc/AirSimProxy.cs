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

using MsgPackRpc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AirSimRpc
{
    /// <summary>Implementation of <see cref="IAirSimProxy" />.</summary>
    public class AirSimProxy : IAirSimProxy, IDisposable
    {
        /// <summary>Wire up proxy.</summary>
        public AirSimProxy()
        {
            _proxy = new RpcProxy();
        }

        /// <inheritdoc cref="IAirSimProxy.ConnectionClosed" />
        public event EventHandler ConnectionClosed
        {
            add => _proxy.ConnectionClosed += value;
            remove => _proxy.ConnectionClosed -= value;
        }

        /// <inheritdoc cref="IAirSimProxy.Connected" />
        public bool Connected => _proxy.Connected;

        /// <inheritdoc cref="IAirSimProxy.ArmDisarmAsync" />
        public Task<RpcResult<bool>> ArmDisarmAsync(bool armVehicle)
        {
            return _proxy.CallAsync<bool>("armDisarm", armVehicle);
        }

        /// <inheritdoc cref="RpcProxy.ConnectAsync(IPEndPoint)" />
        public Task<bool> ConnectAsync(IPEndPoint endpoint)
        {
            return _proxy.ConnectAsync(endpoint);
        }

        /// <inheritdoc cref="RpcProxy.ConnectAsync(IPEndPoint, TimeSpan)" />
        public Task<bool> ConnectAsync(IPEndPoint endpoint, TimeSpan timeout)
        {
            return _proxy.ConnectAsync(endpoint, timeout);
        }

        /// <inheritdoc cref="RpcProxy.ConnectAsync(IPEndPoint, CancellationToken)" />
        public Task<bool> ConnectAsync(IPEndPoint endpoint, CancellationToken token)
        {
            return _proxy.ConnectAsync(endpoint, token);
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public virtual void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _proxy.Dispose();
            }
        }

        /// <inheritdoc cref="IAirSimProxy.EnableApiControlAsync" />
        public Task<RpcResult> EnableApiControlAsync(bool enable)
        {
            return _proxy.CallAsync("enableApiControl", enable);
        }

        /// <inheritdoc cref="IAirSimProxy.GetCameraInfoAsync" />
        public Task<RpcResult<CameraInfo>> GetCameraInfoAsync(int cameraId)
        {
            return _proxy.CallAsync<CameraInfo>("getCameraInfo", cameraId);
        }

        /// <inheritdoc cref="IAirSimProxy.GetCollisionInfoAsync" />
        public Task<RpcResult<CollisionInfo>> GetCollisionInfoAsync()
        {
            return _proxy.CallAsync<CollisionInfo>("getCollisionInfo");
        }

        /// <inheritdoc cref="IAirSimProxy.GetHomeGeoPointAsync" />
        public Task<RpcResult<GeoPoint>> GetHomeGeoPointAsync()
        {
            return _proxy.CallAsync<GeoPoint>("getHomeGeoPoint");
        }

        /// <inheritdoc cref="IAirSimProxy.IsApiControlEnabledAsync" />
        public Task<RpcResult<bool>> IsApiControlEnabledAsync()
        {
            return _proxy.CallAsync<bool>("isApiControlEnabled");
        }

        /// <inheritdoc cref="IAirSimProxy.ResetAsync" />
        public Task<RpcResult> ResetAsync()
        {
            return _proxy.CallAsync("reset");
        }

        /// <inheritdoc cref="IAirSimProxy.SetCameraOrientationAsync" />
        public Task<RpcResult<bool>> SetCameraOrientationAsync(int cameraId, QuaternionR orientation)
        {
            return _proxy.CallAsync<bool>("setCameraOrientation", cameraId, orientation);
        }

        /// <inheritdoc cref="IAirSimProxy.SetSimulationModeAsync" />
        public Task<RpcResult<bool>> SetSimulationModeAsync(bool simulate)
        {
            return _proxy.CallAsync<bool>("setSimulationMode", simulate);
        }

        /// <inheritdoc cref="IAirSimProxy.SimContinueForTimeAsync(TimeSpan)" />
        public Task<RpcResult> SimContinueForTimeAsync(TimeSpan time)
        {
            return _proxy.CallAsync("simContinueForTime", time.TotalSeconds);
        }

        /// <inheritdoc cref="IAirSimProxy.SimGetImageAsync" />
        public Task<RpcResult<byte[]>> SimGetImageAsync(int cameraId, ImageType imageType)
        {
            return _proxy.CallAsync<byte[]>("simGetImage", cameraId, imageType);
        }

        /// <inheritdoc cref="IAirSimProxy.SimGetImagesAsync" />
        public Task<RpcResult<IEnumerable<ImageResponse>>> SimGetImagesAsync(IEnumerable<ImageRequest> requests)
        {
            return _proxy.CallAsync<IEnumerable<ImageResponse>>("simGetImages", requests);
        }

        /// <inheritdoc cref="IAirSimProxy.SimGetObjectPoseAsync" />
        public Task<RpcResult<Pose>> SimGetObjectPoseAsync(string objectName)
        {
            return _proxy.CallAsync<Pose>("simGetObjectPose", objectName);
        }

        /// <inheritdoc cref="IAirSimProxy.SimGetPoseAsync" />
        public Task<RpcResult<Pose>> SimGetPoseAsync()
        {
            return _proxy.CallAsync<Pose>("simGetPose");
        }

        /// <inheritdoc cref="IAirSimProxy.SimIsPausedAsync" />
        public Task<RpcResult<bool>> SimIsPausedAsync()
        {
            return _proxy.CallAsync<bool>("simIsPaused");
        }

        /// <inheritdoc cref="IAirSimProxy.SimPauseAsync" />
        public Task<RpcResult> SimPauseAsync(bool pause)
        {
            return _proxy.CallAsync("simPause", pause);
        }

        /// <inheritdoc cref="IAirSimProxy.SimSetPoseAsync" />
        public Task<RpcResult<bool>> SimSetPoseAsync(Pose pose, bool ignoreCollision)
        {
            return _proxy.CallAsync<bool>("simSetPose", pose, ignoreCollision);
        }

        protected readonly RpcProxy _proxy;

        private bool _disposed = false;
    }
}