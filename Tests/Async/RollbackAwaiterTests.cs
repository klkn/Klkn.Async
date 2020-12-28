using System.Threading;
using System.Threading.Tasks;
using Klkn.Async;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Internal;

namespace Tests.Async
{
    [TestClass]
    public class RollbackAwaiterTests
    {
        [TestMethod]
        public async Task RollbackTest()
        {
            var threadContext1 = new ThreadSynchronizationContext();
            var threadContext2 = new ThreadSynchronizationContext();
            await threadContext1;
            Assert.AreEqual(threadContext1.Thread.ManagedThreadId, Thread.CurrentThread.ManagedThreadId);
            await using (await threadContext2)
            {
                Assert.AreEqual(threadContext2.Thread.ManagedThreadId, Thread.CurrentThread.ManagedThreadId);
                await using (await Await.ThreadPool)
                {
                    Assert.AreNotEqual(threadContext1.Thread.ManagedThreadId, Thread.CurrentThread.ManagedThreadId);
                    Assert.AreNotEqual(threadContext2.Thread.ManagedThreadId, Thread.CurrentThread.ManagedThreadId);
                    Assert.IsTrue(Thread.CurrentThread.IsThreadPoolThread);
                }
                Assert.AreEqual(threadContext2.Thread.ManagedThreadId, Thread.CurrentThread.ManagedThreadId);
            }
            Assert.AreEqual(threadContext1.Thread.ManagedThreadId, Thread.CurrentThread.ManagedThreadId);
            await Await.ThreadPool;
        }
    }
}
