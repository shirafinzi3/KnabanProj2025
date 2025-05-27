using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
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
            email = email.ToLower().Trim();
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
        public void Logout(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                Log.Error("Provided email is null or empty");
                throw new ArgumentNullException("Provided email is null or empty");
            }
            email = email.ToLower().Trim();
            if (!users.ContainsKey(email))
            {
                Log.Error($"Provided email: {email} does not exist in the system ");
                throw new ArgumentException($"Provided email: {email} does not exist in the system ");
            }
            auth.Logout(email);
        }
        public UserBL Register(string email, string pass)
        {
            if (string.IsNullOrEmpty(email))
            {
                Log.Error("Provided email is null or empty");
                throw new ArgumentNullException("Provided email is null or empty");
            }
            email = email.ToLower().Trim();
            if (users.ContainsKey(email))
            {
                Log.Error($"Provided email: {email} already exists in the system ");
                throw new ArgumentException($"Provided email: {email} already exists in the system ");
            }
            UserBL user = new UserBL(email, pass);
            users.Add(email, user);
            auth.Login(email);
            return user;
        }
        public void ChangePassword(string email, string newpass)
        {
            if (string.IsNullOrEmpty(email))
            {
                Log.Error("Provided email is null or empty");
                throw new ArgumentNullException("Provided email is null or empty");
            }
            email = email.ToLower().Trim();
            if (!users.ContainsKey(email))
            {
                Log.Error($"Provided email: {email} does not exist in the system ");
                throw new ArgumentException($"Provided email: {email} does not exist in the system ");
            }
            if (!auth.IsLoggedIn(email))
            {
                Log.Error($"User {email} is not logged in ");
                throw new ArgumentException($"User {email} is not logged in");
            }
            UserBL user = users[email];
            user.Password = newpass;
        }
        public void LoadAllUsers()
        {
            UserController userController = new UserController();
            List<UserDTO> userDTOs = userController.SelectAll();
            foreach (UserDTO userDTO in userDTOs)
            {
                users[userDTO.Email] = new UserBL(userDTO);
            }
            Log.Info("Users data uploaded from database");
        }
        public void DeleteAllUsers()
        {
            UserController userController = new UserController();
            foreach(UserBL userBL in users.Values)
            {
                userController.Delete(userBL.UDTO); 
            }
            users.Clear();
            Log.Info("Users data deleted  from database");
        }
     
    }
}
