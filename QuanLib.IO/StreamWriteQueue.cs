using QuanLib.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public class StreamWriteQueue : RunnableBase
    {
        public StreamWriteQueue(Stream stream, ILoggerProvider? loggerProvider = null) : base(loggerProvider)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));

            BaseStream = stream;
            _queue = new();
            _enqueue = new(false);
        }

        private readonly ConcurrentQueue<byte[]> _queue;

        private readonly AutoResetEvent _enqueue;

        public Stream BaseStream { get; }

        public void Submit(byte[] bytes)
        {
            ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

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
