using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net;

namespace Backend.BuisnessLayer
{
    internal class UserBL
    {
        private string email;
        private string password;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserBL(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }
        public bool Login(string pass)
        {
            return pass == this.password;
        }
        public string Email
        {
            get => this.email;
            set
            {
                string pattern = @"^[^@\s]+@[^@\s]+(\.[^@\s]+)+$";
                if (Regex.IsMatch(value, pattern))
                {
                    this.email = value;
                }
                else
                {
                    Log.Error($"The email: {value} is invalid");
                    throw new ArgumentException($"The email: {value} is invalid");
                }
            }
        }
        private string Password
        {
            set
            {
                if (validatePass(value))
                {
                    this.password = value;
                }
                else
                {
                    Log.Error("Invalid password");
                    throw new ArgumentException("Invalid password - it must contain at least one uppercase letter, one lowercase letter, one digit, and be 6-20 characters long.");
                }
            }
        }
        private bool validatePass(string pass)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,20}$";
            return Regex.IsMatch(pass, pattern);
        }

    }
}
