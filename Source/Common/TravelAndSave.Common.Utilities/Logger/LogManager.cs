namespace TravelAndSave.Common.Utilities.Logger
{
    using log4net.Config;
    using System;

    public class LogManager : ILogManager
    {
        static readonly ILogManager _logManager;

        static LogManager()
        {
            XmlConfigurator.Configure();
            _logManager = new LogManager();
        }

        public static ILogger GetLogger<T>()
        {
            return _logManager.GetLogger(typeof(T));
        }

        public ILogger GetLogger(Type type)
        {
            var logger = log4net.LogManager.GetLogger(type);
            return new Logger(logger);
        }
    }
}
