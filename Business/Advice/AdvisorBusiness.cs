using Auctus.DataAccess.Advice;
using Auctus.DataAccess.Core;
using Auctus.DomainObjects.Advice;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Business.Advice
{
    public class AdvisorBusiness : BaseBusiness<Advisor, AdvisorData>
    {
        public int DefaultAdvisorId { get { return 1; } }

        public AdvisorBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public Advisor Create(string email, string name, string description, int period, double price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
            if (name.Length > 50)
                throw new ArgumentException("Name is too long.");

            var user = UserBusiness.GetValidUser(email);
            var advisor = new Advisor();
            using (var transaction = new TransactionalDapperCommand())
            {
                advisor.Name = name;
                transaction.Insert(advisor);
                var detail = AdvisorDetailBusiness.SetNew(advisor.Id, description, period, price, false);
                transaction.Insert(detail);
                advisor.Detail = detail;
                transaction.Commit();
            }
            return advisor;
        }

        public Advisor GetWithOwner(int id, string email)
        {
            return Data.GetWithOwner(id, email);
        }

        public Advisor GetWithDetail(int id)
        {
            var advisor = Data.GetWithDetail(id);
            if (advisor == null)
                throw new ArgumentException("Advisor cannot be found.");
            return advisor;
        }

        public IEnumerable<Model.Advisor> ListAvailable(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            var purchases = Task.Factory.StartNew(() => BuyBusiness.ListPurchases(user.Id));
            var advisors = Data.ListAvailable();
            var advisorsQty = Task.Factory.StartNew(() => BuyBusiness.ListAdvisorsPurchases(advisors.Select(c => c.Id)));
            var portfolios = Task.Factory.StartNew(() => PortfolioBusiness.List(advisors.Select(c => c.Id)));

            Task.WaitAll(purchases, advisorsQty, portfolios);
            
            return advisors.Select(c => new Model.Advisor()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Detail.Description,
                Period = c.Detail.Period,
                Price = c.Detail.Price,
                Purchased = purchases.Result.Any(x => x.AdvisorId == c.Id),
                PurchaseQuantity = advisorsQty.Result[c.Id],
                RiskProjection = portfolios.Result[c.Id].Select(x => new KeyValuePair<int, double>(x.Risk, x.Projection.ProjectionValue)).ToDictionary(x => x.Key, x => x.Value)
            });
        }
    }
}
