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
        public BusyLoop()
        {
            _loopTasks = new();
            _waitTasks = new();

            Loop += OnLoop;
        }

        private readonly ConcurrentQueue<LoopTask> _loopTasks;

        private readonly ConcurrentDictionary<Guid, WaitTask> _waitTasks;

        public event EventHandler<BusyLoop, EventArgs> Loop;

        protected virtual void OnLoop(BusyLoop sender, EventArgs e) { }

        protected override void Run()
        {
            do
            {
                Loop.Invoke(this, EventArgs.Empty);

                while (_loopTasks.TryDequeue(out var loopTask))
                {
                    loopTask.Start();
                    if (loopTask.State == LoopTaskState.Failed && loopTask.Exception is not null)
                        throw new AggregateException(loopTask.Exception);
                }

                foreach (var item in _waitTasks)
                {
                    if (item.Value.CheckExpression())
                        _waitTasks.Remove(item.Key, out _);
                }

                Thread.Yield();
            }
            while (IsRunning);
        }

        public async Task<LoopTask> SubmitAndWaitAsync(Action action)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));

            LoopTask loopTask = new(action);
            _loopTasks.Enqueue(loopTask);
            await loopTask.WaitForCompleteAsync();
            return loopTask;
        }

        public async Task SubmitAndWaitAsync(Func<bool> expression)
        {
            ArgumentNullException.ThrowIfNull(expression, nameof(expression));
            
            Guid guid = Guid.NewGuid();
            WaitTask waitTask = new(expression);
            _waitTasks.TryAdd(guid, waitTask);
            await waitTask.WaitForSuccessAsync();
        }
    }
}
