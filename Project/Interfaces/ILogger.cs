using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Interfaces
{
    public interface ILogger : NLog.ILogger
    {
        void Log(string message);

        void Log(string message, SQLiteException exception);

        void Log(string message, Exception exception);
    }
}
