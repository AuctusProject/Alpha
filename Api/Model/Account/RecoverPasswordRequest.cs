using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Model.Account
{
    public class RecoverPasswordRequest
    {
        public string Code { get; set; }
        public string Password { get; set; }
    }
}
