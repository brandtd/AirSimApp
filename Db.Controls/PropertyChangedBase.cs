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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Db.Controls
{
    /// <summary>
    ///     Provides method for firing property changed event with correct property name.
    /// </summary>
    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        /// <inheritdoc cref="INotifyPropertyChanged" />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Fires property changed event, automatically figuring out the property name if called
        ///     from within the setter of a property.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        ///     Sets the property and fires <see cref="PropertyChanged" /> event if new value is not
        ///     equal to current value.
        /// </summary>
        protected void SetProperty<T>(ref T property, T value, [CallerMemberName] string name = "") where T : IEquatable<T>
        {
            if (!property.Equals(value))
            {
                property = value;
                OnPropertyChanged(name);
            }
        }
    }
}