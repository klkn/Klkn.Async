using System;
using System.Threading;
using System.Threading.Tasks;
using Klkn.Async.Internal;

namespace Klkn.Async
{
	/// <summary>
	/// Async Disposable
	/// </summary>
	public static class ReleaseWaitHandleExtension
	{

		#region Async mutex operation

		/// <summary>
		/// Awaiter for Mutex
		/// </summary>
		/// <param name="mutex"></param>
		/// <returns></returns>
		public static ReleaseWaitHandleAwaiter GetAwaiter(this Mutex mutex)
		{
			return new ReleaseWaitHandleAwaiter(mutex, TimeSpan.MaxValue, CancellationToken.None, ReleaseMutex);
		}

		/// <summary>
		/// Configured await for mutex
		/// </summary>
		/// <param name="mutex"></param>
		/// <param name="timeout"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static ReleaseWaitHandleAwaiter ConfigureAwait(this Mutex mutex, TimeSpan timeout, CancellationToken token)
		{
			return new ReleaseWaitHandleAwaiter(mutex, timeout, token, ReleaseMutex);
		}

		/// <summary>
		/// Configured await for mutex
		/// </summary>
		/// <param name="mutex"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static ReleaseWaitHandleAwaiter ConfigureAwait(this Mutex mutex, CancellationToken token)
		{
			return new ReleaseWaitHandleAwaiter(mutex, TimeSpan.MaxValue, token, ReleaseMutex);
		}

		/// <summary>
		/// Configured await for mutex
		/// </summary>
		/// <param name="mutex"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public static ReleaseWaitHandleAwaiter ConfigureAwait(this Mutex mutex, TimeSpan timeout)
		{
			return new ReleaseWaitHandleAwaiter(mutex, timeout, CancellationToken.None, ReleaseMutex);
		}

		/// <summary>
		/// Mutex Async Lock 
		/// </summary>
		/// <param name="mutex"></param>
		/// <param name="timeout"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static Task<IDisposable> LockAsync(this Mutex mutex, TimeSpan timeout, CancellationToken token)
		{
			return new ReleaseWaitHandleAwaiter(mutex, timeout, token, ReleaseMutex).Task;
		}

		/// <summary>
		/// Mutex Async Lock
		/// </summary>
		/// <param name="mutex"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static Task<IDisposable> LockAsync(this Mutex mutex, CancellationToken token)
		{
			return new ReleaseWaitHandleAwaiter(mutex, TimeSpan.MaxValue, token, ReleaseMutex).Task;
		}

		/// <summary>
		/// Mutex Async Lock
		/// </summary>
		/// <param name="mutex"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public static Task<IDisposable> LockAsync(this Mutex mutex, TimeSpan timeout)
		{
			return new ReleaseWaitHandleAwaiter(mutex, timeout, CancellationToken.None, ReleaseMutex).Task;
		}

		/// <summary>
		/// Mutex Async Lock
		/// </summary>
		/// <param name="mutex"></param>
		/// <returns></returns>
		public static Task<IDisposable> LockAsync(this Mutex mutex)
		{
			return new ReleaseWaitHandleAwaiter(mutex, TimeSpan.MaxValue, CancellationToken.None, ReleaseMutex).Task;
		}

		private static void ReleaseMutex(WaitHandle mutex)
		{
			((Mutex)mutex).ReleaseMutex();
		}

		#endregion Async mutex operation

		#region Async semaphore operation

		/// <summary>
		/// Default awaiter for Semaphore
		/// </summary>
		/// <param name="semaphore"></param>
		/// <returns></returns>
		public static ReleaseWaitHandleAwaiter GetAwaiter(this Semaphore semaphore)
		{
			return new ReleaseWaitHandleAwaiter(semaphore, TimeSpan.MaxValue, CancellationToken.None, ReleaseSemaphore);
		}

		/// <summary>
		/// Configured await for semaphore
		/// </summary>
		/// <param name="mutex"></param>
		/// <param name="timeout"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static ReleaseWaitHandleAwaiter ConfigureAwait(this Semaphore mutex, TimeSpan timeout, CancellationToken token)
		{
			return new ReleaseWaitHandleAwaiter(mutex, timeout, token, ReleaseSemaphore);
		}

		/// <summary>
		/// Configured await for semaphore
		/// </summary>
		/// <param name="mutex"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public static ReleaseWaitHandleAwaiter ConfigureAwait(this Semaphore mutex, TimeSpan timeout)
		{
			return new ReleaseWaitHandleAwaiter(mutex, timeout, CancellationToken.None, ReleaseSemaphore);
		}

		/// <summary>
		/// Configured await for semaphore
		/// </summary>
		/// <param name="mutex"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static ReleaseWaitHandleAwaiter ConfigureAwait(this Semaphore mutex, CancellationToken token)
		{
			return new ReleaseWaitHandleAwaiter(mutex, TimeSpan.MaxValue, token, ReleaseSemaphore);
		}

		/// <summary>
		/// Asynchronous Lock for Semaphore
		/// </summary>
		/// <param name="mutex"></param>
		/// <param name="timeout"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static Task<IDisposable> LockAsync(this Semaphore mutex, TimeSpan timeout, CancellationToken token)
		{
			return new ReleaseWaitHandleAwaiter(mutex, timeout, token, ReleaseSemaphore).Task;
		}

		/// <summary>
		/// Asynchronous Lock for Semaphore
		/// </summary>
		/// <param name="mutex"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public static Task<IDisposable> LockAsync(this Semaphore mutex, TimeSpan timeout)
		{
			return new ReleaseWaitHandleAwaiter(mutex, timeout, CancellationToken.None, ReleaseSemaphore).Task;
		}

		/// <summary>
		/// Asynchronous Lock for Semaphore
		/// </summary>
		/// <param name="mutex"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static Task<IDisposable> LockAsync(this Semaphore mutex, CancellationToken token)
		{
			return new ReleaseWaitHandleAwaiter(mutex, TimeSpan.MaxValue, token, ReleaseSemaphore).Task;
		}

		/// <summary>
		/// Asynchronous Lock for Semaphore
		/// </summary>
		/// <param name="mutex"></param>
		/// <returns></returns>
		public static Task<IDisposable> LockAsync(this Semaphore mutex)
		{
			return new ReleaseWaitHandleAwaiter(mutex, TimeSpan.MaxValue, CancellationToken.None, ReleaseSemaphore).Task;
		}

		private static void ReleaseSemaphore(WaitHandle mutex)
		{
			((Semaphore)mutex).Release();
		}

		#endregion Async semaphore operation

	}
}
