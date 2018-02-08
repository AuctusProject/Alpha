using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Model.Account
{
    public class LoginRequest
    {
        public string Address { get; set; }
        public string EmailOrUsername { get; set; }
        public string Password { get; set; }
    }
}
