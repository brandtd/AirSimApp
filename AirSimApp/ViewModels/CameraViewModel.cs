using AirSimApp.Models;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace AirSimApp.ViewModels
{
    public class CameraViewModel : PropertyChangedBase, IDisposable
    {
        public CameraViewModel(CameraModel model)
        {
            _model = model;
            _model.PropertyChanged += onModelPropertyChanged;
        }

        public bool Connected => _model.Connected;
        public ImageSource Image => _model.Image;

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                _model.PropertyChanged -= onModelPropertyChanged;
            }
        }

        private readonly CameraModel _model;
        private bool _disposed = false;

        private void onModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_model.Image):
                    OnPropertyChanged(nameof(Image));
                    break;

                case nameof(_model.Connected):
                    OnPropertyChanged(nameof(Connected));
                    break;
            }
        }
    }
}