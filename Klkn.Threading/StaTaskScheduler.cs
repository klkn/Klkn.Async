using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Klkn.Threading
{
	/// <summary>
	/// STA TaskScheduler
	/// </summary>
	public class StaTaskScheduler : TaskScheduler
	{
		/// <summary>
		/// STA Scheduler
		/// </summary>
		public static TaskScheduler Scheduler { get; } = new StaTaskScheduler();

		/// <summary>
		/// TaskFactory for current scheduler
		/// </summary>
		public static TaskFactory Factory { get; } = new TaskFactory(new StaTaskScheduler());

		private StaTaskScheduler()
		{
		}

		#region Overrides of TaskScheduler

		/// <inheritdoc />
		protected override void QueueTask(Task task)
		{
			StaThreadPool.QueueUserWorkItem(ExecuteTask, task);
		}

		private void ExecuteTask(object state)
		{
			TryExecuteTask((Task)state);
		}

		/// <inheritdoc />
		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => false;

		/// <inheritdoc />
		protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

		#endregion
	}
}
