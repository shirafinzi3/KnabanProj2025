using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Backend.BuisnessLayer
{
    internal class UserFacade
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<string, UserBL> users;
        private readonly AuthenticationFacade auth;

        public UserFacade(AuthenticationFacade auth)
        {
            this.users = new Dictionary<string, UserBL>();
            this.auth = auth;
        }
        public UserBL Login(string email, string pass)
        {
            if (string.IsNullOrEmpty(email))
            {
                Log.Error("Provided email is null or empty");
                throw new ArgumentNullException("Provided email is null or empty");
            }
            if (!users.ContainsKey(email.ToLower()))
            {
                Log.Error($"Provided email: {email} does not exist in the system ");
                throw new ArgumentException($"Provided email: {email} does not exist in the system ");
            }
            UserBL user = users[email.ToLower()];
            if (user.Login(pass))
            {
                auth.Login(email.ToLower());
                return user;
            }
            else
            {
                Log.Error("Email or password are invalid");
                throw new ArgumentException("Email or password are invalid");
            }
        }
        public void Logout(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                Log.Error("Provided email is null or empty");
                throw new ArgumentNullException("Provided email is null or empty");
            }
            if (!users.ContainsKey(email.ToLower()))
            {
                Log.Error($"Provided email: {email} does not exist in the system ");
                throw new ArgumentException($"Provided email: {email} does not exist in the system ");
            }
            auth.Logout(email.ToLower());
        }
        public UserBL Register(string email, string pass)
        {
            if (string.IsNullOrEmpty(email))
            {
                Log.Error("Provided email is null or empty");
                throw new ArgumentNullException("Provided email is null or empty");
            }
            if (users.ContainsKey(email.ToLower()))
            {
                Log.Error($"Provided email: {email} already exists in the system ");
                throw new ArgumentException($"Provided email: {email} already exists in the system ");
            }
            UserBL user = new UserBL(email.ToLower(), pass);
            users.Add(email.ToLower(), user);
            auth.Login(email.ToLower());
            return user;
        }
     
    }
}
