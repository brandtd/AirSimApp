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

        /// <inheritdoc cref="IAirSimProxy.CmdArmDisarmAsync" />
        public Task<RpcResult<bool>> CmdArmDisarmAsync(bool armVehicle)
        {
            return _proxy.CallAsync<bool>("armDisarm", armVehicle);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdGoHomeAsync" />
        public Task<RpcResult<bool>> CmdGoHomeAsync()
        {
            return _proxy.CallAsync<bool>("goHome");
        }

        /// <inheritdoc cref="IAirSimProxy.CmdHoverInPlaceAsync" />
        public Task<RpcResult<bool>> CmdHoverInPlaceAsync()
        {
            return _proxy.CallAsync<bool>("hover");
        }

        /// <inheritdoc cref="IAirSimProxy.CmdLandAsync" />
        public Task<RpcResult<bool>> CmdLandAsync(float maxWaitSeconds)
        {
            return _proxy.CallAsync<bool>("land", maxWaitSeconds);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdMoveByAngleThrottleAsync" />
        public Task<RpcResult<bool>> CmdMoveByAngleThrottleAsync(float pitch, float roll, float throttle, float yaw_rate, float duration)
        {
            return _proxy.CallAsync<bool>("moveByAngleThrottle", pitch, roll, throttle, yaw_rate, duration);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdMoveByAngleZAsync" />
        public Task<RpcResult<bool>> CmdMoveByAngleZAsync(float pitch, float roll, float z, float yaw, float duration)
        {
            return _proxy.CallAsync<bool>("moveByAngleZ", pitch, roll, z, yaw, duration);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdMoveByManualAsync" />
        public Task<RpcResult<bool>> CmdMoveByManualAsync(float vx_max, float vy_max, float z_min, float duration, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead)
        {
            return _proxy.CallAsync<bool>("moveByManual", vx_max, vy_max, z_min, duration, drivetrain, yawMode, lookahead, adaptiveLookahead);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdMoveByVelocityAsync" />
        public Task<RpcResult<bool>> CmdMoveByVelocityAsync(float vx, float vy, float vz, float duration, DrivetrainType drivetrain, YawMode yawMode)
        {
            return _proxy.CallAsync<bool>("moveByVelocity", vx, vy, vz, duration, drivetrain, yawMode);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdMoveByVelocityZAsync" />
        public Task<RpcResult<bool>> CmdMoveByVelocityZAsync(float vx, float vy, float z, float duration, DrivetrainType drivetrain, YawMode yawMode)
        {
            return _proxy.CallAsync<bool>("moveByVelocityZ", vx, vy, z, duration, drivetrain, yawMode);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdMoveOnPathAsync" />
        public Task<RpcResult<bool>> CmdMoveOnPathAsync(IEnumerable<Vector3R> path, float velocity, float maxWaitSeconds, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead)
        {
            return _proxy.CallAsync<bool>("moveOnPath", path, velocity, maxWaitSeconds, drivetrain, yawMode, lookahead, adaptiveLookahead);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdMoveToPositionAsync" />
        public Task<RpcResult<bool>> CmdMoveToPositionAsync(float x, float y, float z, float velocity, float maxWaitSeconds, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead)
        {
            return _proxy.CallAsync<bool>("moveToPosition", x, y, z, velocity, maxWaitSeconds, drivetrain, yawMode, lookahead, adaptiveLookahead);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdMoveToZAsync" />
        public Task<RpcResult<bool>> CmdMoveToZAsync(float z, float velocity, float maxWaitSeconds, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead)
        {
            return _proxy.CallAsync<bool>("moveToZ", z, velocity, maxWaitSeconds, drivetrain, yawMode, lookahead, adaptiveLookahead);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdResetAsync" />
        public Task<RpcResult> CmdResetAsync()
        {
            return _proxy.CallAsync("reset");
        }

        /// <inheritdoc cref="IAirSimProxy.CmdRotateByYawRateAsync" />
        public Task<RpcResult<bool>> CmdRotateByYawRateAsync(float yaw_rate, float duration)
        {
            return _proxy.CallAsync<bool>("rotateByYawRate", yaw_rate, duration);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdRotateToYawAsync" />
        public Task<RpcResult<bool>> CmdRotateToYawAsync(float yaw, float maxWaitSeconds, float margin)
        {
            return _proxy.CallAsync<bool>("rotateToYaw", yaw, maxWaitSeconds, margin);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdSetApiControlAsync" />
        public Task<RpcResult> CmdSetApiControlAsync(bool enable)
        {
            return _proxy.CallAsync("enableApiControl", enable);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdSetCameraOrientationAsync" />
        public Task<RpcResult<bool>> CmdSetCameraOrientationAsync(int cameraId, QuaternionR orientation)
        {
            return _proxy.CallAsync<bool>("setCameraOrientation", cameraId, orientation);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdSetPoseAsync" />
        public Task<RpcResult<bool>> CmdSetPoseAsync(Pose pose, bool ignoreCollision)
        {
            return _proxy.CallAsync<bool>("simSetPose", pose, ignoreCollision);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdSimulationModeAsync" />
        public Task<RpcResult<bool>> CmdSimulationModeAsync(bool simulate)
        {
            return _proxy.CallAsync<bool>("setSimulationMode", simulate);
        }

        /// <inheritdoc cref="IAirSimProxy.CmdTakeoffAsync" />
        public Task<RpcResult<bool>> CmdTakeoffAsync(float maxWaitSeconds)
        {
            return _proxy.CallAsync<bool>("takeoff", maxWaitSeconds);
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
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _proxy.Dispose();
            }
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

        /// <inheritdoc cref="IAirSimProxy.GetGpsLocationAsync" />
        public Task<RpcResult<GeoPoint>> GetGpsLocationAsync()
        {
            return _proxy.CallAsync<GeoPoint>("getGpsLocation");
        }

        /// <inheritdoc cref="IAirSimProxy.GetHomeGeoPointAsync" />
        public Task<RpcResult<GeoPoint>> GetHomeGeoPointAsync()
        {
            return _proxy.CallAsync<GeoPoint>("getHomeGeoPoint");
        }

        /// <inheritdoc cref="IAirSimProxy.GetImageAsync" />
        public Task<RpcResult<byte[]>> GetImageAsync(int cameraId, ImageType imageType)
        {
            return _proxy.CallAsync<byte[]>("simGetImage", cameraId, imageType);
        }

        /// <inheritdoc cref="IAirSimProxy.GetImagesAsync" />
        public Task<RpcResult<IEnumerable<ImageResponse>>> GetImagesAsync(IEnumerable<ImageRequest> requests)
        {
            return _proxy.CallAsync<IEnumerable<ImageResponse>>("simGetImages", requests);
        }

        /// <inheritdoc cref="IAirSimProxy.GetIsApiControlEnabledAsync" />
        public Task<RpcResult<bool>> GetIsApiControlEnabledAsync()
        {
            return _proxy.CallAsync<bool>("isApiControlEnabled");
        }

        /// <inheritdoc cref="IAirSimProxy.GetIsSimulationMode" />
        public Task<RpcResult<bool>> GetIsSimulationMode()
        {
            return _proxy.CallAsync<bool>("isSimulationMode");
        }

        /// <inheritdoc cref="IAirSimProxy.GetLandedStateAsync" />
        public Task<RpcResult<LandedState>> GetLandedStateAsync()
        {
            return _proxy.CallAsync<LandedState>("getLandedState");
        }

        /// <inheritdoc cref="IAirSimProxy.GetMultirotorStateAsync" />
        public Task<RpcResult<MultirotorState>> GetMultirotorStateAsync()
        {
            return _proxy.CallAsync<MultirotorState>("getMultirotorState");
        }

        /// <inheritdoc cref="IAirSimProxy.GetOrientationAsync" />
        public Task<RpcResult<QuaternionR>> GetOrientationAsync()
        {
            return _proxy.CallAsync<QuaternionR>("getOrientation");
        }

        /// <inheritdoc cref="IAirSimProxy.GetPositionAsync" />
        public Task<RpcResult<Vector3R>> GetPositionAsync()
        {
            return _proxy.CallAsync<Vector3R>("getPosition");
        }

        /// <inheritdoc cref="IAirSimProxy.GetRcDataAsync" />
        public Task<RpcResult<RcData>> GetRcDataAsync()
        {
            return _proxy.CallAsync<RcData>("getRCData");
        }

        /// <inheritdoc cref="IAirSimProxy.GetSimObjectPoseAsync" />
        public Task<RpcResult<Pose>> GetSimObjectPoseAsync(string objectName)
        {
            return _proxy.CallAsync<Pose>("simGetObjectPose", objectName);
        }

        /// <inheritdoc cref="IAirSimProxy.GetSimPoseAsync" />
        public Task<RpcResult<Pose>> GetSimPoseAsync()
        {
            return _proxy.CallAsync<Pose>("simGetPose");
        }

        /// <inheritdoc cref="IAirSimProxy.GetVelocityAsync" />
        public Task<RpcResult<Vector3R>> GetVelocityAsync()
        {
            return _proxy.CallAsync<Vector3R>("getVelocity");
        }

        private readonly RpcProxy _proxy;

        private bool _disposed = false;
    }
}