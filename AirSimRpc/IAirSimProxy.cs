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
using System.Threading.Tasks;

namespace AirSimRpc
{
    /// <summary>Interface for an AirSim RPC proxy.</summary>
    public interface IAirSimProxy
    {
        /// <inheritdoc cref="RpcProxy.ConnectionClosed" />
        event EventHandler ConnectionClosed;

        /// <inheritdoc cref="RpcProxy.Connected" />
        bool Connected { get; }

        Task<RpcResult<bool>> ArmDisarmAsync(bool armVehicle);

        /// <summary>Connect to the AirSim RPC server.</summary>
        /// <param name="endpoint">Server location.</param>
        /// <returns><c>true</c> if connection is successful, <c>false</c> othewrise.</returns>
        Task<bool> ConnectAsync(IPEndPoint endpoint);

        Task<RpcResult> EnableApiControlAsync(bool enable);

        Task<RpcResult<CameraInfo>> GetCameraInfoAsync(int cameraId);

        Task<RpcResult<CollisionInfo>> GetCollisionInfoAsync();

        Task<RpcResult<GeoPoint>> GetHomeGeoPointAsync();

        Task<RpcResult<bool>> IsApiControlEnabledAsync();

        Task<RpcResult> ResetAsync();

        Task<RpcResult<bool>> SetCameraOrientationAsync(int cameraId, QuaternionR orientation);

        Task<RpcResult<bool>> SetSimulationModeAsync(bool simulate);

        Task<RpcResult> SimContinueForTimeAsync(TimeSpan time);

        Task<RpcResult<byte[]>> SimGetImageAsync(int cameraId, ImageType imageType);

        Task<RpcResult<IEnumerable<ImageResponse>>> SimGetImagesAsync(IEnumerable<ImageRequest> requests);

        Task<RpcResult<Pose>> SimGetObjectPoseAsync(string objectName);

        Task<RpcResult<Pose>> SimGetPoseAsync();

        Task<RpcResult<bool>> SimIsPausedAsync();

        Task<RpcResult> SimPauseAsync(bool pause);

        Task<RpcResult<bool>> SimSetPoseAsync(Pose pose, bool ignoreCollision);
    }
}