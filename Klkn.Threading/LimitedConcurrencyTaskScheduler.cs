using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Klkn.Threading.Internal;

namespace Klkn.Threading
{
	/// <summary>
	/// TaskScheduler with limited concurrency
	/// </summary>
	public class LimitedConcurrencyTaskScheduler : TaskScheduler
	{
		private readonly int _maximumConcurrencyLevel;
		private readonly ConcurrentQueue<Task> _tasks = new ConcurrentQueue<Task>();
		private volatile int _currentPool;

		/// <summary>
		/// Factory for this Scheduler
		/// </summary>
		public TaskFactory Factory { get; }

		/// <summary>
		/// Default Constructor
		/// </summary>
		public LimitedConcurrencyTaskScheduler()
			: this(Environment.ProcessorCount)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="maximumConcurrencyLevel"></param>
		public LimitedConcurrencyTaskScheduler(int maximumConcurrencyLevel)
		{
			_maximumConcurrencyLevel = maximumConcurrencyLevel <= 0 ? Environment.ProcessorCount : maximumConcurrencyLevel;
			Factory = new TaskFactory(this);
		}

		#region Overrides of TaskScheduler

		/// <inheritdoc />
		protected override void QueueTask(Task task)
		{
			var state = task;
			for (; ; )
			{
				var currentCount = _currentPool;
				if (currentCount >= _maximumConcurrencyLevel)
					break;
				if (Interlocked.CompareExchange(ref _currentPool, currentCount + 1, currentCount) != currentCount)
				{
					continue;
				}
				ThreadPool.QueueUserWorkItem(ThreadPoolCallBack, state);
				return;
			}
			_tasks.Enqueue(state);
			var curCount = _currentPool;
			if (curCount == 0)
			{
				if (Interlocked.CompareExchange(ref _currentPool, curCount + 1, curCount) != curCount)
				{
					return;
				}
				ThreadPool.QueueUserWorkItem(ThreadPoolCallBack, null);
			}
		}

		/// <inheritdoc />
		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => false;

		/// <inheritdoc />
		protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

		#endregion

		private void ThreadPoolCallBack(object state)
		{
			Task task;
			if (state == null)
			{
				if (!_tasks.TryDequeue(out task))
				{
					Interlocked.Decrement(ref _currentPool);
					if (!_tasks.TryDequeue(out task))
					{
						return;
					}
					Interlocked.Increment(ref _currentPool);
				}
			}
			else
			{
				task = (Task)state;
			}
			var res = this.TryExecuteTask(task);
			Task nextTask;
			if (_tasks.TryDequeue(out nextTask))
			{
				ThreadPool.QueueUserWorkItem(ThreadPoolCallBack, nextTask);
				return;
			}

			Interlocked.Decrement(ref _currentPool);
			if (!_tasks.TryDequeue(out nextTask))
				return;
			Interlocked.Increment(ref _currentPool);
			ThreadPool.QueueUserWorkItem(ThreadPoolCallBack, nextTask);
		}
	}
}
