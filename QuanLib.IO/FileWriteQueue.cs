using QuanLib.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public class FileWriteQueue : RunnableBase
    {
        public FileWriteQueue(ILoggerProvider? loggerProvider = null) : base(loggerProvider)
        {
            _queue = new();
            _enqueue = new(false);
        }


        private readonly ConcurrentQueue<WriteTask> _queue;

        private readonly AutoResetEvent _enqueue;

        public void Submit(WriteTask writeTask)
        {
            ArgumentNullException.ThrowIfNull(writeTask, nameof(writeTask));

            _queue.Enqueue(writeTask);
            _enqueue.Set();
        }

        protected override void Run()
        {
            while (IsRunning)
            {
                while (_queue.TryDequeue(out var task))
                    task.Run();

                _enqueue.WaitOne(10);
            }
        }
    }
}
