namespace MsgPackRpc
{
    /// <summary>
    /// Result of RPC, signalling failure through properties instead of throwing exceptions.
    /// </summary>
    /// <typeparam name="T">Return value of RPC.</typeparam>
    public class RpcResult<T>
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
        /// Return value of RPC.
        /// </summary>
        public T Value { get; internal set; } = default(T);

        /// <summary>
        /// Error associated with RPC.
        /// </summary>
        public string Error { get; internal set; } = string.Empty;
    }
}
