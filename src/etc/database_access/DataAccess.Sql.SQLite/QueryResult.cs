using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Microsoft.Data.Sqlite;

namespace DataAccess.Sql.SQLite
{
    internal class QueryResult : DisposableObject, IQueryResult
    {
        private SqliteCommand _command;
        private SqliteDataReader p_dataReader;

        private SqliteDataReader DataReader
        {
            get
            {
                return p_dataReader;
            }
            set
            {
                p_dataReader = value;
            }
        }


        public QueryResult(SqliteCommand command, SqliteDataReader dataReader)
        {
            _command = command;
            DataReader = dataReader;
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public IEnumerator<IRow> GetEnumerator()
        {
            return new Enumerator() { Master = this };
        }


        protected override void OnDispose()
        {
            base.OnDispose();
            
            _command.DisposeAsync();
            _command = null;
            
            DataReader.DisposeAsync();
            DataReader = null;
            
        }


        private class Enumerator : DisposableObject, IEnumerator<IRow>, IRow
        {
            public QueryResult Master { get; set; }


            public IRow Current => this;
            object IEnumerator.Current => Current;


            public bool MoveNext() => Master.DataReader?.Read() ?? false;

            public void Reset() => throw new NotSupportedException();

            public int GetInt(int columnIndex) => Master.DataReader.GetInt32(columnIndex);

            public string GetString(int columnIndex) => Master.DataReader.GetString(columnIndex);

            public bool GetBool(int columnIndex) => Master.DataReader.GetBoolean(columnIndex);

            public bool IsNull(int columnIndex) => Master.DataReader.IsDBNull(columnIndex);

            protected override void OnDispose()
            {
                base.OnDispose();
                Master = null;
            }
        }

    }
}