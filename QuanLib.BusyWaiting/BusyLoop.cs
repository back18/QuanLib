using QuanLib.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.BusyWaiting
{
    public class BusyLoop : RunnableBase
    {
        public BusyLoop(uint delayMilliseconds, ILoggerGetter? loggerGetter = null) : base(loggerGetter)
        {
            DelayMilliseconds = delayMilliseconds;
            IsPaused = false;

            _pauseSemaphore = new();
            _pauseSemaphore.Release();
            _loopTasks = new();
            _waitTasks = new();

            Loop += OnLoop;
        }

        private readonly object _lock = new();

        private TaskSemaphore _pauseSemaphore;

        private readonly ConcurrentQueue<LoopTask> _loopTasks;

        private readonly ConcurrentDictionary<Guid, WaitTask> _waitTasks;

        public uint DelayMilliseconds { get; }

        public bool IsPaused { get; private set; }

        public event EventHandler<BusyLoop, EventArgs> Loop;

        protected virtual void OnLoop(BusyLoop sender, EventArgs e) { }

        protected override void OnStopped(IRunnable sender, EventArgs e)
        {
            base.OnStopped(sender, e);

            CleanupTasks();
        }

        protected override void Run()
        {
            while (IsRunning)
            {
                if (!_pauseSemaphore.Wait(100))
                    continue;

                Loop.Invoke(this, EventArgs.Empty);

                int handled = 0;
                handled += HandleLoopTasks();
                handled += HandleWaitTasks();

                if (IsRunning && handled == 0)
                    Delay.Sleep(DelayMilliseconds);
            }
        }

        public void Pause()
        {
            lock (_lock)
            {
                if (!IsPaused)
                {
                    IsPaused = true;
                    _pauseSemaphore = new();
                }
            }
        }

        public void Resume()
        {
            lock (_lock)
            {
                if (IsPaused)
                {
                    IsPaused = false;
                    _pauseSemaphore.Release();
                }
            }
        }

        public void HandleSingleLoop()
        {
            lock (_lock)
            {
                if (IsRunning && !IsPaused)
                    return;

                Loop.Invoke(this, EventArgs.Empty);
                HandleLoopTasks();
                HandleWaitTasks();
            }
        }

        public LoopTask Submit(Action action)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));
            CheckIsRunning();

            LoopTask loopTask = new(action);
            _loopTasks.Enqueue(loopTask);
            return loopTask;
        }

        public void SubmitAndWait(Action action)
        {
            Submit(action).WaitForComplete();
        }

        public Task SubmitAndWaitAsync(Action action)
        {
            return Submit(action).WaitForCompleteAsync();
        }

        public WaitTask Submit(Func<bool> expression)
        {
            ArgumentNullException.ThrowIfNull(expression, nameof(expression));
            CheckIsRunning();

            Guid guid = Guid.NewGuid();
            WaitTask waitTask = new(expression);
            _waitTasks.TryAdd(guid, waitTask);
            return waitTask;
        }

        public void SubmitAndWait(Func<bool> expression)
        {
            Submit(expression).WaitForComplete();
        }

        public Task SubmitAndWaitAsync(Func<bool> expression)
        {
            return Submit(expression).WaitForCompleteAsync();
        }

        private int HandleLoopTasks()
        {
            int count = 0;
            while (_loopTasks.TryDequeue(out var loopTask))
            {
                loopTask.Start();
                count++;
            }
            return count;
        }

        private int HandleWaitTasks()
        {
            int count = 0;
            foreach (var item in _waitTasks)
            {
                WaitTask waitTask = item.Value;
                if (waitTask.CheckCondition() || waitTask.IsFailed || waitTask.IsCanceled)
                {
                    _waitTasks.Remove(item.Key, out _);
                    count++;
                }
            }
            return count;
        }

        private void CleanupTasks()
        {
            IsRunning = false;
            Thread.Sleep(1);

            HandleLoopTasks();
            HandleWaitTasks();

            foreach (WaitTask waitTask in _waitTasks.Values)
                waitTask.Cancel();

            _waitTasks.Clear();
        }

        private void CheckIsRunning()
        {
            if (!IsRunning)
                throw new InvalidOperationException("主循环已停止，无法继续操作");
        }
    }
}
