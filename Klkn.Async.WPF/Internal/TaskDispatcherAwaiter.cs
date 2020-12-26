using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Klkn.Async.WPF.Internal
{
	/// <summary>
	/// Task switch to dispatcher
	/// </summary>
	public class TaskDispatcherAwaiter<TOutput> : TaskDispatcherAwaiterBase
	{
		/// <inheritdoc />
		public TaskDispatcherAwaiter(Task<TOutput> task, Dispatcher dispatcher, DispatcherPriority priority, bool forceBreak) : base(task, dispatcher, priority, forceBreak)
		{
		}

		/// <summary>
		/// Get Result
		/// </summary>
		public TOutput GetResult()
		{
			return ((Task<TOutput>)_task).Result;
		}

		/// <summary>
		/// To Awaiter, if we used ConfigureAwait
		/// </summary>
		/// <returns></returns>
		public TaskDispatcherAwaiter<TOutput> GetAwaiter()
		{
			return this;
		}
	}

	/// <summary>
	/// Task switch to dispatcher
	/// </summary>
	public class TaskDispatcherAwaiter : TaskDispatcherAwaiterBase
	{
		/// <inheritdoc />
		public TaskDispatcherAwaiter(Task task, Dispatcher dispatcher, DispatcherPriority priority, bool forceBreak) : base(task, dispatcher, priority, forceBreak)
		{
		}

		/// <summary>
		/// Get Result
		/// </summary>
		public void GetResult()
		{
			_task.Wait();
		}

		/// <summary>
		/// To Awaiter, if we used ConfigureAwait
		/// </summary>
		/// <returns></returns>
		public TaskDispatcherAwaiter GetAwaiter()
		{
			return this;
		}
	}


	/// <summary>
	/// Task switch to dispatcher
	/// </summary>
	public class TaskDispatcherAwaiterBase : INotifyCompletion
	{
		internal readonly Task _task;
		private readonly Dispatcher _dispatcher;
		private readonly DispatcherPriority _priority;
		private readonly bool _forceBreak;

		public TaskDispatcherAwaiterBase(Task task, Dispatcher dispatcher, DispatcherPriority priority,
			bool forceBreak)
		{
			_task = task;
			_dispatcher = dispatcher;
			_priority = priority;
			_forceBreak = forceBreak;
		}

		/// <summary>
		/// Check context
		/// </summary>
		public bool IsCompleted => !_forceBreak && _task.IsCompleted && _dispatcher.CheckAccess();

		/// <inheritdoc/>
		public void OnCompleted(Action continuation)
		{
			if (!_forceBreak && _task.IsCompleted && _dispatcher.CheckAccess())
			{
				continuation();
				return;
			}

			if (_task.IsCompleted)
			{
				_dispatcher.BeginInvoke(_priority, continuation);
				return;
			}

			Action action = continuation;
			_task.ContinueWith(t => { action(); }, new DispatcherTaskScheduler(_dispatcher, _priority));
		}

	}
}
