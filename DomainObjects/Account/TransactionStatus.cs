using Auctus.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Account
{
    public class TransactionStatus : IntType
    {
        public static readonly TransactionStatus Pending = new TransactionStatus(0);
        public static readonly TransactionStatus Success = new TransactionStatus(1);
        public static readonly TransactionStatus Error = new TransactionStatus(2);
        public static readonly TransactionStatus Cancel = new TransactionStatus(3);
        public static readonly TransactionStatus Lost = new TransactionStatus(4);
        public static readonly TransactionStatus Fraud = new TransactionStatus(5);

        private TransactionStatus(int status) : base(status)
        { }

        public static TransactionStatus Get(int status)
        {
            switch (status)
            {
                case 0:
                    return Pending;
                case 1:
                    return Success;
                case 2:
                    return Error;
                case 3:
                    return Cancel;
                case 4:
                    return Lost;
                case 5:
                    return Fraud;
                default:
                    throw new ArgumentException("Invalid status.");
            }
        }
    }
}
