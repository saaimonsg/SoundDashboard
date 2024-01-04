using System;
using System.Data;
using System.Data.SQLite;
using Dapper;

namespace ArlongStreambot.core
{
    public class SqliteDatabaseConnection : IDisposable
    {
        public IDbConnection Connection { get; set; }

        public SqliteDatabaseConnection(String name)
        {
            Connection = new SQLiteConnection($"Data Source={name}.db");
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Close();
            Connection.Dispose();
        }
    }
}