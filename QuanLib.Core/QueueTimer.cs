﻿using QuanLib.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public class QueueTimer
    {
        public QueueTimer(int maxCount)
        {
            ThrowHelper.ArgumentOutOfMin(1, maxCount, nameof(maxCount));
            MaxCount = maxCount;
            TotalTime = TimeSpan.Zero;

            _queue = new();

            Added += OnAdded;
        }

        private readonly Queue<TimeSpan> _queue;

        public int MaxCount { get; }

        public TimeSpan TotalTime { get; private set; }

        public TimeSpan AverageTime
        {
            get
            {
                if (_queue.Count == 0)
                    return TimeSpan.Zero;
                return TotalTime / _queue.Count;
            }
        }

        public event EventHandler<QueueTimer, TimeSpanEventArgs> Added;

        protected virtual void OnAdded(QueueTimer sender, TimeSpanEventArgs args) { }

        public void Append(TimeSpan time)
        {
            if (_queue.Count > MaxCount)
                TotalTime -= _queue.Dequeue();

            TotalTime += time;
            _queue.Enqueue(time);
            Added.Invoke(this, new(time));
        }

        public TimeSpan[] ToArray()
        {
            return _queue.ToArray();
        }
    }
}
