using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.BusyWaiting
{
    public class LoopTask
    {
        public LoopTask(Action action)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));

            _action = action;
            _waitSemaphore = new();
            State = LoopTaskState.NotStarted;
        }

        private readonly TaskSemaphore _waitSemaphore;

        private readonly Action _action;

        public LoopTaskState State { get; private set; }

        public Exception? Exception { get; private set; }

        internal void Start()
        {
            if (State != LoopTaskState.NotStarted)
                return;

            try
            {
                State = LoopTaskState.Running;
                _action.Invoke();
                State = LoopTaskState.Completed;
            }
            catch (Exception ex)
            {
                Exception = ex;
                State = LoopTaskState.Failed;
            }
            finally
            {
                _waitSemaphore.Release();
            }
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
            if (State == LoopTaskState.Failed)
                throw new TaskFailedException(Exception);
        }
    }
}
