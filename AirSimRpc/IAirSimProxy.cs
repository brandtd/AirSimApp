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

        Task<RpcResult<bool>> GoHomeAsync();

        Task<RpcResult<bool>> HoverAsync();

        Task<RpcResult<bool>> LandAsync(float maxWaitSeconds);

        Task<RpcResult<bool>> MoveByAngleThrottleAsync(float pitch, float roll, float throttle, float yaw_rate, float duration);

        Task<RpcResult<bool>> MoveByAngleZAsync(float pitch, float roll, float z, float yaw, float duration);

        Task<RpcResult<bool>> MoveByManualAsync(float vx_max, float vy_max, float z_min, float duration, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead);

        Task<RpcResult<bool>> MoveByVelocityAsync(float vx, float vy, float vz, float duration, DrivetrainType drivetrain, YawMode yawMode);

        Task<RpcResult<bool>> MoveByVelocityZAsync(float vx, float vy, float z, float duration, DrivetrainType drivetrain, YawMode yawMode);

        Task<RpcResult<bool>> MoveOnPathAsync(IEnumerable<Vector3R> path, float velocity, float maxWaitSeconds, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead);

        Task<RpcResult<bool>> MoveToPositionAsync(float x, float y, float z, float velocity, float maxWaitSeconds, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead);

        Task<RpcResult<bool>> MoveToZAsync(float z, float velocity, float maxWaitSeconds, YawMode yawMode, float lookahead, float adaptiveLookahead);

        Task<RpcResult> ResetAsync();

        Task<RpcResult<bool>> RotateByYawRateAsync(float yaw_rate, float duration);

        Task<RpcResult<bool>> RotateToYawAsync(float yaw, float maxWaitSeconds, float margin);

        Task<RpcResult> EnableApiControlAsync(bool enable);

        Task<RpcResult<bool>> SetCameraOrientationAsync(int cameraId, QuaternionR orientation);

        Task<RpcResult<bool>> SimSetPoseAsync(Pose pose, bool ignoreCollision);

        Task<RpcResult<bool>> SetSimulationModeAsync(bool simulate);

        Task<RpcResult<bool>> TakeoffAsync(float maxWaitSeconds);

        /// <summary>Connect to the AirSim RPC server.</summary>
        /// <param name="endpoint">Server location.</param>
        /// <returns><c>true&gt;</c> if connection is successful, <c>false</c> othewrise.</returns>
        Task<bool> ConnectAsync(IPEndPoint endpoint);

        // simGetSegmentationObjectID TODO simPrintLogMessage TODO setSafety TODO setRCData

        Task<RpcResult<CameraInfo>> GetCameraInfoAsync(int cameraId);

        Task<RpcResult<CollisionInfo>> GetCollisionInfoAsync();

        Task<RpcResult<GeoPoint>> GetGpsLocationAsync();

        Task<RpcResult<GeoPoint>> GetHomeGeoPointAsync();

        Task<RpcResult<byte[]>> SimGetImageAsync(int cameraId, ImageType imageType);

        Task<RpcResult<IEnumerable<ImageResponse>>> SimGetImagesAsync(IEnumerable<ImageRequest> requests);

        Task<RpcResult<bool>> IsApiControlEnabledAsync();

        Task<RpcResult<bool>> IsSimulationMode();

        Task<RpcResult<LandedState>> GetLandedStateAsync();

        Task<RpcResult<MultirotorState>> GetMultirotorStateAsync();

        Task<RpcResult<QuaternionR>> GetOrientationAsync();

        Task<RpcResult<Vector3R>> GetPositionAsync();

        Task<RpcResult<RcData>> GetRcDataAsync();

        Task<RpcResult<Pose>> SimGetObjectPoseAsync(string objectName);

        Task<RpcResult<Pose>> SimGetPoseAsync();

        Task<RpcResult<Vector3R>> GetVelocityAsync();
    }
}
