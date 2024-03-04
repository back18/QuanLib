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

        public ExceptionBase(string? messge) : base(null)
        {
            InitialMessage = messge;
        }

        public ExceptionBase(string? messge, Exception innerException) : base(null, innerException)
        {
            InitialMessage = messge;
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
