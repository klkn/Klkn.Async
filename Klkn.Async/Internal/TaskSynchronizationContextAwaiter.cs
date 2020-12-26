using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Klkn.Async.Internal
{
	/// <summary>
	/// Awaiter for switching to SynchronizationContext after task
	/// </summary>
	/// <typeparam name="TOutput"></typeparam>
	public class TaskSynchronizationContextAwaiter<TOutput> : TaskSynchronizationContextAwaiterBase
	{
		internal TaskSynchronizationContextAwaiter(Task<TOutput> task, SynchronizationContext synchronizationContext)
			: base(task, synchronizationContext)
		{
		}

		/// <summary>
		/// Get Result
		/// </summary>
		public TOutput GetResult()
		{
			return ((Task<TOutput>) Task).Result;
		}
	}

	public class TaskSynchronizationContextAwaiter : TaskSynchronizationContextAwaiterBase
	{
		internal TaskSynchronizationContextAwaiter(Task task, SynchronizationContext synchronizationContext)
			: base(task, synchronizationContext)
		{
		}

		/// <summary>
		/// Get Result
		/// </summary>
		public void GetResult()
		{
			Task.Wait();
		}

	}

	/// <summary>
	/// Awaiter to continue process in particular SynchronizationContext
	/// </summary>
	public abstract class TaskSynchronizationContextAwaiterBase : INotifyCompletion
	{
		protected readonly Task Task;
		private readonly SynchronizationContext _synchronizationContext;

		internal TaskSynchronizationContextAwaiterBase(Task task, SynchronizationContext synchronizationContext)
		{
			Task = task;
			_synchronizationContext = synchronizationContext;
		}

		/// <summary>
		/// Check context
		/// </summary>
		public bool IsCompleted => Task.IsCompleted && _synchronizationContext == SynchronizationContext.Current;

		/// <inheritdoc/>
		public void OnCompleted(Action continuation)
		{
			var culture = Thread.CurrentThread.CurrentCulture;
			var uiCulture = Thread.CurrentThread.CurrentUICulture;
			if (Task.IsCompleted)
			{
				_synchronizationContext.Post(s =>
				{
					Thread.CurrentThread.CurrentCulture = culture;
					Thread.CurrentThread.CurrentUICulture = uiCulture;
					continuation();
				}, null);
				return;
			}

			Task.ContinueWith(t =>
				{
					Thread.CurrentThread.CurrentCulture = culture;
					Thread.CurrentThread.CurrentUICulture = uiCulture;
					continuation();
				},
				new Threading.SynchronizationContextTaskScheduler(_synchronizationContext));
		}

	}
}
