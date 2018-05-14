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

using Db.Controls;
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