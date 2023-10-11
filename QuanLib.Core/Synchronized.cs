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
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            _semaphore.Wait();
            try
            {
                action.Invoke();
            }
            catch
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task InvokeAsync(Func<Task> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            await _semaphore.WaitAsync();
            try
            {
                await func.Invoke();
            }
            catch
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async ValueTask InvokeAsync(Func<ValueTask> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            await _semaphore.WaitAsync();
            try
            {
                await func.Invoke();
            }
            catch
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public T Invoke<T>(Func<T> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            _semaphore.Wait();
            try
            {
                return func.Invoke();
            }
            catch
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<T> InvokeAsync<T>(Func<Task<T>> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            await _semaphore.WaitAsync();
            try
            {
                return await func.Invoke();
            }
            catch
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async ValueTask<T> InvokeAsync<T>(Func<ValueTask<T>> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            await _semaphore.WaitAsync();
            try
            {
                return await func.Invoke();
            }
            catch
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
