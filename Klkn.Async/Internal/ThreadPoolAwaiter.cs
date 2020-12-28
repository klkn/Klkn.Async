using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Klkn.Async.Internal
{
	/// <summary>
	/// Awaiter for switch to ThreadPool
	/// </summary>
	public class ThreadPoolAwaiter : RollbackAwaiter, INotifyCompletion
	{
		private readonly bool _forceSwitch;

		internal ThreadPoolAwaiter(bool forceSwitch)
		{
			_forceSwitch = forceSwitch;
		}

        #region Awaiter

		/// <summary>
		/// Check context
		/// </summary>
		public bool IsCompleted => !_forceSwitch && Thread.CurrentThread.IsThreadPoolThread;

		/// <inheritdoc/>
		public void OnCompleted(Action continuation)
		{
			var culture = Thread.CurrentThread.CurrentCulture;
			var uiCulture = Thread.CurrentThread.CurrentUICulture;
			var action = continuation;
			ThreadPool.QueueUserWorkItem(o =>
			{
				Thread.CurrentThread.CurrentCulture = culture;
				Thread.CurrentThread.CurrentUICulture = uiCulture;
				action();
			}, action);
		}

		/// <summary>
		/// Get Result
		/// </summary>
        public IAsyncDisposable GetResult() => this;

		#endregion Awaiter

	}
}
