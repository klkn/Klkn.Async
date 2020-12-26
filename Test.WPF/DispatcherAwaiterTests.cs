using System.Threading;
using System.Threading.Tasks;
using Klkn.Async;
using Klkn.Async.WPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.WPF
{
	[TestClass]
	public class DispatcherAwaiterTests
	{
		[TestMethod]
		public async Task DispatcherAwaiterSmokeTest()
		{
			var mainThreadId = Thread.CurrentThread.ManagedThreadId;
			using (var ctx = new ThreadDispatcher())
			{
				await ctx.Dispatcher;
				Assert.AreNotEqual(mainThreadId, Thread.CurrentThread.ManagedThreadId);
				Assert.AreEqual(ctx.ThreadId, Thread.CurrentThread.ManagedThreadId);
				await Await.ThreadPool;
			}
		}

		[TestMethod]
		public async Task TaskDispatcherAwaiterSmokeTest()
		{
			var mainThreadId = Thread.CurrentThread.ManagedThreadId;
			using (var ctx = new ThreadDispatcher())
			{
				var val = await Task.Factory.StartNew(() => 100500, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default)
					.ConfigureAwait(ctx.Dispatcher);
				Assert.AreEqual(100500, val);
				Assert.AreNotEqual(mainThreadId, Thread.CurrentThread.ManagedThreadId);
				Assert.AreEqual(ctx.ThreadId, Thread.CurrentThread.ManagedThreadId);
				await Await.ThreadPool;
			}
		}
	}
}
