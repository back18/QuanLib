using log4net.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public abstract class StreamWriteQueueBase : RunnableBase
    {
        public StreamWriteQueueBase(Func<Type, LogImpl> logger) : base(logger)
        {
            _queue = new();
            _enqueue = new(false);
        }

        private readonly ConcurrentQueue<byte[]> _queue;

        private readonly AutoResetEvent _enqueue;

        public abstract Stream BaseStream { get; }

        public void Submit(byte[] bytes)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));

            _queue.Enqueue(bytes);
            _enqueue.Set();
        }

        protected override void Run()
        {
            while (IsRunning)
            {
                while (_queue.TryDequeue(out var bytes))
                    BaseStream.Write(bytes);

                _enqueue.WaitOne(10);
            }
        }
    }
}
