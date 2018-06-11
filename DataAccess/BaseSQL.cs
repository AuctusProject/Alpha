using Auctus.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Auctus.DataAccess.Core;
using Auctus.Util.NotShared;

namespace Auctus.DataAccess
{
    public abstract class BaseSQL<T> : DapperRepositoryBase, IBaseData<T>
    {
        protected BaseSQL() : base(Config.CONNECTION_STRING)
        { }

        public IEnumerable<T> SelectByObject(T criteria)
        {
            return base.SelectByObject<T>(criteria);
        }

        public IEnumerable<T> SelectAll()
        {
            return base.SelectAll<T>();
        }

        public void Insert(T obj)
        {
            base.Insert<T>(obj);
        }

        public void Update(T obj)
        {
            base.Update(obj);
        }

        public void Delete(T obj)
        {
            base.Delete<T>(obj);
        }
    }
}
