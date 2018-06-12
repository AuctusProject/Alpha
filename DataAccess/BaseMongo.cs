using Auctus.DataAccess.Core;
using Auctus.DomainObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task InsertOneAsync(MongoDomainObject document)
        {
            return base.InsertOneAsync(CollectionName, document);
        }

        public Task InsertManyAsync(IEnumerable<MongoDomainObject> documents)
        {
            return base.InsertManyAsync(CollectionName, documents);
        }

        #region IBaseData implementation
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
        #endregion
    }
}
