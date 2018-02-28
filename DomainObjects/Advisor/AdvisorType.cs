using Auctus.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Advisor
{
    public class AdvisorType : IntType
    {
        public static readonly AdvisorType Human = new AdvisorType(0);
        public static readonly AdvisorType Robo = new AdvisorType(1);

        private AdvisorType(int type) : base(type)
        { }

        public static AdvisorType Get(int type)
        {
            switch (type)
            {
                case 0:
                    return Human;
                case 1:
                    return Robo;
                default:
                    throw new ArgumentException("Invalid type.");
            }
        }
    }
}
