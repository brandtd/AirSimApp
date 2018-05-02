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

        /// <summary>Whether vehicle is flying.</summary>
        public bool IsFlying
        {
            get => !_isLanded;
            set => IsLanded = !value;
        }

        /// <summary>Whether vehicle is landed.</summary>
        public bool IsLanded
        {
            get => _isLanded;
            set
            {
                if (_isLanded != value)
                {
                    _isLanded = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsFlying));

                    if (IsLanded)
                    {
                        Mode = VehicleMode.Landed;
                    }
                }
            }
        }

        public VehicleMode Mode
        {
            get => _mode;
            set
            {
                if (_mode != value)
                {
                    _mode = value;
                    OnPropertyChanged();
                }
            }
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
            await Controller.Proxy?.ArmDisarmAsync(armIfTrue);
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
            Mode = VehicleMode.Recovering;
            await Controller.Proxy?.GoHomeAsync();
        }

        public async Task HoverAsync()
        {
            await Controller.Proxy?.HoverAsync();
        }

        public Task LandAsync() => LandAsync(TimeSpan.FromSeconds(60));

        public async Task LandAsync(TimeSpan allowedTimeToLand)
        {
            Mode = VehicleMode.Landing;
            await Controller.Proxy?.LandAsync((float)allowedTimeToLand.TotalSeconds);
        }

        public async Task MoveByAngleThrottleAsync(Angle pitch, Angle roll, float throttle, float yaw_rate, TimeSpan duration)
        {
            Mode = VehicleMode.Moving;
            await Controller.Proxy?.MoveByAngleThrottleAsync(
                (float)pitch.DecimalDegrees,
                (float)roll.DecimalDegrees,
                throttle,
                yaw_rate,
                (float)duration.TotalSeconds);
            Mode = VehicleMode.Hovering;
        }

        public async Task MoveByAngleZAsync(Angle pitch, Angle roll, Distance z, Angle yaw, TimeSpan duration)
        {
            Mode = VehicleMode.Moving;
            await Controller.Proxy?.MoveByAngleZAsync(
                (float)pitch.DecimalDegrees,
                (float)roll.DecimalDegrees,
                (float)z.ToMeters().Value,
                (float)yaw.DecimalDegrees,
                (float)duration.TotalSeconds);
            Mode = VehicleMode.Hovering;
        }

        public async Task MoveByVelocityAsync(Speed vx, Speed vy, Speed vz, TimeSpan duration, DrivetrainType drivetrainType, YawMode yawMode)
        {
            Mode = VehicleMode.Moving;
            await Controller.Proxy?.MoveByVelocityAsync(
                (float)vx.ToMetersPerSecond().Value,
                (float)vy.ToMetersPerSecond().Value,
                (float)vz.ToMetersPerSecond().Value,
                (float)duration.TotalSeconds,
                drivetrainType,
                yawMode);
            Mode = VehicleMode.Hovering;
        }

        public async Task MoveByVelocityZAsync(Speed vx, Speed vy, Distance z, TimeSpan duration, DrivetrainType drivetrainType, YawMode yawMode)
        {
            Mode = VehicleMode.Moving;
            await Controller.Proxy?.MoveByVelocityZAsync(
                (float)vx.ToMetersPerSecond().Value,
                (float)vy.ToMetersPerSecond().Value,
                (float)z.ToMeters().Value,
                (float)duration.TotalSeconds,
                drivetrainType,
                yawMode);
            Mode = VehicleMode.Hovering;
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
            Mode = VehicleMode.Moving;
            IEnumerable<Vector3R> nedPositions = positions.Select(pos => toNedFromPosition(pos));
            await Controller.Proxy?.MoveOnPathAsync(
                nedPositions,
                (float)speed.ToMetersPerSecond().Value,
                (float)allowedTimeToComplete.TotalSeconds,
                drivetrainType,
                yawMode,
                lookahead,
                adaptiveLookahead);
            Mode = VehicleMode.Hovering;
        }

        public Task MoveToAltitudeAsync(Distance altitude, Speed speed) => MoveToAltitudeAsync(altitude, speed, TimeSpan.Zero);

        public Task MoveToAltitudeAsync(Distance altitude, Speed speed, TimeSpan allowedTimeToComplete)
            => MoveToAltitudeAsync(altitude, speed, allowedTimeToComplete, new YawMode { IsRate = false, YawOrRate = 0 }, -1.0f, 0.0f);

        public Task MoveToAltitudeAsync(Distance altitude, Speed speed, TimeSpan allowedTimeToComplete, YawMode yawMode, float lookahead, float adaptiveLookahead)
            => MoveToZAsync(HomeLocation.Altitude - altitude, speed, allowedTimeToComplete, yawMode, lookahead, adaptiveLookahead);

        public Task MoveToAsync(Position position, Speed approachSpeed) => MoveToAsync(position, approachSpeed, TimeSpan.Zero);

        public Task MoveToAsync(Position position, Speed approachSpeed, TimeSpan allowedTime) => MoveToAsync(new Position3D(VehicleLocation.Altitude, position), approachSpeed, allowedTime);

        public Task MoveToAsync(Position3D position, Speed approachSpeed) => MoveToAsync(position, approachSpeed, TimeSpan.Zero);

        public async Task MoveToAsync(Position3D position, Speed approachSpeed, TimeSpan allowedTime)
        {
            Mode = VehicleMode.Moving;
            Vector3R ned = toNedFromPosition(position);
            await Controller.Proxy?.MoveToPositionAsync(
                ned.X,
                ned.Y,
                ned.Z,
                (float)approachSpeed.InMetersPerSecond(),
                (float)allowedTime.TotalSeconds,
                DrivetrainType.ForwardOnly,
                new YawMode { IsRate = false, YawOrRate = 0 },
                -1.0f,
                0.0f);
            Mode = VehicleMode.Hovering;
        }

        public async Task MoveToZAsync(
            Distance z,
            Speed speed,
            TimeSpan allowedTimeToComplete,
            YawMode yawMode,
            float lookahead,
            float adaptiveLookahead)
        {
            Mode = VehicleMode.Moving;
            await Controller.Proxy?.MoveToZAsync(
                (float)z.ToMeters().Value,
                (float)speed.ToMetersPerSecond().Value,
                (float)allowedTimeToComplete.TotalSeconds,
                yawMode,
                lookahead,
                adaptiveLookahead);
            Mode = VehicleMode.Hovering;
        }

        public async Task ResetAsync()
        {
            await Controller.Proxy?.ResetAsync();
        }

        public async Task SetApiControlAllowedAsync(bool allowedIfTrue)
        {
            await Controller.Proxy?.EnableApiControlAsync(allowedIfTrue);
        }

        public async Task SetSimulationModeAsync(bool simulateIfTrue)
        {
            await Controller.Proxy?.SetSimulationModeAsync(simulateIfTrue);
        }

        public Task TakeoffAsync() => TakeoffAsync(TimeSpan.FromSeconds(5));

        public async Task TakeoffAsync(TimeSpan allowedTimeToTakeoff)
        {
            Mode = VehicleMode.Launching;
            await Controller.Proxy?.TakeoffAsync((float)allowedTimeToTakeoff.TotalSeconds);
            Mode = VehicleMode.Hovering;
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
                _vehicleDown = vehicleNedFromHome.Value.Z;
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
            RpcResult<bool> isApiControlEnabled = await Controller.Proxy?.IsApiControlEnabledAsync();
            if (isApiControlEnabled != null && isApiControlEnabled.Successful)
            {
                ApiEnabled = isApiControlEnabled.Value;
            }

            token.ThrowIfCancellationRequested();
            RpcResult<LandedState> landedState = await Controller.Proxy?.GetLandedStateAsync();
            if (landedState != null && landedState.Successful)
            {
                IsLanded = landedState.Value == LandedState.Landed;
            }
        }

        private bool _apiEnabled = false;
        private bool _disposed = false;
        private Position3D _homeLocation = new Position3D(Latitude.Invalid, Longitude.Invalid, Distance.Invalid);
        private bool _isLanded;
        private VehicleMode _mode = VehicleMode.Unknown;
        private float _vehicleDown = 0;
        private Position3D _vehicleLocation = new Position3D(Latitude.Invalid, Longitude.Invalid, Distance.Invalid);
        private Position3D _vehicleLocationGps = new Position3D(Latitude.Invalid, Longitude.Invalid, Distance.Invalid);
        private Angle _vehiclePitch = Angle.Invalid;
        private Angle _vehicleRoll = Angle.Invalid;
        private Angle _vehicleYaw = Angle.Invalid;

        private Vector3R toNedFromPosition(Position3D lla)
        {
            if (!HomeLocation.IsInvalid())
            {
                NedPoint ned = lla.ToNedPoint(HomeLocation);
                return new Vector3R { X = (float)ned.N.InMeters(), Y = (float)ned.E.InMeters(), Z = (float)ned.D.InMeters() };
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