using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.BuisnessLayer;

namespace Backend.ServiceLayer
{
    public class UserService
    {
        private UserFacade UF;
        /// <summary>
        /// This construcor intiates a UserFacade object
        /// </summary>
        internal UserService(UserFacade UF)
        {
            this.UF = UF;
        }
        /// <summary>
        /// This method logs in and existing user.
        /// </summary>
        /// <param name="email">Inserted email attempt, used by the UserFacade to try to log in</param>
        /// <param name="password">Inserted password attempt, used by the UserFacade to try to log in</param>
        /// <returns>A UserSL response or and error message</returns>
        public string Login (string email,  string password)
        {
            try
            {
                UserBL ubl = UF.Login(email, password);
                Response<UserSL> res = new Response<UserSL>(new UserSL(ubl.Email));
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<UserSL>(e.Message));
            }
        }
        /// <summary>
        /// This method logs out a logged in user.
        /// </summary>
        /// <param name="email"> The email of the logged in user</param>
        /// <returns>A boolean - true for succesful logout and false for a not succesful logout ot an error message</returns>
        public string Logout(string email)
        {
            try
            {
                UF.Logout(email);
                Response<string> res1 = new Response<string>();
                return JsonSerializer.Serialize(res1);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<string>(e.Message));
            }
        }
        /// <summary>
        /// This method registers a new user to the system
        /// </summary>
        /// <param name="email">Inserted email attempt, used by the UserFacade to try to register</param>
        /// <param name="password">Inserted password attempt, used by the UserFacade to try to register</param>
        /// <returns>A UserSL response or an error message</returns>
        public string Register (string email, string password) 
        {
            try
            {
                UserBL ubl = UF.Register(email, password);
                Response<UserSL> res = new Response<UserSL>(new UserSL(ubl.Email));
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<UserSL>(e.Message));
            }
        }
        /// <summary>
        /// This method allows a user to change his/hers password
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="newpass">The new password the user is changing to</param>
        /// <returns>An empty response or an error message</returns>
        public string ChangePassword(string email, string newpass)
        {
            try
            {
                UF.ChangePassword(email, newpass);
                Response<string> res = new Response<string>();
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<string>(e.Message));
            }
        }
        /// <summary>
        /// This method loads all user data from the data base
        /// <returns>An empty response or an error message</returns>
        public string LoadAllUsers()
        {
            try
            {
                UF.LoadAllUsers();
                Response<string> res = new Response<string>();
                return JsonSerializer.Serialize(res);
            }
            catch(Exception e) 
            {
                    return JsonSerializer.Serialize(new Response<string>(e.Message));
            }
        }
        /// <summary>
        /// This method deletes all user data from the data base
        /// <returns>An empty response or an error message</returns>
        public string DeleteAllUsers()
        {
            try
            {
                UF.DeleteAllUsers();
                Response<string> res = new Response<string>();
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<string>(e.Message));
            }
        }

    }
}
