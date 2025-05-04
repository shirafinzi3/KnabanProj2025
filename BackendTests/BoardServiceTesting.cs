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
    internal class BoardServiceTesting
    {
        public UserService US;
        public BoardService BS;
        public void setup()
        {
            
            ServiceFactory sf = new ServiceFactory();
            this.US = sf.US;
            this.BS = sf.BS;
        }
        public void BoardCreationTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string boardName = "Maya's Board";
            int[] maxTasks = { 25, 25, 25 };
            int[] noLimTask = { -1, -1, -1 };
            US.Register(email, "Mm212178");
            TestBoardCreation(email, boardName, noLimTask);//Valid creation without max tasks
            boardName = "Shira's Board";
            TestBoardCreation(email, boardName, maxTasks );//Valid creation with max tasks
            TestBoardCreation(email, "Maya's Board", noLimTask);//Invalid creation - same name
        }
        public void TestBoardCreation(string email, string boardName, int[] maxTasks)
        {
            string str = BS.CreateBoard(email, boardName,maxTasks);
            Response<BoardSL>? res = JsonSerializer.Deserialize<Response<BoardSL>>(str);
            if (res.ErrorMsg == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void BoardDeletionTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string boardName = "Maya's Board";
            int[] maxTasks = { 25,25, 25 };
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName,maxTasks);
            TestBoardDeletion(email, boardName);//Valid deletion 
            boardName = "Shira's Board";
            TestBoardDeletion(email, boardName);//Invalid deltion - non existent board 
            BS.CreateBoard(email, boardName, maxTasks);
            TestBoardDeletion("shira@post.bgu.ac.il", boardName);//Valid deletion 
        }
        public void TestBoardDeletion(string email, string boardName)
        {
            string str = BS.DeleteBoard(email, boardName);
            Response<String>? res = JsonSerializer.Deserialize<Response<String>>(str);
            if (res.ErrorMsg == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void GetAllBoardsTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string boardName1 = "Maya's Board";
            string boardName2 = "Shira's Board";
            string boardName3 = "Or's Board";
            int[] maxTasks = { 25, 25, 25 };
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName1, maxTasks);
            BS.CreateBoard(email, boardName2, maxTasks);
            TestGetAllBoards(email, 2);
            TestGetAllBoards("wrong@email.com", 0); // Invalid - user not exists
            BS.CreateBoard(email, boardName3, maxTasks);
            TestGetAllBoards(email, 3); // Valid - user has 3 boards
        }
        public void TestGetAllBoards(string email, int expectedBoardsCount)
        {
            string str = BS.GetAllBoards(email); 
            Response<Dictionary<string,BoardSL>>? res = JsonSerializer.Deserialize<Response<Dictionary<string,BoardSL>>>(str);
            if (res.ErrorMsg == null && res.RetVal.Count == expectedBoardsCount)
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
