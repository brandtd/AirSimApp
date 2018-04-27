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

using AirSimApp.Models;
using System;
using System.ComponentModel;
using static AirSimApp.Models.RcModel;

namespace AirSimApp.ViewModels
{
    public class RcViewModel : PropertyChangedBase, IDisposable
    {
        public RcViewModel(RcModel model)
        {
            _model = model;
            _model.PropertyChanged += onModelPropertyChanged;
        }

        /// <summary>Remote's pitch position.</summary>
        public float PitchAxis => _model.Pitch;

        /// <summary>Maximum pitch value.</summary>
        public float PitchMax => _model.PitchMax;

        /// <summary>Minimum pitch value.</summary>
        public float PitchMin => _model.PitchMin;

        /// <summary>Remote's roll position.</summary>
        public float RollAxis => _model.Roll;

        /// <summary>Minimum roll value.</summary>
        public float RollMax => _model.RollMax;

        /// <summary>Maximum roll value.</summary>
        public float RollMin => _model.RollMin;

        public bool Sw1 => _model.SwitchesPressed.HasFlag(Switches.Sw1);

        public bool Sw2 => _model.SwitchesPressed.HasFlag(Switches.Sw2);

        public bool Sw3 => _model.SwitchesPressed.HasFlag(Switches.Sw3);

        public bool Sw4 => _model.SwitchesPressed.HasFlag(Switches.Sw4);

        public bool Sw5 => _model.SwitchesPressed.HasFlag(Switches.Sw5);

        public bool Sw6 => _model.SwitchesPressed.HasFlag(Switches.Sw6);

        public bool Sw7 => _model.SwitchesPressed.HasFlag(Switches.Sw7);

        public bool Sw8 => _model.SwitchesPressed.HasFlag(Switches.Sw8);

        /// <summary>Remote's throttle position.</summary>
        public float ThrottleAxis => _model.Throttle;

        /// <summary>Maximum throttle value.</summary>
        public float ThrottleMax => _model.ThrottleMax;

        /// <summary>Minimum throttle value.</summary>
        public float ThrottleMin => _model.ThrottleMin;

        /// <summary>Whether values may be considered valid.</summary>
        public bool Valid => _model.Valid;

        /// <summary>Remote's yaw position.</summary>
        public float YawAxis => _model.Yaw;

        /// <summary>Maximum yaw value.</summary>
        public float YawMax => _model.YawMax;

        /// <summary>Minimum yaw value.</summary>
        public float YawMin => _model.YawMin;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                _model.PropertyChanged += onModelPropertyChanged;
            }
        }

        private readonly RcModel _model;
        private bool _disposed = false;

        private void onModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_model.Throttle):
                    OnPropertyChanged(nameof(ThrottleAxis));
                    break;

                case nameof(_model.Roll):
                    OnPropertyChanged(nameof(RollAxis));
                    break;

                case nameof(_model.Pitch):
                    OnPropertyChanged(nameof(PitchAxis));
                    break;

                case nameof(_model.Yaw):
                    OnPropertyChanged(nameof(YawAxis));
                    break;

                case nameof(_model.ThrottleMin):
                    OnPropertyChanged(nameof(ThrottleMin));
                    break;

                case nameof(_model.RollMin):
                    OnPropertyChanged(nameof(RollMin));
                    break;

                case nameof(_model.PitchMin):
                    OnPropertyChanged(nameof(PitchMin));
                    break;

                case nameof(_model.YawMin):
                    OnPropertyChanged(nameof(YawMin));
                    break;

                case nameof(_model.ThrottleMax):
                    OnPropertyChanged(nameof(ThrottleMax));
                    break;

                case nameof(_model.RollMax):
                    OnPropertyChanged(nameof(RollMax));
                    break;

                case nameof(_model.PitchMax):
                    OnPropertyChanged(nameof(PitchMax));
                    break;

                case nameof(_model.YawMax):
                    OnPropertyChanged(nameof(YawMax));
                    break;

                case nameof(_model.SwitchesPressed):
                    OnPropertyChanged(nameof(Sw1));
                    OnPropertyChanged(nameof(Sw2));
                    OnPropertyChanged(nameof(Sw3));
                    OnPropertyChanged(nameof(Sw4));
                    OnPropertyChanged(nameof(Sw5));
                    OnPropertyChanged(nameof(Sw6));
                    OnPropertyChanged(nameof(Sw7));
                    OnPropertyChanged(nameof(Sw8));
                    break;

                case nameof(_model.Valid):
                    OnPropertyChanged(nameof(Valid));
                    break;
            }
        }
    }
}