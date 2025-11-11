using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public class TaskSemaphore : IWaitible
    {
        public TaskSemaphore()
        {
            _taskCompletionSource = new();
            _taskWaitHandle = new(_taskCompletionSource.Task);
        }

        private readonly TaskCompletionSource _taskCompletionSource;

        private readonly TaskWaitHandle _taskWaitHandle;

        public bool IsCompleted => _taskCompletionSource.Task.IsCompleted;

        public IWaitible GetWaiter()
        {
            return _taskWaitHandle;
        }

        public bool Release()
        {
            return _taskCompletionSource.TrySetResult();
        }

        public void Wait()
        {
            _taskWaitHandle.Wait();
        }

        public void Wait(CancellationToken cancellationToken)
        {
            _taskWaitHandle.Wait(cancellationToken);
        }

        public bool Wait(int millisecondsTimeout)
        {
            return _taskWaitHandle.Wait(millisecondsTimeout);
        }

        public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
        {
            return _taskWaitHandle.Wait(millisecondsTimeout, cancellationToken);
        }

        public bool Wait(TimeSpan timeout)
        {
            return _taskWaitHandle.Wait(timeout);
        }

        public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
        {
            return _taskWaitHandle.Wait(timeout, cancellationToken);
        }

        public Task WaitAsync()
        {
            return _taskWaitHandle.WaitAsync();
        }

        public Task WaitAsync(CancellationToken cancellationToken)
        {
            return _taskWaitHandle.WaitAsync(cancellationToken);
        }

        public Task<bool> WaitAsync(int millisecondsTimeout)
        {
            return _taskWaitHandle.WaitAsync(millisecondsTimeout);
        }

        public Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken)
        {
            return _taskWaitHandle.WaitAsync(millisecondsTimeout, cancellationToken);
        }

        public Task<bool> WaitAsync(TimeSpan timeout)
        {
            return _taskWaitHandle.WaitAsync(timeout);
        }

        public Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            return _taskWaitHandle.WaitAsync(timeout, cancellationToken);
        }
    }
}
