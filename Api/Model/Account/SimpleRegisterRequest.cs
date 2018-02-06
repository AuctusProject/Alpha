using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Model.Account
{
    public class SimpleRegisterRequest
    {
        public string Address { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
