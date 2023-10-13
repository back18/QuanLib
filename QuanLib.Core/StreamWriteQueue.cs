﻿using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public class StreamWriteQueue : StreamWriteQueueBase
    {
        public StreamWriteQueue(Stream stream, Func<Type, LogImpl> logger) : base(logger)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            _func = () => stream;
        }

        public StreamWriteQueue(Func<Stream> stream, Func<Type, LogImpl> logger) : base(logger)
        {
            _func = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        private readonly Func<Stream> _func;

        public override Stream BaseStream => _func.Invoke();
    }
}
