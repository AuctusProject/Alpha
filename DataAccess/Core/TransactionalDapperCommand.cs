using Auctus.Util.NotShared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using Dapper;

namespace Auctus.DataAccess.Core
{
    public class TransactionalDapperCommand : DapperRepositoryBase, IDisposable
    {
        public override string TableName => throw new NotImplementedException();

        private SqlConnection Connection { get; }
        private SqlTransaction Transaction { get; }

        public TransactionalDapperCommand(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) : base(Config.CONNECTION_STRING)
        {
            Connection = GetOpenConnection();
            Transaction = Connection.BeginTransaction(isolationLevel);
        }

        public new void Delete<T>(T obj, string tableName = null)
        {
            base.Delete<T>(obj, GetTableName<T>(tableName));
        }

        public new void Update<T>(T obj, string tableName = null)
        {
            base.Update<T>(obj, GetTableName<T>(tableName));
        }

        public new void Insert<T>(T obj, string tableName = null)
        {
            base.Insert<T>(obj, GetTableName<T>(tableName));
        }

        private string GetTableName<T>(string tableName)
        {
            return string.Format("{0}", tableName ?? typeof(T).Name);
        }

        public void Dispose()
        {
            Transaction.Dispose();
            Connection.Dispose();
            Connection.Close();
        }

        public void Commit()
        {
            try
            {
                Transaction.Commit();
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
        }

        public int Execute(string sql, dynamic param = null, int commandTimeout = _defaultTimeout)
        {
            return this.Execute(sql, param, commandTimeout, Transaction);
        }

        public int ExecuteScalar<T>(string sql, dynamic param = null, int commandTimeout = _defaultTimeout)
        {
            try
            {
                return SqlMapper.ExecuteScalar<T>(Connection, sql, param, Transaction, commandTimeout, CommandType.Text);
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
        }

        protected override int Execute(string sql, dynamic param = null, int commandTimeout = _defaultTimeout, IDbTransaction transaction = null)
        {
            try
            {
                return SqlMapper.Execute(Connection, sql, param, Transaction, commandTimeout, CommandType.Text);
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
        }

        protected override int ExecuteReturningIdentity(string sql, dynamic param = null, int commandTimeout = _defaultTimeout, IDbTransaction transaction = null)
        {
            try
            {
                return SqlMapper.ExecuteScalar<int>(Connection, ParseIdentityCommandQuery(sql), param, Transaction, commandTimeout, CommandType.Text);
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
        }
    }
}
