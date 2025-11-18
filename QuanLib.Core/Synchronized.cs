using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public class Synchronized
    {
        public Synchronized()
        {
            _semaphore = new(1);
        }

        private readonly SemaphoreSlim _semaphore;

        public void Invoke(Action action)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));

            _semaphore.Wait();
            try
            {
                action.Invoke();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task InvokeAsync(Func<Task> func)
        {
            ArgumentNullException.ThrowIfNull(func, nameof(func));

            await _semaphore.WaitAsync();
            try
            {
                await func.Invoke();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async ValueTask InvokeAsync(Func<ValueTask> func)
        {
            ArgumentNullException.ThrowIfNull(func, nameof(func));

            await _semaphore.WaitAsync();
            try
            {
                await func.Invoke();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public T Invoke<T>(Func<T> func)
        {
            ArgumentNullException.ThrowIfNull(func, nameof(func));

            _semaphore.Wait();
            try
            {
                return func.Invoke();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<T> InvokeAsync<T>(Func<Task<T>> func)
        {
            ArgumentNullException.ThrowIfNull(func, nameof(func));

            await _semaphore.WaitAsync();
            try
            {
                return await func.Invoke();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async ValueTask<T> InvokeAsync<T>(Func<ValueTask<T>> func)
        {
            ArgumentNullException.ThrowIfNull(func, nameof(func));

            await _semaphore.WaitAsync();
            try
            {
                return await func.Invoke();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
