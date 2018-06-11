using Auctus.DataAccess.Core;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess
{
    public abstract class BaseMongo<T> : MongoDBRepository, IBaseData<T>
    {
        public abstract string CollectionName { get; }

        public IMongoCollection<T> Collection
        {
           get
            {
                var database = MongoDBRepository.GetDataBase();
                var collection = database.GetCollection<T>(CollectionName);
                return collection;
            }
        }

        void IBaseData<T>.Delete(T obj)
        {
            throw new NotImplementedException();
        }

        void IBaseData<T>.Insert(T obj)
        {
            throw new NotImplementedException();
        }

        IEnumerable<T> IBaseData<T>.SelectAll()
        {
            throw new NotImplementedException();
        }

        IEnumerable<T> IBaseData<T>.SelectByObject(T criteria)
        {
            throw new NotImplementedException();
        }

        void IBaseData<T>.Update(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
