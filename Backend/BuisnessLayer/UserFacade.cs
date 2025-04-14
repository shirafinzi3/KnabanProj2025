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
        private readonly Dictionary<string, UserBL> users = new Dictionary<string, UserBL>();
        private readonly AuthenticationFacade auth = new AuthenticationFacade();
        public UserBL Login(string email, string pass)
        {
            if (string.IsNullOrEmpty(email))
            {
                Log.Error("Provided email is null or empty");
                throw new ArgumentNullException("Provided email is null or empty");
            }
            if (!users.ContainsKey(email))
            {
                Log.Error($"Provided email: {email} does not exist in the system ");
                throw new ArgumentException($"Provided email: {email} does not exist in the system ");
            }
            UserBL user = users[email];
            if (user.Login(pass))
            {
                auth.Login(email);
                return user;
            }
            else
            {
                Log.Error("Email or password are invalid");
                throw new ArgumentException("Email or password are invalid");
            }
        }
        public bool Logout(string email)
        {
            return auth.Logout(email);
        }
        public UserBL Register(string email, string pass)
        {
            if (string.IsNullOrEmpty(email))
            {
                Log.Error("Provided email is null or empty");
                throw new ArgumentNullException("Provided email is null or empty");
            }
            if (users.ContainsKey(email))
            {
                Log.Error($"Provided email: {email} already exists in the system ");
                throw new ArgumentException($"Provided email: {email} already exists in the system ");
            }
            if (!validatePass(pass))
            {
                Log.Error("Invalid password");
                throw new ArgumentException("Invalid password - it must contain at least one uppercase letter, one lowercase letter, one digit, and be 6-20 characters long.");
            }
            UserBL user = new UserBL(email, pass);
            users.Add(email, user);
            return user;
        }
        private bool validatePass(string pass)
        {
            bool hasLower = false;
            bool hasUpper = false;
            bool hasDigit = false;
            if (string.IsNullOrEmpty(pass) || pass.Length < 6 || pass.Length > 20)
            {
                return false;
            }
            foreach (char c in pass)
            {
                if (char.IsLower(c))
                {
                    hasLower = true;
                }
                if (char.IsUpper(c))
                {
                    hasUpper = true;
                }
                if (char.IsDigit(c))
                {
                    hasDigit = true;
                }
                if (hasLower && hasUpper && hasDigit) { return true; }
            }
            return false;
        }
    }
}
