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