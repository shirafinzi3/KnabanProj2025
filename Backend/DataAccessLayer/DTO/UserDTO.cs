using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    internal class UserDTO
    {
        private string email; //make personalized property with get and set for emial and pass
        private string password;
        public const string emailColumnName = "email";
        public const string passColumnName = "password";
        public UserController userController { get; set; }
        private bool isPersistent = false;

        public string Email
        {
            get => email;
            set
            {
                //do we need? if we cant change these?
            }
        }

        public string Password
        {
            get => password;
            set
            {

            }
        }

        public UserDTO(string email, string password)
        {
            email = Email;
            password = Password;
            userController = new UserController();
        }

        public void save()
        {
            userController.Insert(this);
            isPersistent = true;
        }
    }
}