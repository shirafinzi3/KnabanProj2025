using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTests
{
    public class Main
    {
        public void main(string[] args)
        {
            //UserService
            UserServiceTesting ust = new UserServiceTesting();
            ust.setup();
            ust.RegisterTestCases();
            ust.LoginTestCases();
            ust.LogoutTestCases();
            //BoardService
            BoardServiceTesting bst = new BoardServiceTesting();
            bst.setup();
            bst.BoardCreationTestCases();
            bst.BoardDeletionTestCases();
            bst.GetAllBoardsTestCases();
            //TaskService
            TaskServiceTesting tst = new TaskServiceTesting();
            tst.setup();
            tst.AddTaskTestCases();
            tst.MoveTaskTestCases();
            tst.UpdateTitleTestCases();
            tst.UpdateDescTestCases();
            tst.UpdateDueDateTestCases();
            tst.InProgressListTestCases();
        }
    }
}
