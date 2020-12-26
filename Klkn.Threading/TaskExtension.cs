using System;
using System.Linq;
using System.Threading.Tasks;

namespace Klkn.Threading
{
	/// <summary>
	/// Helpers for Tasks
	/// </summary>
	public static class TaskExtension
	{
		/// <summary>
		/// Get Result, first exception on aggregate exception
		/// </summary>
		/// <param name="task"></param>
		public static void GetResultUnwindException(this Task task)
		{
			try
			{
				task.Wait();
			}
			catch (AggregateException exception)
			{
				var ex = exception;
				for (;;)
				{
					var e = ex.InnerException ?? ex.InnerExceptions.FirstOrDefault();
					if (e == null)
						throw ex;
					ex = e as AggregateException ?? throw e;
				}
			}
		}

		/// <summary>
		/// Get Result, first exception on aggregate exception
		/// </summary>
		/// <typeparam name="TOutput"></typeparam>
		/// <param name="task"></param>
		/// <returns></returns>
		public static TOutput GetResultUnwindException<TOutput>(this Task<TOutput> task)
		{
			try
			{
				return task.Result;
			}
			catch (AggregateException exception)
			{
				var ex = exception;
				for (; ; )
				{
					var e = ex.InnerException ?? ex.InnerExceptions.FirstOrDefault();
					if (e == null)
						throw ex;
					ex = e as AggregateException ?? throw e;
				}
			}
		}
	}
}
