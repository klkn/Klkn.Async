using System.Threading.Tasks;
using Klkn.Async.Internal;
using Klkn.Threading;

namespace Klkn.Async
{
	/// <summary>
	/// Await Helpers
	/// </summary>
	public static class Await
	{
		/// <summary>
		/// Switching to ThreadPool
		/// </summary>
		public static ThreadPoolAwaiter ThreadPool { get; } = new ThreadPoolAwaiter(false);

		/// <summary>
		/// Switching to Sta
		/// </summary>
		public static TaskSchedulerAwaiter StaPool { get; } = new TaskSchedulerAwaiter(StaTaskScheduler.Scheduler, TaskCreationOptions.None);

		/// <summary>
		/// Continue in STA Thread
		/// </summary>
		/// <param name="task"></param>
		/// <returns></returns>
		public static TaskTaskSchedulerAwaiter ContinueAwaitSta(this Task task)
		{
			return new TaskTaskSchedulerAwaiter(task, StaTaskScheduler.Scheduler, TaskContinuationOptions.None);
		}

		/// <summary>
		/// Continue in STA Thread
		/// </summary>
		/// <param name="task"></param>
		/// <returns></returns>
		public static TaskTaskSchedulerAwaiter<TOutput> ContinueAwaitSta<TOutput>(this Task<TOutput> task)
		{
			return new TaskTaskSchedulerAwaiter<TOutput>(task, StaTaskScheduler.Scheduler, TaskContinuationOptions.None);
		}

	}
}
