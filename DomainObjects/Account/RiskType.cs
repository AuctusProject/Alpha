using Auctus.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Account
{
    public class RiskType : IntType
    {
        public static readonly RiskType VeryLow = new RiskType(1);
        public static readonly RiskType Low = new RiskType(2);
        public static readonly RiskType Medium = new RiskType(3);
        public static readonly RiskType High = new RiskType(4);
        public static readonly RiskType VeryHigh = new RiskType(5);
        
        private RiskType(int risk) : base (risk)
        { }

        public static RiskType Get(int risk)
        {
            switch (risk)
            {
                case 1:
                    return VeryLow;
                case 2:
                    return Low;
                case 3:
                    return Medium;
                case 4:
                    return High;
                case 5:
                    return VeryHigh;
                default:
                    throw new ArgumentException("Invalid risk.");
            }
        }

        public static RiskType Get(int goalRisk, int goalOptionRisk)
        {
            RiskType optionRisk = Get(goalOptionRisk);
            switch (goalRisk)
            {
                case 1:
                    if (optionRisk == High || optionRisk == VeryHigh)
                        return Low;
                    else
                        return VeryLow;
                case 2:
                    if (optionRisk == VeryLow)
                        return VeryLow;
                    else if (optionRisk == VeryHigh)
                        return Medium;
                    else
                        return Low;
                case 3:
                    if (optionRisk == VeryLow)
                        return Low;
                    else if (optionRisk == VeryHigh)
                        return High;
                    else
                        return Medium;
                case 4:
                    if (optionRisk == VeryLow)
                        return Medium;
                    else if (optionRisk == VeryHigh)
                        return VeryHigh;
                    else
                        return High;
                case 5:
                    if (optionRisk == Low || optionRisk == VeryLow)
                        return High;
                    else
                        return VeryHigh;
                default:
                    throw new ArgumentException("Invalid risk.");
            }
        }
    }
}
