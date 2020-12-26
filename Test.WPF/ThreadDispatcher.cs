using System;
using System.Threading;
using System.Windows.Threading;

namespace Test.WPF
{
	public class ThreadDispatcher : IDisposable
	{
		public Dispatcher Dispatcher { get; private set; }

		public int ThreadId => _thread.ManagedThreadId;

		private Thread _thread;

		public ThreadDispatcher()
		{
			_thread = new Thread(ThreadWork);
			_thread.Start();
			while (Dispatcher == null)
			{
				Thread.Sleep(10);
			}
		}

		private void ThreadWork()
		{
			Dispatcher = Dispatcher.CurrentDispatcher;
			Dispatcher.Run();
		}



		#region IDisposable

		/// <inheritdoc />
		public void Dispose()
		{
			Dispatcher.InvokeShutdown();
		}

		#endregion
	}
}
