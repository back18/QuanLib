using QuanLib.Core;
using QuanLib.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public class PollingFileListener : RunnableBase
    {
        public PollingFileListener(string path, ILoggerGetter? loggerGetter = null) : base(loggerGetter)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("需要倾听的文件不存在", path);

            Path = path;
            _TimeInterval = 500;

            Polling += OnPolling;
            FileInfoChanged += OnFileInfoChanged;
            WriteBytes += OnWriteBytes;
            FileReset += OnFileReset;
            FileDeleted += OnFileDeleted;
        }

        public string Path { get; }

        public int IntervalTime
        {
            get => _TimeInterval;
            set
            {
                ThrowHelper.ArgumentOutOfMin(0, value, nameof(value));
                _TimeInterval = value;
            }
        }
        private int _TimeInterval;

        public event EventHandler<PollingFileListener, ValueChangedEventArgs<FileInfo>> Polling;

        public event EventHandler<PollingFileListener, ValueChangedEventArgs<FileInfo>> FileInfoChanged;

        public event EventHandler<PollingFileListener, EventArgs<byte[]>> WriteBytes;

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
            if (e.NewValue.Length > e.OldValue.Length)
            {
                FileStream fs = new(Path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete);
                fs.Seek(e.OldValue.Length, SeekOrigin.Begin);
                byte[] buffer = new byte[e.NewValue.Length - e.OldValue.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                WriteBytes.Invoke(this, new(buffer));
            }
            else if (e.NewValue.Length < e.NewValue.Length / 2)
            {
                FileStream fs = new(Path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete);
                byte[] buffer = new byte[e.NewValue.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                FileReset.Invoke(this, EventArgs.Empty);
                WriteBytes.Invoke(this, new(buffer));
            }
        }

        protected virtual void OnWriteBytes(PollingFileListener sender, EventArgs<byte[]> e) { }

        protected virtual void OnFileReset(PollingFileListener sender, EventArgs e) { }

        protected virtual void OnFileDeleted(PollingFileListener sender, EventArgs e) { }

        protected override void Run()
        {
            FileInfo oldFile = new(Path);
            while (IsRunning)
            {
                FileInfo newFile = new(Path);
                if (!newFile.Exists)
                {
                    FileDeleted.Invoke(this, EventArgs.Empty);
                    break;
                }

                Polling.Invoke(this, new(oldFile, newFile));
                oldFile = newFile;

                Thread.Sleep(IntervalTime);
            }
        }
    }
}
