using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Klkn.Async.Internal
{
	/// <summary>
	/// Awaiter for Task Factory
	/// </summary>
	public struct TaskSchedulerAwaiter : INotifyCompletion
	{
		private readonly TaskScheduler _taskScheduler;
		private readonly TaskCreationOptions _taskCreationOptions;

		internal TaskSchedulerAwaiter(TaskScheduler taskScheduler, TaskCreationOptions taskCreationOptions)
		{
			_taskScheduler = taskScheduler;
			_taskCreationOptions = taskCreationOptions;
		}


		/// <summary>
		/// Check context
		/// </summary>
		public bool IsCompleted => false;

		/// <inheritdoc/>
		public void OnCompleted(Action continuation)
		{
			var culture = Thread.CurrentThread.CurrentCulture;
			var uiCulture = Thread.CurrentThread.CurrentUICulture;
			var action = continuation;
			Task.Factory.StartNew(() =>
			{
				Thread.CurrentThread.CurrentCulture = culture;
				Thread.CurrentThread.CurrentUICulture = uiCulture;
				action();
			}, CancellationToken.None, _taskCreationOptions, _taskScheduler);
		}

		/// <summary>
		/// Get Result
		/// </summary>
		public void GetResult() { }

		/// <summary>
		/// To Awaiter, if we used ConfigureAwait
		/// </summary>
		/// <returns></returns>
		public TaskSchedulerAwaiter GetAwaiter()
		{
			return this;
		}

	}
}
