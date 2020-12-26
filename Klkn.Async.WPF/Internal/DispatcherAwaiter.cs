using System;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace Klkn.Async.WPF.Internal
{
	/// <summary>
	/// Awaiter for switching to Dispatcher
	/// </summary>
	public struct DispatcherAwaiter : INotifyCompletion
	{
		private readonly Dispatcher _dispatcher;
		private readonly DispatcherPriority _priority;
		private readonly bool _forceBreak;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dispatcher"></param>
		/// <param name="priority"></param>
		/// <param name="forceBreak"></param>
		internal DispatcherAwaiter(Dispatcher dispatcher, DispatcherPriority priority, bool forceBreak)
		{
			_dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
			_priority = priority;
			_forceBreak = forceBreak;
		}

		/// <summary>
		/// Check context
		/// </summary>
		public bool IsCompleted => !_forceBreak && _dispatcher.CheckAccess();

		/// <inheritdoc/>
		public void OnCompleted(Action continuation)
		{
			_dispatcher.BeginInvoke(_priority, continuation);
		}

		/// <summary>
		/// Get Result
		/// </summary>
		public void GetResult() { }

		/// <summary>
		/// Get Awaiter
		/// </summary>
		/// <returns></returns>
		public DispatcherAwaiter GetAwaiter()
		{
			return this;
		}
	}
}
