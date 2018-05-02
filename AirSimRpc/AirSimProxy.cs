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

        /// <inheritdoc cref="IAirSimProxy.GoHomeAsync" />
        public Task<RpcResult<bool>> GoHomeAsync()
        {
            return _proxy.CallAsync<bool>("goHome");
        }

        /// <inheritdoc cref="IAirSimProxy.HoverAsync" />
        public Task<RpcResult<bool>> HoverAsync()
        {
            return _proxy.CallAsync<bool>("hover");
        }

        /// <inheritdoc cref="IAirSimProxy.LandAsync" />
        public Task<RpcResult<bool>> LandAsync(float maxWaitSeconds)
        {
            return _proxy.CallAsync<bool>("land", maxWaitSeconds);
        }

        /// <inheritdoc cref="IAirSimProxy.MoveByAngleThrottleAsync" />
        public Task<RpcResult<bool>> MoveByAngleThrottleAsync(float pitch, float roll, float throttle, float yaw_rate, float duration)
        {
            return _proxy.CallAsync<bool>("moveByAngleThrottle", pitch, roll, throttle, yaw_rate, duration);
        }

        /// <inheritdoc cref="IAirSimProxy.MoveByAngleZAsync" />
        public Task<RpcResult<bool>> MoveByAngleZAsync(float pitch, float roll, float z, float yaw, float duration)
        {
            return _proxy.CallAsync<bool>("moveByAngleZ", pitch, roll, z, yaw, duration);
        }

        /// <inheritdoc cref="IAirSimProxy.MoveByManualAsync" />
        public Task<RpcResult<bool>> MoveByManualAsync(float vx_max, float vy_max, float z_min, float duration, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead)
        {
            return _proxy.CallAsync<bool>("moveByManual", vx_max, vy_max, z_min, duration, drivetrain, yawMode, lookahead, adaptiveLookahead);
        }

        /// <inheritdoc cref="IAirSimProxy.MoveByVelocityAsync" />
        public Task<RpcResult<bool>> MoveByVelocityAsync(float vx, float vy, float vz, float duration, DrivetrainType drivetrain, YawMode yawMode)
        {
            return _proxy.CallAsync<bool>("moveByVelocity", vx, vy, vz, duration, drivetrain, yawMode);
        }

        /// <inheritdoc cref="IAirSimProxy.MoveByVelocityZAsync" />
        public Task<RpcResult<bool>> MoveByVelocityZAsync(float vx, float vy, float z, float duration, DrivetrainType drivetrain, YawMode yawMode)
        {
            return _proxy.CallAsync<bool>("moveByVelocityZ", vx, vy, z, duration, drivetrain, yawMode);
        }

        /// <inheritdoc cref="IAirSimProxy.MoveOnPathAsync" />
        public Task<RpcResult<bool>> MoveOnPathAsync(IEnumerable<Vector3R> path, float velocity, float maxWaitSeconds, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead)
        {
            return _proxy.CallAsync<bool>("moveOnPath", path, velocity, maxWaitSeconds, drivetrain, yawMode, lookahead, adaptiveLookahead);
        }

        /// <inheritdoc cref="IAirSimProxy.MoveToPositionAsync" />
        public Task<RpcResult<bool>> MoveToPositionAsync(float x, float y, float z, float velocity, float maxWaitSeconds, DrivetrainType drivetrain, YawMode yawMode, float lookahead, float adaptiveLookahead)
        {
            return _proxy.CallAsync<bool>("moveToPosition", x, y, z, velocity, maxWaitSeconds, drivetrain, yawMode, lookahead, adaptiveLookahead);
        }

        /// <inheritdoc cref="IAirSimProxy.MoveToZAsync" />
        public Task<RpcResult<bool>> MoveToZAsync(float z, float velocity, float maxWaitSeconds, YawMode yawMode, float lookahead, float adaptiveLookahead)
        {
            return _proxy.CallAsync<bool>("moveToZ", z, velocity, maxWaitSeconds, yawMode, lookahead, adaptiveLookahead);
        }

        /// <inheritdoc cref="IAirSimProxy.ResetAsync" />
        public Task<RpcResult> ResetAsync()
        {
            return _proxy.CallAsync("reset");
        }

        /// <inheritdoc cref="IAirSimProxy.RotateByYawRateAsync" />
        public Task<RpcResult<bool>> RotateByYawRateAsync(float yaw_rate, float duration)
        {
            return _proxy.CallAsync<bool>("rotateByYawRate", yaw_rate, duration);
        }

        /// <inheritdoc cref="IAirSimProxy.RotateToYawAsync" />
        public Task<RpcResult<bool>> RotateToYawAsync(float yaw, float maxWaitSeconds, float margin)
        {
            return _proxy.CallAsync<bool>("rotateToYaw", yaw, maxWaitSeconds, margin);
        }

        /// <inheritdoc cref="IAirSimProxy.EnableApiControlAsync" />
        public Task<RpcResult> EnableApiControlAsync(bool enable)
        {
            return _proxy.CallAsync("enableApiControl", enable);
        }

        /// <inheritdoc cref="IAirSimProxy.SetCameraOrientationAsync" />
        public Task<RpcResult<bool>> SetCameraOrientationAsync(int cameraId, QuaternionR orientation)
        {
            return _proxy.CallAsync<bool>("setCameraOrientation", cameraId, orientation);
        }

        /// <inheritdoc cref="IAirSimProxy.SimSetPoseAsync" />
        public Task<RpcResult<bool>> SimSetPoseAsync(Pose pose, bool ignoreCollision)
        {
            return _proxy.CallAsync<bool>("simSetPose", pose, ignoreCollision);
        }

        /// <inheritdoc cref="IAirSimProxy.SetSimulationModeAsync" />
        public Task<RpcResult<bool>> SetSimulationModeAsync(bool simulate)
        {
            return _proxy.CallAsync<bool>("setSimulationMode", simulate);
        }

        /// <inheritdoc cref="IAirSimProxy.TakeoffAsync" />
        public Task<RpcResult<bool>> TakeoffAsync(float maxWaitSeconds)
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

        /// <inheritdoc cref="IAirSimProxy.IsApiControlEnabledAsync" />
        public Task<RpcResult<bool>> IsApiControlEnabledAsync()
        {
            return _proxy.CallAsync<bool>("isApiControlEnabled");
        }

        /// <inheritdoc cref="IAirSimProxy.IsSimulationMode" />
        public Task<RpcResult<bool>> IsSimulationMode()
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

        /// <inheritdoc cref="IAirSimProxy.GetVelocityAsync" />
        public Task<RpcResult<Vector3R>> GetVelocityAsync()
        {
            return _proxy.CallAsync<Vector3R>("getVelocity");
        }

        private readonly RpcProxy _proxy;

        private bool _disposed = false;
    }
}
