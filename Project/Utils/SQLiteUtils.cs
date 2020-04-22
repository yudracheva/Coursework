using Project.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Project.Utils
{
    /// <summary>
    /// Исключение полученное при работе с БД SQLite. Содержит в себе полный текст sql-команды.
    /// </summary>
    public class SQLiteSQLException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public string SQLStatement;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sqlStatement"></param>
        public SQLiteSQLException(string message, string sqlStatement)
            : base(message)
        {
            SQLStatement = sqlStatement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sqlStatement"></param>
        /// <param name="innerException"></param>
        public SQLiteSQLException(string message, string sqlStatement, Exception innerException)
            : base(message, innerException)
        {
            SQLStatement = sqlStatement;
        }
    }

    /// <summary>
    /// Вспомогательный класс для возможности работы с внутренним и внешнем подключением к БД SQLite.
    /// </summary>
    /// <remarks>
    /// Внешним подключением должно считаться подключение, которое не закрывается после выполнения одной команды или метода, т.к. содержат в себе транзакцию завершение которой контролируется в другом месте.
    /// Внутренние подключения - "запасные" подключения, которые позволяют выполнить запрос в рамках автоматической транзакции, которая будет зафиксирована сразу же.
    /// </remarks>
    public sealed class SQLiteConnectionExt : IDisposable
    {
        private readonly ILogger _logger;
        private readonly List<SQLiteCommand> _commands;
        private readonly List<SQLiteDataReader> _readers;

        /// <summary>
        /// Признак использования внешнего подключения
        /// </summary>
        /// <remarks>
        /// Подключения к БД переданные в качестве внешнего не закрываются в методе Dispose.
        /// </remarks>
        public bool IsExtConnection { get; private set; }

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        public SQLiteConnection Connection { get; set; }

        /// <summary>
        /// Конструктор. Использует внешнее подключение по-умолчанию. Если оно отсутствует, то использует строку подключения для создания нового подключения.
        /// </summary>
        /// <param name="extDbConnection">Внешнее подключение (длинная транзакция). В приоритете.</param>
        /// <param name="сonnectionString">Строка для подключения БД (короткая транзакция).</param>
        /// <param name="logger">Логгер для протоколирования вызовов</param>
        public SQLiteConnectionExt(SQLiteConnection extDbConnection, string сonnectionString, ILogger logger = null)
        {
            _logger = logger;
            _commands = new List<SQLiteCommand>();
            _readers = new List<SQLiteDataReader>();

            if (extDbConnection == null)
            {
                Connection = new SQLiteConnection(сonnectionString);
            }
            else
            {
                Connection = extDbConnection;
                IsExtConnection = true;
            }
            Connection.ConnectDatabase();
        }

        /// <summary>
        /// Создание команды. Все созданные команды закрываются при Dispose.
        /// </summary>
        /// <param name="cmdText">Текст команды (sql-запроса)</param>
        /// <param name="commandTimeOut">Максимальное время ожидания результатов выполнения. Если 0, используется время по-умолчанию.</param>
        public SQLiteCommand CreateCommand(string cmdText, int commandTimeOut)
        {
            var result = new SQLiteCommand(cmdText, Connection);
            _commands.Add(result);

            if (commandTimeOut > 0)
                result.CommandTimeout = commandTimeOut;

            return result;
        }

        /// <summary>
        /// Создание ридера. Все созданные ридеры автоматически закрываются при Dispose.
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <returns></returns>
        public SQLiteDataReader ExecuteReaderEx(SQLiteCommand dbCommand)
        {
            try
            {
                var result = dbCommand.ExecuteReader();
                _readers.Add(result);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Вызывает метод <see cref="IDisposable.Dispose"/> у <see cref="Connection">подключения</see>, если <see cref="IsExtConnection"/> = false (внешние подключения не завершаются).
        /// </summary>
        public void Dispose()
        {
            try
            {
                foreach (var dbCommand in _commands)
                {
                    if (dbCommand != null)
                        dbCommand.Dispose();
                }

                foreach (var dbReader in _readers)
                {
                    if (dbReader != null)
                        dbReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.Log("Ошибка при SQLiteCommand.Dispose()", ex);
            }

            if (!IsExtConnection)
                Connection.Dispose();
        }
    }

    /// <summary>
    /// Утилитарные методы для работы с БД SQLite
    /// </summary>
    public static class SQLiteUtils
    {
        /// <summary>
        /// ORA-00054 - данные заблокированы
        /// </summary>
        public const string BLOCKED_ERROR_CODE = "ORA-00054";

        /// <summary>
        /// Создание строки подключения
        /// </summary>
        /// <param name="databaseSource">База данных</param>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="connectionTimeout">Таймаут подключения</param>
        /// <param name="maxConnections">Максимальное количество подключений в пуле соединений</param>
        /// <param name="minConnections">Минимальное количество подключений в пуле соединений</param>
        /// <param name="poolIncr">Количество подключений для добавления в пул в случае отсутствия подключений свободных для использования</param>
        /// <param name="lifeTime">Время жизни подключения</param>
        /// <param name="connectionPoolHeatUp">Признак необходимости создание и открытия минимального количества подключений</param>
        public static string GetConnectionString(string databaseSource, string username, string password, int connectionTimeout, int maxConnections, int minConnections, int poolIncr, int lifeTime, bool connectionPoolHeatUp)
        {
            var connectionStringBuilder = new SQLiteConnectionStringBuilder()
            {
                DataSource = databaseSource
            };

            var connectionString = connectionStringBuilder.ConnectionString;

            if (connectionPoolHeatUp && minConnections > 1)
            {
                var dbConnections = new SQLiteConnection[minConnections];

                for (int i = 0; i < minConnections; i++)
                {
                    dbConnections[i] = new SQLiteConnection(connectionString);
                    dbConnections[i].Open();
                }

                for (int i = 0; i < minConnections; i++)
                {
                    dbConnections[i].Dispose();
                }
            }

            return connectionString;
        }

        /// <summary>
        /// Возвращает значение блоб-поля
        /// </summary>
        /// <param name="dbReader">DBReader со значениями</param>
        /// <param name="columnName">Имя колонки</param>
        public static byte[] GetBytes(this SQLiteDataReader dbReader, string columnName)
        {
            var value = dbReader[columnName];
            if (value == null || value is DBNull)
                return null;

            return value as byte[];
        }

        /// <summary>
        /// Возвращает значение поля в виде строки
        /// </summary>
        /// <param name="dbReader">DBReader со значениями</param>
        /// <param name="columnName">Имя колонки</param>
        public static string GetString(this SQLiteDataReader dbReader, string columnName)
        {
            return dbReader[columnName] as string;
        }

        /// <summary>
        /// Возвращает значение поля в виде целого числа
        /// </summary>
        /// <param name="dbReader">DBReader со значениями</param>
        /// <param name="columnName">Имя колонки</param>
        public static int GetInt(this SQLiteDataReader dbReader, string columnName)
        {
            var value = dbReader[columnName];
            if (value == null || value is DBNull)
                return 0;

            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Возвращает значение поля в виде decimal или -1, в случае null
        /// </summary>
        /// <param name="dbReader">DBReader со значениями</param>
        /// <param name="columnName">Имя колонки</param>
        public static decimal GetDecimalOrMinusOne(this SQLiteDataReader dbReader, string columnName)
        {
            var value = dbReader[columnName];
            if (value == null || value is DBNull)
                return Decimal.MinusOne;

            return Convert.ToDecimal(value);
        }

        /// <summary>
        /// Возвращает значение поля в виде decimal или -1
        /// </summary>
        /// <param name="dbReader">DBReader со значениями</param>
        /// <param name="columnName">Имя колонки</param>
        public static decimal GetDecimal(this SQLiteDataReader dbReader, string columnName)
        {
            var value = dbReader[columnName];
            if (value == null || value is DBNull)
                return Decimal.Zero;

            return Convert.ToDecimal(value);
        }

        /// <summary>
        /// Возвращает значение поля в виде даты или минимального значения, если значение отсуствует
        /// </summary>
        /// <param name="dbReader">DBReader со значениями</param>
        /// <param name="columnName">Имя колонки</param>
        public static DateTime GetDateTimeOrMin(this SQLiteDataReader dbReader, string columnName)
        {
            return GetDateTime(dbReader, columnName);
        }

        /// <summary>
        /// Возвращает значение поля в виде даты или максимального значения, если значение отсуствует
        /// </summary>
        /// <param name="dbReader">DBReader со значениями</param>
        /// <param name="columnName">Имя колонки</param>
        public static DateTime GetDateTimeOrMax(this SQLiteDataReader dbReader, string columnName)
        {
            var result = GetDateTime(dbReader, columnName);
            if (result == DateTime.MinValue)
                result = DateTime.MaxValue;

            return result;
        }

        /// <summary>
        /// Возвращает значение поля в виде даты
        /// </summary>
        /// <param name="dbReader">DBReader со значениями</param>
        /// <param name="columnName">Имя колонки</param>
        public static DateTime GetDateTime(this SQLiteDataReader dbReader, string columnName)
        {
            var value = dbReader[columnName];
            var result = value is DBNull ? DateTime.MinValue : Convert.ToDateTime(value);
            return result;
        }

        /// <summary>
        /// Открытие соединения к базе данных
        /// </summary>
        public static void ConnectDatabase(this IDbConnection dbConnection, ILogger logger = null)
        {
            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
            }
            catch (SQLiteException ex)
            {
                var connectionStringBuilder = new StringBuilder();

                if (!String.IsNullOrEmpty(dbConnection.ConnectionString))
                {
                    string[] parts = dbConnection.ConnectionString.Split(';');
                    foreach (string part in parts)
                    {
                        if (!part.StartsWith("PASSWORD"))
                            connectionStringBuilder.Append(part + ";");
                    }
                }

                throw new Exception(String.Format("Ошибка подключения к БД. Строка подключения: {0}", connectionStringBuilder.ToString()), ex);
            }
        }

        /// <summary>
        /// Попытка подключения к БД. Попытки повторяются пока статус соединения не станет Open
        /// </summary>
        /// <param name="dbConnection">Соединение для открытия</param>
        /// <param name="tryCount">Количество попыток (отрицательное число для бесконечных попыток)</param>
        /// <param name="retryTime">Время (в мс.) между попытками</param>
        /// <param name="onFail">Действие, которое вызывается в случае если все попытки установить соединение были безуспешны</param>
        public static void Reconnect(this IDbConnection dbConnection, int tryCount = -1, int retryTime = 5000, Action onFail = null)
        {
            var counter = 0;
            while (dbConnection.State != ConnectionState.Open || counter != tryCount)
            {
                try
                {
                    dbConnection.Open();
                }
                catch
                {
                    counter++;
                    Thread.Sleep(retryTime);
                }
            }

            if (onFail != null)
                onFail();
        }

        /// <summary>
        /// Выполнение ExecuteNonQuery обернутый в try\catch
        /// </summary>
        public static int ExecuteNonQueryEx(this IDbCommand dbCommand, ILogger logger = null)
        {
            try
            {
                var result = dbCommand.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                throw new SQLiteSQLException(ex.Message, GetFullSQL(dbCommand), ex);
            }
        }

        /// <summary>
        /// Выполнение ExecuteReader обернутый в try\catch
        /// </summary>
        public static IDataReader ExecuteReaderEx(this IDbCommand dbCommand, ILogger logger = null)
        {
            try
            {
                var result = dbCommand.ExecuteReader();
                return result;
            }
            catch (Exception ex)
            {
                throw new SQLiteSQLException(ex.Message, GetFullSQL(dbCommand), ex);
            }
        }

        /// <summary>
        /// Выполнение ExecuteReader обернутый в try\catch
        /// </summary>
        public static SQLiteDataReader ExecuteReaderEx(this SQLiteCommand dbCommand, ILogger logger = null)
        {
            try
            {
                var result = dbCommand.ExecuteReader();
                return result;
            }
            catch (Exception ex)
            {
                throw new SQLiteSQLException(ex.Message, GetFullSQL(dbCommand), ex);
            }
        }

        /// <summary>
        /// Выполнение ExecuteScalar обернутый в try\catch
        /// </summary>
        public static object ExecuteScalarEx(this IDbCommand dbCommand, ILogger logger = null)
        {
            try
            {
                var result = dbCommand.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                throw new SQLiteSQLException(ex.Message, GetFullSQL(dbCommand), ex);
            }
        }

        /// <summary>
        /// Возвращает true если объект null или DBNull
        /// </summary>
        public static bool IsDBNull(this object obj)
        {
            return obj == null || (obj is DBNull);
        }

        /// <summary>
        /// Добавляет параметр к команде. Если комманда - SQLiteCommand, то автоматически включается параметр BindByName, для правильной работы параметров.
        /// </summary>
        /// <param name="dbCommand">Команда</param>
        /// <param name="name">Наименование параметра</param>
        /// <param name="value">Значение</param>
        public static IDbDataParameter AddParameter(this IDbCommand dbCommand, string name, string value)
        {
            var parameter = AddParameter(dbCommand, name, value, DbType.String);
            return parameter;
        }

        /// <summary>
        /// Добавляет параметр к команде. Если комманда - SQLiteCommand, то автоматически включается параметр BindByName, для правильной работы параметров.
        /// </summary>
        /// <param name="dbCommand">Команда</param>
        /// <param name="name">Наименование параметра</param>
        /// <param name="value">Значение</param>
        public static IDbDataParameter AddParameter(this IDbCommand dbCommand, string name, bool value)
        {
            var parameter = AddParameter(dbCommand, name, value, DbType.Boolean);
            return parameter;
        }

        /// <summary>
        /// Добавляет параметр к команде. Если комманда - SQLiteCommand, то автоматически включается параметр BindByName, для правильной работы параметров.
        /// </summary>
        /// <param name="dbCommand">Команда</param>
        /// <param name="name">Наименование параметра</param>
        /// <param name="value">Значение</param>
        public static IDbDataParameter AddParameter(this IDbCommand dbCommand, string name, decimal value)
        {
            var parameter = AddParameter(dbCommand, name, value, DbType.Decimal);
            return parameter;
        }

        /// <summary>
        /// Добавляет параметр к команде. Если комманда - SQLiteCommand, то автоматически включается параметр BindByName, для правильной работы параметров.
        /// </summary>
        /// <param name="dbCommand">Команда</param>
        /// <param name="name">Наименование параметра</param>
        /// <param name="value">Значение</param>
        public static IDbDataParameter AddParameter(this IDbCommand dbCommand, string name, double value)
        {
            var parameter = AddParameter(dbCommand, name, value, DbType.Double);
            return parameter;
        }

        /// <summary>
        /// Добавляет параметр к команде. Если комманда - SQLiteCommand, то автоматически включается параметр BindByName, для правильной работы параметров.
        /// </summary>
        /// <param name="dbCommand">Команда</param>
        /// <param name="name">Наименование параметра</param>
        /// <param name="value">Значение</param>
        public static IDbDataParameter AddParameter(this IDbCommand dbCommand, string name, int value)
        {
            var parameter = AddParameter(dbCommand, name, value, DbType.Int32);
            return parameter;
        }

        /// <summary>
        /// Добавляет параметр к команде. Если комманда - SQLiteCommand, то автоматически включается параметр BindByName, для правильной работы параметров.
        /// </summary>
        /// <param name="dbCommand">Команда</param>
        /// <param name="name">Наименование параметра</param>
        /// <param name="value">Значение</param>
        /// <param name="dbType">Тип значения</param>
        public static IDbDataParameter AddParameter(this IDbCommand dbCommand, string name, object value, DbType dbType)
        {
            if (dbCommand == null)
                throw new ArgumentNullException("dbCommand");

            var SQLiteCommand = dbCommand as SQLiteCommand;
            //if (SQLiteCommand != null)
            //    SQLiteCommand.BindByName = true;

            var parameter = dbCommand.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = dbType;
            parameter.Value = value;

            dbCommand.Parameters.Add(parameter);

            return parameter;
        }

        /// <summary>
        /// Возвращает текст sql-команды
        /// </summary>
        public static string GetFullSQL(IDbCommand dbCommand)
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append(dbCommand.CommandText);

            var oraParams = dbCommand.Parameters as SQLiteParameterCollection;
            if (oraParams == null)
                return resultBuilder.ToString();

            foreach (SQLiteParameter cmdParam in dbCommand.Parameters)
            {
                var paramValue = cmdParam.Value == null ? "NULL" : cmdParam.Value.ToString();

                if (cmdParam.DbType == DbType.String)
                    paramValue = String.Concat("'", paramValue, "'");

                resultBuilder.Replace(":" + cmdParam.ParameterName, paramValue);
            }

            var result = resultBuilder.ToString();
            result = Regex.Replace(result, @"\s+", " ");

            return result;
        }
    }
}
