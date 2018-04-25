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
        /// <inheritdoc cref="RpcProxy.Connected" />
        bool Connected { get; }

        /// <inheritdoc cref="RpcProxy.ConnectionClosed" />
        event EventHandler ConnectionClosed;

        /// <summary>Connect to the AirSim RPC server.</summary>
        /// <param name="endpoint">Server location.</param>
        /// <returns><c>true&gt;</c> if connection is successful, <c>false</c> othewrise.</returns>
        Task<bool> ConnectAsync(IPEndPoint endpoint);

        // TODO simGetImages TODO simGetImage TODO simSetSegmentationObjectID TODO
        // simGetSegmentationObjectID TODO simPrintLogMessage TODO setSafety TODO setRCData

        Task<RpcResult<MultirotorState>> GetMultirotorStateAsync();

        Task<RpcResult<Vector3R>> GetPositionAsync();

        Task<RpcResult<Vector3R>> GetVelocityAsync();

        Task<RpcResult<QuaternionR>> GetOrientationAsync();

        Task<RpcResult<LandedState>> GetLandedStateAsync();

        Task<RpcResult<CameraInfo>> GetCameraInfoAsync();

        Task<RpcResult<RcData>> GetRcDataAsync();

        Task<RpcResult<GeoPoint>> GetGpsLocationAsync();

        Task<RpcResult<bool>> GetIsSimulationMode();

        Task<RpcResult<Pose>> GetSimPoseAsync();

        Task<RpcResult<Pose>> GetSimObjectPoseAsync(string objectName);

        Task<RpcResult<CollisionInfo>> GetCollisionInfoAsync();

        Task<RpcResult<GeoPoint>> GetHomeGeoPointAsync();

        Task<RpcResult<bool>> GetIsApiControlEnabledAsync();

        Task<RpcResult<bool>> CmdSetPoseAsync(Pose pose, bool ignoreCollision);

        Task<RpcResult<bool>> CmdArmDisarmAsync(bool armVehicle);

        Task<RpcResult<bool>> CmdSimulationModeAsync(bool simulate);

        Task<RpcResult<bool>> CmdTakeoffAsync(float maxWaitSeconds);

        Task<RpcResult<bool>> CmdLandAsync(float maxWaitSeconds);

        Task<RpcResult<bool>> CmdGoHomeAsync();

        Task<RpcResult<bool>> CmdMoveByAngleZAsync(float pitch, float roll, float z, float yaw, float duration);

        Task<RpcResult<bool>> CmdMoveByAngleThrottleAsync(float pitch, float roll, float throttle, float yaw_rate, float duration);

        Task<RpcResult<bool>> CmdMoveByVelocityAsync(float vx, float vy, float vz, float duration, DrivetrainType drivetrain, YawMode yawMode);

        Task<RpcResult<bool>> CmdMoveByVelocityZAsync(float vx, float vy, float z, float duration, DrivetrainType drivetrain, YawMode yawMode);

        Task<RpcResult<bool>> CmdMoveOnPathAsync(IEnumerable<Vector3R> path, float velocity, float maxWaitSeconds, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead);

        Task<RpcResult<bool>> CmdMoveToPositionAsync(float x, float y, float z, float velocity, float maxWaitSeconds, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead);

        Task<RpcResult<bool>> CmdMoveToZAsync(float z, float velocity, float maxWaitSeconds, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead);

        Task<RpcResult<bool>> CmdMoveByManualAsync(float vx_max, float vy_max, float z_min, float duration, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead);

        Task<RpcResult<bool>> CmdRotateToYawAsync(float yaw, float maxWaitSeconds, float margin);

        Task<RpcResult<bool>> CmdRotateByYawRateAsync(float yaw_rate, float duration);

        Task<RpcResult<bool>> CmdHoverInPlaceAsync();

        Task<RpcResult> CmdResetAsync();

        Task<RpcResult<bool>> CmdSetCameraOrientationAsync(int cameraId, QuaternionR orientation);

        Task<RpcResult> CmdSetApiControlAsync(bool enable);
    }
}