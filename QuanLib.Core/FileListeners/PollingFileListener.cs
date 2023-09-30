using log4net.Core;
using QuanLib.Core.Events;
using QuanLib.Core.ExceptionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.FileListeners
{
    public class PollingFileListener : RunnableBase
    {
        public PollingFileListener(string path, Func<Type, LogImpl> logger) : base(logger)
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

        public event EventHandler<PollingFileListener, FileInfoChangedEventArge> Polling;

        public event EventHandler<PollingFileListener, FileInfoChangedEventArge> FileInfoChanged;

        public event EventHandler<PollingFileListener, BytesEventArgs> WriteBytes;

        public event EventHandler<PollingFileListener, EventArgs> FileReset;

        public event EventHandler<PollingFileListener, EventArgs> FileDeleted;

        protected virtual void OnPolling(PollingFileListener sender, FileInfoChangedEventArge e)
        {
            if (e.NewFileInfo.LastWriteTime > e.OldFileInfo.LastWriteTime)
            {
                FileInfoChanged.Invoke(this, e);
            }
        }

        protected virtual void OnFileInfoChanged(PollingFileListener sender, FileInfoChangedEventArge e)
        {
            if (e.NewFileInfo.Length > e.OldFileInfo.Length)
            {
                FileStream fs = new(Path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete);
                fs.Seek(e.OldFileInfo.Length, SeekOrigin.Begin);
                byte[] buffer = new byte[e.NewFileInfo.Length - e.OldFileInfo.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                WriteBytes.Invoke(this, new(buffer));
            }
            else if (e.NewFileInfo.Length < e.NewFileInfo.Length / 2)
            {
                FileStream fs = new(Path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete);
                byte[] buffer = new byte[e.NewFileInfo.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                FileReset.Invoke(this, EventArgs.Empty);
                WriteBytes.Invoke(this, new(buffer));
            }
        }

        protected virtual void OnWriteBytes(PollingFileListener sender, BytesEventArgs e) { }

        protected virtual void OnFileReset(PollingFileListener sender, EventArgs e) { }

        protected virtual void OnFileDeleted(PollingFileListener sender, EventArgs e) { }

        protected override void Run()
        {
            FileInfo oldFile = new(Path);
            while (IsRuning)
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
