using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests
{
    internal class UserServiceTesting
    {
        public UserService US;
        public void setup()
        {
            ServiceFactory sf = new ServiceFactory();
            this.US = sf.US; 
        }
        public void RegisterTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string pass = "Maya12";
            TestRegister(email, pass);//Valid Register
            TestRegister(email,pass);//Trying to register with the same email
            pass = "AAAAAA";
            TestRegister(email,pass);//Invalid password - no numbers
            pass = "A12345";
            TestRegister(email, pass);//Invalid password - no lowercase letters
            pass = "a12345";
            TestRegister(email, pass);//Invalid password - no uppercase letters
            pass = "Ab1";
            TestRegister(email, pass);//Invalid password - too short password
            pass = "1234567890ABCDEFGHIJKlmnop";
            TestRegister(email, pass);//Invalid password - too long password
            TestRegister("", pass); // Invalid - Empty email
            TestRegister(email, ""); // Invalid - Empty password
            TestRegister(null, pass);// Invalid - null email
            TestRegister(email, null);// Invalid - null password
            TestRegister("not-an-email", pass); // Invalid - invalid email format
            TestRegister("missing@domain", pass);// Invalid - invalid email format
            TestRegister("missingdomain.com", pass);// Invalid - invalid email format
        }
        public void TestRegister(String email, String pass)
        {
            string str = US.Register(email,pass);
            Response<UserSL>? res = JsonSerializer.Deserialize<Response<UserSL>>(str);
            if (res.ErrorMessage == null)
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
            TestLogin("user@post.bgu.ac.il", "MM21212"); // Invalid - wrong password, Correct password: "Mm21212"
            TestLogin("", pass); // Invalid - Empty email
            TestLogin(email, ""); // Invalid - Empty password
            TestLogin(null, pass);// Invalid - null email
            TestLogin(email, null);// Invalid - null password
        }
        public void TestLogin(String email, String pass)
        {
            string str = US.Login(email, pass);
            Response<UserSL>? res = JsonSerializer.Deserialize<Response<UserSL>>(str);
            Console.WriteLine(str);
            if (res.ErrorMessage == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void LogoutTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            US.Register(email, "Mm2121728");
            US.Login(email, "Mm2121728");
            LogoutTest(email); // Valid logout
            LogoutTest(email); // Try to logout a not logged in user
            LogoutTest("tomer@post.bgu.ac.il");//Try to logout a non existent user
            LogoutTest(null);// Invalid - null email
            LogoutTest("");// Invalid - empty email
        }
        public void LogoutTest(string email)
        {
            string str = US.Logout(email);
            Response<bool>? res = JsonSerializer.Deserialize<Response<bool>>(str);
            if (res.ErrorMessage == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
    }
}
