using System;

namespace AirSimApp.Commands
{
    /// <summary>
    ///     Implements the "InProgress" logic for a class inheriting from <see
    ///     cref="ICommandWithIndeterminateProgress" />.
    /// </summary>
    public abstract class CommandWithIndeterminateProgress : PropertyChangedBase, ICommandWithIndeterminateProgress
    {
        /// <inheritdoc cref="ICommand.CanExecuteChanged" />
        public abstract event EventHandler CanExecuteChanged;

        /// <inheritdoc cref="ICommandWithIndeterminateProgress.InProgress" />
        public virtual bool InProgress
        {
            get => _inprogress;
            private set
            {
                if (_inprogress != value)
                {
                    _inprogress = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <inheritdoc cref="ICommand.CanExecute" />
        public abstract bool CanExecute(object parameter);

        /// <inheritdoc cref="ICommand.Execute" />
        public abstract void Execute(object parameter);

        /// <summary>Call to signal that an execution has completed.</summary>
        protected void ExecutionCompleted()
        {
            _executionCount--;
            if (_executionCount <= 0)
            {
                InProgress = false;
            }
        }

        /// <summary>Call to signal that an execution is starting.</summary>
        protected void StartingExecution()
        {
            _executionCount++;
            if (_executionCount > 0)
            {
                InProgress = true;
            }
        }

        private int _executionCount = 0;
        private bool _inprogress = false;
    }
}