namespace TravelAndSave.Common.Utilities.Logger
{
    using System;

    public interface ILogManager
    {
        ILogger GetLogger(Type type);
    }
}
