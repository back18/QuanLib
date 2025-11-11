using QuanLib.Core;
using QuanLib.Core.Events;
using QuanLib.IO.Events;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public class PollingFileListener : RunnableBase
    {
        public PollingFileListener(string path, int delayMilliseconds = 500, ILoggerGetter? loggerGetter = null) : base(loggerGetter)
        {
            ThrowHelper.FileNotFound(path);
            ThrowHelper.ArgumentOutOfMin(0, delayMilliseconds, nameof(delayMilliseconds));

            Path = path;
            DelayMilliseconds = delayMilliseconds;

            Polling += OnPolling;
            FileInfoChanged += OnFileInfoChanged;
            WriteBytes += OnWriteBytes;
            FileReset += OnFileReset;
            FileDeleted += OnFileDeleted;
        }

        public string Path { get; }

        public int DelayMilliseconds { get; }

        public event EventHandler<PollingFileListener, ValueChangedEventArgs<FileInfo>> Polling;

        public event EventHandler<PollingFileListener, ValueChangedEventArgs<FileInfo>> FileInfoChanged;

        public event EventHandler<PollingFileListener, BytesEventArgs> WriteBytes;

        public event EventHandler<PollingFileListener, EventArgs> FileReset;

        public event EventHandler<PollingFileListener, EventArgs> FileDeleted;

        protected virtual void OnPolling(PollingFileListener sender, ValueChangedEventArgs<FileInfo> e)
        {
            if (e.NewValue.LastWriteTime > e.OldValue.LastWriteTime)
            {
                FileInfoChanged.Invoke(this, e);
            }
        }

        protected virtual void OnFileInfoChanged(PollingFileListener sender, ValueChangedEventArgs<FileInfo> e)
        {
            FileInfo oldFile = e.OldValue;
            FileInfo newFile = e.NewValue;

            if (newFile.Length <= oldFile.Length && newFile.CreationTime <= oldFile.LastWriteTime)
                return;

            if (newFile.Length == 0)
                return;

            ArrayPool<byte> bytesPool = ArrayPool<byte>.Shared;
            FileStream? fileStream = null;
            byte[]? buffer = null;
            int length;

            try
            {
                fileStream = new(Path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete);

                if (newFile.Length > oldFile.Length)
                {
                    fileStream.Seek(oldFile.Length, SeekOrigin.Begin);
                    length = (int)(newFile.Length - oldFile.Length);
                }
                else
                {
                    FileReset.Invoke(this, EventArgs.Empty);
                    length = (int)newFile.Length;
                }

                buffer = bytesPool.Rent(length);
                fileStream.Read(buffer, 0, length);
                WriteBytes.Invoke(this, new(buffer, 0, length));
            }
            finally
            {
                fileStream?.Close();
                if (buffer is not null)
                    bytesPool.Return(buffer);
            }
        }

        protected virtual void OnWriteBytes(PollingFileListener sender, BytesEventArgs e) { }

        protected virtual void OnFileReset(PollingFileListener sender, EventArgs e) { }

        protected virtual void OnFileDeleted(PollingFileListener sender, EventArgs e) { }

        protected override void Run()
        {
            int fileNotFound = 0;
            FileInfo oldFile = new(Path);

            while (IsRunning)
            {
                FileInfo newFile = new(Path);
                if (!newFile.Exists)
                {
                    fileNotFound++;
                    if (fileNotFound < 10)
                    {
                        Thread.Sleep(DelayMilliseconds / 10);
                        continue;
                    }

                    FileDeleted.Invoke(this, EventArgs.Empty);
                    break;
                }

                Polling.Invoke(this, new(oldFile, newFile));
                oldFile = newFile;
                fileNotFound = 0;

                Thread.Sleep(DelayMilliseconds);
            }
        }
    }
}
