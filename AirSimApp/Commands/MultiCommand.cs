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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace AirSimApp.Commands
{
    /// <summary>Combines multiple commands into a single command object.</summary>
    public class MultiCommand : ICommand, IDisposable
    {
        public MultiCommand(IEnumerable<ICommand> commands)
        {
            _commands = new List<ICommand>();
            foreach (ICommand command in commands)
            {
                _commands.Add(command);
                command.CanExecuteChanged += onCanExecuteChanged;
            }
        }

        /// <inheritdoc cref="ICommand.CanExecuteChanged" />
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc cref="ICommand.CanExecute" />
        public bool CanExecute(object parameter)
        {
            return _commands.Any(cmd => cmd.CanExecute(parameter));
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            foreach (ICommand command in _commands)
            {
                command.CanExecuteChanged -= onCanExecuteChanged;
            }
        }

        /// <inheritdoc cref="ICommand.Execute" />
        public void Execute(object parameter)
        {
            foreach (ICommand command in _commands)
            {
                if (command.CanExecute(parameter))
                {
                    command.Execute(parameter);
                }
            }
        }

        private readonly List<ICommand> _commands;

        private void onCanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }
    }
}