using Auctus.Util.DapperAttributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Asset
{
    public class AssetValue : MongoDomainObject
    {
        public int AssetId { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
