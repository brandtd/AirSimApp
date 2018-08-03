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

using System.Windows;
using System.Windows.Controls;

namespace Db.Controls
{
    /// <summary>A normal button with an "executing" property used to style the button.</summary>
    public class IndeterminateButton : Button
    {
        /// <summary>Whether command is currently executing.</summary>
        public static readonly DependencyProperty ExecutingProperty =
            DependencyProperty.Register(
            nameof(Executing),
            typeof(bool),
            typeof(IndeterminateButton),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        static IndeterminateButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IndeterminateButton), new FrameworkPropertyMetadata(typeof(IndeterminateButton)));
        }

        /// <inheritdoc cref="ExecutingProperty" />
        public bool Executing
        {
            get => (bool)GetValue(ExecutingProperty);
            set => SetValue(ExecutingProperty, value);
        }
    }
}