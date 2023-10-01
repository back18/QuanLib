using log4net.Core;
using log4net.Repository.Hierarchy;
using QuanLib.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public abstract class RunnableBase : IRunnable, ILogable
    {
        protected RunnableBase(Func<Type, LogImpl> logger)
        {
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            Type type = GetType();
            Logger = logger(type);
            IsRunning = false;
            ThreadName = type.FullName ?? type.Name;
            _stopSemaphore = new(0);
            _stopTask = GetStopTask();

            Started += OnStarted;
            Stopped += OnStopped;
            ThrowException += OnThrowException;
        }

        protected readonly object _lock = new();

        protected readonly SemaphoreSlim _stopSemaphore;

        protected Task _stopTask;

        public LogImpl Logger { get; }

        public virtual Thread? Thread { get; protected set; }

        public virtual string ThreadName { get; protected set; }

        public virtual bool IsRunning { get; protected set; }

        public event EventHandler<IRunnable, EventArgs> Started;

        public event EventHandler<IRunnable, EventArgs> Stopped;

        public event EventHandler<IRunnable, ExceptionEventArgs> ThrowException;

        protected virtual void OnStarted(IRunnable sender, EventArgs e)
        {
            Logger.Info($"线程({Thread?.Name ?? "null"})已启动");
        }

        protected virtual void OnStopped(IRunnable sender, EventArgs e)
        {
            Logger.Info($"线程({Thread?.Name ?? "null"})已停止");
        }

        protected virtual void OnThrowException(IRunnable sender, ExceptionEventArgs e)
        {
            Logger.Error($"线程({Thread?.Name ?? "null"})抛出了异常", e.Exception);
        }

        protected abstract void Run();

        public virtual bool Start(string threadName)
        {
            if (string.IsNullOrEmpty(threadName))
                throw new ArgumentException($"“{nameof(threadName)}”不能为 null 或空。", nameof(threadName));

            ThreadName = threadName;
            return Start();
        }

        public virtual bool Start()
        {
            lock (_lock)
            {
                if (IsRunning)
                    return false;

                IsRunning = true;
                Thread = new(ThreadStart);
                Thread.Name = ThreadName;
                Thread.Start();
                return true;
            }
        }

        public virtual void Stop()
        {
            lock (_lock)
            {
                if (IsRunning)
                {
                    IsRunning = false;
                    int i = 0;
                    try
                    {
                        while (Thread is not null)
                        {
                            Thread.Join(1000);
                            if (!Thread.IsAlive)
                                break;
                            i++;
                            Logger.Warn($"正在等待线程({Thread?.Name})停止，已等待{i}秒");
                            if (i >= 5)
                            {
                                Logger.Warn($"即将强行停止线程({Thread?.Name})");
                                _stopSemaphore.Release();
                                _stopTask = GetStopTask();
                                Thread.Abort();
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Thread is not null && Thread.IsAlive)
                            Logger.Error($"无法停止进程({Thread?.Name})", ex);
                    }
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
                IsRunning = false;
                _stopSemaphore.Release();
                _stopTask = GetStopTask();
                ThrowException.Invoke(this, new(ex));
            }
            finally
            {
                IsRunning = false;
                _stopSemaphore.Release();
                _stopTask = GetStopTask();
                Stopped.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
