﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public abstract class UnmanagedBase : IDisposable
    {
        protected UnmanagedBase()
        {
            IsDisposed = false;
        }

        private readonly object _lock = new();

        public bool IsDisposed { get; protected set; }

        protected abstract void DisposeUnmanaged();

        protected virtual void NotDisposeUnmanaged()
        {

        }

        protected void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (IsDisposed)
                    return;

                if (disposing)
                    DisposeUnmanaged();
                else
                    NotDisposeUnmanaged();

                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~UnmanagedBase()
        {
            Dispose(disposing: false);
        }
    }
}
