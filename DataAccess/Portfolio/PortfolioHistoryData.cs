using Auctus.DomainObjects.Portfolio;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Auctus.DataAccess.Portfolio
{
    public class PortfolioHistoryData : BaseData<PortfolioHistory>
    {
        public override string TableName => "PortfolioHistory";

        private readonly string SQL_LIST_BY_ASSET_ID = @"SELECT TOP 1 ph.* 
                                                FROM 
                                                PortfolioHistory ph
                                                where ph.PortfolioId = @PortfolioId
                                                ORDER BY Date desc";

        public PortfolioHistory LastHistory(int portfolioId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("PortfolioId", portfolioId, DbType.Int32);
            return Query<PortfolioHistory>(SQL_LIST_BY_ASSET_ID, parameters).FirstOrDefault();
        }

        public List<PortfolioHistory> ListHistory(int portfolioId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("PortfolioId", portfolioId, DbType.Int32);
            return SelectByParameters<PortfolioHistory>(parameters, "Date").ToList();
        }
    }
}
