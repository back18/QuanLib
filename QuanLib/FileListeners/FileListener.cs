using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.FileListeners
{
    public class FileListener
    {
        public FileListener(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentException("文件不存在", nameof(path));

            Path = path;
            _isrun = false;
            _TimeInterval = 500;

            OnLastWriteTime += FileListener_OnLastWriteTime;
        }

        private bool _isrun;

        public bool IsRun => _isrun;

        public string Path { get; }

        public int IntervalTime
        {
            get => _TimeInterval;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "参数不能小于0");
                _TimeInterval = value;
            }
        }
        private int _TimeInterval;

        public event Action OnListenerStart = () => { };

        public event Action OnListenerStop = () => { };

        public event Action<Exception> OnListenerException = (obj) => { };

        public event Action<FileInfo, FileInfo> OnLastWriteTime;

        public event Action<byte[]> OnWriteByte = (obj) => { };

        public event Action OnFileDelete = () => { };

        private void FileListener_OnLastWriteTime(FileInfo oldFile, FileInfo newFile)
        {
            if (newFile.Length > oldFile.Length)
            {
                FileStream fs = new(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fs.Seek(oldFile.Length, SeekOrigin.Begin);
                byte[] buffer = new byte[newFile.Length - oldFile.Length - 1];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                OnWriteByte.Invoke(buffer);
            }
            else if (newFile.Length < oldFile.Length / 2)
            {
                Console.WriteLine("检测到监视的文件重置，将从新读取全部文件内容");
                FileStream fs = new(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] buffer = new byte[newFile.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                OnWriteByte.Invoke(buffer);
            }
        }

        public void Start()
        {
            _isrun = true;
            OnListenerStart.Invoke();

            FileInfo oldFile = new(Path);
            while (_isrun)
            {
                try
                {
                    FileInfo newFile = new(Path);
                    if (!newFile.Exists)
                    {
                        OnFileDelete.Invoke();
                        continue;
                    }

                    if (newFile.LastWriteTime > oldFile.LastWriteTime)
                    {
                        OnLastWriteTime.Invoke(oldFile, newFile);
                    }
                    else
                    {
                        Thread.Sleep(IntervalTime);
                    }
                    oldFile = newFile;
                }
                catch (Exception ex)
                {
                    OnListenerException.Invoke(ex);
                    continue;
                }
            }

            OnListenerStop.Invoke();
        }

        public void Stop()
        {
            _isrun = false;
        }
    }
}
