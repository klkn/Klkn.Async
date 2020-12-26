using System;
using System.Threading;
using System.Threading.Tasks;
using Klkn.Async;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Internal;

namespace Tests.Async
{
	[TestClass]
	public class SynchronizationContextAsyncExtensionTests
	{
		[TestMethod]
		public async Task SynchronizationContextAwaiterExceptionTest()
		{
			using (var ctx = new ThreadSynchronizationContext())
			{
				var currentThreadId = Thread.CurrentThread.ManagedThreadId;
				Console.WriteLine($"Step 1 - {Thread.CurrentThread.ManagedThreadId}");
				int secondId = 0;
				Exception ex = null;
				try
				{
					await ctx;
					secondId = Thread.CurrentThread.ManagedThreadId;
					Console.WriteLine($"Step 2 - {Thread.CurrentThread.ManagedThreadId}");
					ex = new Exception("ex");
					throw ex;
					Console.WriteLine($"Step 2 - {Thread.CurrentThread.ManagedThreadId}");
				}
				catch (Exception e)
				{
					Console.WriteLine($"Step 3 - {Thread.CurrentThread.ManagedThreadId}");
					Assert.AreNotEqual(currentThreadId, Thread.CurrentThread.ManagedThreadId);
					Assert.AreEqual(ex, e);
				}
				finally
				{
					await Await.ThreadPool.ForceSwitch();
				}
			}
		}

		[TestMethod]
		public async Task SynchronizationContextAwaiterTest()
		{
			using (var ctx = new ThreadSynchronizationContext())
			{
				try
				{
					var currentThreadId = Thread.CurrentThread.ManagedThreadId;
					Console.WriteLine($"Step 1 - {Thread.CurrentThread.ManagedThreadId}");

					await ctx;
					var second = Thread.CurrentThread.ManagedThreadId;
					Console.WriteLine($"Step 2 - {Thread.CurrentThread.ManagedThreadId}");
					Assert.AreEqual(ctx.Thread.ManagedThreadId, Thread.CurrentThread.ManagedThreadId);
					await Await.ThreadPool.ForceSwitch();
					Assert.AreNotEqual(ctx.Thread.ManagedThreadId, Thread.CurrentThread.ManagedThreadId);
				}
				finally
				{
					await Await.ThreadPool.ForceSwitch();
				}
			}
		}


	}
}
