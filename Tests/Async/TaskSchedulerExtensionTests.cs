using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Klkn.Async;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Async
{
	[TestClass]
	public class TaskSchedulerExtensionTests
	{

		#region TaskTaskSchedulerAwaiter

		[TestMethod]
		public async Task TaskTaskSchedulerAwaiterExceptionTest()
		{
			var currentThreadId = Thread.CurrentThread.ManagedThreadId;
			Console.WriteLine($"Step 1 - {Thread.CurrentThread.ManagedThreadId}");
			int secondId = 0;
			Exception ex = null;
			try
			{
				await Task.Factory.StartNew(() =>
				{
					secondId = Thread.CurrentThread.ManagedThreadId;
					Console.WriteLine($"Step 2 - {Thread.CurrentThread.ManagedThreadId}");
					Assert.AreNotEqual(currentThreadId, Thread.CurrentThread.ManagedThreadId);
					ex = new Exception("ex");
					throw ex;
					Console.WriteLine($"Step 2 - {Thread.CurrentThread.ManagedThreadId}");
				}).ConfigureAwait(Task.Factory, TaskContinuationOptions.LongRunning);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Step 3 - {Thread.CurrentThread.ManagedThreadId}");
				Assert.AreNotEqual(currentThreadId, Thread.CurrentThread.ManagedThreadId);
				var ee = (e as AggregateException)?.InnerException ?? e;
				Assert.AreEqual(ex, ee);
			}
		}

		[TestMethod]
		public async Task TaskTaskSchedulerAwaiterTest()
		{
			var currentThreadId = Thread.CurrentThread.ManagedThreadId;
			Console.WriteLine($"Step 1 - {Thread.CurrentThread.ManagedThreadId}");
			int secondId = 0;
			var val = await Task.Factory.StartNew(() =>
			{
				secondId = Thread.CurrentThread.ManagedThreadId;
				Thread.Sleep(100);
				Console.WriteLine($"Step 2 - {Thread.CurrentThread.ManagedThreadId}");
				Assert.AreNotEqual(currentThreadId, Thread.CurrentThread.ManagedThreadId);
				return 10;
			}).ConfigureAwait(Task.Factory, TaskContinuationOptions.LongRunning);
			Assert.AreEqual(10, val);
			Console.WriteLine($"Step 3 - {Thread.CurrentThread.ManagedThreadId}");
			Assert.AreNotEqual(secondId, Thread.CurrentThread.ManagedThreadId);
		}

		#endregion TaskTaskSchedulerAwaiter

		#region TaskScheduler

		[TestMethod]
		public async Task TaskSchedulerAwaiterExceptionTest()
		{
			var currentThreadId = Thread.CurrentThread.ManagedThreadId;
			Console.WriteLine($"Step 1 - {Thread.CurrentThread.ManagedThreadId}");
			var enCulture = CultureInfo.GetCultureInfo("en-US");
			var ruCulture = CultureInfo.GetCultureInfo("ru-RU");
			CultureInfo.DefaultThreadCurrentCulture = enCulture;
			CultureInfo.DefaultThreadCurrentUICulture = enCulture;
			Thread.CurrentThread.CurrentCulture = ruCulture;
			Thread.CurrentThread.CurrentUICulture = ruCulture;

			int secondId = 0;
			Exception ex = null;
			try
			{
				await TaskScheduler.Default;
				secondId = Thread.CurrentThread.ManagedThreadId;
				Console.WriteLine($"Step 2 - {Thread.CurrentThread.ManagedThreadId}");
				ex = new Exception("ex");
				throw ex;
				Console.WriteLine($"Step 2 - {Thread.CurrentThread.ManagedThreadId}");
				Assert.AreNotEqual(currentThreadId, Thread.CurrentThread.ManagedThreadId);
				Assert.AreEqual(ruCulture, Thread.CurrentThread.CurrentCulture);
				Assert.AreEqual(ruCulture, Thread.CurrentThread.CurrentUICulture);

				await TaskScheduler.Default;
				Assert.AreNotEqual(currentThreadId, Thread.CurrentThread.ManagedThreadId);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Step 3 - {Thread.CurrentThread.ManagedThreadId}");
				Assert.AreNotEqual(currentThreadId, Thread.CurrentThread.ManagedThreadId);
				Assert.AreEqual(ruCulture, Thread.CurrentThread.CurrentCulture);
				Assert.AreEqual(ruCulture, Thread.CurrentThread.CurrentUICulture);
				Assert.AreEqual(ex, e);
			}
		}

		[TestMethod]
		public async Task TaskSchedulerAwaiterTest()
		{
			var currentThreadId = Thread.CurrentThread.ManagedThreadId;
			Console.WriteLine($"Step 1 - {Thread.CurrentThread.ManagedThreadId}");
			var enCulture = CultureInfo.GetCultureInfo("en-US");
			var ruCulture = CultureInfo.GetCultureInfo("ru-RU");
			CultureInfo.DefaultThreadCurrentCulture = enCulture;
			CultureInfo.DefaultThreadCurrentUICulture = enCulture;
			Thread.CurrentThread.CurrentCulture = ruCulture;
			Thread.CurrentThread.CurrentUICulture = ruCulture;

			await TaskScheduler.Default;

			var secondId = Thread.CurrentThread.ManagedThreadId;
			Console.WriteLine($"Step 2 - {Thread.CurrentThread.ManagedThreadId}");
			Assert.AreNotEqual(currentThreadId, Thread.CurrentThread.ManagedThreadId);
			Assert.AreEqual(ruCulture, Thread.CurrentThread.CurrentCulture);
			Assert.AreEqual(ruCulture, Thread.CurrentThread.CurrentUICulture);

			await TaskScheduler.Default;
			Assert.AreNotEqual(currentThreadId, Thread.CurrentThread.ManagedThreadId);
			//Assert.AreNotEqual(secondId, Thread.CurrentThread.ManagedThreadId);
		}

		#endregion TaskScheduler

	}
}
