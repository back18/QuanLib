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
        protected RunnableBase(ILoggerGetter? loggerGetter = null)
        {
            Type type = GetType();
            if (loggerGetter is not null)
                _logger = loggerGetter.GetLogger(type);

            IsRunning = false;

            _subtasks = [];
            _stopSemaphore = new(0);
            StopTimeout = 5;
            Started += OnStarted;
            Started += OnStartedLoggging;
            Stopped += OnStopped;
            Stopped += OnStoppedLogging;
            ThrowException += OnThrowException;
            ThrowException += OnThrowExceptionLogging;
        }

        private readonly object _lock = new();

        private readonly ILogger? _logger;

        private readonly List<RunnableBase> _subtasks;

        private readonly SemaphoreSlim _stopSemaphore;

        private Task? _stopTask;

        private ThreadOptions? _defaultThreadOptions;

        public virtual Thread? Thread { get; protected set; }

        public virtual Exception? Exception { get; protected set; }

        public virtual bool IsRunning { get; protected set; }

        public virtual int StopTimeout { get; protected set; }

        public event EventHandler<IRunnable, EventArgs> Started;

        public event EventHandler<IRunnable, EventArgs> Stopped;

        public event EventHandler<IRunnable, ExceptionEventArgs> ThrowException;

        protected virtual void OnStarted(IRunnable sender, EventArgs e) { }

        protected virtual void OnStopped(IRunnable sender, EventArgs e) { }

        protected virtual void OnThrowException(IRunnable sender, ExceptionEventArgs e) { }

        private void OnStartedLoggging(IRunnable sender, EventArgs e)
        {
            _logger?.Info($"线程({GetThreadName(Thread)})已启动");
        }

        private void OnStoppedLogging(IRunnable sender, EventArgs e)
        {
            _logger?.Info($"线程({GetThreadName(Thread)})已停止");
        }

        private void OnThrowExceptionLogging(IRunnable sender, ExceptionEventArgs e)
        {
            _logger?.Error($"线程({GetThreadName(Thread)})抛出了异常", e.Exception);
        }

        public void SetDefaultThreadOptions(ThreadOptions? threadOptions)
        {
            _defaultThreadOptions = threadOptions;
        }

        public void SetDefaultThreadName(string threadName)
        {
            _defaultThreadOptions ??= new();
            _defaultThreadOptions.Name = threadName;
        }

        protected void AddSubtask(RunnableBase runnable)
        {
            ArgumentNullException.ThrowIfNull(runnable, nameof(runnable));

            _subtasks.Add(runnable);
        }

        protected abstract void Run();

        public virtual bool Start(ThreadOptions threadOptions)
        {
            ArgumentNullException.ThrowIfNull(threadOptions, nameof(threadOptions));

            lock (_lock)
            {
                if (IsRunning)
                    return false;

                IsRunning = true;
                Exception = null;
                Thread = new(ThreadStart);
                Thread.IsBackground = threadOptions.IsBackground;
                Thread.Priority = threadOptions.Priority;
                if (!string.IsNullOrEmpty(threadOptions.Name))
                    Thread.Name = threadOptions.Name;
                Thread.Start();
                _stopTask = WaitSemaphoreAsync();
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

                StopSubTasks();

                try
                {
                    for (int i = 1; i <= StopTimeout; i++)
                    {
                        if (thread.Join(1000))
                            return;

                        _logger?.Warn($"正在等待线程({GetThreadName(thread)})停止，已等待{i}秒");
                    }

                    _logger?.Warn($"即将强行停止线程({GetThreadName(thread)})");
                    thread.Abort();
                }
                catch (Exception ex)
                {
                    Exception = ex;
                    _logger?.Error($"无法停止线程({GetThreadName(thread)})", ex);
                }
            }
        }

        public void WaitForStop()
        {
            if (_stopTask is null)
                return;

            _stopTask.Wait();
        }

        public async Task WaitForStopAsync()
        {
            if (_stopTask is null)
                return;

            await _stopTask;
        }

        protected void ThreadStart()
        {
            try
            {
                Started.Invoke(this, EventArgs.Empty);

                StartSubTasks();

                Run();

                Task.WaitAll(_subtasks.Select(s => s.WaitForStopAsync()).ToArray());
            }
            catch (Exception ex)
            {
                Exception = ex;
                ThrowException.Invoke(this, new(ex));
            }
            finally
            {
                IsRunning = false;
                _stopSemaphore.Release();
                Stopped.Invoke(this, EventArgs.Empty);
            }
        }

        protected void CheckedException()
        {
            if (Exception is not null)
                throw new AggregateException($"线程({GetThreadName(Thread)})抛出了异常", Exception);
        }

        protected void StartSubTasks()
        {
            foreach (IRunnable runnable in _subtasks)
                runnable.Start();
        }

        protected void StopSubTasks()
        {
            foreach (IRunnable runnable in _subtasks)
                runnable.Stop();
        }

        protected void CheckedSubtasksException()
        {
            List<Exception> exceptions = [];
            foreach (RunnableBase runnable in _subtasks)
            {
                try
                {
                    runnable.CheckedException();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
                throw new AggregateException($"线程({GetThreadName(Thread)})的一个或多个子任务抛出了异常", exceptions);
        }

        private async Task WaitSemaphoreAsync()
        {
            try
            {
                while (true)
                {
                    CheckedException();

                    if (await _stopSemaphore.WaitAsync(1000))
                    {
                        CheckedException();
                        break;
                    }
                }
            }
            finally
            {
                _stopTask = null;
            }
        }

        private static string GetThreadName(Thread? thread)
        {
            return thread?.Name ?? "null";
        }
    }
}
