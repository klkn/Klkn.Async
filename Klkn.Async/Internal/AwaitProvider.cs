using System;

namespace Klkn.Async.Internal
{
    /// <summary>
    /// Stub to static awaiter
    /// </summary>
    /// <typeparam name="TAwaiter"></typeparam>
    public class AwaitProvider<TAwaiter>
    {
        private readonly Func<TAwaiter> _create;

        internal AwaitProvider(Func<TAwaiter> create)
        {
            _create = create;
        }

        /// <summary>
        /// Create awaiter
        /// </summary>
        /// <returns></returns>
        public TAwaiter GetAwaiter() => _create();
    }
}
