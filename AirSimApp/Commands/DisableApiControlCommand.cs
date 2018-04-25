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
using System.Windows.Input;

namespace AirSimApp.Commands
{
    /// <summary>Command for disabling API control.</summary>
    public class DisableApiControlCommand : ICommand, IDisposable
    {
        private readonly MultirotorVehicleModel _vehicle;

        private bool _canExecute;

        /// <inheritdoc cref="ICommand.CanExecuteChanged" />
        public event EventHandler CanExecuteChanged;

        /// <summary>Wire up command.</summary>
        public DisableApiControlCommand(MultirotorVehicleModel vehicle)
        {
            _vehicle = vehicle;

            _vehicle.PropertyChanged += onControllerPropertyChanged;

            _canExecute = CanExecute(null);
        }

        /// <inheritdoc cref="ICommand.CanExecute" />
        public bool CanExecute(object parameter)
        {
            return _vehicle.Connected && _vehicle.ApiEnabled;
        }

        /// <inheritdoc cref="ICommand.Execute" />
        public async void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                await _vehicle.SetApiControlAllowedAsync(false);
            }
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _vehicle.PropertyChanged -= onControllerPropertyChanged;
        }

        private void onControllerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool newCanExecute = CanExecute(null);
            if (newCanExecute != _canExecute)
            {
                _canExecute = newCanExecute;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}