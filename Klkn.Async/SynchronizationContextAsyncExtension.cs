using System.Threading;
using System.Threading.Tasks;
using Klkn.Async.Internal;

namespace Klkn.Async
{
	/// <summary>
	/// Extension for async switching SynchronizationContext
	/// </summary>
	public static class SynchronizationContextAsyncExtension
	{
		/// <summary>
		/// Continue with specific SynchronizationContext
		/// </summary>
		/// <param name="synchronizationContext"></param>
		/// <returns></returns>
		public static SynchronizationContextAwaiter GetAwaiter(this SynchronizationContext synchronizationContext)
		{
			return new SynchronizationContextAwaiter(synchronizationContext);
		}

		/// <summary>
		/// Continue with specific SynchronizationContext
		/// </summary>
		/// <param name="task"></param>
		/// <param name="synchronizationContext"></param>
		/// <returns></returns>
		/// 
		public static TaskSynchronizationContextAwaiter<TOutput> ConfigureAwait<TOutput>(this Task<TOutput> task, SynchronizationContext synchronizationContext)
		{
			return new TaskSynchronizationContextAwaiter<TOutput>(task, synchronizationContext);
		}

		/// <summary>
		/// Continue with specific SynchronizationContext
		/// </summary>
		/// <param name="task"></param>
		/// <param name="synchronizationContext"></param>
		/// <returns></returns>
		public static TaskSynchronizationContextAwaiter ConfigureAwait(this Task task, SynchronizationContext synchronizationContext)
		{
			return new TaskSynchronizationContextAwaiter(task, synchronizationContext);
		}
	}
}
