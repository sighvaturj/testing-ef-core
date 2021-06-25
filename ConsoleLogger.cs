using Microsoft.Extensions.Logging;
using System;
using static System.Console;

namespace Packt.Shared
{
    // a provider class that returns a logger
    public class ConsoleLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new ConsoleLogger();
        }

        // if logger uses unmanaged resources memory can be released here
        public void Dispose() { }
    }

    // a logger class
    public class ConsoleLogger : ILogger
    {
        // if logger uses unmanaged resources we can return an IDisposable class here
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
        public bool IsEnabled(LogLevel logLevel)
        {
            // avoid overlogging by filtering on the log level
            switch(logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Information:
                case LogLevel.None:
                    return false;
                case LogLevel.Debug:
                case LogLevel.Warning:
                case LogLevel.Error:
                case LogLevel.Critical:
                default:
                    return true;
            };
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, 
            TState state, Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            if (eventId == 20100)
            {
                // log the level and event identifier
                Write($"Level: {logLevel}, EventId: {eventId.Id}");
                // only output the state or exception if it exists
                if (state != null)
                {
                    Write($", State: {state}");
                }
                if (exception != null)
                {
                    Write($", Exception: {exception.Message}");
                }
                WriteLine();
            }
        }
    }
}