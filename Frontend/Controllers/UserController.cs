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
<<<<<<< HEAD
        public bool Logout(UserModel user)
        {
            Response<bool> res = JsonSerializer.Deserialize<Response<bool>>(us.Logout(user.Email));
=======
        public UserModel Register(string email, string password)
        {
            Response<UserSL> res = JsonSerializer.Deserialize<Response<UserSL>>(us.Register(email, password));
>>>>>>> f05138e8dac2f68b42c3e778b7888744f5486310
            if (res.ErrorMessage != null)
            {
                throw new Exception(res.ErrorMessage);
            }
<<<<<<< HEAD
            return res.ReturnValue;
        }
        // Handles connection with service layer
        // If a response holds an error message we will throw and the view model will catch and send to screen
=======
            return new UserModel(res.ReturnValue);
        }
>>>>>>> f05138e8dac2f68b42c3e778b7888744f5486310
    }
}
