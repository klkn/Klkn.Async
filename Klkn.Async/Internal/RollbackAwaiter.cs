using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Klkn.Async.Internal
{
    /// <summary>
    /// Hold SynchronizationContext and back on Dispose
    /// </summary>
    public class RollbackAwaiter : IValueTaskSource, IAsyncDisposable
    {
        private readonly SynchronizationContext _synchronizationContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public RollbackAwaiter()
        {
            _synchronizationContext = SynchronizationContext.Current;
        }

        #region Implementation of IValueTaskSource

        /// <inheritdoc />
        public ValueTaskSourceStatus GetStatus(short token)
        {
            return _synchronizationContext == null ? ValueTaskSourceStatus.Succeeded : ValueTaskSourceStatus.Pending;
        }

        /// <inheritdoc />
        public void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags)
        {
            if (_synchronizationContext == null)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(continuation), state);
            }
            else
            {
                _synchronizationContext.Post(new SendOrPostCallback(continuation), state);
            }
        }

        /// <inheritdoc />
        public void GetResult(short token)
        {
        }

        #endregion

        #region Implementation of IAsyncDisposable

        /// <inheritdoc />
        public ValueTask DisposeAsync()
        {
            return new ValueTask(this, 0);
        }

        #endregion
    }
}
