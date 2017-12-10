using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Model.Account
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
