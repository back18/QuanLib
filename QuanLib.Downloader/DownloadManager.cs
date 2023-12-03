using Downloader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Downloader
{
    public class DownloadManager : IReadOnlyList<DownloadTask>
    {
        public DownloadManager(DownloadConfiguration? configuration = null)
        {
            _configuration = configuration;
            _tasks = [];
        }

        private readonly DownloadConfiguration? _configuration;

        private readonly List<DownloadTask> _tasks;

        public DownloadTask this[int index] => _tasks[index];

        public int Count => _tasks.Count;

        public int RunningCount => GetTaskCount(DownloadStatus.Running);

        public int StoppedCount => GetTaskCount(DownloadStatus.Stopped);

        public int PausedCount => GetTaskCount(DownloadStatus.Paused);

        public int CompletedCount => GetTaskCount(DownloadStatus.Completed);

        public int FailedCount => GetTaskCount(DownloadStatus.Failed);

        public long TotalBytes
        {
            get
            {
                long count = 0;
                foreach (var task in _tasks)
                    count += task.Download.TotalFileSize;
                return count;
            }
        }

        public long DownloadedBytes
        {
            get
            {
                long count = 0;
                foreach (var task in _tasks)
                    count += task.Download.DownloadedFileSize;
                return count;
            }
        }

        public int GetTaskCount(DownloadStatus status) => GetTasks(status).Count();

        public IEnumerable<DownloadTask> GetTasks(DownloadStatus status)
        {
            return (from task in _tasks
                    where task.Download.Status == status
                    select task);
        }

        public IEnumerable<DownloadTask> GetRunningTasks() => GetTasks(DownloadStatus.Running);

        public IEnumerable<DownloadTask> GetPausedTasks() => GetTasks(DownloadStatus.Paused);

        public IEnumerable<DownloadTask> GetStoppedTasks() => GetTasks(DownloadStatus.Stopped);

        public IEnumerable<DownloadTask> GetCompletedTasks() => GetTasks(DownloadStatus.Completed);

        public IEnumerable<DownloadTask> GetFailedTasks() => GetTasks(DownloadStatus.Failed);

        public void Add(string url, string? path)
        {
            DownloadTask task = new(url, path, _configuration);
            _ = task.StartAsync();
            _tasks.Add(task);
        }

        public void ClearAll()
        {
            while (_tasks.Count > 0)
            {
                var task = _tasks[0];
                task.Dispose();
                _tasks.RemoveAt(0);
            }
        }

        public void ClearTasks(DownloadStatus status)
        {
            var tasks = GetTasks(status);
            foreach (var task in tasks)
            {
                task.Dispose();
                _tasks.Remove(task);
            }
        }

        public void ClearCompleted() => ClearTasks(DownloadStatus.Completed);

        public void ClearStopped() => ClearTasks(DownloadStatus.Stopped);

        public void ClearFailed() => ClearTasks(DownloadStatus.Failed);

        public async Task WaitAllTaskCompletedAsync()
        {
            var tasks = from task in _tasks
                        select task.Task;
            await Task.WhenAll(tasks);
        }

        public IEnumerator<DownloadTask> GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_tasks).GetEnumerator();
        }
    }
}
