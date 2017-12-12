using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Model.Account
{
    public class SimpleRegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
