using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.ServiceLayer;

namespace BackendTests
{
    internal class UserServiceTesting
    {
        public UserService US;
        public void setup()
        {
            US = new UserService(); 
        }
        public void RegisterTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string pass = "Maya12";
            TestRegister(pass, email);//Valid Register
            TestRegister(pass, email);//Trying to register with the same email
            pass = "AAAAAA";
            TestRegister(pass, email);//Invalid password - no numbers
            pass = "A12345";
            TestRegister(pass, email);//Invalid password - no lowercase letters
            pass = "a12345";
            TestRegister(pass, email);//Invalid password - no uppercase letters
            pass = "Ab1";
            TestRegister(pass, email);//Invalid password - too short password
            pass = "1234567890ABCDEFGHIJKlmnop";
            TestRegister(pass, email);//Invalid password - too long password
        }
        public void TestRegister(String email, String pass)
        {
            string str = US.Register(email,pass);
            Response<UserSL>? res = JsonSerializer.Deserialize<Response<UserSL>>(str);
            if (res.ErrorMsg == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void LoginTestCases()
        {
            US.Register("MayaLich@post.bgu.ac.il", "Mm2121728");
            String email = "MayaLich@post.bgu.ac.il";
            String pass = "Mm2121728";
            TestLogin(email, pass);//Valid login
            email = "tomer@post.bgu.ac.il";
            TestLogin(email, pass);//Non existent user with this email
            email = "MayaLich@post.bgu.ac.il";
            pass = "Mm21212";
            TestLogin(email, pass);//Wrong password
        }
        public void TestLogin(String email, String pass)
        {
            string str = US.Login(email, pass);
            Response<UserSL>? res = JsonSerializer.Deserialize<Response<UserSL>>(str);
            if (res.ErrorMsg == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void LogoutTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            US.Register(email, "Mm2121728");
            LogoutTest(email); // Try to logout a not logged in user
                               // 
            //Logout and try to access your borad/tasks
        }
        public void LogoutTest(string email)
        {
            string str = US.Logout(email);
            Response<bool>? res = JsonSerializer.Deserialize<Response<bool>>(str);
            if (res.ErrorMsg == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
    }
}
