using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.BusyWaiting
{
    public class WaitTask
    {
        public WaitTask(Func<bool> expression)
        {
            ArgumentNullException.ThrowIfNull(expression, nameof(expression));

            _expression = expression;
            _waitSemaphore = new();
        }

        private readonly Func<bool> _expression;

        private readonly TaskSemaphore _waitSemaphore;

        public bool IsFailed { get; private set; }

        public bool IsCanceled { get; private set; }

        public Exception? Exception { get; private set; }

        internal bool CheckCondition()
        {
            try
            {
                if (!IsFailed && !IsCanceled && _expression.Invoke())
                {
                    _waitSemaphore.Release();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                IsFailed = true;
                Exception = ex;
                _waitSemaphore.Release();
                return false;
            }
        }

        public void Cancel()
        {
            IsCanceled = true;
            _waitSemaphore.Release();
        }

        public void WaitForComplete()
        {
            _waitSemaphore.Wait();
            ThrowIfException();
        }

        public async Task WaitForCompleteAsync()
        {
            await _waitSemaphore.WaitAsync().ConfigureAwait(false);
            ThrowIfException();
        }

        private void ThrowIfException()
        {
            if (IsFailed)
                throw new TaskFailedException(Exception);
        }
    }
}
