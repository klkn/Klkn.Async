using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Klkn.Async;
using Klkn.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Async
{
	[TestClass]
	public class ReleaseWaitHandleExtensionTests
	{
		[TestMethod]
		public async Task MutexAwaitTest()
		{
			var mutex = new Mutex();
			using (await mutex)
			{
				Assert.ThrowsException<TimeoutException>(() =>
				{
					Task.Factory.StartNew(() =>
					{
						using (mutex.LockAsync(TimeSpan.FromMilliseconds(10)).Result)
						{
						}
					}).GetResultUnwindException();
				});
			}

			Task.Factory.StartNew(() =>
			{
				using (mutex.LockAsync(TimeSpan.FromMilliseconds(10)).Result)
				{
				}
			}).GetResultUnwindException();

			using (await mutex)
			{
			}
		}

		[TestMethod]
		public async Task SemaphoreAwaitTest()
		{
			var semaphore = new Semaphore(1, 1);
			using (await semaphore)
			{
				Assert.ThrowsException<TimeoutException>(() =>
				{
					Task.Factory.StartNew(() =>
					{
						using (semaphore.LockAsync(TimeSpan.FromMilliseconds(10)).Result)
						{
						}
					}).GetResultUnwindException();
				});
			}

			Task.Factory.StartNew(() =>
			{
				using (semaphore.LockAsync(TimeSpan.FromMilliseconds(10)).Result)
				{
				}
			}).GetResultUnwindException();

			using (await semaphore)
			{
			}
		}

	}
}
