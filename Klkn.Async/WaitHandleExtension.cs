using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Klkn.Async.Internal;

namespace Klkn.Async
{
	/// <summary>
	/// Extension for awaiting WaitHandle
	/// </summary>
	public static class WaitHandleExtension
	{

		#region Awaiter

		/// <summary>
		/// Awaiting WaitHandle
		/// await waitHandle
		/// </summary>
		/// <param name="waitHandle"></param>
		/// <returns></returns>
		public static WaitHandleAwaiter GetAwaiter(this WaitHandle waitHandle)
		{
			return waitHandle.ConfigureAwait(TimeSpan.MaxValue, CancellationToken.None);
		}

		/// <summary>
		/// Awaiting WaitHandle
		/// </summary>
		/// <param name="waitHandle"></param>
		/// <param name="timeout"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static WaitHandleAwaiter ConfigureAwait(this WaitHandle waitHandle, TimeSpan timeout, CancellationToken token)
		{
			if(waitHandle == null)
				throw new ArgumentNullException(nameof(waitHandle));
			return new WaitHandleAwaiter(waitHandle, timeout, token);
		}

		/// <summary>
		/// Awaiting WaitHandle
		/// </summary>
		/// <param name="waitHandle"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public static WaitHandleAwaiter ConfigureAwait(this WaitHandle waitHandle, TimeSpan timeout)
		{
			return waitHandle.ConfigureAwait(timeout, CancellationToken.None);
		}

		/// <summary>
		/// Awaiting WaitHandle
		/// </summary>
		/// <param name="waitHandle"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static WaitHandleAwaiter ConfigureAwait(this WaitHandle waitHandle, CancellationToken token)
		{
			return waitHandle.ConfigureAwait(TimeSpan.MaxValue, token);
		}

		/// <summary>
		/// Awaiting WaitHandle
		/// </summary>
		/// <param name="waitHandle"></param>
		/// <param name="millisecondsTimeout"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static WaitHandleAwaiter ConfigureAwait(this WaitHandle waitHandle, int millisecondsTimeout, CancellationToken token)
		{
			return waitHandle.ConfigureAwait(TimeSpan.FromMilliseconds(millisecondsTimeout), token);
		}

		/// <summary>
		/// Awaiting WaitHandle
		/// </summary>
		/// <param name="waitHandle"></param>
		/// <param name="millisecondsTimeout"></param>
		/// <returns></returns>
		public static WaitHandleAwaiter ConfigureAwait(this WaitHandle waitHandle, int millisecondsTimeout)
		{
			return waitHandle.ConfigureAwait(TimeSpan.FromMilliseconds(millisecondsTimeout));
		}

		#endregion Awaiter

		#region WaitAsync

		/// <summary>
		/// Asynchronous task for waiting WaitHandle
		/// </summary>
		/// <param name="waitHandle"></param>
		/// <param name="timeout"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static Task<bool> WaitAsync(this WaitHandle waitHandle, TimeSpan timeout, CancellationToken token)
		{
			if(waitHandle == null)
				throw new ArgumentNullException(nameof(waitHandle));
			if(timeout == TimeSpan.Zero)
				return Task.FromResult(waitHandle.WaitOne(0));
			if(waitHandle.WaitOne(0))
				return Task.FromResult(true);
			return new WaitHandleAwaiter(waitHandle, timeout, token).Task;
		}

		/// <summary>
		/// Asynchronous task for waiting WaitHandle
		/// </summary>
		/// <param name="waitHandle"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public static Task<bool> WaitAsync(this WaitHandle waitHandle, TimeSpan timeout)
		{
			return waitHandle.WaitAsync(timeout, CancellationToken.None);
		}

		/// <summary>
		/// Asynchronous task for waiting WaitHandle
		/// </summary>
		/// <param name="waitHandle"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static Task WaitAsync(this WaitHandle waitHandle, CancellationToken token)
		{
			return waitHandle.WaitAsync(TimeSpan.MaxValue, token);
		}

		/// <summary>
		/// Asynchronous task for waiting WaitHandle
		/// </summary>
		/// <param name="waitHandle"></param>
		/// <param name="millisecondsTimeout"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static Task<bool> WaitAsync(this WaitHandle waitHandle, int millisecondsTimeout, CancellationToken token)
		{
			return waitHandle.WaitAsync(TimeSpan.FromMilliseconds(millisecondsTimeout), token);
		}

		/// <summary>
		/// Asynchronous task for waiting WaitHandle
		/// </summary>
		/// <param name="waitHandle"></param>
		/// <param name="millisecondsTimeout"></param>
		/// <returns></returns>
		public static Task<bool> WaitAsync(this WaitHandle waitHandle, int millisecondsTimeout)
		{
			return waitHandle.WaitAsync(TimeSpan.FromMilliseconds(millisecondsTimeout), CancellationToken.None);
		}

		#endregion WaitAsync

	}
}
