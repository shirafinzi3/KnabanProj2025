using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.ServiceLayer
{
    public class UserSL
    {
        public string email { get; set; }
        /// <summary>
        /// This constructor get an email and creates a UserSL
        /// </summary>
        /// <param name="email">The email of the user</param>
        public UserSL(string email)
        { 
            this.email = email;
        }


    }
}
