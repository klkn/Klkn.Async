using System;
using System.Threading;
using Klkn.Threading.Internal;

namespace Klkn.Threading
{
	/// <summary>
	/// Each thread has Synchronization Context because usually STA required async/await method should be continued in the same thread
	/// </summary>
	public static class StaThreadPool
	{
		private static readonly StaThreadWorker[] Threads;

		static StaThreadPool()
		{
			var threads = new StaThreadWorker[Math.Max(2, Environment.ProcessorCount)];
			for(var i = 0; i < threads.Length; i++)
			{
				threads[i] = new StaThreadWorker();
			}
			Threads = threads;
		}

		/// <summary>
		/// Queue WorkItem
		/// </summary>
		/// <param name="callBack"></param>
		/// <returns></returns>
		public static bool QueueUserWorkItem(WaitCallback callBack)
		{
			return QueueUserWorkItem(callBack, null);
		}

		/// <summary>
		/// Queue WorkItem
		/// </summary>
		/// <param name="callBack"></param>
		/// <param name="state"></param>
		/// <returns></returns>
		public static bool QueueUserWorkItem(WaitCallback callBack, object state)
		{
			if(callBack == null)
				throw new ArgumentNullException(nameof(callBack));
			var item = new StaThreadWorker.SyncItem()
			{
				CallBack = s => callBack(s),
				State = state
			};
			var minTasks = int.MaxValue;
			StaThreadWorker minWorker = null;
			foreach (var thread in Threads)
			{
				var tasks = thread._queue.Count;
				if (thread.IsInWork == 0 && tasks == 0)
				{
					thread._queue.Add(item);
					return true;
				}
				if (tasks >= minTasks)
					continue;
				minTasks = tasks;
				minWorker = thread;
			}
			minWorker._queue.Add(item);
			return true;
		}
	
	}
}
