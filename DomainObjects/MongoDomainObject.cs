using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects
{
    public abstract class MongoDomainObject
    {
        public ObjectId _id { get; set; }
    }
}
