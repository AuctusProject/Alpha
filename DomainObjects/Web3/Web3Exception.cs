using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Web3
{
    [Serializable]
    public class Web3Exception : Exception
    {
        public int Code { get; private set; }
        public Web3Exception(int code, string message) : base (message)
        {
            Code = code;
        }
    }
}
