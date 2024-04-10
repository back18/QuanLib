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
        public WaitTask(BusyLoop owner, Func<bool> expression)
        {
            ArgumentNullException.ThrowIfNull(expression, nameof(expression));
            ArgumentNullException.ThrowIfNull(owner, nameof(owner));

            _owner = owner;
            _expression = expression;
            _waitSemaphore = new(0);
            _waitTask = WaitSemaphoreAsync();
        }

        private readonly BusyLoop _owner;

        private readonly Func<bool> _expression;

        private readonly SemaphoreSlim _waitSemaphore;

        private readonly Task _waitTask;

        internal bool CheckExpression()
        {
            try
            {
                if (!_waitTask.IsCompleted && _expression.Invoke())
                {
                    _waitSemaphore.Release();
                    return true;
                }
                else if (!_owner.IsRunning)
                {
                    _waitSemaphore.Release();
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                _waitSemaphore.Release();
                return false;
            }
        }

        public async Task WaitForSuccessAsync()
        {
            await _waitTask;
        }

        private async Task WaitSemaphoreAsync()
        {
            await _waitSemaphore.WaitAsync();
        }
    }
}
