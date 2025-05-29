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
            ust.deconstruct();
            ust.setup();
            Console.WriteLine("Testing login:");
            ust.LoginTestCases();
            ust.deconstruct();
            ust.setup();
            Console.WriteLine("Testing logout:");
            ust.LogoutTestCases();
            ust.deconstruct();
            //BoardService
            Console.WriteLine("BOARD SERVICE:");
            BoardServiceTesting bst = new BoardServiceTesting();
            bst.setup();
            bst.deconstruct();
            Console.WriteLine("Testing create board:");
            bst.BoardCreationTestCases();
            bst.deconstruct();
            bst.setup();
            Console.WriteLine("Testing delete board:");
            bst.BoardDeletionTestCases();
            bst.deconstruct();
            bst.setup();
            Console.WriteLine("Testing change max tasks:");
            bst.ChangeMaxTaskCases();
            bst.deconstruct();
            bst.setup();
            Console.WriteLine("Testing Get max tasks:");
            bst.GetColumnLimitCases();
            bst.deconstruct();
            bst.setup();
            Console.WriteLine("Testing Get board name:");
            bst.GetColumnNameCases();
            bst.deconstruct();
            bst.setup();
            Console.WriteLine("Testing Get board column:");
            bst.GetColumnCases();
            bst.deconstruct();
            bst.setup();
            Console.WriteLine("Testing Join board:");
            bst.JoinBoardCases();
            bst.deconstruct();
            bst.setup();
            Console.WriteLine("Testing Leave board:");
            bst.LeaveBoardCases();
            bst.deconstruct();
            bst.setup();
            Console.WriteLine("Testing Transfer ownership:");
            bst.TransferOwnershipCases();
            bst.deconstruct();
            bst.setup();
            Console.WriteLine("Testing Get User boards:");
            bst.GetUserBoardsCases();
            bst.deconstruct();
            //TaskService
            Console.WriteLine("TASK SERVICE:");
            TaskServiceTesting tst = new TaskServiceTesting();
            tst.setup();
            Console.WriteLine("Testing add task:");
            tst.AddTaskTestCases();
            tst.deconstruct();
            tst.setup();
            Console.WriteLine("Testing move task:");
            tst.MoveTaskTestCases();
            tst.deconstruct();
            tst.setup();
            Console.WriteLine("Testing update title:");
            tst.UpdateTitleTestCases();
            tst.deconstruct();
            tst.setup();
            Console.WriteLine("Testing update description:");
            tst.UpdateDescTestCases();
            tst.deconstruct();
            tst.setup();
            Console.WriteLine("Testing update due date:");
            tst.UpdateDueDateTestCases();
            tst.deconstruct();
            tst.setup();
            Console.WriteLine("Testing get in progress list:");
            tst.InProgressListTestCases();
            tst.deconstruct();
            tst.setup();
            Console.WriteLine("Testing delete task:");
            tst.DeleteTaskTestCases();
            tst.deconstruct();
        }
    }
}
