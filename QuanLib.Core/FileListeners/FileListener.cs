using QuanLib.Core.Event;
using QuanLib.Core.ExceptionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.FileListeners
{
    public class FileListener
    {
        public FileListener(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("要倾听的文件不存在", path);

            Path = path;
            _TimeInterval = 500;
            _runing = false;

            Started += OnStarted;
            Stopped += OnStopped;
            ListenerException += OnListenerException;
            FileInfoChanged += OnFileInfoChanged;
            WriteBytes += OnWriteBytes;
            FileReset += OnFileReset;
            FileDeleted += OnFileDeleted;
        }

        private bool _runing;

        public bool Runing => _runing;

        public string Path { get; }

        public int IntervalTime
        {
            get => _TimeInterval;
            set
            {
                ThrowHelper.TryThrowArgumentOutOfMinException(0, value, nameof(value));
                _TimeInterval = value;
            }
        }
        private int _TimeInterval;

        public event EventHandler<FileListener, EventArgs> Started;

        public event EventHandler<FileListener, EventArgs> Stopped;

        public event EventHandler<FileListener, ExceptionEventArgs> ListenerException;

        public event EventHandler<FileListener, FileInfoChangedEventArge> FileInfoChanged;

        public event EventHandler<FileListener, BytesEventArgs> WriteBytes;

        public event EventHandler<FileListener, EventArgs> FileReset;

        public event EventHandler<FileListener, EventArgs> FileDeleted;

        protected virtual void OnStarted(FileListener sender, EventArgs e) { }

        protected virtual void OnStopped(FileListener sender, EventArgs e) { }

        protected virtual void OnListenerException(FileListener sender, ExceptionEventArgs e) { }

        protected virtual void OnFileInfoChanged(FileListener sender, FileInfoChangedEventArge e)
        {
            if (e.NewFileInfo.Length > e.OldFileInfo.Length)
            {
                FileStream fs = new(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fs.Seek(e.OldFileInfo.Length, SeekOrigin.Begin);
                byte[] buffer = new byte[e.NewFileInfo.Length - e.OldFileInfo.Length - 1];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                WriteBytes.Invoke(this, new(buffer));
            }
            else if (e.NewFileInfo.Length < e.NewFileInfo.Length / 2)
            {
                FileStream fs = new(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] buffer = new byte[e.NewFileInfo.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                FileReset.Invoke(this, EventArgs.Empty);
                WriteBytes.Invoke(this, new(buffer));
            }
        }

        protected virtual void OnWriteBytes(FileListener sender, BytesEventArgs e) { }

        protected virtual void OnFileReset(FileListener sender, EventArgs e) { }

        protected virtual void OnFileDeleted(FileListener sender, EventArgs e) { }

        public void Start()
        {
            if (_runing)
                return;
            _runing = true;

            Started.Invoke(this, EventArgs.Empty);

            FileInfo oldFile = new(Path);
            while (_runing)
            {
                try
                {
                    FileInfo newFile = new(Path);
                    if (!newFile.Exists)
                    {
                        FileDeleted.Invoke(this, EventArgs.Empty);
                        break;
                    }

                    if (newFile.LastWriteTime > oldFile.LastWriteTime)
                    {
                        FileInfoChanged.Invoke(this, new(oldFile, newFile));
                        oldFile = newFile;
                    }

                    Thread.Sleep(IntervalTime);
                }
                catch (Exception ex)
                {
                    ListenerException.Invoke(this, new(ex));
                    continue;
                }
            }

            _runing = false;
            Stopped.Invoke(this, EventArgs.Empty);
        }

        public void Stop()
        {
            _runing = false;
        }
    }
}
