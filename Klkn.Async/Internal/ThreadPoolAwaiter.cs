using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Klkn.Async.Internal
{
	/// <summary>
	/// Awaiter for switch to ThreadPool
	/// </summary>
	public class ThreadPoolAwaiter : INotifyCompletion
	{
		private readonly bool _forceSwitch;

		internal ThreadPoolAwaiter(bool forceSwitch)
		{
			_forceSwitch = forceSwitch;
		}

		/// <summary>
		/// Create new task in pool request
		/// </summary>
		/// <returns></returns>
		public ThreadPoolAwaiter ForceSwitch()
		{
			return new ThreadPoolAwaiter(true);
		}

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
		public void GetResult() { }

		/// <summary>
		/// To Awaiter, if we used ConfigureAwait
		/// </summary>
		/// <returns></returns>
		public ThreadPoolAwaiter GetAwaiter()
		{
			return this;
		}
	}
}
