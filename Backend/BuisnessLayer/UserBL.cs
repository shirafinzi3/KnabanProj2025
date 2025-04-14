using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Backend.BuisnessLayer
{
    internal class UserBL
    {
        private readonly string email;
        private readonly string password;
        public UserBL(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
        public bool Login(string pass)
        {
            return pass == this.password;
        }
        public string Email
        {
            get;
        }
       
    }
}
