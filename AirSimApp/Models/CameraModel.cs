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
        public CameraModel(int cameraId, ProxyController controller) : base(controller)
        {
            _cameraId = cameraId;
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
            RpcResult<byte[]> image = await Controller.Proxy?.GetImageAsync(_cameraId, ImageType.Scene);
            if (image != null && image.Successful)
            {
                _stream?.Dispose();
                _stream = new MemoryStream(image.Value);
                PngBitmapDecoder decoder = new PngBitmapDecoder(_stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                Image = decoder.Frames[0];
            }
        }

        private readonly int _cameraId;
        private ImageSource _bitmap;
        private bool _disposed = false;
        private MemoryStream _stream;
    }
}