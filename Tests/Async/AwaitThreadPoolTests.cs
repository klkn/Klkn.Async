using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Klkn.Async;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Async
{
	[TestClass]
	public class AwaitThreadPoolTests
	{

		[TestMethod]
		public async Task ThreadPoolAwaiterExceptionTest()
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
				await Await.ThreadPool;
				secondId = Thread.CurrentThread.ManagedThreadId;
				Console.WriteLine($"Step 2 - {Thread.CurrentThread.ManagedThreadId}");
				ex = new Exception("ex");
				throw ex;
				Assert.AreNotEqual(currentThreadId, Thread.CurrentThread.ManagedThreadId);
				Assert.AreEqual(ruCulture, Thread.CurrentThread.CurrentCulture);
				Assert.AreEqual(ruCulture, Thread.CurrentThread.CurrentUICulture);

				await Await.ThreadPool.ForceSwitch();
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
		public async Task ThreadPoolAwaiterTest()
		{
			var currentThreadId = Thread.CurrentThread.ManagedThreadId;
			Console.WriteLine($"Step 1 - {Thread.CurrentThread.ManagedThreadId}");
			var enCulture = CultureInfo.GetCultureInfo("en-US");
			var ruCulture = CultureInfo.GetCultureInfo("ru-RU");
			CultureInfo.DefaultThreadCurrentCulture = enCulture;
			CultureInfo.DefaultThreadCurrentUICulture = enCulture;
			Thread.CurrentThread.CurrentCulture = ruCulture;
			Thread.CurrentThread.CurrentUICulture = ruCulture;
			await Await.ThreadPool;
			var secondId = Thread.CurrentThread.ManagedThreadId;
			Console.WriteLine($"Step 2 - {Thread.CurrentThread.ManagedThreadId}");
			Assert.AreNotEqual(currentThreadId, Thread.CurrentThread.ManagedThreadId);
			Assert.AreEqual(ruCulture, Thread.CurrentThread.CurrentCulture);
			Assert.AreEqual(ruCulture, Thread.CurrentThread.CurrentUICulture);

			await Await.ThreadPool.ForceSwitch();
			Assert.AreNotEqual(currentThreadId, Thread.CurrentThread.ManagedThreadId);
		}

	}
}
