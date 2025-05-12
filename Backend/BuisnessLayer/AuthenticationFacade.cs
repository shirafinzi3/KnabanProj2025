using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Backend.BuisnessLayer
{
    internal class AuthenticationFacade
    {
       

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<string, bool> logins = new Dictionary<string, bool>();
        public bool IsLoggedIn(string email) {
            email = email.ToLower().Trim();
            if (!logins.ContainsKey(email)) 
            {
                Log.Error($"Provided email: {email} does not exist");
                throw new Exception($"Provided email: {email} does not exist");
            }
            return logins[email];
        }
        public void Login(string email)
        {
            email = email.ToLower().Trim();
            logins[email] = true;
            Log.Info($"User: {email} logged in");
        }
        public void Logout(string email) 
        {
            email = email.ToLower().Trim();
            if (!logins.ContainsKey(email))
            {
                Log.Error($"Provided email: {email} does not exist so connot be logged out");
                throw new Exception($"Provided email: {email} does not existso connot be logged out");
            }
            if (logins[email] == false)
            {
                Log.Error($"Provided email: {email} is not logged in");
                throw new Exception($"Provided email: {email} is not logged in");
            }
            logins[email] = false;
        }
    }
}
