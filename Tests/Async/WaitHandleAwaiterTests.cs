using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Klkn.Async;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Async
{
	[TestClass]
	public class WaitHandleAwaiterTests
	{
		[TestMethod]
		public async Task SmokeTest()
		{
			var waitHandle = new ManualResetEvent(false);
			var stopWatch = Stopwatch.StartNew();
			Task.Factory.StartNew(() =>
			{
				Thread.Sleep(100);
				waitHandle.Set();
			});
			await waitHandle;
			Assert.IsTrue(waitHandle.WaitOne(0));
			stopWatch.Stop();
			Assert.IsTrue(stopWatch.ElapsedMilliseconds >= 100);
		}
	}
}
