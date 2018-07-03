using Auctus.DataAccess.Portfolio;
using Auctus.DataAccess.Core;
using Auctus.DomainObjects.Advisor;
using Auctus.DomainObjects.Portfolio;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Auctus.DomainObjects.Account;

namespace Auctus.Business.Portfolio
{
    public class FollowBusiness : BaseBusiness<Follow, FollowData>
    {
        public FollowBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public Follow Create(string email, string address, int portfolioId)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address must be filled.");
            
            var user = UserBusiness.GetValidUser(email, address);
            if (user?.ConfirmationDate == null)
                throw new ArgumentException("User didn't confirm e-mail.");
            var portfolio = PortfolioBusiness.GetWithDetails(portfolioId);

            if (portfolio == null || !portfolio.Detail.Enabled || !portfolio.Advisor.Detail.Enabled)
                throw new ArgumentException("Invalid portfolio.");
            if (portfolio.Advisor.UserId == user.Id)
                throw new ArgumentException("User is the advisor owner.");
           
            var following = ListFollowing(user.Id);
            if (following.Any(c => c.PortfolioId == portfolio.Id))
                throw new ArgumentException("Portfolio already followed.");

            Follow follow = SetNew(portfolio.Id, portfolio.ProjectionId.Value, portfolio.Detail.Id, user.Id);
            Data.Insert(follow);
            follow.Portfolio = portfolio;
            follow.PortfolioDetail = portfolio.Detail;
            return follow;
        }
        
        public Follow SetNew(int portfolioId, int projectionId, int portfolioDetailId, int userId)
        {
            var follow = new Follow();
            follow.CreationDate = DateTime.UtcNow;
            follow.PortfolioId = portfolioId;
            follow.ProjectionId = projectionId;
            follow.PortfolioDetailId = portfolioDetailId;
            follow.UserId = userId;
            return follow;
        }

        public void Delete(string email, string address, int portfolioId)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address must be filled.");

            var user = UserBusiness.GetValidUser(email, address);
            if (user?.ConfirmationDate == null)
                throw new ArgumentException("User didn't confirm e-mail.");

            var follow = Get(user.Id, portfolioId);
            if (follow == null)
                throw new ArgumentException("Portfolio not followed.");

            Delete(follow);
        }
        public List<Follow> ListFollowing(int userId)
        {
            return Data.ListFollowing(userId);
        }

        public Follow Get(int id)
        {
            return Data.Get(id);
        }
        
        public Follow Get(int userId, int portfolioId)
        {
            return Data.Get(userId, portfolioId);
        }

        public List<Follow> ListUserAdvisorFollows(int userId, int advisorId)
        {
            return Data.ListUserAdvisorFollows(userId, advisorId);
        }
        
        public Dictionary<int, int> ListAdvisorsFollowers(IEnumerable<int> advisorIds)
        {
            return Data.ListAdvisorsFollowers(advisorIds);
        }

        public Dictionary<int, int> ListPortfoliosFollowers(IEnumerable<int> portfolioIds)
        {
            return Data.ListPortfoliosFollowers(portfolioIds);
        }

        public IEnumerable<User> GetUsersFollowersFromPortfolio(int portfolioId)
        {
            return Data.GetUsersFollowersFromPortfolio(portfolioId);
        }
    }
}
