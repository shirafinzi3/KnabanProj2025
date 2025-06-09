using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Frontend.Controllers;
using Backend.ServiceLayer;

namespace IntroSE.Kanban.Frontend.Model
{
    internal class UserModel
    {
        public string Email { get; set; }
        /// <summary>
        /// This constructor get an email and creates a UserSL
        /// </summary>
        /// <param name="email">The email of the user</param>
        public UserModel(UserSL user)
        {
            this.Email = user.Email;
        }
        //May contain other data - can hold board related data
    }
}
