using Auctus.DataAccess.Account;
using Auctus.DataAccess.Core;
using Auctus.DomainObjects.Account;
using Auctus.Model;
using Auctus.Util;
using Auctus.Util.NotShared;
using Dapper;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Auctus.Business.Account
{
    public class UserBusiness : BaseBusiness<User, UserData>
    {
        public UserBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public Login Login(string address, string emailOrUsername, string password)
        {
            BaseEmailOrUsernameValidation(emailOrUsername);
            BasePasswordValidation(password);
            address = WalletBusiness.GetAddressFormatted(address);

            var user = Data.GetByEmailOrUsername(emailOrUsername);
            if(user == null)
                throw new ArgumentException("Email or username is invalid.");
            else if (user.Wallet.Address?.ToUpper() != address?.ToUpper())
                throw new ArgumentException("Wallet is invalid.");
            else if (user.Password != Security.Hash(password))
                throw new ArgumentException("Password is invalid.");
            
            var result = new Model.Login()
            {
                Address = user.Wallet.Address,
                Email = user.Email,
                Username = user.Username,
                PendingConfirmation = !user.ConfirmationDate.HasValue
            };
            if (!result.PendingConfirmation)
            {
                MemoryCache.Set<User>(user.Email, user);
                var purchases = Task.Factory.StartNew(() => BuyBusiness.ListPurchases(user.Id));
                var advisor = Task.Factory.StartNew(() => AdvisorBusiness.SimpleGetByOwner(user.Id));

                Task.WaitAll(purchases, advisor);

                result.HumanAdvisorId = advisor.Result?.Id;
                result.HasInvestment = purchases.Result.Count > 0;
            }

            return result;
        }

        public async Task<Login> SimpleRegister(string address, string username, string email, string password)
        {
            User user;
            using (var transaction = new TransactionalDapperCommand())
            {
                user = SetBaseUserCreation(username, email, password);
                transaction.Insert(user);
                var wallet = SetWalletCreation(user.Id, address);
                user.Wallet = wallet;
                transaction.Insert(wallet);
                var deposit = CashFlowBusiness.SetNew(user.Id, CashFlowBusiness.InitialDeposit);
                transaction.Insert(deposit);
                transaction.Commit();
            }

            await SendEmailConfirmation(user.Email, user.ConfirmationCode);

            return new Model.Login()
            {
                Address = user.Wallet.Address,
                Email = user.Email,
                Username = user.Username,
                PendingConfirmation = true
            };
        }

        public async Task ResendEmailConfirmation(string email)
        {
            var user = GetValidUser(email);
            user.ConfirmationCode = Guid.NewGuid().ToString();
            Data.Update(user);

            await SendEmailConfirmation(email, user.ConfirmationCode);
        }
        
        public Login ConfirmEmail(string code)
        {
            var user = Data.GetByConfirmationCode(code);
            if (user == null)
                throw new ArgumentException("Invalid confirmation code.");

            user.ConfirmationDate = DateTime.UtcNow;
            Data.Update(user);

            return new Login()
            {
                Address = user.Wallet.Address,
                Email = user.Email,
                Username = user.Username,
                PendingConfirmation = !user.ConfirmationDate.HasValue
            };
        }

        public bool CheckTelegramParticipation(string phoneNumber)
        {
            return Telegram.TelegramValidator.CheckPhoneIsMember(phoneNumber);
        }

        public bool IsValidEmailToRegister(string email)
        {
            return Email.IsValidEmail(email) && Data.GetByEmail(email) == null;
        }

        public bool IsValidUsernameToRegister(string username)
        {
            return Data.GetByUsername(username) == null;
        }

        public bool IsValidAddressToRegister(string address)
        {
            return WalletBusiness.IsValidAddress(address) && Data.GetByWalletAddress(address) == null;
        }

        public void ChangePassword(string email, string currentPassword, string newPassword)
        {
            var user = GetValidUser(email);
            if (user.Password != Security.Hash(currentPassword))
                throw new ArgumentException("Current password is incorrect.");
            UpdatePassword(user, newPassword);
        }

        public void UpdatePassword(User user, string password)
        {
            BasePasswordValidation(password);
            PasswordValidation(password);

            user.Password = Security.Hash(password);
            Data.Update(user);
        }
        
        public User GetValidUser(string email, string address = null)
        {
            BaseEmailValidation(email);
            var cacheKey = email.ToLower().Trim();
            var user = MemoryCache.Get<User>(cacheKey);
            if (user == null)
            {
                EmailValidation(email);
                user = Data.Get(email);
                if (user == null)
                    throw new ArgumentException("User cannot be found.");

                ValidateUserAddress(user, address);

                if (user.ConfirmationDate.HasValue)
                    MemoryCache.Set<User>(cacheKey, user);
                return user;
            }
            else
            {
                ValidateUserAddress(user, address);
                return user;
            }
        }

        private void ValidateUserAddress(User user, string address)
        {
            if (address != null)
            {
                address = WalletBusiness.GetAddressFormatted(address);
                if (user.Wallet.Address != address)
                    throw new ArgumentException("Invalid user wallet.");
            }
        }
        
        public User Get(Guid guid)
        {
            return Data.Get(guid);
        }

        public User Get(int id)
        {
            return Data.Get(id);
        }

        public User GetWithWallet(int id)
        {
            return Data.GetWithWallet(id);
        }

        public User GetOwner(int buyId)
        {
            return Data.GetOwner(buyId);
        }

        private async Task SendEmailConfirmation(string email, string code)
        {
            await Email.SendAsync(
                new string[] { email },
                "Verify your email address - Auctus Alpha",
                string.Format(@"Hello,
<br/><br/>
To activate your account please verify your email address and complete your registration <a href='{0}/confirm?c={1}' target='_blank'>click here</a>.
<br/><br/>
If you didn’t ask to verify this address, you can ignore this email.
<br/><br/>
Thanks,
<br/>
Auctus Team", Config.WEB_URL, code));
        }

        private User SetBaseUserCreation(string username, string email, string password)
        {
            BaseEmailValidation(email);
            EmailValidation(email);
            BasePasswordValidation(password);
            PasswordValidation(password);

            User user = Data.Get(email);
            if (user != null)
                throw new ArgumentException("Email already registered.");

            user = Data.GetByUsername(username);
            if (user != null)
                throw new ArgumentException("Username already registered.");

            user = new User();
            user.Email = email.ToLower().Trim();
            user.Username = username.Trim();
            user.CreationDate = DateTime.UtcNow;
            user.Password = Security.Hash(password);
            user.ConfirmationCode = Guid.NewGuid().ToString();
            return user;
        }

        private Wallet SetWalletCreation(int userId, string address)
        {
            address = WalletBusiness.GetAddressFormatted(address);

            User user = Data.GetByWalletAddress(address);
            if (user != null)
                throw new ArgumentException("Address already registered.");

            Wallet wallet = new Wallet();
            wallet.UserId = userId;
            wallet.Address = address;
            return wallet;
        }

        private void BaseEmailOrUsernameValidation(string emailOrUsername)
        {
            if (string.IsNullOrEmpty(emailOrUsername))
                throw new ArgumentException("Email or username must be filled.");
        }


        private void BaseEmailValidation(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email must be filled.");
        }

        private void EmailValidation(string email)
        {
            if (!Email.IsValidEmail(email))
                throw new ArgumentException("Email informed is invalid.");
        }

        private void BasePasswordValidation(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password must be filled.");
        }

        private void PasswordValidation(string password)
        {
            if (password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters.");
            if (password.Length > 100)
                throw new ArgumentException("Password cannot have more than 100 characters.");
            if (password.Contains(" "))
                throw new ArgumentException("Password cannot have spaces.");
        }

        public decimal GetCurrentInvestedAmountValue(int userId) {
            var user = Get(userId);
            List<Model.Portfolio> portfolios = PortfolioBusiness.ListPurchased(user.Email);
            decimal investedAmount = 0;
            foreach (Model.Portfolio portfolio in portfolios) {
                List<DomainObjects.Portfolio.PortfolioHistory> portfolioHistories = PortfolioHistoryBusiness.ListHistory(portfolio.Id);
                int days = (int)Math.Ceiling(DateTime.UtcNow.Subtract(portfolio.EffectiveTransactionDate ?? DateTime.UtcNow).TotalDays) - 1;
                var historyResult = days > 0 ? PortfolioHistoryBusiness.GetHistoryResult(days, portfolioHistories) : null;
                investedAmount += historyResult != null ? portfolio.Invested * ((decimal)historyResult.Value / 100L + 1) : 0;
            }

            return investedAmount;
        }

        public List<User> ListUsersByPerformance()
        {
            var allUsers = Data.ListAll();
            List<Task<UserBalance>> userBalanceList = new List<Task<UserBalance>>();
            foreach (User user in allUsers) {
                userBalanceList.Add(Task.Factory.StartNew(() => UserBusiness.GetUserBalance(user)));
            }
            Task.WaitAll(userBalanceList.ToArray());

            List<int> userIdsOrderedByPerformance = userBalanceList.Select(ubt => ubt.Result)
                            .OrderByDescending(ub => ub.InvestedAmount)
                            .Select(ub => ub.UserId)
                            .ToList();

            List<User> usersByPerformance = allUsers.OrderBy(u => userIdsOrderedByPerformance.IndexOf(u.Id)).ToList();
            return usersByPerformance;
        }

        public decimal GetAvailableToInvest(int userId) {
            return CashFlowBusiness.GetUserBalance(userId);
        }

        public User GetByEmail(string email) {
            return Data.GetByEmail(email);
        }

        public UserBalance GetUserBalance(string email) {
            var user = UserBusiness.GetByEmail(email);
            return GetUserBalance(user);
        }

        public UserBalance GetUserBalance(User user)
        {
            string cacheKey = string.Format("UserBalance{0}", user.Id);
            var userBalance = MemoryCache.Get<UserBalance>(cacheKey);
            if (userBalance != null) {
                return userBalance;
            }

            var investedAmountValue = Task.Factory.StartNew(() => UserBusiness.GetCurrentInvestedAmountValue(user.Id));
            var availableToInvest = Task.Factory.StartNew(() => UserBusiness.GetAvailableToInvest(user.Id));

            Task.WaitAll(investedAmountValue, availableToInvest);

            userBalance = new UserBalance()
            {
                UserId = user.Id,
                InvestedAmount = investedAmountValue != null ? investedAmountValue.Result : 0,
                AvailableAmount = availableToInvest != null ? availableToInvest.Result : 0
            };

            MemoryCache.Set<UserBalance>(cacheKey, userBalance);

            return userBalance;
        }

    }
}
