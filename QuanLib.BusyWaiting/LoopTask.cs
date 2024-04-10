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
            _semaphore = new(0);
            _task = WaitSemaphoreAsync();
            State = LoopTaskState.NotStarted;
        }

        private readonly SemaphoreSlim _semaphore;

        private readonly Task _task;

        private readonly Action _action;

        public LoopTaskState State { get; private set; }

        public Exception? Exception { get; private set; }

        internal void Start()
        {
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
                _semaphore.Release();
            }
        }

        public async Task WaitForCompleteAsync()
        {
            await _task;
        }

        private async Task WaitSemaphoreAsync()
        {
            await _semaphore.WaitAsync();
        }
    }
}
