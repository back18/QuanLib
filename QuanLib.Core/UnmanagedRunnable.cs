using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public abstract class UnmanagedRunnable : RunnableBase, IDisposable
    {
        protected UnmanagedRunnable(ILoggerProvider? loggerProvider = null) : base(loggerProvider)
        {
            IsDisposed = false;
        }

        private readonly Lock _lock = new();

        public virtual bool IsDisposed { get; protected set; }

        public override bool Start()
        {
            lock (_lock)
            {
                if (IsDisposed)
                    return false;

                return base.Start();
            }
        }

        protected abstract void DisposeUnmanaged();

        protected virtual void NotDisposeUnmanaged() { }

        protected void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (IsDisposed)
                    return;

                if (IsRunning)
                    Logger?.Warn($"线程({GetThreadName(Thread)})在Stop前调用了Dispose，可能会导致资源泄露");

                try
                {
                    if (disposing)
                        DisposeUnmanaged();
                    else
                        NotDisposeUnmanaged();
                }
                catch (Exception ex)
                {
                    Logger?.Error($"线程({GetThreadName(Thread)})在释放非托管资源时抛出了异常", ex);
                }
                finally
                {
                    IsDisposed = true;
                }
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private static string GetThreadName(Thread? thread)
        {
            return thread?.Name ?? "null";
        }

        ~UnmanagedRunnable()
        {
            Dispose(disposing: false);
        }
    }
}
