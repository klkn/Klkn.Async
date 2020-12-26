using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Klkn.Async.Internal
{
	/// <summary>
	/// Awaiter for Releasable WaitHandle
	/// </summary>
	public class ReleaseWaitHandleAwaiter : INotifyCompletion
	{
		private readonly Action<WaitHandle> _release;
		private static readonly TimeSpan MaxTimeSpan = TimeSpan.FromMilliseconds(int.MaxValue);
		internal Task<IDisposable> Task => _taskCompletionSource.Task;

		private readonly WaitHandle _waitHandle;
		private readonly CancellationToken _token;
		private readonly SynchronizationContext _synchronizationContext;
		private readonly TimeSpan _timeout;
		private readonly TaskCompletionSource<IDisposable> _taskCompletionSource;
		private readonly CancellationTokenRegistration _tokenRegistration;
		private readonly RegisteredWaitHandle _registeredHandle;

		internal ReleaseWaitHandleAwaiter(WaitHandle waitHandle, TimeSpan timeout, CancellationToken token, Action<WaitHandle> release)
		{
			_release = release;
			_waitHandle = waitHandle;
			_timeout = timeout;
			_token = token;
			_taskCompletionSource = new TaskCompletionSource<IDisposable>();
			_synchronizationContext = SynchronizationContext.Current;
			if (timeout == TimeSpan.Zero)
			{
				if (_waitHandle.WaitOne(0))
				{
					_taskCompletionSource.TrySetResult(new ReleaseDispose(_waitHandle, _release));
				}
				else
				{
					_taskCompletionSource.TrySetException(new TimeoutException());
				}
				return;
			}
			if (_waitHandle.WaitOne(0))
			{
				_taskCompletionSource.TrySetResult(new ReleaseDispose(_waitHandle, _release));
				return;
			}
			if (token.IsCancellationRequested)
			{
				_taskCompletionSource.SetCanceled();
				return;
			}

			var tm = timeout > MaxTimeSpan ? MaxTimeSpan : timeout;
			_tokenRegistration = token.Register(TokenCallback, true);
			_registeredHandle = ThreadPool.RegisterWaitForSingleObject(
				_waitHandle,
				ThreadPoolCallBack,
				null,
				tm,
				true);
		}

		/// <summary>
		/// Check context
		/// </summary>
		public bool IsCompleted => _taskCompletionSource.Task.IsCompleted;


		/// <inheritdoc />
		void INotifyCompletion.OnCompleted(Action continuation)
		{
			if (_taskCompletionSource.Task.IsCompleted)
			{
				continuation();
				return;
			}

			if (_synchronizationContext == null || _synchronizationContext.GetType() == typeof(SynchronizationContext))
			{
				_taskCompletionSource.Task.ContinueWith((t, state) => { ((Action)state)(); }, continuation,
					TaskScheduler.Default);
			}
			else
			{
				_taskCompletionSource.Task.ContinueWith((t, state) => { ((Action)state)(); }, continuation,
					new Threading.SynchronizationContextTaskScheduler(_synchronizationContext));
			}
		}

		/// <summary>
		/// Get Result
		/// </summary>
		public IDisposable GetResult()
		{
			return _taskCompletionSource.Task.Result;
		}

		/// <summary>
		/// Get awaiter 
		/// </summary>
		/// <returns></returns>
		public ReleaseWaitHandleAwaiter GetAwaiter()
		{
			return this;
		}

		private void ThreadPoolCallBack(object state, bool timeout)
		{
			_tokenRegistration.Dispose();
			_registeredHandle?.Unregister(_waitHandle);
			if(timeout)
			{
				_taskCompletionSource.TrySetException(new TimeoutException());
			}
			else
			{
				_taskCompletionSource.TrySetResult(new ReleaseDispose(_waitHandle, _release));
			}
		}

		private void TokenCallback(object state)
		{
			_tokenRegistration.Dispose();
			_registeredHandle?.Unregister(_waitHandle);
			_taskCompletionSource.TrySetCanceled();
		}


		private class ReleaseDispose : IDisposable
		{
			private readonly WaitHandle _waitHandle;
			private readonly Action<WaitHandle> _release;

			public ReleaseDispose(WaitHandle waitHandle, Action<WaitHandle> release)
			{
				_waitHandle = waitHandle;
				_release = release;
			}

			/// <inheritdoc />
			public void Dispose()
			{
				_release?.Invoke(_waitHandle);
			}
		}
	}
}
