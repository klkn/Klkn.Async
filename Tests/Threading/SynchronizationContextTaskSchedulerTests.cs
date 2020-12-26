using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Internal;

namespace Tests.Threading
{
	[TestClass]
	public class SynchronizationContextTaskSchedulerTests
	{
		[TestMethod]
		public async Task SmokeTest()
		{
			using (var ctx = new ThreadSynchronizationContext())
			{
				var currentThreadId = Thread.CurrentThread.ManagedThreadId;
				Console.WriteLine($"Step 1 - {Thread.CurrentThread.ManagedThreadId}");
				var scheduler = new Klkn.Threading.SynchronizationContextTaskScheduler(ctx);
				await scheduler.Factory.StartNew(() =>
				{
					Assert.AreEqual(ctx.Thread.ManagedThreadId, Thread.CurrentThread.ManagedThreadId);
				});
			}
		}
	}
}
