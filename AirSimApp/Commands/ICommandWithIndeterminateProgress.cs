using System.ComponentModel;
using System.Windows.Input;

namespace AirSimApp.Commands
{
    /// <summary>
    ///     Extends the <see cref="ICommand" /> interface to include an "in progress" value.
    /// </summary>
    public interface ICommandWithIndeterminateProgress : INotifyPropertyChanged, ICommand
    {
        /// <summary>Whether command is currently in progress.</summary>
        bool InProgress { get; }
    }
}