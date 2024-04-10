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
        public BusyLoop(ILoggerGetter? loggerGetter = null) : base(loggerGetter)
        {
            _pauseSemaphore = new(0);
            _pauseTask = WaitSemaphoreAsync();

            _loopTasks = new();
            _waitTasks = new();

            Loop += OnLoop;
        }

        private readonly SemaphoreSlim _pauseSemaphore;

        private Task _pauseTask;

        private readonly ConcurrentQueue<LoopTask> _loopTasks;

        private readonly ConcurrentDictionary<Guid, WaitTask> _waitTasks;

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

                Thread.Yield();
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
            _pauseTask = WaitSemaphoreAsync();
        }

        public void Resume()
        {
            _pauseSemaphore.Release();
        }

        public LoopTask Submit(Action action)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));

            if (!IsRunning)
                throw new InvalidOperationException("主循环未在运行，因此无法提交任务");

            LoopTask loopTask = new(action);
            _loopTasks.Enqueue(loopTask);
            return loopTask;
        }

        public async Task<LoopTask> SubmitAndWaitAsync(Action action)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));

            if (!IsRunning)
                throw new InvalidOperationException("主循环未在运行，因此无法提交任务");

            LoopTask loopTask = new(action);
            _loopTasks.Enqueue(loopTask);
            await loopTask.WaitForCompleteAsync();
            return loopTask;
        }

        public async Task SubmitAndWaitAsync(Func<bool> expression)
        {
            ArgumentNullException.ThrowIfNull(expression, nameof(expression));

            if (!IsRunning)
                throw new InvalidOperationException("主循环未在运行，因此无法提交任务");

            Guid guid = Guid.NewGuid();
            WaitTask waitTask = new(this, expression);
            _waitTasks.TryAdd(guid, waitTask);
            await waitTask.WaitForSuccessAsync();
        }

        private async Task WaitSemaphoreAsync()
        {
            while (IsRunning && !await _pauseSemaphore.WaitAsync(10)) { }
        }

        private void HandleLoopTasks()
        {
            while (_loopTasks.TryDequeue(out var loopTask))
            {
                loopTask.Start();
                if (loopTask.State == LoopTaskState.Failed && loopTask.Exception is not null)
                    throw new AggregateException(loopTask.Exception);
            }
        }

        private void HandleWaitTasks()
        {
            foreach (var item in _waitTasks)
            {
                if (item.Value.CheckExpression())
                    _waitTasks.Remove(item.Key, out _);
            }
        }
    }
}
