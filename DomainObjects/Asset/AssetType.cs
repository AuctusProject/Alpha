using Auctus.Util;
using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Asset
{
    public class AssetType : IntType
    {
        public static readonly AssetType Traditional = new AssetType(0);
        public static readonly AssetType Crypto = new AssetType(1);

        private AssetType(int type) : base(type)
        { }

        public static AssetType Get(int type)
        {
            switch (type)
            {
                case 0:
                    return Traditional;
                case 1:
                    return Crypto;
                default:
                    throw new ArgumentException("Invalid type.");
            }
        }
    }
}
