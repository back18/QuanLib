using log4net;
using log4net.Appender;
using log4net.Repository;
using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Logging
{
    public interface ILog4NetProvider : ILoggerProvider
    {
        public ILog GetLog4NetLogger();

        public ILog GetLog4NetLogger(Type type);

        public ILog GetLog4NetLogger(string name);

        public ILoggerRepository GetRepository();

        public TAppender[] GetAppenders<TAppender>() where TAppender : IAppender;
    }
}
