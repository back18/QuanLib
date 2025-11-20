using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public interface ILoggerProvider
    {
        public ILogger GetLogger();

        public ILogger GetLogger(Type type);

        public ILogger GetLogger(string name);
    }
}
