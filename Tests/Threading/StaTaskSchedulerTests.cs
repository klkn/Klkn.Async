using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Klkn.Async;
using Klkn.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Threading
{
	[TestClass]
	public class StaTaskSchedulerTests
	{
		[TestMethod]
		public async Task SmokeTest()
		{
			await StaTaskScheduler.Factory.StartNew(() =>
				{
					Assert.AreEqual(ApartmentState.STA, Thread.CurrentThread.GetApartmentState());
				});
		}

		[TestMethod]
		public async Task AsyncSmokeTest()
		{
			await StaTaskScheduler.Scheduler;

			var staThread = Thread.CurrentThread;

			Assert.AreEqual(ApartmentState.STA, Thread.CurrentThread.GetApartmentState());
			var taskThreadId = Thread.CurrentThread.ManagedThreadId;
			await Task.Delay(100);
			Assert.AreEqual(ApartmentState.STA, Thread.CurrentThread.GetApartmentState());
			Assert.AreEqual(taskThreadId, Thread.CurrentThread.ManagedThreadId);
			await Task.Factory.StartNew(() =>
			{
				Thread.Sleep(10);
				Assert.AreEqual(ApartmentState.MTA, Thread.CurrentThread.GetApartmentState());
				Assert.AreNotEqual(taskThreadId, Thread.CurrentThread.ManagedThreadId);
			}, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default)
				;

			Assert.AreEqual(ApartmentState.STA, Thread.CurrentThread.GetApartmentState());
			Assert.AreEqual(taskThreadId, Thread.CurrentThread.ManagedThreadId);

			await TaskScheduler.Default;
			Assert.AreEqual(ApartmentState.MTA, Thread.CurrentThread.GetApartmentState());
			Assert.AreNotEqual(taskThreadId, Thread.CurrentThread.ManagedThreadId);

			await Task.Delay(100);

		}
	}
}
