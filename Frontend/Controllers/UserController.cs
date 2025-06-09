using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.ServiceLayer;
using IntroSE.Kanban.Frontend.Model;

namespace IntroSE.Kanban.Frontend.Controllers
{
    internal class UserController
    {
        private UserService us;
        public UserController(UserService us) 
        {
            this.us = us;
        }
        public UserModel Login(string email, string password)
        {
            Response<UserSL> res = JsonSerializer.Deserialize<Response<UserSL>>(us.Login(email, password));
            if (res.ErrorMessage != null)
            {
                throw new Exception(res.ErrorMessage);
            }
            return new UserModel(res.ReturnValue);
        }
        public UserModel Register(string email, string password)
        {
            Response<UserSL> res = JsonSerializer.Deserialize<Response<UserSL>>(us.Register(email, password));
            if (res.ErrorMessage != null)
            {
                throw new Exception(res.ErrorMessage);
            }
            return new UserModel(res.ReturnValue);
        }
    }
}
