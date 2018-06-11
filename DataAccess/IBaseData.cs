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
    public interface IBaseData<T>
    {
        IEnumerable<T> SelectByObject(T criteria);
        IEnumerable<T> SelectAll();
        void Insert(T obj);
        void Update(T obj);
        void Delete(T obj);
    }
}
