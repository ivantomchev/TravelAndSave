namespace TravelAndSave.Common.Utilities.Logger
{
    using log4net;
    using System;

    public class Logger : ILogger
    {
        private readonly ILog log;

        internal Logger(ILog log)
        {
            this.log = log;
        }

        public void Debug(object message)
        {
            if (!this.log.IsDebugEnabled)
            {
                return;
            }
            this.log.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            if (!this.log.IsDebugEnabled)
            {
                return;
            }
            this.log.Debug(message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (!this.log.IsDebugEnabled)
            {
                return;
            }
            this.log.DebugFormat(format, args);
        }

        public void Info(object message)
        {
            if (!this.log.IsInfoEnabled)
            {
                return;
            }
            this.log.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            if (!this.log.IsInfoEnabled)
            {
                return;
            }
            this.log.Info(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (!this.log.IsInfoEnabled)
            {
                return;
            }
            this.log.InfoFormat(format, args);
        }

        public void Warn(object message)
        {
            if (!this.log.IsWarnEnabled)
            {
                return;
            }
            this.log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            if (!this.log.IsWarnEnabled)
            {
                return;
            }
            this.log.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (!this.log.IsWarnEnabled)
            {
                return;
            }
            this.log.WarnFormat(format, args);
        }

        public void Error(object message)
        {
            if (!this.log.IsErrorEnabled)
            {
                return;
            }
            this.log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            if (!this.log.IsErrorEnabled)
            {
                return;
            }
            this.log.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (!this.log.IsErrorEnabled)
            {
                return;
            }
            this.log.ErrorFormat(format, args);
        }

        public void Fatal(object message)
        {
            if (!this.log.IsFatalEnabled)
            {
                return;
            }
            this.log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            if (!this.log.IsFatalEnabled)
            {
                return;
            }
            this.log.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (!this.log.IsFatalEnabled)
            {
                return;
            }
            this.log.FatalFormat(format, args);
        }

        public bool IsDebugEnabled
        {
            get { return this.log.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return this.log.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return this.log.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return this.log.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return this.log.IsFatalEnabled; }
        }
    }
}
