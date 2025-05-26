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
            Console.Write("Expected: Success, Actual: ");
            TestRegister(email, pass);//Valid Register
            Console.Write("Expected: Fail?, Actual: ");
            TestRegister(email,pass);//Trying to register with the same email
            pass = "AAAAAA";
            Console.Write("Expected: Fail, Actual: ");
            TestRegister(email,pass);//Invalid password - no numbers
            pass = "A12345";
            Console.Write("Expected: Fail, Actual: ");
            TestRegister(email, pass);//Invalid password - no lowercase letters
            pass = "a12345";
            Console.Write("Expected: Fail, Actual: ");
            TestRegister(email, pass);//Invalid password - no uppercase letters
            pass = "Ab1";
            Console.Write("Expected: Fail, Actual: ");
            TestRegister(email, pass);//Invalid password - too short password
            pass = "1234567890ABCDEFGHIJKlmnop";
            Console.Write("Expected: Fail, Actual: ");
            TestRegister(email, pass);//Invalid password - too long password
            Console.Write("Expected: Fail, Actual: ");
            TestRegister("", pass); // Invalid - Empty email
            Console.Write("Expected: Fail, Actual: ");
            TestRegister(email, ""); // Invalid - Empty password
            Console.Write("Expected: Fail, Actual: ");
            TestRegister(null, pass);// Invalid - null email
            Console.Write("Expected: Fail, Actual: ");
            TestRegister(email, null);// Invalid - null password
            Console.Write("Expected: Fail, Actual: ");
            TestRegister("not-an-email", pass); // Invalid - invalid email format
            Console.Write("Expected: Fail, Actual: ");
            TestRegister("missing@domain", pass);// Invalid - invalid email format
            Console.Write("Expected: Fail, Actual: ");
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
            Console.Write("Expected: Success, Actual: ");
            TestLogin(email, pass);//Valid login
            email = "tomer@post.bgu.ac.il";
            Console.Write("Expected: Fail, Actual: ");
            TestLogin(email, pass);//Non existent user with this email
            email = "MayaLich@post.bgu.ac.il";
            pass = "Mm21212";
            Console.Write("Expected: Fail, Actual: ");
            TestLogin(email, pass);//Wrong password
            Console.Write("Expected: Fail, Actual: ");
            TestLogin("user@post.bgu.ac.il", "MM21212"); // Invalid - wrong password, Correct password: "Mm21212"
            Console.Write("Expected: Fail, Actual: ");
            TestLogin("", pass); // Invalid - Empty email
            Console.Write("Expected: Fail, Actual: ");
            TestLogin(email, ""); // Invalid - Empty password
            Console.Write("Expected: Fail, Actual: ");
            TestLogin(null, pass);// Invalid - null email
            Console.Write("Expected: Fail, Actual: ");
            TestLogin(email, null);// Invalid - null password
        }
        public void TestLogin(String email, String pass)
        {
            string str = US.Login(email, pass);
            Response<UserSL>? res = JsonSerializer.Deserialize<Response<UserSL>>(str);
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
            Console.Write("Expected: Sucess, Actual: ");
            LogoutTest(email); // Valid logout
            Console.Write("Expected: Fail, Actual: ");
            LogoutTest(email); // Try to logout a not logged in user
            Console.Write("Expected: Fail, Actual: ");
            LogoutTest("tomer@post.bgu.ac.il");//Try to logout a non existent user
            Console.Write("Expected: Fail, Actual: ");
            LogoutTest(null);// Invalid - null email
            Console.Write("Expected: Fail, Actual: ");
            LogoutTest("");// Invalid - empty email
        }
        public void LogoutTest(string email)
        {
            string str = US.Logout(email);
            Response<string>? res = JsonSerializer.Deserialize<Response<string>>(str);
            if (res.ErrorMessage == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
    }
}
