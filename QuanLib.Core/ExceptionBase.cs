using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public abstract class ExceptionBase : Exception
    {
        public ExceptionBase() : base(null)
        {
            InitialMessage = DefaultMessage;
        }

        public ExceptionBase(string? message) : base(null)
        {
            InitialMessage = message;
        }

        public ExceptionBase(string? message, Exception? innerException) : base(null, innerException)
        {
            InitialMessage = message;
        }

        public ExceptionBase(Exception? innerException) : base(null, innerException)
        {
            InitialMessage = DefaultMessage;
        }

        public override string Message
        {
            get
            {
                if (!string.IsNullOrEmpty(InitialMessage))
                    return InitialMessage;
                else
                    return DefaultMessage;
            }
        }

        protected virtual string? InitialMessage { get; set; }

        protected abstract string DefaultMessage { get; }
    }
}
