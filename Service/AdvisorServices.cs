﻿using Auctus.DomainObjects.Advisor;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Service
{
    public class AdvisorServices : BaseServices
    {
        public AdvisorServices(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public Advisor CreateAdvisor(string email, string name, string description, int period, double price)
        {
            return AdvisorBusiness.Create(email, name, description, period, price);
        }

        public AdvisorDetail UpdateAdvisor(string email, int advisorId, string description, int period, double price, bool enabled)
        {
            return AdvisorDetailBusiness.Create(email, advisorId, description, period, price, enabled);
        }

        public Model.Advisor ListAdvisorDetails(string email, int advisorId)
        {
            return AdvisorBusiness.ListDetails(email, advisorId);
        }

        public Buy Buy(string email, int advisorId)
        {
            return BuyBusiness.Create(email, advisorId);
        }

        public IEnumerable<Model.Advisor> ListAdvisors(string email)
        {
            return AdvisorBusiness.ListAvailable(email);
        }
    }
}