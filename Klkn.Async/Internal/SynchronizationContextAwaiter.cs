using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Klkn.Async.Internal
{
	/// <summary>
	/// Awaiter to particular SynchronizationContext
	/// await SynchronizationContext
	/// </summary>
	public struct SynchronizationContextAwaiter : INotifyCompletion
	{
		private readonly SynchronizationContext _synchronizationContext;

		internal SynchronizationContextAwaiter(SynchronizationContext synchronizationContext)
		{
			_synchronizationContext = synchronizationContext;
		}

		/// <summary>
		/// Check context
		/// </summary>
		public bool IsCompleted => _synchronizationContext == SynchronizationContext.Current;

		/// <inheritdoc/>
		public void OnCompleted(Action continuation)
		{
			var culture = Thread.CurrentThread.CurrentCulture;
			var uiCulture = Thread.CurrentThread.CurrentUICulture;
			_synchronizationContext.Post(s =>
			{
				Thread.CurrentThread.CurrentCulture = culture;
				Thread.CurrentThread.CurrentUICulture = uiCulture;
				continuation();
			}, null);
		}

		/// <summary>
		/// Get Result
		/// </summary>
		public void GetResult() { }

	}
}
