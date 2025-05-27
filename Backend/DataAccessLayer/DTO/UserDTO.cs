using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    internal class UserDTO
    {
        public string Email { get; private set; }
        private string password;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public const string emailColumnName = "email";
        public const string passColumnName = "password";
        private readonly UserController userController;
        public string Password
        {
            get => password;
            set
            {
                if (isPersistent)
                {
                    if (userController.UpdatePassword(Email, value))
                    {
                        password = value;
                    }
                    else
                    {
                        Log.Error("Failed to update password in DB");
                        throw new InvalidOperationException("Failed to update password in DB");
                    }
                }
                else
                {
                    Log.Error("Tried to update password before persistence");
                    throw new InvalidOperationException("User must be saved before updating password");
                }

            }
        }
        private bool isPersistent = false;
        public UserDTO(string email, string password)
        {
            this.Email = email; 
            this.password = password;
            userController = new UserController();
        }

        public UserController UserController { get { return userController; } } 
        public void Save()
        {
            if (isPersistent) throw new InvalidOperationException("Cannot dave persisted object");
            if (userController.Insert(this))
            {
                isPersistent = true;
                Log.Info("Usr data saved to database");
            }
            else
            {
                Log.Error("Failed to insert user into DB");
                throw new InvalidOperationException("Failed to insert user into DB");
            }

        }
    }
}
