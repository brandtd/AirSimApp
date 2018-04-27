using AirSimRpc;
using MsgPackRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AirSimApp.Models
{
    /// <summary>Model for a camera.</summary>
    public class CameraModel : ProxyModel, IDisposable
    {
        /// <summary>Wire up model.</summary>
        public CameraModel(ProxyController controller) : base(controller)
        {
        }

        public ImageSource Image
        {
            get => _bitmap;
            set
            {
                _bitmap = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public override void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                base.Dispose();

                _stream?.Dispose();
            }
        }

        /// <inheritdoc cref="ProxyModel.UpdateState(CancellationToken)" />
        protected override async Task UpdateState(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            RpcResult<byte[]> image = await Controller.Proxy?.GetImageAsync(0, ImageType.Scene);
            if (image != null && image.Successful)
            {
                _stream?.Dispose();
                _stream = new MemoryStream(image.Value);
                PngBitmapDecoder decoder = new PngBitmapDecoder(_stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                Image = decoder.Frames[0];
            }
        }

        private ImageSource _bitmap;
        private bool _disposed = false;
        private MemoryStream _stream;
    }
}