using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Backend.ServiceLayer;
using log4net.Config;
using log4net;

namespace BackendTests
{
    public class MainTest
    {
        public static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            //UserService
            Console.WriteLine("USER SERVICE:");
            UserServiceTesting ust = new UserServiceTesting();
            ust.setup();
            Console.WriteLine("Testing register:");
            ust.RegisterTestCases();
            ust.setup();
            Console.WriteLine("Testing login:");
            ust.LoginTestCases();
            ust.setup();
            Console.WriteLine("Testing login:");
            ust.LogoutTestCases();
            //BoardService
            Console.WriteLine("BOARD SERVICE:");
            BoardServiceTesting bst = new BoardServiceTesting();
            bst.setup();
            Console.WriteLine("Testing create board:");
            bst.BoardCreationTestCases();
            bst.setup();
            Console.WriteLine("Testing delete board:");
            bst.BoardDeletionTestCases();
            bst.setup();
            Console.WriteLine("Testing get all boards:");
            bst.GetAllBoardsTestCases();
            //TaskService
            Console.WriteLine("TASK SERVICE:");
            TaskServiceTesting tst = new TaskServiceTesting();
            tst.setup();
            Console.WriteLine("Testing add task:");
            tst.AddTaskTestCases();
            tst.setup();
            Console.WriteLine("Testing nove task:");
            tst.MoveTaskTestCases();
            tst.setup();
            Console.WriteLine("Testing update title:");
            tst.UpdateTitleTestCases();
            tst.setup();
            Console.WriteLine("Testing update description:");
            tst.UpdateDescTestCases();
            tst.setup();
            Console.WriteLine("Testing update due date:");
            tst.UpdateDueDateTestCases();
            tst.setup();
            Console.WriteLine("Testing get in progress list:");
            tst.InProgressListTestCases();
        }
    }
}
