using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public class TaskWaitHandle : IWaitible
    {
        public TaskWaitHandle(Task task)
        {
            ArgumentNullException.ThrowIfNull(task, nameof(task));

            _task = task;
        }

        private readonly Task _task;

        public void Wait()
        {
            _task.Wait();
        }

        public void Wait(CancellationToken cancellationToken)
        {
            _task.Wait(cancellationToken);
        }

        public bool Wait(int millisecondsTimeout)
        {
            return _task.Wait(millisecondsTimeout);
        }

        public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
        {
            return _task.Wait(millisecondsTimeout, cancellationToken);
        }

        public bool Wait(TimeSpan timeout)
        {
            return _task.Wait(timeout);
        }

        public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
        {
            return _task.Wait(timeout, cancellationToken);
        }

        public async Task WaitAsync()
        {
            await _task;
        }

        public async Task WaitAsync(CancellationToken cancellationToken)
        {
            await _task.WaitAsync(cancellationToken);
        }

        public async Task<bool> WaitAsync(int millisecondsTimeout)
        {
            try
            {
                await _task.WaitAsync(TimeSpan.FromMilliseconds(millisecondsTimeout));
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }

        public async Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken)
        {
            try
            {
                await _task.WaitAsync(TimeSpan.FromMilliseconds(millisecondsTimeout), cancellationToken);
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }

        public async Task<bool> WaitAsync(TimeSpan timeout)
        {
            try
            {
                await _task.WaitAsync(timeout);
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }

        public async Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            try
            {
                await _task.WaitAsync(timeout, cancellationToken);
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }
    }
}
