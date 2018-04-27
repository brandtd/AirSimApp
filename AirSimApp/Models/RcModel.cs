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
using MsgPackRpc;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace AirSimApp.Models
{
    /// <summary>Model for a remote controller.</summary>
    public class RcModel : PropertyChangedBase, IDisposable
    {
        public RcModel(ProxyController controller)
        {
            _controller = controller;
            _controller.PropertyChanged += onControllerPropertyChanged;

            startOrStopStateLoop();
        }

        [Flags]
        public enum Switches
        {
            Sw1 = 1 << 0,
            Sw2 = 1 << 1,
            Sw3 = 1 << 2,
            Sw4 = 1 << 3,
            Sw5 = 1 << 4,
            Sw6 = 1 << 5,
            Sw7 = 1 << 6,
            Sw8 = 1 << 7,
        }

        /// <summary>Whether model is connected with underlying data source.</summary>
        public bool Connected
        {
            get => _connected; set
            {
                SetProperty(ref _connected, value);
                if (!_connected)
                {
                    Valid = false;
                }
            }
        }

        public float Pitch { get => _pitch; set => SetProperty(ref _pitch, value); }

        public float PitchMax => 1.0f;

        public float PitchMin => -1.0f;

        public float Roll { get => _roll; set => SetProperty(ref _roll, value); }

        public float RollMax => 1.0f;

        public float RollMin => -1.0f;

        public Switches SwitchesPressed
        {
            get => _switchesPressed;
            set
            {
                if (_switchesPressed != value)
                {
                    _switchesPressed = value;
                    OnPropertyChanged();
                }
            }
        }

        public float Throttle { get => _throttle; set => SetProperty(ref _throttle, value); }

        public float ThrottleMax => 1.0f;

        public float ThrottleMin => 0.0f;

        /// <summary>Whether the R/C values may be considered valid.</summary>
        public bool Valid { get => _valid; set => SetProperty(ref _valid, value); }

        public float Yaw { get => _yaw; set => SetProperty(ref _yaw, value); }

        public float YawMax => 1.0f;

        public float YawMin => -1.0f;

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                _controller.PropertyChanged -= onControllerPropertyChanged;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        private readonly ProxyController _controller;

        private CancellationTokenSource _cancellationTokenSource;
        private bool _connected = false;
        private bool _disposed = false;
        private float _pitch = float.NaN;
        private float _roll = float.NaN;
        private Switches _switchesPressed;
        private float _throttle = float.NaN;
        private bool _valid = false;
        private float _yaw = float.NaN;

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

        private async void updateStateLoop(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    RpcResult<RcData> rcData = await _controller.Proxy?.GetRcDataAsync();
                    if (rcData.Successful)
                    {
                        Throttle = rcData.Value.Throttle;
                        Roll = rcData.Value.Roll;
                        Pitch = rcData.Value.Pitch;
                        Yaw = rcData.Value.Yaw;

                        SwitchesPressed =
                            (rcData.Value.Switch1 > 0 ? Switches.Sw1 : 0) |
                            (rcData.Value.Switch2 > 0 ? Switches.Sw2 : 0) |
                            (rcData.Value.Switch3 > 0 ? Switches.Sw3 : 0) |
                            (rcData.Value.Switch4 > 0 ? Switches.Sw4 : 0) |
                            (rcData.Value.Switch5 > 0 ? Switches.Sw5 : 0) |
                            (rcData.Value.Switch6 > 0 ? Switches.Sw6 : 0) |
                            (rcData.Value.Switch7 > 0 ? Switches.Sw7 : 0) |
                            (rcData.Value.Switch8 > 0 ? Switches.Sw8 : 0);

                        Valid = rcData.Value.IsValid;
                    }

                    await Task.Delay(100);
                }
            }
            catch (OperationCanceledException) { }
        }
    }
}