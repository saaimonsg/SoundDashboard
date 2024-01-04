using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ArlongStreambot.audioplayer.soundboard;
using ArlongStreambot.core.resource.sqlite;
using Dapper;
using refleciton_tool.database;

namespace ArlongStreambot.core
{
    public abstract class Repository<T, ID> : IRepository<T, ID>
    {
        public abstract string DbName { get; }
        public abstract string TableName { get; }
        protected SqliteDatabaseConnection db;

        protected Repository()
        {
            this.db = new SqliteDatabaseConnection(DbName);
        }

        public T save(T t)
        {
            try
            {
                db.Connection.Execute(PrepareTableStatement(DbMethodTypes.INSERT, TableName, t));
                return t;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return default;
        }

        public T update(T t)
        {
            throw new System.NotImplementedException();
        }

        public T remove(ID id)
        {
            throw new System.NotImplementedException();
        }

        public T FindByID(ID id)
        {
            return db.Connection.Query<T>($"SELECT * FROM {TableName} WHERE Id = {id}").SingleOrDefault();
        }

        public T FindFirst()
        {
            return db.Connection.QuerySingle<T>($"SELECT * FROM {TableName}");
        }

        public List<T> FetchAll()
        {
            IEnumerable<T> soundClips = db.Connection.Query<T>("SELECT * FROM ap_soundclip");
            return new List<T>(soundClips);
        }

        /**
         * Statement for table management
         * Method: Select Insert Update Delete
         */
        private String PrepareTableStatement(DbMethodTypes method, String tableName, T t, long id = default)
        {
            //"SELECT * FROM {tableName}"
            StringBuilder stringBuilder = new StringBuilder();
            if (method == DbMethodTypes.INSERT)
            {
                stringBuilder.Append($"INSERT INTO {tableName} (");

                String values = "";
                String columns = "";

                PropertyInfo[] propertyInfos = t.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                for (var i = 0; i < propertyInfos.Length; i++)
                {
                    var fieldInfo = propertyInfos[i];
                    if (fieldInfo.Name != "id")
                    {
                        columns += $"{fieldInfo.Name}";
                        var data = fieldInfo.GetValue(t);
                        if (fieldInfo.PropertyType.Name == "String")
                        {
                            values += $"'{data}'";
                        }
                        else if (fieldInfo.PropertyType.Name == "Boolean")
                        {
                            if (data is true)
                            {
                                values += "1";
                            }
                            else
                            {
                                values += "0";
                            }
                        }
                        else
                        {
                            values += $"{data}";
                        }
                    }

                    if (i < propertyInfos.Length-1 && i >= 1)
                    {
                        values += ",";
                        columns += ",";
                    }
                }

                stringBuilder.Append(columns);
                stringBuilder.Append(") VALUES (");
                stringBuilder.Append(values);
                stringBuilder.Append(")");
            }

            return stringBuilder.ToString();
        }
    }
}