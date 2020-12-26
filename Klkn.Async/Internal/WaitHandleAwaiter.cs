using System;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Klkn.Async.Internal
{
	/// <summary>
	/// Awaiter for WaitHandle
	/// </summary>
	public class WaitHandleAwaiter : INotifyCompletion
	{
		private static readonly TimeSpan MaxTimeSpan = TimeSpan.FromMilliseconds(int.MaxValue);
		internal Task<bool> Task => _taskCompletionSource.Task;

		private readonly WaitHandle _waitHandle;
		private readonly CancellationToken _token;
		private readonly SynchronizationContext _synchronizationContext;
		private readonly TimeSpan _timeout;
		private readonly TaskCompletionSource<bool> _taskCompletionSource;
		private readonly CancellationTokenRegistration _tokenRegistration;
		private readonly RegisteredWaitHandle _registeredHandle;

		internal WaitHandleAwaiter(WaitHandle waitHandle, TimeSpan timeout, CancellationToken token)
		{
			_waitHandle = waitHandle;
			_timeout = timeout;
			_token = token;
			_taskCompletionSource = new TaskCompletionSource<bool>();
			_synchronizationContext = SynchronizationContext.Current;
			if (timeout == TimeSpan.Zero)
			{
				_taskCompletionSource.TrySetResult(_waitHandle.WaitOne(0));
				return;
			}
			if (_waitHandle.WaitOne(0))
			{
				_taskCompletionSource.TrySetResult(true);
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
				_taskCompletionSource.Task.ContinueWith((t, state) => { ((Action) state)(); }, continuation,
					TaskScheduler.Default);
			}
			else
			{
				_taskCompletionSource.Task.ContinueWith((t, state) => { ((Action) state)(); }, continuation,
					new Threading.SynchronizationContextTaskScheduler(_synchronizationContext));
			}
		}

		/// <summary>
		/// Get Result
		/// </summary>
		public bool GetResult()
		{
			return _taskCompletionSource.Task.Result;
		}

		private void ThreadPoolCallBack(object state, bool timeout)
		{
			_tokenRegistration.Dispose();
			_registeredHandle?.Unregister(_waitHandle);
			_taskCompletionSource.TrySetResult(timeout);
		}

		private void TokenCallback(object state)
		{
			_tokenRegistration.Dispose();
			_registeredHandle?.Unregister(_waitHandle);
			_taskCompletionSource.TrySetCanceled();
		}

	}
}
