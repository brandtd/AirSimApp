#region MIT License (c) 2018

// Copyright 2018 Dan Brandt
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#endregion

namespace MsgPackRpc
{
    /// <summary>
    /// Result of RPC, signalling failure through properties instead of throwing exceptions.
    /// </summary>
    public class RpcResult
    {
        private bool _successful;

        /// <summary>
        /// Whether RPC was successful.
        /// </summary>
        public bool Successful
        {
            get => _successful;
            internal set
            {
                _successful = value;
            }
        }

        /// <summary>
        /// Whether RPC failed.
        /// </summary>
        public bool Failed
        {
            get => !_successful;
            internal set
            {
                _successful = !value;
            }
        }

        /// <summary>
        /// Error associated with RPC.
        /// </summary>
        public string Error { get; internal set; } = string.Empty;
    }

    /// <inheritdoc cref="RpcResult" />
    /// <typeparam name="T">Return value of RPC.</typeparam>
    public class RpcResult<T> : RpcResult
    {
        /// <summary>
        /// Return value of RPC.
        /// </summary>
        public T Value { get; internal set; } = default(T);
    }
}
