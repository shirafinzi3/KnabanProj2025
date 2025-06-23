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
        public UserModel(UserSL user)
        {
            this.Email = user.Email;
        }
    }
}
