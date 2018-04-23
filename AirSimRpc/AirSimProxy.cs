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
using System.Collections.Generic;
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

        /// <inheritdoc cref="IAirSim.GetMultirotorStateAsync" />
        public Task<RpcResult<MultirotorState>> GetMultirotorStateAsync()
        {
            return _proxy.CallAsync<MultirotorState>("getMultirotorState");
        }

        /// <inheritdoc cref="IAirSim.GetRcDataAsync" />
        public Task<RpcResult<RcData>> GetRcDataAsync()
        {
            return _proxy.CallAsync<RcData>("getRCData");
        }

        /// <inheritdoc cref="IAirSim.GetGpsLocationAsync" />
        public Task<RpcResult<GeoPoint>> GetGpsLocationAsync()
        {
            return _proxy.CallAsync<GeoPoint>("getGpsLocation");
        }

        /// <inheritdoc cref="IAirSim.GetIsSimulationMode" />
        public Task<RpcResult<bool>> GetIsSimulationMode()
        {
            return _proxy.CallAsync<bool>("isSimulationMode");
        }

        /// <inheritdoc cref="IAirSim.GetSimPoseAsync" />
        public Task<RpcResult<Pose>> GetSimPoseAsync()
        {
            return _proxy.CallAsync<Pose>("simGetPose");
        }

        /// <inheritdoc cref="IAirSim.GetSimObjectPoseAsync" />
        public Task<RpcResult<Pose>> GetSimObjectPoseAsync(string objectName)
        {
            return _proxy.CallAsync<Pose>("simGetObjectPose", objectName);
        }

        /// <inheritdoc cref="IAirSim.CmdSetPoseAsync" />
        public Task<RpcResult<bool>> CmdSetPoseAsync(Pose pose, bool ignoreCollision)
        {
            return _proxy.CallAsync<bool>("simSetPose", pose, ignoreCollision);
        }

        /// <inheritdoc cref="IAirSim.CmdArmDisarmAsync" />
        public Task<RpcResult<bool>> CmdArmDisarmAsync(bool armVehicle)
        {
            return _proxy.CallAsync<bool>("armDisarm", armVehicle);
        }

        /// <inheritdoc cref="IAirSim.CmdSimulationModeAsync" />
        public Task<RpcResult<bool>> CmdSimulationModeAsync(bool simulate)
        {
            return _proxy.CallAsync<bool>("setSimulationMode", simulate);
        }

        /// <inheritdoc cref="IAirSim.CmdTakeoffAsync" />
        public Task<RpcResult<bool>> CmdTakeoffAsync(float maxWaitSeconds)
        {
            return _proxy.CallAsync<bool>("takeoff", maxWaitSeconds);
        }

        /// <inheritdoc cref="IAirSim.CmdLandAsync" />
        public Task<RpcResult<bool>> CmdLandAsync(float maxWaitSeconds)
        {
            return _proxy.CallAsync<bool>("land", maxWaitSeconds);
        }

        /// <inheritdoc cref="IAirSim.CmdGoHomeAsync" />
        public Task<RpcResult<bool>> CmdGoHomeAsync(float maxWaitSeconds)
        {
            return _proxy.CallAsync<bool>("goHome", maxWaitSeconds);
        }

        /// <inheritdoc cref="IAirSim.CmdMoveByAngleZAsync" />
        public Task<RpcResult<bool>> CmdMoveByAngleZAsync(float pitch, float roll, float z, float yaw, float duration)
        {
            return _proxy.CallAsync<bool>("moveByAngleZ", pitch, roll, z, yaw, duration);
        }

        /// <inheritdoc cref="IAirSim.CmdMoveByAngleThrottleAsync" />
        public Task<RpcResult<bool>> CmdMoveByAngleThrottleAsync(float pitch, float roll, float throttle, float yaw_rate, float duration)
        {
            return _proxy.CallAsync<bool>("moveByAngleThrottle", pitch, roll, throttle, yaw_rate, duration);
        }

        /// <inheritdoc cref="IAirSim.CmdMoveByVelocityAsync" />
        public Task<RpcResult<bool>> CmdMoveByVelocityAsync(float vx, float vy, float vz, float duration, DrivetrainType drivetrain, YawMode yawMode)
        {
            return _proxy.CallAsync<bool>("moveByVelocity", vx, vy, vz, duration, drivetrain, yawMode);
        }

        /// <inheritdoc cref="IAirSim.CmdMoveByVelocityZAsync" />
        public Task<RpcResult<bool>> CmdMoveByVelocityZAsync(float vx, float vy, float z, float duration, DrivetrainType drivetrain, YawMode yawMode)
        {
            return _proxy.CallAsync<bool>("moveByVelocityZ", vx, vy, z, duration, drivetrain, yawMode);
        }

        /// <inheritdoc cref="IAirSim.CmdMoveOnPathAsync" />
        public Task<RpcResult<bool>> CmdMoveOnPathAsync(IEnumerable<Vector3R> path, float velocity, float maxWaitSeconds, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead)
        {
            return _proxy.CallAsync<bool>("moveOnPath", path, velocity, maxWaitSeconds, drivetrain, yawMode, lookahead, adaptiveLookahead);
        }

        /// <inheritdoc cref="IAirSim.CmdMoveToPositionAsync" />
        public Task<RpcResult<bool>> CmdMoveToPositionAsync(float x, float y, float z, float velocity, float maxWaitSeconds, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead)
        {
            return _proxy.CallAsync<bool>("moveToPosition", x, y, z, velocity, maxWaitSeconds, drivetrain, yawMode, lookahead, adaptiveLookahead);
        }

        /// <inheritdoc cref="IAirSim.CmdMoveToZAsync" />
        public Task<RpcResult<bool>> CmdMoveToZAsync(float z, float velocity, float maxWaitSeconds, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead)
        {
            return _proxy.CallAsync<bool>("moveToZ", z, velocity, maxWaitSeconds, drivetrain, yawMode, lookahead, adaptiveLookahead);
        }

        /// <inheritdoc cref="IAirSim.CmdMoveByManualAsync" />
        public Task<RpcResult<bool>> CmdMoveByManualAsync(float vx_max, float vy_max, float z_min, float duration, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead)
        {
            return _proxy.CallAsync<bool>("moveByManual", vx_max, vy_max, z_min, duration, drivetrain, yawMode, lookahead, adaptiveLookahead);
        }

        /// <inheritdoc cref="IAirSim.CmdRotateToYawAsync" />
        public Task<RpcResult<bool>> CmdRotateToYawAsync(float yaw, float maxWaitSeconds, float margin)
        {
            return _proxy.CallAsync<bool>("rotateToYaw", yaw, maxWaitSeconds, margin);
        }

        /// <inheritdoc cref="IAirSim.CmdRotateByYawRateAsync" />
        public Task<RpcResult<bool>> CmdRotateByYawRateAsync(float yaw_rate, float duration)
        {
            return _proxy.CallAsync<bool>("rotateByYawRate", yaw_rate, duration);
        }

        /// <inheritdoc cref="IAirSim.CmdHoverInPlaceAsync" />
        public Task<RpcResult<bool>> CmdHoverInPlaceAsync()
        {
            return _proxy.CallAsync<bool>("hover");
        }

        /// <inheritdoc cref="IAirSim.CmdResetAsync" />
        public Task<RpcResult> CmdResetAsync()
        {
            return _proxy.CallAsync("reset");
        }

        /// <inheritdoc cref="IAirSim.CmdSetCameraOrientationAsync" />
        public Task<RpcResult<bool>> CmdSetCameraOrientationAsync(int cameraId, QuaternionR orientation)
        {
            return _proxy.CallAsync<bool>("setCameraOrientation", cameraId, orientation);
        }

        /// <inheritdoc cref="IAirSim.CmdSetApiControlAsync" />
        public Task<RpcResult> CmdSetApiControlAsync(bool enable)
        {
            return _proxy.CallAsync("enableApiControl", enable);
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
