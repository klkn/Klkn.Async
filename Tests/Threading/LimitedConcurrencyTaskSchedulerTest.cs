using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Klkn.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Threading
{
	[TestClass]
	public class LimitedConcurrencyTaskSchedulerTest
	{
		[TestMethod]
		public async Task SmokeExceptionTest()
		{
			var scheduler = new LimitedConcurrencyTaskScheduler();
			var counter = 0;
			var taskCount = 100;
			var values = Enumerable.Range(0, taskCount).ToArray();
			var tasks = values.Select(v => scheduler.Factory.StartNew(() =>
				{
					Interlocked.Add(ref counter, 1);
					if (v >= 50)
						throw new Exception("Ex");
					return v;
				}))
				.ToArray();
			Assert.AreEqual(taskCount, tasks.Length);
			try
			{
				var result = await Task.WhenAll(tasks);
			}
			catch (Exception e)
			{

			}
			Assert.AreEqual(50, tasks.Aggregate(0, (sum, t) => sum + (t.IsFaulted ? 1 : 0)));
			Assert.AreEqual(taskCount, counter);
		}

		[TestMethod]
		public async Task SmokeTest()
		{
			var scheduler = new LimitedConcurrencyTaskScheduler(10);
			var counter = 0;
			var taskCount = 100;
			var values = Enumerable.Range(0, taskCount).ToArray();
			var tasks = values.Select(v => scheduler.Factory.StartNew(() =>
				{
					Interlocked.Add(ref counter, 1);
					return v;
				}))
				.ToArray();
			Assert.AreEqual(taskCount, tasks.Length);
			var result = await Task.WhenAll(tasks);
			Assert.AreEqual(taskCount, counter);
			CollectionAssert.AreEqual(values, result);
		}

	}
}
