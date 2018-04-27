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
using DotSpatial.Positioning;
using DotSpatialExtensions;
using MsgPackRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AirSimApp.Models
{
    /// <summary>Model for a multirotor vehicle.</summary>
    public class MultirotorVehicleModel : ProxyModel, IDisposable
    {
        /// <summary>Wire up model.</summary>
        public MultirotorVehicleModel(ProxyController controller) : base(controller)
        {
        }

        public bool ApiEnabled
        {
            get => _apiEnabled;
            set => SetProperty(ref _apiEnabled, value);
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
            await Controller.Proxy?.CmdArmDisarmAsync(armIfTrue);
        }

        public Task DisarmAsync()
        {
            return ArmDisarmAsync(false);
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public override void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                base.Dispose();
            }
        }

        public async Task GoHomeAsync()
        {
            await Controller.Proxy?.CmdGoHomeAsync();
        }

        public async Task HoverAsync()
        {
            await Controller.Proxy?.CmdHoverInPlaceAsync();
        }

        public Task LandAsync()
        {
            return LandAsync(TimeSpan.Zero);
        }

        public async Task LandAsync(TimeSpan allowedTimeToLand)
        {
            await Controller.Proxy?.CmdLandAsync((float)allowedTimeToLand.TotalSeconds);
        }

        public async Task MoveByAngleThrottleAsync(Angle pitch, Angle roll, float throttle, float yaw_rate, TimeSpan duration)
        {
            await Controller.Proxy?.CmdMoveByAngleThrottleAsync(
                (float)pitch.DecimalDegrees,
                (float)roll.DecimalDegrees,
                throttle,
                yaw_rate,
                (float)duration.TotalSeconds);
        }

        public async Task MoveByAngleZAsync(Angle pitch, Angle roll, Distance z, Angle yaw, TimeSpan duration)
        {
            await Controller.Proxy?.CmdMoveByAngleZAsync(
                (float)pitch.DecimalDegrees,
                (float)roll.DecimalDegrees,
                (float)z.ToMeters().Value,
                (float)yaw.DecimalDegrees,
                (float)duration.TotalSeconds);
        }

        public async Task MoveByVelocityAsync(Speed vx, Speed vy, Speed vz, TimeSpan duration, DrivetrainType drivetrainType, YawMode yawMode)
        {
            await Controller.Proxy?.CmdMoveByVelocityAsync(
                (float)vx.ToMetersPerSecond().Value,
                (float)vy.ToMetersPerSecond().Value,
                (float)vz.ToMetersPerSecond().Value,
                (float)duration.TotalSeconds,
                drivetrainType,
                yawMode);
        }

        public async Task MoveByVelocityZAsync(Speed vx, Speed vy, Distance z, TimeSpan duration, DrivetrainType drivetrainType, YawMode yawMode)
        {
            await Controller.Proxy?.CmdMoveByVelocityZAsync(
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
            await Controller.Proxy?.CmdMoveOnPathAsync(
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
            await Controller.Proxy?.CmdMoveToPositionAsync(
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
            await Controller.Proxy?.CmdMoveToZAsync(
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
            await Controller.Proxy?.CmdResetAsync();
        }

        public async Task SetApiControlAllowedAsync(bool allowedIfTrue)
        {
            await Controller.Proxy?.CmdSetApiControlAsync(allowedIfTrue);
        }

        public async Task SetSimulationModeAsync(bool simulateIfTrue)
        {
            await Controller.Proxy?.CmdSimulationModeAsync(simulateIfTrue);
        }

        public Task TakeoffAsync()
        {
            return TakeoffAsync(TimeSpan.Zero);
        }

        public async Task TakeoffAsync(TimeSpan allowedTimeToTakeoff)
        {
            await Controller.Proxy?.CmdTakeoffAsync((float)allowedTimeToTakeoff.TotalSeconds);
        }

        /// <inheritdoc cref="ProxyModel.UpdateState(CancellationToken)" />
        protected override async Task UpdateState(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            RpcResult<GeoPoint> homeLocation = await Controller.Proxy?.GetHomeGeoPointAsync();
            if (homeLocation != null && homeLocation.Successful)
            {
                HomeLocation = new Position3D(
                    new Latitude(homeLocation.Value.Latitude),
                    new Longitude(homeLocation.Value.Longitude),
                    Distance.FromMeters(homeLocation.Value.Altitude));
            }

            token.ThrowIfCancellationRequested();
            RpcResult<Vector3R> vehicleNedFromHome = await Controller.Proxy?.GetPositionAsync();
            if (vehicleNedFromHome != null && vehicleNedFromHome.Successful && !HomeLocation.IsInvalid())
            {
                VehicleLocation = toPositionFromNed(vehicleNedFromHome.Value);
            }

            token.ThrowIfCancellationRequested();
            RpcResult<QuaternionR> vehicleOrientation = await Controller.Proxy?.GetOrientationAsync();
            if (vehicleOrientation != null && vehicleOrientation.Successful)
            {
                VectorMath.ToEulerAngles(vehicleOrientation.Value, out double roll, out double pitch, out double yaw);
                VehicleRoll = Angle.FromRadians(roll);
                VehiclePitch = Angle.FromRadians(pitch);
                VehicleYaw = Angle.FromRadians(yaw);
            }

            token.ThrowIfCancellationRequested();
            RpcResult<MultirotorState> vehicleState = await Controller.Proxy?.GetMultirotorStateAsync();
            if (vehicleState != null && vehicleState.Successful)
            {
                VehicleLocationGps = new Position3D(
                    new Latitude(vehicleState.Value.GpsLocation.Latitude),
                    new Longitude(vehicleState.Value.GpsLocation.Longitude),
                    Distance.FromMeters(vehicleState.Value.GpsLocation.Altitude));
            }

            token.ThrowIfCancellationRequested();
            RpcResult<bool> isApiControlEnabled = await Controller.Proxy?.GetIsApiControlEnabledAsync();
            if (isApiControlEnabled != null && isApiControlEnabled.Successful)
            {
                ApiEnabled = isApiControlEnabled.Value;
            }
        }

        private bool _apiEnabled = false;
        private bool _disposed = false;
        private Position3D _homeLocation = new Position3D(Latitude.Invalid, Longitude.Invalid, Distance.Invalid);
        private Position3D _vehicleLocation = new Position3D(Latitude.Invalid, Longitude.Invalid, Distance.Invalid);
        private Position3D _vehicleLocationGps = new Position3D(Latitude.Invalid, Longitude.Invalid, Distance.Invalid);
        private Angle _vehiclePitch = Angle.Invalid;
        private Angle _vehicleRoll = Angle.Invalid;
        private Angle _vehicleYaw = Angle.Invalid;

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
    }
}