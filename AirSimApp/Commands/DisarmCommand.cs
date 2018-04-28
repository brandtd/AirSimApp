using AirSimApp.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace AirSimApp.Commands
{
    /// <summary>Command for disarming vehicle.</summary>
    public class DisarmCommand : ICommand, IDisposable
    {
        /// <summary>Wire up command.</summary>
        public DisarmCommand(MultirotorVehicleModel vehicle)
        {
            _vehicle = vehicle;

            _vehicle.PropertyChanged += onControllerPropertyChanged;

            _canExecute = CanExecute(null);
        }

        /// <inheritdoc cref="ICommand.CanExecuteChanged" />
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc cref="ICommand.CanExecute" />
        public bool CanExecute(object parameter)
        {
            return _vehicle.Connected && _vehicle.ApiEnabled;
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _vehicle.PropertyChanged -= onControllerPropertyChanged;
        }

        /// <inheritdoc cref="ICommand.Execute" />
        public async void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                await _vehicle.DisarmAsync();
            }
        }

        private readonly MultirotorVehicleModel _vehicle;

        private bool _canExecute;

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