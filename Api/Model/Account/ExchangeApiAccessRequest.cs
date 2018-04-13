using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Model.Account
{
    public class ExchangeApiAccessRequest
    {
        public int ExchangeId { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecretKey { get; set; }
    }
}
