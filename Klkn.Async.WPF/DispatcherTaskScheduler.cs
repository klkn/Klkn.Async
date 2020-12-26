using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Klkn.Async.WPF
{
	/// <summary>
	/// TaskScheduler for Dispatcher
	/// </summary>
	public class DispatcherTaskScheduler : TaskScheduler
	{
		private readonly Dispatcher _dispatcher;
		private readonly DispatcherPriority _priority;

		public DispatcherTaskScheduler(Dispatcher dispatcher, DispatcherPriority priority)
		{
			_dispatcher = dispatcher;
			_priority = priority;
		}

		#region Overrides of TaskScheduler

		/// <inheritdoc />
		protected override void QueueTask(Task task)
		{
			_dispatcher.BeginInvoke(_priority, new Action<Task>(t =>
			{
				TryExecuteTask(t);
			}), task);
		}

		/// <inheritdoc />
		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => false;

		/// <inheritdoc />
		protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

		#endregion
	}
}
