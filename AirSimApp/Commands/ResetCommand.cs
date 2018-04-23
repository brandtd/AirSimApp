using System;
using System.ComponentModel;
using System.Windows.Input;

namespace AirSimApp.Commands
{
    /// <summary>
    /// Command for reseting simulation.
    /// </summary>
    public class ResetCommand : ICommand, IDisposable
    {
        private readonly ProxyController _controller;

        private bool _canExecute;

        /// <inheritdoc cref="ICommand.CanExecuteChanged" />
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Wire up command.
        /// </summary>
        public ResetCommand(ProxyController controller)
        {
            _controller = controller;

            _controller.PropertyChanged += onControllerPropertyChanged;

            _canExecute = CanExecute(null);
        }

        /// <inheritdoc cref="ICommand.CanExecute" />
        public bool CanExecute(object parameter)
        {
            return _controller.Connected;
        }

        /// <inheritdoc cref="ICommand.Execute" />
        public async void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                await _controller.CmdResetAsync();
            }
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _controller.PropertyChanged -= onControllerPropertyChanged;
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
