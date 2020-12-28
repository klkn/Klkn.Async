using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Tests.Internal
{
	public class ThreadSynchronizationContext : SynchronizationContext, IDisposable
	{
		private class QueueItem
		{
			public SendOrPostCallback Action;

			public object State;

			public Exception Exception;

			public ManualResetEvent Event;
		}

		public Thread Thread;
		private BlockingCollection<QueueItem> _queue = new BlockingCollection<QueueItem>();

		public ThreadSynchronizationContext()
		{
			Thread = new Thread(ThreadScope);
			Thread.Start();
		}

		private void ThreadScope()
		{
			SynchronizationContext.SetSynchronizationContext(this);
			foreach (var item in _queue.GetConsumingEnumerable())
			{
				try
				{
					item.Action(item.State);
				}
				catch (Exception e)
				{
					item.Exception = e;
				}

				item.Event?.Set();
			}
		}


		#region Overrides of SynchronizationContext

		/// <inheritdoc />
		public override void Post(SendOrPostCallback d, object state)
		{
			_queue.Add(new QueueItem()
			{
				Action = d,
				State = state
			});
		}

		/// <inheritdoc />
		public override void Send(SendOrPostCallback d, object state)
		{
			if (Thread.ManagedThreadId == Thread.ManagedThreadId)
			{
				d(state);
				return;
			}

			var item = new QueueItem()
			{
				Action = d,
				State = state,
				Event = new ManualResetEvent(false)
			};
			_queue.Add(item);
			item.Event.WaitOne();
			if (item.Exception != null)
				throw new AggregateException(item.Exception);
		}

		#endregion

		#region IDisposable

		/// <inheritdoc />
		public void Dispose()
		{
			_queue.CompleteAdding();
			Thread.Join();
		}

		#endregion
	}
}
