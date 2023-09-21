using QuanLib.Core.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public abstract class RunnableBase : IRunnable
    {
        protected RunnableBase()
        {
            IsRuning = false;
            _stopSemaphore = new(0);
            _stopTask = GetStopTask();

            Started += OnStarted;
            Stopped += OnStopped;
            ThrowException += OnThrowException;
        }

        protected readonly object _lock = new();

        protected readonly SemaphoreSlim _stopSemaphore;

        protected Task _stopTask;

        public virtual Thread? Thread { get; protected set; }

        public virtual bool IsRuning { get; protected set; }

        public event EventHandler<IRunnable, EventArgs> Started;

        public event EventHandler<IRunnable, EventArgs> Stopped;

        public event EventHandler<IRunnable, ExceptionEventArgs> ThrowException;

        protected virtual void OnStarted(IRunnable sender, EventArgs e) { }

        protected virtual void OnStopped(IRunnable sender, EventArgs e) { }

        protected virtual void OnThrowException(IRunnable sender, ExceptionEventArgs e) { }

        protected abstract void Run();

        public virtual bool Start()
        {
            lock (_lock)
            {
                if (IsRuning)
                    return false;

                IsRuning = true;
                Thread = new(ThreadStart);
                Thread.Start();
                return true;
            }
        }

        public virtual void Stop()
        {
            lock (_lock)
            {
                if (IsRuning)
                {
                    IsRuning = false;
                    _stopSemaphore.Release();
                    _stopTask = GetStopTask();
                    if (Thread is null)
                        return;

                    int i = 0;
                    while (Thread.IsAlive)
                    {
                        try
                        {
                            Thread.Join(1000);
                            i++;
                            if (i >= 5)
                            {
                                Thread.Abort();
                                break;
                            }
                        }
                        catch
                        {

                        }
                    }

                    Thread = null;
                }
            }
        }

        public virtual void WaitForStop()
        {
            _stopTask.Wait();
        }

        public virtual async Task WaitForStopAsync()
        {
            await _stopTask;
        }

        protected virtual async Task GetStopTask()
        {
            await _stopSemaphore.WaitAsync();
        }

        protected void ThreadStart()
        {
            try
            {
                Started.Invoke(this, EventArgs.Empty);
                Run();
            }
            catch (Exception ex)
            {
                ThrowException.Invoke(this, new(ex));
            }
            finally
            {
                lock (_lock)
                {
                    if (IsRuning)
                    {
                        IsRuning = false;
                        _stopSemaphore.Release();
                        _stopTask = GetStopTask();
                        Thread = null;
                    }
                }
                Stopped.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
