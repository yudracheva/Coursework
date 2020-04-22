using System;
using System.Data.SQLite;

namespace Project.Interfaces
{
    public interface ILogger : NLog.ILogger
    {
        void Log(string message);

        void Log(string message, SQLiteException exception);

        void Log(string message, Exception exception);
    }
}
