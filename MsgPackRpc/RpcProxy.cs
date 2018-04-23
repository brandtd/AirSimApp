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

using MessagePack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MsgPackRpc
{
    /// <summary>
    /// Calls RPCs on a remote RPC server.
    /// </summary>
    public class RpcProxy : IDisposable
    {
        private readonly Dictionary<uint, TaskCompletionSource<RpcResponse>> _requests = new Dictionary<uint, TaskCompletionSource<RpcResponse>>();
        private readonly object _lockObject = new object();

        private bool _disposed = false;
        private CancellationTokenSource _cancellationTokenSource;
        private TcpClient _client;
        private uint _msgIdCounter = 0;

        /// <summary>
        /// Whether this proxy is connected with a server.
        /// </summary>
        public bool Connected => _client != null && _client.Connected;

        /// <summary>
        /// Connect with the RPC server.
        /// </summary>
        public async Task<bool> ConnectAsync(IPEndPoint endpoint)
        {
            _cancellationTokenSource?.Cancel();
            _client?.Dispose();
            _client = new TcpClient()
            {
                ReceiveTimeout = 1000,
                SendTimeout = 1000
            };

            // TODO: allow for cancellation token on ConnectAsync and configuring connect timeout
            await _client.ConnectAsync(endpoint.Address, endpoint.Port);

            _cancellationTokenSource = new CancellationTokenSource();
            runReceiveLoop(_cancellationTokenSource.Token);

            return _client.Connected;
        }

        /// <summary>
        /// Make an RPC call.
        /// </summary>
        /// <typeparam name="T">RPC return type.</typeparam>
        /// <param name="method">Name of RPC method.</param>
        /// <param name="args">Arguments for method.</param>
        /// <returns>Awaitable task with RPC result.</returns>
        public async Task<RpcResult<T>> CallAsync<T>(string method, params object[] args)
        {
            if (_disposed)
            {
                return new RpcResult<T>
                {
                    Failed = true,
                    Error = "Proxy disposed",
                };
            }
            if (_client == null || !_client.Connected)
            {
                return new RpcResult<T>
                {
                    Failed = true,
                    Error = "Proxy not connected with RPC server",
                };
            }

            RpcRequest request = new RpcRequest
            {
                Type = 0,
                MsgId = _msgIdCounter,
                Method = method,
                Params = args ?? new object[] { },
            };
            ++_msgIdCounter;

            addRequest(request.MsgId);

            byte[] data = MessagePackSerializer.Serialize(request);
            await _client.GetStream().WriteAsync(data, 0, data.Length);
            RpcResponse response = await taskForRequest(request.MsgId).Task;

            removeRequest(request.MsgId);

            if (response.Error != null)
            {
                return new RpcResult<T>
                {
                    Failed = true,
                    Error = $"{response.Error}"
                };
            }

            return new RpcResult<T>
            {
                Successful = true,
                Value = JsonConvert.DeserializeObject<T>(response.ResultAsJson)
            };
        }

        /// <summary>
        /// Make an RPC call with no return value.
        /// </summary>
        /// <param name="method">Name of RPC method.</param>
        /// <param name="args">Arguments for method.</param>
        /// <returns>Awaitable task with RPC result.</returns>
        public async Task<RpcResult> CallAsync(string method, params object[] args)
        {
            if (_disposed)
            {
                return new RpcResult
                {
                    Failed = true,
                    Error = "Proxy disposed",
                };
            }
            if (_client == null || !_client.Connected)
            {
                return new RpcResult
                {
                    Failed = true,
                    Error = "Proxy not connected with RPC server",
                };
            }

            RpcRequest request = new RpcRequest
            {
                Type = 0,
                MsgId = _msgIdCounter,
                Method = method,
                Params = args ?? new object[] { },
            };
            ++_msgIdCounter;

            addRequest(request.MsgId);

            byte[] data = MessagePackSerializer.Serialize(request);
            await _client.GetStream().WriteAsync(data, 0, data.Length);
            RpcResponse response = await taskForRequest(request.MsgId).Task;

            removeRequest(request.MsgId);

            if (response.Error != null)
            {
                return new RpcResult
                {
                    Failed = true,
                    Error = $"{response.Error}"
                };
            }

            return new RpcResult
            {
                Successful = true
            };
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                _cancellationTokenSource?.Cancel();
                _client?.Dispose();
            }
        }

        private void addRequest(uint requestId)
        {
            lock (_lockObject)
            {
                _requests[requestId] = new TaskCompletionSource<RpcResponse>();
            }
        }

        private TaskCompletionSource<RpcResponse> taskForRequest(uint requestId)
        {
            lock (_lockObject)
            {
                return _requests[requestId];
            }
        }

        private void removeRequest(uint requestId)
        {
            lock (_lockObject)
            {
                _requests.Remove(requestId);
            }
        }

        private async void runReceiveLoop(CancellationToken token)
        {
            try
            {
                byte[] buffer = new byte[32 * 1 << 10];
                while (!_disposed && _client != null && _client.Connected)
                {
                    token.ThrowIfCancellationRequested();

                    int numBytes = await _client.GetStream().ReadAsync(buffer, 0, buffer.Length, token);
                    if (numBytes > 0)
                    {
                        RpcResponse response = MessagePackSerializer.Deserialize<RpcResponse>(buffer);
                        response.ResultAsJson = JsonConvert.SerializeObject(response.Result);
                        taskForRequest(response.MsgId).SetResult(response);
                    }
                }
            }
            catch (OperationCanceledException e)
            {
                if (e.CancellationToken == token)
                {
                    throw;
                }
            }
        }
    }
}
