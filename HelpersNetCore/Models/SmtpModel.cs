using System;
using System.Collections.Generic;
using System.Text;

namespace HitHelpersNetCore.Models
{
    public class SmtpModel
    {
        public string smtp { get; set; }
        public string port { get; set; }
        public string ssl { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string sender { get; set; }
    }
}
