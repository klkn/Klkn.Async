using System;
using System.Threading.Tasks;
using Klkn.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Threading
{
	[TestClass]
	public class TaskExtensionTests
	{


		[TestMethod]
		public void GetResultUnwindExceptionSmokeTest()
		{
			Task.Factory.StartNew(() => {
			}).GetResultUnwindException();

			Assert.AreEqual(10, Task.Factory.StartNew(() => 10).GetResultUnwindException());
		}

		[TestMethod]
		public void GetResultUnwindExceptionExceptionTest()
		{
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => 
			Task.Factory.StartNew(() => throw new ArgumentOutOfRangeException()).GetResultUnwindException());

			Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
			Task.Factory.StartNew<int>(() =>
			{
				throw new ArgumentOutOfRangeException();
				return 10;
			}).GetResultUnwindException());
		}
	}
}
