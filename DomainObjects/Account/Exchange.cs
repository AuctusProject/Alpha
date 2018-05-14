using Auctus.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Account
{
    public class Exchange : IntType
    {
        public static readonly Exchange Bitfinex = new Exchange(1);
        public static readonly Exchange Binance = new Exchange(2);
        public static readonly Exchange Bittrex = new Exchange(3);
        public static readonly Exchange Poloniex = new Exchange(4);

        private Exchange(int exchangeId) : base(exchangeId)
        { }

        public static Exchange Get(int exchangeId)
        {
            switch (exchangeId)
            {
                case 1:
                    return Bitfinex;
                case 2:
                    return Binance;
                case 3:
                    return Bittrex;
                case 4:
                    return Poloniex;
                default:
                    throw new ArgumentException("Invalid exchange.");
            }
        }

        public static String GetName(int status)
        {
            switch (status)
            {
                case 1:
                    return "Bitfinex";
                case 2:
                    return "Binance";
                default:
                    throw new ArgumentException("Invalid exchange.");
            }
        }
    }
}
