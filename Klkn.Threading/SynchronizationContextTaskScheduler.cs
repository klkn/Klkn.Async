using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Klkn.Threading.Internal;

namespace Klkn.Threading
{
	/// <summary>
	/// Scheduler that execute tasks in scheduler
	/// </summary>
	public class SynchronizationContextTaskScheduler : TaskScheduler
	{
		/// <summary>
		/// TaskFactory for current scheduler
		/// </summary>
		public TaskFactory Factory => _taskFactory ?? (_taskFactory = new TaskFactory(this));

		private TaskFactory _taskFactory;

		private readonly SynchronizationContext _synchronizationContext;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="synchronizationContext"></param>
		public SynchronizationContextTaskScheduler(SynchronizationContext synchronizationContext)
		{
			_synchronizationContext = synchronizationContext ?? throw new ArgumentNullException(nameof(synchronizationContext));
		}

		#region Overrides of TaskScheduler

		/// <inheritdoc />
		protected override void QueueTask(Task task)
		{
			_synchronizationContext.Post(SendOrPostCallback, task);
		}

		/// <inheritdoc />
		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => false;

		/// <inheritdoc />
		protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

		/// <inheritdoc />
		public override int MaximumConcurrencyLevel => 1;

		#endregion

		private void SendOrPostCallback(object state)
		{
			var task = (Task)state;
			TryExecuteTask(task);
		}
	}
}
