using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Klkn.Async.Internal
{
	/// <inheritdoc/>
	public class TaskTaskSchedulerAwaiter<TOutput> : TaskTaskSchedulerAwaiterBase
	{
		/// <inheritdoc />
		public TaskTaskSchedulerAwaiter(Task<TOutput> task, TaskScheduler taskScheduler, TaskContinuationOptions taskContinuationOptions) 
			: base(task, taskScheduler, taskContinuationOptions)
		{
		}

		/// <summary>
		/// Get Result
		/// </summary>
		public TOutput GetResult()
		{
			return ((Task<TOutput>)Task).Result;
		}

		/// <summary>
		/// To Awaiter, if we used ConfigureAwait
		/// </summary>
		/// <returns></returns>
		public TaskTaskSchedulerAwaiter<TOutput> GetAwaiter()
		{
			return this;
		}
	}



	/// <inheritdoc/>
	public class TaskTaskSchedulerAwaiter : TaskTaskSchedulerAwaiterBase
	{
		/// <inheritdoc />
		public TaskTaskSchedulerAwaiter(Task task, TaskScheduler taskScheduler, TaskContinuationOptions taskContinuationOptions) 
			: base(task, taskScheduler, taskContinuationOptions)
		{
		}

		/// <summary>
		/// Get Result
		/// </summary>
		public IAsyncDisposable GetResult()
		{
			Task.Wait();
			return this;
        }

		/// <summary>
		/// To Awaiter, if we used ConfigureAwait
		/// </summary>
		/// <returns></returns>
		public TaskTaskSchedulerAwaiter GetAwaiter()
		{
			return this;
		}
	}

	/// <summary>
	/// Awaiter to continue Task
	/// </summary>
	public abstract class TaskTaskSchedulerAwaiterBase : RollbackAwaiter, INotifyCompletion
	{
		protected readonly Task Task;
		private readonly TaskScheduler _taskScheduler;
		private readonly TaskContinuationOptions _taskContinuationOptions;

		internal TaskTaskSchedulerAwaiterBase(Task task, TaskScheduler taskScheduler, TaskContinuationOptions taskContinuationOptions)
		{
			Task = task ?? throw new ArgumentNullException(nameof(task));
			_taskScheduler = taskScheduler ?? throw new ArgumentNullException(nameof(taskScheduler));
			_taskContinuationOptions = taskContinuationOptions;
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
			Task.ContinueWith(t =>
			{
				Thread.CurrentThread.CurrentCulture = culture;
				Thread.CurrentThread.CurrentUICulture = uiCulture;
				action();
			}, CancellationToken.None, _taskContinuationOptions, _taskScheduler);
		}
	}
}
