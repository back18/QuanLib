using QuanLib.Core.Events;
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
        protected RunnableBase(ILoggerProvider? loggerProvider = null)
        {
            if (loggerProvider is not null)
            {
                Type type = GetType();
                Logger = loggerProvider.GetLogger(type);
            }

            IsRunning = false;
            StoppingRetryCount = 5;

            Started += OnStarted;
            Stopped += OnStopped;
            ThrowException += OnThrowException;

            Started += OnStartedLoggging;
            Stopped += OnStoppedLogging;
            ThrowException += OnThrowExceptionLogging;
        }

        private readonly Lock _lock = new();

        private TaskSemaphore? _stopSemaphore;

        private ThreadOptions? _defaultThreadOptions;

        protected virtual ILogger? Logger { get; private set; }

        protected IWaitable? StopWaiter => _stopSemaphore?.GetWaiter();

        public virtual Thread? Thread { get; protected set; }

        public virtual Exception? Exception { get; protected set; }

        public virtual bool IsRunning { get; protected set; }

        public virtual int StoppingRetryCount { get; protected set; }

        public event EventHandler<IRunnable, EventArgs> Started;

        public event EventHandler<IRunnable, EventArgs> Stopped;

        public event EventHandler<IRunnable, EventArgs<Exception>> ThrowException;

        protected virtual void OnStarted(IRunnable sender, EventArgs e) { }

        protected virtual void OnStopped(IRunnable sender, EventArgs e) { }

        protected virtual void OnThrowException(IRunnable sender, EventArgs<Exception> e) { }

        private void OnStartedLoggging(IRunnable sender, EventArgs e)
        {
            Logger?.Info($"线程({GetThreadName(Thread)})已启动");
        }

        private void OnStoppedLogging(IRunnable sender, EventArgs e)
        {
            Logger?.Info($"线程({GetThreadName(Thread)})已停止");
        }

        private void OnThrowExceptionLogging(IRunnable sender, EventArgs<Exception> e)
        {
            Logger?.Error($"线程({GetThreadName(Thread)})抛出了异常", e.Argument);
        }

        public void SetDefaultThreadOptions(ThreadOptions? threadOptions)
        {
            if (IsRunning)
                throw new InvalidOperationException("线程正在运行，无法更改线程配置");

            _defaultThreadOptions = threadOptions;
        }

        public void SetDefaultThreadName(string threadName)
        {
            ArgumentException.ThrowIfNullOrEmpty(threadName, nameof(threadName));
            if (IsRunning)
                throw new InvalidOperationException("线程正在运行，无法更改线程配置");

            _defaultThreadOptions ??= new();
            _defaultThreadOptions.Name = threadName;
        }

        protected abstract void Run();

        private void ThreadStart()
        {
            Started.Invoke(this, EventArgs.Empty);

            try
            {
                Run();
            }
            catch (ThreadInterruptedException ex)
            {
                Logger?.Warn($"线程({GetThreadName(Thread)})已被强制中断", ex);
            }
            catch (Exception ex)
            {
                Exception = ex;
                ThrowException.Invoke(this, new(ex));
            }
            finally
            {
                IsRunning = false;
                _stopSemaphore?.Release();
                _stopSemaphore = null;
                Stopped.Invoke(this, EventArgs.Empty);
            }
        }

        public virtual bool Start(ThreadOptions threadOptions)
        {
            ArgumentNullException.ThrowIfNull(threadOptions, nameof(threadOptions));

            lock (_lock)
            {
                if (IsRunning)
                    return false;

                _stopSemaphore = new();
                IsRunning = true;
                Exception = null;
                Thread = new(ThreadStart)
                {
                    IsBackground = threadOptions.IsBackground,
                    Priority = threadOptions.Priority
                };

                if (!string.IsNullOrEmpty(threadOptions.Name))
                    Thread.Name = threadOptions.Name;

                Thread.Start();
                return true;
            }
        }

        public virtual bool Start(string threadName)
        {
            ArgumentException.ThrowIfNullOrEmpty(threadName, nameof(threadName));

            ThreadOptions threadOptions = _defaultThreadOptions ?? new();
            threadOptions.Name = threadName;

            return Start(threadOptions);
        }

        public virtual bool Start()
        {
            ThreadOptions threadOptions = _defaultThreadOptions ?? new();
            return Start(threadOptions);
        }

        public virtual void Stop()
        {
            lock (_lock)
            {
                if (!IsRunning)
                    return;

                IsRunning = false;
                Thread? thread = Thread;
                if (thread is null)
                    return;

                if (thread == Thread.CurrentThread)
                {
                    Logger?.Warn($"尝试使用当前线程停止当前线程({GetThreadName(thread)})，将立即中断当前线程");
                    throw new ThreadInterruptedException();
                }

                try
                {
                    for (int i = 1; i <= StoppingRetryCount; i++)
                    {
                        if (thread.Join(1000))
                            return;

                        Logger?.Warn($"正在等待线程({GetThreadName(thread)})停止，已等待{i}秒");
                    }

                    Logger?.Warn($"即将强行中断线程({GetThreadName(thread)})");
                    thread.Interrupt();
                }
                catch (ThreadInterruptedException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Exception = ex;
                    Logger?.Error($"无法停止线程({GetThreadName(thread)})", ex);
                }
            }
        }

        public virtual void WaitForStop()
        {
            IWaitable? waiter = StopWaiter;
            if (waiter is null)
                return;

            do
            {
                ThrowIfException();
            } while (!waiter.Wait(1000));

            ThrowIfException();
        }

        public virtual async Task WaitForStopAsync()
        {
            IWaitable? waiter = StopWaiter;
            if (waiter is null)
                return;

            await Task.Yield();

            do
            {
                ThrowIfException();
            } while (!(await waiter.WaitAsync(1000).ConfigureAwait(false)));

            ThrowIfException();
        }

        protected virtual void ThrowIfException()
        {
            Exception? exception = Exception;
            if (exception is not null)
                throw new AggregateException($"线程({GetThreadName(Thread)})抛出了异常", exception);
        }

        private static string GetThreadName(Thread? thread)
        {
            return thread?.Name ?? "null";
        }
    }
}
