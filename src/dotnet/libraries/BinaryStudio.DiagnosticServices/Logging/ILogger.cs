using System;

namespace BinaryStudio.DiagnosticServices.Logging
    {
    public interface ILogger
        {
        Boolean IsEnabled(LogLevel loglevel);
        void Log(LogLevel loglevel, String message);
        void Log(LogLevel loglevel, String format, params Object[] args);
        void Log(LogLevel loglevel, Exception e);
        }
    }