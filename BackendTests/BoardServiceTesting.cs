using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.VisualBasic;

namespace BackendTests
{
    internal class BoardServiceTesting
    {
        UserService US;
        TaskService TS;
        BoardService BS;
        public void setup()
        {
            ServiceFactory sf = new ServiceFactory();
            US = sf.US;
            TS = sf.TS;
            BS = sf.BS;
        }
        public void BoardCreationTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string boardName = "Maya's Board";
            US.Register(email, "Mm212178");
            TestBoardCreation(email, boardName);//Valid creation without max tasks
            boardName = "Tomer's board";
            BS.ChangeMaxTasks(email, boardName, 0, 25);
            TestBoardCreation(email,boardName);//Valid creation with partial max tasks
            boardName = "Shira's Board";
            string email2 = "Tomer@post.bgu.ac.il";
            BS.ChangeMaxTasks(email, boardName, 0, 10);
            BS.ChangeMaxTasks(email, boardName, 1, 10);
            BS.ChangeMaxTasks(email, boardName, 2, 10);
            TestBoardCreation(email, boardName );//Valid creation with max tasks to all columns
            TestBoardCreation(email, "Maya's Board");//Invalid creation - same name
            TestBoardCreation(email2, "Tomer's Board");//Invalid creation - not register email
        }
        public void TestBoardCreation(string email, string boardName)
        {
            string str = BS.CreateBoard(email, boardName);
            Response<BoardSL>? res = JsonSerializer.Deserialize<Response<BoardSL>>(str);
            if (res.ErrorMessage == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void BoardDeletionTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string email2 = "Tomer@post.bgu.ac.il";
            string boardName = "Maya's Board";
            string boardName2 = "Shira's Board Number 2";
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName);
            TestBoardDeletion(email, boardName);//Valid deletion 
            BS.CreateBoard(email,boardName2 );
            boardName = "Shira's Board";
            TestBoardDeletion(email, boardName);//Invalid deltion - non existent board 
            TestBoardDeletion(email2, boardName2);//Invalid deltion - delete board that belongs to someone else 
            BS.CreateBoard(email, boardName);
            TestBoardDeletion("shira@post.bgu.ac.il", boardName);//Invalid deletion - email not register
        }
        public void TestBoardDeletion(string email, string boardName)
        {
            string str = BS.DeleteBoard(email, boardName);
            Response<String>? res = JsonSerializer.Deserialize<Response<String>>(str);
            if (res.ErrorMessage == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void GetAllBoardsTestCases()
        {
            string email = "Tal@post.bgu.ac.il";
            string email2 = "Amit@post.bgu.ac.il";
            string boardName1 = "Tal's Board";
            string boardName2 = "Amit's Board";
            string boardName3 = "Or's Board";
            US.Register(email, "Tal123");
            US.Register(email2, "Amit123");
            BS.CreateBoard(email, boardName1);
            BS.CreateBoard(email, boardName2);
            TestGetAllBoards(email, 2);//Valid
            TestGetAllBoards(email2, 0);//Valid - user with no boards
            TestGetAllBoards("wrong@email.com", 0); // Invalid - user not exists
            BS.CreateBoard(email, boardName3);
            TestGetAllBoards(email, 3); // Valid - user has 3 boards
        }
        public void TestGetAllBoards(string email, int expectedBoardsCount)
        {
            string str = BS.GetAllBoards(email); 
            Response<Dictionary<string,BoardSL>>? res = JsonSerializer.Deserialize<Response<Dictionary<string,BoardSL>>>(str);
            if (res.ErrorMessage == null && res.ReturnValue.Count == expectedBoardsCount)
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        public void ChangeMaxTaskCases()
        {
            string email2 = "Tal@post.bgu.ac.il";
            US.Register(email2, "Tal1905");
            BS.CreateBoard(email2, "TalBoard");
            TestChangeMaxTask(email2, "TalBoard", 0, 3);//Valid
            TS.AddTask(email2, "TalBoard", "Task1", "desc", DateTime.Now.AddDays(3));
            TS.AddTask(email2, "TalBoard", "Task2", "desc", DateTime.Now.AddDays(3));
            TS.AddTask(email2, "TalBoard", "Task3", "desc", DateTime.Now.AddDays(3));
            TestChangeMaxTask(email2, "TalBoard", 0, 2);// Invalid change max tasks - you need to delete tasks
            TestChangeMaxTask(email2, "TalBoard", 0, -5);// Invalid change max tasks

        }
        public void TestChangeMaxTask(string email, string boardName,int colIdx, int newlim)
        {
            string str = BS.ChangeMaxTasks(email,boardName,colIdx,newlim);
            Response<string>? res = JsonSerializer.Deserialize<Response<string>>(str);
            if (res.ErrorMessage == null )
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }
        public void GetColumnLimitCases()
        {
            string email2 = "Tal@post.bgu.ac.il";
            US.Register(email2, "Tal1905");
            BS.CreateBoard(email2, "TalBoard");
            TestGetColumnLimit(email2, "TalBoard", 0,-1);//valid - no lim check
            BS.ChangeMaxTasks(email2, "TalBoard", 0, 20);
            TestGetColumnLimit(email2, "TalBoard", 0, 20);//valid - lim check
            BS.ChangeMaxTasks(email2, "TalBoard", 1, 10);
            TestGetColumnLimit(email2, "TalBoard", 1, 10);//valid - lim check

        }
        public void TestGetColumnLimit(String email, String boardName, int colIdx,int expected)
        {
            string str = BS.GetColumnLimit(email, boardName, colIdx);
            Response<int>? res = JsonSerializer.Deserialize<Response<int>>(str);
            if (res.ErrorMessage == null&& res.ReturnValue.Equals(expected))
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }
        public void GetColumnNameCases()
        {
            string email2 = "Tal@post.bgu.ac.il";
            US.Register(email2, "Tal1905");
            BS.CreateBoard(email2, "TalBoard");
            TestGetColumnName(email2, "TalBoard", 0, "backlog");//valid - backlog
            TestGetColumnName(email2, "TalBoard", 1, "in progress");//valid - in progress
            TestGetColumnName(email2, "TalBoard", 2, "done");//valid - done
        }
        public void TestGetColumnName(String email, String boardName, int colIdx, string expected)
        {
            string str = BS.GetColumnName(email, boardName, colIdx);
            Response<string>? res = JsonSerializer.Deserialize<Response<string>>(str);
            if (res.ErrorMessage == null && res.ReturnValue.Equals(expected))
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }
        public void GetColumnCases()
        {
            string email2 = "Tal@post.bgu.ac.il";
            US.Register(email2, "Tal1905");
            BS.CreateBoard(email2, "TalBoard");
            TS.AddTask(email2, "TalBoard", "Task1", "desc", DateTime.Now.AddDays(3));
            TS.AddTask(email2, "TalBoard", "Task2", "desc", DateTime.Now.AddDays(3));
            TS.AddTask(email2, "TalBoard", "Task3", "desc", DateTime.Now.AddDays(3));
            TestGetColumn(email2, "TalBoard", 0, 3);//Valid - 3 tasks in "in progress"
            TS.MoveTask(email2, "TalBoard", 1);
            TestGetColumn(email2, "TalBoard", 0, 2);//Valid - 2 tasks in "in progress"
            TestGetColumn(email2, "TalBoard", 1, 1);//Valid - 3 tasks in "backlog"
            TestGetColumn(email2, "TalBoard", 2, 0);//Valid - 0 tasks in "done"
        }
        public void TestGetColumn(String email, String boardName, int colIdx, int expected)
        {
            
            string str = BS.GetColumn(email, boardName, colIdx);
            Response<List<TaskSL>>? res = JsonSerializer.Deserialize<Response<List<TaskSL>>>(str);
            if (res.ErrorMessage == null && res.ReturnValue.Count.Equals(expected))
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }
    }
}
