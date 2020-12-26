using System.Threading.Tasks;
using System.Windows.Threading;
using Klkn.Async.WPF.Internal;

namespace Klkn.Async.WPF
{
	/// <summary>
	/// WPF async helper
	/// </summary>
	public static class AsyncWPFExtension
	{
		
		#region DispatcherAwaiter

		/// <summary>
		/// Awaiter for switching to Dispatcher
		/// </summary>
		/// <param name="dispatcher"></param>
		/// <returns></returns>
		public static DispatcherAwaiter GetAwaiter(this Dispatcher dispatcher)
		{
			return new DispatcherAwaiter(dispatcher, DispatcherPriority.Normal, false);
		}

		/// <summary>
		/// Awaiter for switching to Dispatcher
		/// </summary>
		/// <param name="dispatcher"></param>
		/// <param name="priority"></param>
		/// <returns></returns>
		public static DispatcherAwaiter ConfigureAwait(this Dispatcher dispatcher, DispatcherPriority priority)
		{
			return new DispatcherAwaiter(dispatcher, priority, true);
		}

		/// <summary>
		/// Awaiter for switching to Dispatcher
		/// </summary>
		/// <param name="dispatcher"></param>
		/// <param name="forceSwitch"></param>
		/// <returns></returns>
		public static DispatcherAwaiter ConfigureAwait(this Dispatcher dispatcher, bool forceSwitch)
		{
			return new DispatcherAwaiter(dispatcher, DispatcherPriority.Normal, forceSwitch);
		}


		/// <summary>
		/// Awaiter for switching to Dispatcher
		/// </summary>
		/// <param name="dispatcherObject"></param>
		/// <returns></returns>
		public static DispatcherAwaiter GetAwaiter(this DispatcherObject dispatcherObject)
		{
			return new DispatcherAwaiter(dispatcherObject.Dispatcher, DispatcherPriority.Normal, false);
		}

		/// <summary>
		/// Awaiter for switching to Dispatcher
		/// </summary>
		/// <param name="dispatcherObject"></param>
		/// <param name="priority"></param>
		/// <returns></returns>
		public static DispatcherAwaiter ConfigureAwait(this DispatcherObject dispatcherObject, DispatcherPriority priority)
		{
			return new DispatcherAwaiter(dispatcherObject.Dispatcher, priority, true);
		}

		/// <summary>
		/// Awaiter for switching to Dispatcher
		/// </summary>
		/// <param name="dispatcherObject"></param>
		/// <param name="forceSwitch"></param>
		/// <returns></returns>
		public static DispatcherAwaiter ConfigureAwait(this DispatcherObject dispatcherObject, bool forceSwitch)
		{
			return new DispatcherAwaiter(dispatcherObject.Dispatcher, DispatcherPriority.Normal, forceSwitch);
		}

		#endregion DispatcherAwaiter

		#region TaskDispatcherAwaiter

		/// <summary>
		/// Continue task with dispatcher
		/// </summary>
		/// <param name="task"></param>
		/// <param name="dispatcher"></param>
		/// <param name="priority"></param>
		/// <returns></returns>
		public static TaskDispatcherAwaiter<TOutput> ConfigureAwait<TOutput>(this Task<TOutput> task, Dispatcher dispatcher, DispatcherPriority priority)
		{
			return new TaskDispatcherAwaiter<TOutput>(task, dispatcher, priority, true);
		}

		/// <summary>
		/// Continue task with dispatcher
		/// </summary>
		/// <param name="task"></param>
		/// <param name="dispatcher"></param>
		/// <returns></returns>
		public static TaskDispatcherAwaiter<TOutput> ConfigureAwait<TOutput>(this Task<TOutput> task, Dispatcher dispatcher)
		{
			return new TaskDispatcherAwaiter<TOutput>(task, dispatcher, DispatcherPriority.Normal, false);
		}


		/// <summary>
		/// Continue task with dispatcher
		/// </summary>
		/// <param name="task"></param>
		/// <param name="dispatcher"></param>
		/// <param name="priority"></param>
		/// <returns></returns>
		public static TaskDispatcherAwaiter ConfigureAwait(this Task task, Dispatcher dispatcher, DispatcherPriority priority)
		{
			return new TaskDispatcherAwaiter(task, dispatcher, priority, true);
		}

		/// <summary>
		/// Continue task with dispatcher
		/// </summary>
		/// <param name="task"></param>
		/// <param name="dispatcher"></param>
		/// <returns></returns>
		public static TaskDispatcherAwaiter ConfigureAwait(this Task task, Dispatcher dispatcher)
		{
			return new TaskDispatcherAwaiter(task, dispatcher, DispatcherPriority.Normal, false);
		}

		#endregion TaskDispatcherAwaiter

		#region TaskDispatcherAwaiter DispatcherObject

		/// <summary>
		/// Continue task with dispatcher
		/// </summary>
		/// <param name="task"></param>
		/// <param name="dispatcherObject"></param>
		/// <param name="priority"></param>
		/// <returns></returns>
		public static TaskDispatcherAwaiter<TOutput> ConfigureAwait<TOutput>(this Task<TOutput> task, DispatcherObject dispatcherObject, DispatcherPriority priority)
		{
			return new TaskDispatcherAwaiter<TOutput>(task, dispatcherObject.Dispatcher, priority, true);
		}

		/// <summary>
		/// Continue task with dispatcher
		/// </summary>
		/// <param name="task"></param>
		/// <param name="dispatcherObject"></param>
		/// <returns></returns>
		public static TaskDispatcherAwaiter<TOutput> ConfigureAwait<TOutput>(this Task<TOutput> task, DispatcherObject dispatcherObject)
		{
			return new TaskDispatcherAwaiter<TOutput>(task, dispatcherObject.Dispatcher, DispatcherPriority.Normal, false);
		}


		/// <summary>
		/// Continue task with dispatcher
		/// </summary>
		/// <param name="task"></param>
		/// <param name="dispatcherObject"></param>
		/// <param name="priority"></param>
		/// <returns></returns>
		public static TaskDispatcherAwaiter ConfigureAwait(this Task task, DispatcherObject dispatcherObject, DispatcherPriority priority)
		{
			return new TaskDispatcherAwaiter(task, dispatcherObject.Dispatcher, priority, true);
		}

		/// <summary>
		/// Continue task with dispatcher
		/// </summary>
		/// <param name="task"></param>
		/// <param name="dispatcherObject"></param>
		/// <returns></returns>
		public static TaskDispatcherAwaiter ConfigureAwait(this Task task, DispatcherObject dispatcherObject)
		{
			return new TaskDispatcherAwaiter(task, dispatcherObject.Dispatcher, DispatcherPriority.Normal, false);
		}

		#endregion TaskDispatcherAwaiter DispatcherObject

	}
}
