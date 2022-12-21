using System;
using System.Collections.Generic;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.DiagnosticServices.Logging;
using log4net;

internal class ClientLogger : DefaultLogger
    {
    private readonly ILog log;
    private static readonly IDictionary<LogLevel,ConsoleColor?> colors = new Dictionary<LogLevel, ConsoleColor?>{
        { LogLevel.Debug,       ConsoleColor.Gray    },
        { LogLevel.Critical,    ConsoleColor.Red     },
        { LogLevel.Error,       ConsoleColor.DarkRed },
        { LogLevel.Trace,       ConsoleColor.Gray    },
        { LogLevel.Warning,     ConsoleColor.Yellow  },
        { LogLevel.Information, null },
        { LogLevel.None,        null }
        };

    public ClientLogger(ILog log)
        {
        this.log = log;
        }

    public override Boolean IsEnabled(LogLevel loglevel)
        {
        switch (loglevel)
            {
            case LogLevel.Trace:       return log.IsDebugEnabled;
            case LogLevel.Debug:       return log.IsDebugEnabled;
            case LogLevel.Information: return log.IsInfoEnabled;
            case LogLevel.Warning:     return log.IsWarnEnabled;
            case LogLevel.Error:       return log.IsErrorEnabled;
            case LogLevel.Critical:    return log.IsFatalEnabled;
            case LogLevel.None: break;
            default: throw new ArgumentOutOfRangeException(nameof(loglevel), loglevel, null);
            }
        return false;
        }

    public override void Log(LogLevel loglevel, String message)
        {
        base.Log(loglevel, message);
        switch (loglevel)
            {
            case LogLevel.Trace:       log.Debug(message); break;
            case LogLevel.Debug:       log.Debug(message); break;
            case LogLevel.Information: log.Info(message);  break;
            case LogLevel.Warning:     log.Warn(message);  break;
            case LogLevel.Error:       log.Error(message); break;
            case LogLevel.Critical:    log.Fatal(message); break;
            case LogLevel.None: break;
            default: throw new ArgumentOutOfRangeException(nameof(loglevel), loglevel, null);
            }
        #if LINUX
        if ((loglevel != LogLevel.Debug) && (loglevel != LogLevel.Trace)) {
        #endif
        var color = colors[loglevel];
        using ((color != null)
            ? new ColorScope(color.Value)
            : null)
            {
            Console.Error.WriteLine(message);
            }
        #if LINUX
            }
        #endif
        }
    }
