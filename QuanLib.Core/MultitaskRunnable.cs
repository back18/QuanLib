using QuanLib.Core.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public abstract class MultitaskRunnable : UnmanagedRunnable
    {
        public MultitaskRunnable(ILoggerGetter? loggerGetter = null) : base(loggerGetter)
        {
            _subtasks = [];

            SubtaskStarted += OnSubtaskStarted;
            SubtaskStopped += OnSubtaskStopped;
            SubtaskThrowException += OnSubtaskThrowException;

            Started += MultitaskRunnable_Started;
            Stopped += MultitaskRunnable_Stopped;
        }

        private readonly object _lock = new();

        private readonly List<IRunnable> _subtasks;

        public event EventHandler<MultitaskRunnable, EventArgs<IRunnable>> SubtaskStarted;

        public event EventHandler<MultitaskRunnable, EventArgs<IRunnable>> SubtaskStopped;

        public event EventHandler<MultitaskRunnable, EventArgs<IRunnable, Exception>> SubtaskThrowException;

        protected virtual void OnSubtaskStopped(MultitaskRunnable sender, EventArgs<IRunnable> e) { }

        protected virtual void OnSubtaskStarted(MultitaskRunnable sender, EventArgs<IRunnable> e) { }

        protected virtual void OnSubtaskThrowException(MultitaskRunnable sender, EventArgs<IRunnable, Exception> e) { }

        private void SubTask_Started(IRunnable sender, EventArgs e)
        {
            SubtaskStarted.Invoke(this, new(sender));
        }

        private void SubTask_Stopped(IRunnable sender, EventArgs e)
        {
            SubtaskStopped.Invoke(this, new(sender));
        }

        private void SubTask_ThrowException(IRunnable sender, EventArgs<Exception> e)
        {
            SubtaskThrowException.Invoke(this, new(sender, e.Argument));
        }

        private void MultitaskRunnable_Started(IRunnable sender, EventArgs e)
        {
            StartSubTasks();
        }

        private void MultitaskRunnable_Stopped(IRunnable sender, EventArgs e)
        {
            StopSubTasks();
        }

        protected void AddSubtask(IRunnable runnable)
        {
            ArgumentNullException.ThrowIfNull(runnable, nameof(runnable));
            if (IsRunning || IsDisposed)
                throw new InvalidOperationException("当前状态无法添加子任务线程");

            lock (_lock)
            {
                _subtasks.Add(runnable);
                runnable.Started += SubTask_Started;
                runnable.Stopped += SubTask_Stopped;
                runnable.ThrowException += SubTask_ThrowException;
            }
        }

        private void StartSubTasks()
        {
            lock (_lock)
            {
                if (_subtasks.Count == 0)
                    return;

                int count = 0;
                foreach (IRunnable runnable in _subtasks)
                {
                    if (runnable.Start())
                        count++;
                }

                if (Logger is not null)
                {
                    if (count == _subtasks.Count)
                        Logger.Info($"线程({GetThreadName(Thread)})启动了{count}个子任务线程，全部启动成功");
                    else
                        Logger.Warn($"线程({GetThreadName(Thread)})启动了{count}个子任务线程，{_subtasks.Count - count}个启动失败");
                }
            }
        }

        private void StopSubTasks()
        {
            lock (_lock)
            {
                var running = _subtasks.Where(s => s.IsRunning).ToArray();
                if (_subtasks.Count == 0 || running.Length == 0)
                    return;

                foreach (IRunnable runnable in running)
                    runnable.Stop();

                Logger?.Info($"线程({GetThreadName(Thread)})停止了{running.Length}个子任务线程");
            }
        }

        protected void WaitAllSubtask()
        {
            if (_subtasks.Count == 0)
                return;

            List<Task> tasks = [];
            foreach (IRunnable runnable in _subtasks)
                tasks.Add(runnable.WaitForStopAsync());

            while (tasks.Count > 0 && IsRunning)
            {
                int index = Task.WaitAny(tasks.ToArray(), 100);
                if (index == -1)
                    continue;

                Task task = tasks[index];
                tasks.RemoveAt(index);

                if (task.IsFaulted)
                {
                    Exception exception = task.Exception;
                    if (exception is AggregateException aggregateException && aggregateException.InnerExceptions.Count == 1)
                        exception = aggregateException.InnerExceptions[0];

                    throw new AggregateException($"线程({GetThreadName(Thread)})的一个或多个子任务线程抛出了异常", exception);
                }
            }
        }

        protected override void DisposeUnmanaged()
        {
            foreach (IRunnable runnable in _subtasks)
            {
                runnable.Started -= SubTask_Started;
                runnable.Stopped -= SubTask_Stopped;
                runnable.ThrowException -= SubTask_ThrowException;

                if (runnable is IDisposable disposable)
                    disposable.Dispose();
            }

            _subtasks.Clear();
        }

        private static string GetThreadName(Thread? thread)
        {
            return thread?.Name ?? "null";
        }
    }
}
