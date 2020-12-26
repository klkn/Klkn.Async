using System.Threading.Tasks;
using Klkn.Async.Internal;

namespace Klkn.Async
{
	/// <summary>
	/// Async helper for awaiting TaskScheduler
	/// </summary>
	public static class TaskSchedulerExtension
	{

		#region TaskTaskSchedulerAwaiter

		/// <summary>
		/// Continue with specific TaskScheduler
		/// </summary>
		/// <param name="task"></param>
		/// <param name="taskFactory"></param>
		/// <param name="taskContinuationOptions"></param>
		/// <returns></returns>
		public static TaskTaskSchedulerAwaiter<TOutput> ConfigureAwait<TOutput>(this Task<TOutput> task, TaskFactory taskFactory, TaskContinuationOptions taskContinuationOptions)
		{
			return new TaskTaskSchedulerAwaiter<TOutput>(task, taskFactory.Scheduler ?? TaskScheduler.Default, taskContinuationOptions);
		}

		/// <summary>
		/// Continue with specific TaskScheduler
		/// </summary>
		/// <param name="task"></param>
		/// <param name="taskFactory"></param>
		/// <returns></returns>
		public static TaskTaskSchedulerAwaiter<TOutput> ConfigureAwait<TOutput>(this Task<TOutput> task, TaskFactory taskFactory)
		{
			return new TaskTaskSchedulerAwaiter<TOutput>(task, taskFactory.Scheduler ?? TaskScheduler.Default, TaskContinuationOptions.None);
		}

		/// <summary>
		/// Continue with specific TaskScheduler
		/// </summary>
		/// <param name="task"></param>
		/// <param name="taskScheduler"></param>
		/// <param name="taskContinuationOptions"></param>
		/// <returns></returns>
		public static TaskTaskSchedulerAwaiter<TOutput> ConfigureAwait<TOutput>(this Task<TOutput> task, TaskScheduler taskScheduler, TaskContinuationOptions taskContinuationOptions)
		{
			return new TaskTaskSchedulerAwaiter<TOutput>(task, taskScheduler, taskContinuationOptions);
		}

		/// <summary>
		/// Continue with specific TaskScheduler
		/// </summary>
		/// <param name="task"></param>
		/// <param name="taskScheduler"></param>
		/// <returns></returns>
		public static TaskTaskSchedulerAwaiter<TOutput> ConfigureAwait<TOutput>(this Task<TOutput> task, TaskScheduler taskScheduler)
		{
			return new TaskTaskSchedulerAwaiter<TOutput>(task, taskScheduler, TaskContinuationOptions.None);
		}



		/// <summary>
		/// Continue with specific TaskScheduler
		/// </summary>
		/// <param name="task"></param>
		/// <param name="taskFactory"></param>
		/// <param name="taskContinuationOptions"></param>
		/// <returns></returns>
		public static TaskTaskSchedulerAwaiter ConfigureAwait(this Task task, TaskFactory taskFactory, TaskContinuationOptions taskContinuationOptions)
		{
			return new TaskTaskSchedulerAwaiter(task, taskFactory.Scheduler ?? TaskScheduler.Default, taskContinuationOptions);
		}

		/// <summary>
		/// Continue with specific TaskScheduler
		/// </summary>
		/// <param name="task"></param>
		/// <param name="taskFactory"></param>
		/// <returns></returns>
		public static TaskTaskSchedulerAwaiter ConfigureAwait(this Task task, TaskFactory taskFactory)
		{
			return new TaskTaskSchedulerAwaiter(task, taskFactory.Scheduler ?? TaskScheduler.Default, TaskContinuationOptions.None);
		}

		/// <summary>
		/// Continue with specific TaskScheduler
		/// </summary>
		/// <param name="task"></param>
		/// <param name="taskScheduler"></param>
		/// <param name="taskContinuationOptions"></param>
		/// <returns></returns>
		public static TaskTaskSchedulerAwaiter ConfigureAwait(this Task task, TaskScheduler taskScheduler, TaskContinuationOptions taskContinuationOptions)
		{
			return new TaskTaskSchedulerAwaiter(task, taskScheduler, taskContinuationOptions);
		}


		/// <summary>
		/// Continue with specific TaskScheduler
		/// </summary>
		/// <param name="task"></param>
		/// <param name="taskScheduler"></param>
		/// <returns></returns>
		public static TaskTaskSchedulerAwaiter ConfigureAwait(this Task task, TaskScheduler taskScheduler)
		{
			return new TaskTaskSchedulerAwaiter(task, taskScheduler, TaskContinuationOptions.None);
		}


		#endregion TaskTaskSchedulerAwaiter

		#region TaskScheduler

		/// <summary>
		/// Switch to Background task, if needed
		/// await Task.Factory;
		/// </summary>
		/// <param name="taskScheduler"></param>
		/// <returns></returns>
		public static TaskSchedulerAwaiter GetAwaiter(this TaskScheduler taskScheduler)
		{
			return new TaskSchedulerAwaiter(taskScheduler, TaskCreationOptions.None);
		}


		/// <summary>
		/// Switch to Background task
		/// await Task.Factory.ConfigureAwait(TaskCreationOptions.LongRunning);
		/// </summary>
		/// <param name="taskScheduler"></param>
		/// <param name="taskCreationOptions"></param>
		/// <returns></returns>
		public static TaskSchedulerAwaiter ConfigureAwait(this TaskScheduler taskScheduler, TaskCreationOptions taskCreationOptions)
		{
			return new TaskSchedulerAwaiter(taskScheduler, taskCreationOptions);
		}

		#endregion TaskScheduler

	}
}
