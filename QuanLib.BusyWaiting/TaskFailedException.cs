using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.BusyWaiting
{
    public class TaskFailedException : ExceptionBase
    {
        public TaskFailedException() : base() { }

        public TaskFailedException(string? messge) : base(messge) { }

        public TaskFailedException(Exception? innerException) : base(innerException) { }

        public TaskFailedException(string? messge, Exception? innerException) : base(messge, innerException) { }

        protected override string DefaultMessage { get; } = "任务执行失败";
    }
}
