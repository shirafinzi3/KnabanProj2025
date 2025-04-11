using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.BuisnessLayer
{
    internal class UserFacade
    {
        Dictionary<string, UserBL> users = new Dictionary<string, UserBL>();
        public UserBL Login(string email, string pass)
        {
            return null;

        }
        public bool Logout(string email)
        {
            return false;
        }
        public UserBL Register(string email, string pass)
        {
            return null;
        }
    }
}
