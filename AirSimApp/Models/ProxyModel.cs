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
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace AirSimApp.Models
{
    /// <summary>Base class for models that need to poll a proxy for their state info.</summary>
    public abstract class ProxyModel : PropertyChangedBase, IDisposable
    {
        public bool Connected
        {
            get => _connected;
            set => SetProperty(ref _connected, value);
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public virtual void Dispose()
        {
            if (!_disposed)
            {
                Controller.PropertyChanged -= onControllerPropertyChanged;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        protected ProxyModel(ProxyController controller)
        {
            Controller = controller;
            Controller.PropertyChanged += onControllerPropertyChanged;

            startOrStopStateLoop();
        }

        /// <summary>Controller for RPC proxy.</summary>
        protected ProxyController Controller { get; set; }

        /// <summary>Called periodically to update model state.</summary>
        protected abstract Task UpdateState(CancellationToken token);

        private CancellationTokenSource _cancellationTokenSource;
        private bool _connected = false;
        private bool _disposed = false;

        private void onControllerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Controller.Connected))
            {
                startOrStopStateLoop();
            }
        }

        private void startOrStopStateLoop()
        {
            Connected = Controller.Connected;
            if (Controller.Connected)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                updateStateLoop(_cancellationTokenSource.Token);
            }
            else
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource = null;
            }
        }

        private async void updateStateLoop(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    await UpdateState(token);
                    await Task.Delay(100);
                }
            }
            catch (OperationCanceledException) { }
        }
    }
}