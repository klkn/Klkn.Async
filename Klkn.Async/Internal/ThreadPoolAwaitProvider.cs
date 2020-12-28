using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klkn.Async.Internal
{
    /// <summary>
    /// Provider object for the threadpool
    /// </summary>
    public class ThreadPoolAwaitProvider
    {
        private static readonly ThreadPoolAwaitProvider _forced = new ThreadPoolAwaitProvider(true);
        private readonly bool _forceSwitch;


        internal ThreadPoolAwaitProvider(bool forceSwitch)
        {
            _forceSwitch = forceSwitch;
        }

        public ThreadPoolAwaitProvider ForceSwitch()
        {
            return _forced;
        }

        /// <summary>
        /// Create Awaiter
        /// </summary>
        /// <returns></returns>
        public ThreadPoolAwaiter GetAwaiter()
        {
            return new ThreadPoolAwaiter(_forceSwitch);
        }
    }
}
