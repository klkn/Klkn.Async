using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Klkn.Threading.Internal
{
	internal class StaThreadWorker : SynchronizationContext
	{
		internal BlockingCollection<SyncItem> _queue = new BlockingCollection<SyncItem>();
		internal int IsInWork = 0;
		internal Thread Thread;

		public StaThreadWorker()
		{
			var thread = new Thread(ThreadTask);
			thread.SetApartmentState(ApartmentState.STA);
			thread.IsBackground = true;
			thread.Start();
			Thread = thread;
		}

		private void ThreadTask()
		{
			SynchronizationContext.SetSynchronizationContext(this);
			foreach (var syncItem in _queue.GetConsumingEnumerable())
			{
				Interlocked.Increment(ref IsInWork);
				ExecuteItem(syncItem);
				SyncItem item;
				while (_queue.TryTake(out item))
				{
					ExecuteItem(item);
				}
				Interlocked.Decrement(ref IsInWork);
			}
		}

		private void ExecuteItem(SyncItem syncItem)
		{
			try
			{
				syncItem.CallBack(syncItem.State);
				syncItem.TaskCompletionSource?.TrySetResult(0);
			}
			catch (OperationCanceledException)
			{
				syncItem.TaskCompletionSource?.SetCanceled();
			}
			catch (Exception ex)
			{
				syncItem.TaskCompletionSource?.TrySetException(ex);
			}
		}

		#region Overrides of SynchronizationContext

		/// <inheritdoc />
		public override void Send(SendOrPostCallback d, object state)
		{
			var item = new SyncItem()
			{
				CallBack = d,
				State = state,
				TaskCompletionSource = new TaskCompletionSource<int>()
			};
			_queue.Add(item);
			item.TaskCompletionSource.Task.Wait();
		}

		/// <inheritdoc />
		public override void Post(SendOrPostCallback d, object state)
		{
			_queue.Add(new SyncItem()
			{
				CallBack = d,
				State = state
			});
		}

		#endregion

		internal class SyncItem
		{
			public SendOrPostCallback CallBack;
			public object State;
			public TaskCompletionSource<int> TaskCompletionSource;
		}
	}
}
