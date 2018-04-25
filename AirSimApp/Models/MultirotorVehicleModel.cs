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
using DotSpatialExtensions;
using DotSpatial.Positioning;
using MsgPackRpc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AirSimApp.Models
{
    /// <summary>Model for a multirotor vehicle.</summary>
    public class MultirotorVehicleModel : PropertyChangedBase, IDisposable
    {
        /// <summary>Wire up model.</summary>
        public MultirotorVehicleModel(ProxyController controller)
        {
            _controller = controller;
            _controller.PropertyChanged += onControllerPropertyChanged;

            startOrStopStateLoop();
        }

        public bool ApiEnabled
        {
            get => _apiEnabled;
            set => SetProperty(ref _apiEnabled, value);
        }

        public bool Connected
        {
            get => _connected;
            set => SetProperty(ref _connected, value);
        }

        public Position3D HomeLocation
        {
            get => _homeLocation;
            set => SetProperty(ref _homeLocation, value);
        }

        public Position3D VehicleLocation
        {
            get => _vehicleLocation;
            set => SetProperty(ref _vehicleLocation, value);
        }

        public Position3D VehicleLocationGps
        {
            get => _vehicleLocationGps;
            set => SetProperty(ref _vehicleLocationGps, value);
        }

        public Angle VehiclePitch
        {
            get => _vehiclePitch;
            set => SetProperty(ref _vehiclePitch, value);
        }

        public Angle VehicleRoll
        {
            get => _vehicleRoll;
            set => SetProperty(ref _vehicleRoll, value);
        }

        public Angle VehicleYaw
        {
            get => _vehicleYaw;
            set => SetProperty(ref _vehicleYaw, value);
        }

        public Task ArmAsync()
        {
            return ArmDisarmAsync(true);
        }

        public async Task ArmDisarmAsync(bool armIfTrue)
        {
            await _controller.Proxy?.CmdArmDisarmAsync(armIfTrue);
        }

        public Task DisarmAsync()
        {
            return ArmDisarmAsync(false);
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!_disposed)
            {
                _controller.PropertyChanged -= onControllerPropertyChanged;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        public async Task GoHomeAsync()
        {
            await _controller.Proxy?.CmdGoHomeAsync();
        }

        public async Task HoverAsync()
        {
            await _controller.Proxy?.CmdHoverInPlaceAsync();
        }

        public Task LandAsync()
        {
            return LandAsync(TimeSpan.FromMinutes(2));
        }

        public async Task LandAsync(TimeSpan allowedTimeToLand)
        {
            await _controller.Proxy?.CmdLandAsync((float)allowedTimeToLand.TotalSeconds);
        }

        public async Task MoveByAngleThrottleAsync(Angle pitch, Angle roll, float throttle, float yaw_rate, TimeSpan duration)
        {
            await _controller.Proxy?.CmdMoveByAngleThrottleAsync(
                (float)pitch.DecimalDegrees,
                (float)roll.DecimalDegrees,
                throttle,
                yaw_rate,
                (float)duration.TotalSeconds);
        }

        public async Task MoveByAngleZAsync(Angle pitch, Angle roll, Distance z, Angle yaw, TimeSpan duration)
        {
            await _controller.Proxy?.CmdMoveByAngleZAsync(
                (float)pitch.DecimalDegrees,
                (float)roll.DecimalDegrees,
                (float)z.ToMeters().Value,
                (float)yaw.DecimalDegrees,
                (float)duration.TotalSeconds);
        }

        public async Task MoveByVelocityAsync(Speed vx, Speed vy, Speed vz, TimeSpan duration, DrivetrainType drivetrainType, YawMode yawMode)
        {
            await _controller.Proxy?.CmdMoveByVelocityAsync(
                (float)vx.ToMetersPerSecond().Value,
                (float)vy.ToMetersPerSecond().Value,
                (float)vz.ToMetersPerSecond().Value,
                (float)duration.TotalSeconds,
                drivetrainType,
                yawMode);
        }

        public async Task MoveByVelocityZAsync(Speed vx, Speed vy, Distance z, TimeSpan duration, DrivetrainType drivetrainType, YawMode yawMode)
        {
            await _controller.Proxy?.CmdMoveByVelocityZAsync(
                (float)vx.ToMetersPerSecond().Value,
                (float)vy.ToMetersPerSecond().Value,
                (float)z.ToMeters().Value,
                (float)duration.TotalSeconds,
                drivetrainType,
                yawMode);
        }

        public async Task MoveOnPathAsync(
            IEnumerable<Position3D> positions,
            Speed speed,
            TimeSpan allowedTimeToComplete,
            DrivetrainType drivetrainType,
            YawMode yawMode,
            float lookahead,
            float adaptiveLookahead)
        {
            IEnumerable<Vector3R> nedPositions = positions.Select(pos => toNedFromPosition(pos));
            await _controller.Proxy?.CmdMoveOnPathAsync(
                nedPositions,
                (float)speed.ToMetersPerSecond().Value,
                (float)allowedTimeToComplete.TotalSeconds,
                drivetrainType,
                yawMode,
                lookahead,
                adaptiveLookahead);
        }

        public Task MoveToAltitudeAsync(
            Distance altitude,
            Speed speed,
            TimeSpan allowedTimeToComplete,
            DrivetrainType drivetrainType,
            YawMode yawMode,
            float lookahead,
            float adaptiveLookahead)
        {
            return MoveToZAsync(altitude - HomeLocation.Altitude, speed, allowedTimeToComplete, drivetrainType, yawMode, lookahead, adaptiveLookahead);
        }

        public async Task MoveToPositionAsync(Vector3R position,
            Speed speed,
            TimeSpan allowedTimeToComplete,
            DrivetrainType drivetrainType,
            YawMode yawMode,
            float lookahead,
            float adaptiveLookahead)
        {
            await _controller.Proxy?.CmdMoveToPositionAsync(
                position.X,
                position.Y,
                position.Z,
                (float)speed.ToMetersPerSecond().Value,
                (float)allowedTimeToComplete.TotalSeconds,
                drivetrainType,
                yawMode,
                lookahead,
                adaptiveLookahead);
        }

        public Task MoveToPositionAsync(Position3D position,
            Speed speed,
            TimeSpan allowedTimeToComplete,
            DrivetrainType drivetrainType,
            YawMode yawMode,
            float lookahead,
            float adaptiveLookahead)
        {
            return MoveToPositionAsync(toNedFromPosition(position), speed, allowedTimeToComplete, drivetrainType, yawMode, lookahead, adaptiveLookahead);
        }

        public async Task MoveToZAsync(
            Distance z,
            Speed speed,
            TimeSpan allowedTimeToComplete,
            DrivetrainType drivetrainType,
            YawMode yawMode,
            float lookahead,
            float adaptiveLookahead)
        {
            await _controller.Proxy?.CmdMoveToZAsync(
                (float)z.ToMeters().Value,
                (float)speed.ToMetersPerSecond().Value,
                (float)allowedTimeToComplete.TotalSeconds,
                drivetrainType,
                yawMode,
                lookahead,
                adaptiveLookahead);
        }

        public async Task ResetAsync()
        {
            await _controller.Proxy?.CmdResetAsync();
        }

        public async Task SetApiControlAllowedAsync(bool allowedIfTrue)
        {
            await _controller.Proxy?.CmdSetApiControlAsync(allowedIfTrue);
        }

        public async Task SetSimulationModeAsync(bool simulateIfTrue)
        {
            await _controller.Proxy?.CmdSimulationModeAsync(simulateIfTrue);
        }

        public Task TakeoffAsync()
        {
            return TakeoffAsync(TimeSpan.FromSeconds(30));
        }

        public async Task TakeoffAsync(TimeSpan allowedTimeToTakeoff)
        {
            await _controller.Proxy?.CmdTakeoffAsync((float)allowedTimeToTakeoff.TotalSeconds);
        }

        private readonly ProxyController _controller;

        private bool _apiEnabled = false;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _connected = false;
        private bool _disposed = false;
        private Position3D _homeLocation = new Position3D(Latitude.Invalid, Longitude.Invalid, Distance.Invalid);
        private Position3D _vehicleLocation = new Position3D(Latitude.Invalid, Longitude.Invalid, Distance.Invalid);
        private Position3D _vehicleLocationGps = new Position3D(Latitude.Invalid, Longitude.Invalid, Distance.Invalid);
        private Angle _vehiclePitch = Angle.Invalid;
        private Angle _vehicleRoll = Angle.Invalid;
        private Angle _vehicleYaw = Angle.Invalid;

        private void onControllerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_controller.Connected))
            {
                startOrStopStateLoop();
            }
        }

        private void startOrStopStateLoop()
        {
            Connected = _controller.Connected;
            if (_controller.Connected)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                updateStateLoop(_cancellationTokenSource.Token);
            }
            else
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource = null;
            }
        }

        private Vector3R toNedFromPosition(Position3D lla)
        {
            if (!HomeLocation.IsInvalid())
            {
                // TODO: write all this math out.
                return new Vector3R();
            }
            else
            {
                return new Vector3R();
            }
        }

        private Position3D toPositionFromNed(Vector3R ned)
        {
            if (!HomeLocation.IsInvalid())
            {
                NedPoint nedPoint = new NedPoint(
                    Distance.FromMeters(ned.X),
                    Distance.FromMeters(ned.Y),
                    Distance.FromMeters(ned.Z));
                return nedPoint.ToPosition3D(HomeLocation);
            }
            else
            {
                return new Position3D(Latitude.Invalid, Longitude.Invalid, Distance.Invalid);
            }
        }

        private async void updateStateLoop(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    RpcResult<GeoPoint> homeLocation = await _controller.Proxy?.GetHomeGeoPointAsync();
                    if (homeLocation != null && homeLocation.Successful)
                    {
                        HomeLocation = new Position3D(
                            new Latitude(homeLocation.Value.Latitude),
                            new Longitude(homeLocation.Value.Longitude),
                            Distance.FromMeters(homeLocation.Value.Altitude));
                    }

                    token.ThrowIfCancellationRequested();
                    RpcResult<Vector3R> vehicleNedFromHome = await _controller.Proxy?.GetPositionAsync();
                    if (vehicleNedFromHome != null && vehicleNedFromHome.Successful && !HomeLocation.IsInvalid())
                    {
                        VehicleLocation = toPositionFromNed(vehicleNedFromHome.Value);
                    }

                    token.ThrowIfCancellationRequested();
                    RpcResult<QuaternionR> vehicleOrientation = await _controller.Proxy?.GetOrientationAsync();
                    if (vehicleOrientation != null && vehicleOrientation.Successful)
                    {
                        VectorMath.ToEulerAngles(vehicleOrientation.Value, out double roll, out double pitch, out double yaw);
                        VehicleRoll = Angle.FromRadians(roll);
                        VehiclePitch = Angle.FromRadians(pitch);
                        VehicleYaw = Angle.FromRadians(yaw);
                    }

                    token.ThrowIfCancellationRequested();
                    RpcResult<MultirotorState> vehicleState = await _controller.Proxy?.GetMultirotorStateAsync();
                    if (vehicleState != null && vehicleState.Successful)
                    {
                        VehicleLocationGps = new Position3D(
                            new Latitude(vehicleState.Value.GpsLocation.Latitude),
                            new Longitude(vehicleState.Value.GpsLocation.Longitude),
                            Distance.FromMeters(vehicleState.Value.GpsLocation.Altitude));
                    }

                    token.ThrowIfCancellationRequested();
                    RpcResult<bool> isApiControlEnabled = await _controller.Proxy?.GetIsApiControlEnabledAsync();
                    if (isApiControlEnabled != null && isApiControlEnabled.Successful)
                    {
                        ApiEnabled = isApiControlEnabled.Value;
                    }

                    await Task.Delay(100);
                }
            }
            catch (OperationCanceledException) { }
        }
    }
}