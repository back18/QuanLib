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

            _pauseSemaphore = new(0);
            _pauseTask = WaitSemaphoreAsync();
            _loopTasks = new();
            _waitTasks = new();

            Loop += OnLoop;
        }

        private readonly object _lock = new();

        private readonly SemaphoreSlim _pauseSemaphore;

        private Task _pauseTask;

        private readonly ConcurrentQueue<LoopTask> _loopTasks;

        private readonly ConcurrentDictionary<Guid, WaitTask> _waitTasks;

        public uint DelayMilliseconds { get; }

        public bool IsPaused { get; private set; }

        public event EventHandler<BusyLoop, EventArgs> Loop;

        protected virtual void OnLoop(BusyLoop sender, EventArgs e) { }

        protected override void Run()
        {
            do
            {
                Loop.Invoke(this, EventArgs.Empty);

                HandleLoopTasks();

                HandleWaitTasks();

                _pauseTask.Wait();

                Delay.Sleep(DelayMilliseconds);
            }
            while (IsRunning);
        }

        protected override void OnStopped(IRunnable sender, EventArgs e)
        {
            base.OnStopped(sender, e);

            HandleWaitTasks();
        }

        public void Pause()
        {
            lock (_lock)
            {
                if (!IsPaused)
                    _pauseTask = WaitSemaphoreAsync();
            }
        }

        public void Resume()
        {
            lock (_lock)
            {
                if (IsPaused)
                    _pauseSemaphore.Release();
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

        public async Task<LoopTask> SubmitAndWaitAsync(Action action)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));
            CheckIsRunning();

            LoopTask loopTask = new(action);
            _loopTasks.Enqueue(loopTask);
            await loopTask.WaitForCompleteAsync();
            return loopTask;
        }

        public async Task SubmitAndWaitAsync(Func<bool> expression)
        {
            ArgumentNullException.ThrowIfNull(expression, nameof(expression));
            CheckIsRunning();

            Guid guid = Guid.NewGuid();
            WaitTask waitTask = new(this, expression);
            _waitTasks.TryAdd(guid, waitTask);
            await waitTask.WaitForSuccessAsync();
        }

        private async Task WaitSemaphoreAsync()
        {
            IsPaused = true;
            while (IsRunning && !await _pauseSemaphore.WaitAsync(10)) { }
            IsPaused = false;
        }

        private void HandleLoopTasks()
        {
            while (_loopTasks.TryDequeue(out var loopTask))
            {
                loopTask.Start();
                if (loopTask.State == LoopTaskState.Failed && loopTask.Exception is not null)
                    throw new AggregateException(loopTask.Exception);
                loopTask.Dispose();
            }
        }

        private void HandleWaitTasks()
        {
            foreach (var item in _waitTasks)
            {
                WaitTask waitTask = item.Value;
                if (waitTask.CheckCondition())
                {
                    if (_waitTasks.Remove(item.Key, out var value))
                        value.Dispose();
                }
            }
        }

        private void CheckIsRunning()
        {
            if (!IsRunning)
                throw new InvalidOperationException("主循环已停止，无法继续操作");
        }
    }
}
