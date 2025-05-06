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
            int [] partialLim = { -1, 10, 10};
            US.Register(email, "Mm212178");
            TestBoardCreation(email, boardName, noLimTask);//Valid creation without max tasks
            TestBoardCreation(email,"Tomer's Board", partialLim);//Valid creation with partial max tasks
            boardName = "Shira's Board";
            string email2 = "Tomer@post.bgu.ac.il";
            TestBoardCreation(email, boardName, maxTasks );//Valid creation with max tasks
            TestBoardCreation(email, "Maya's Board", noLimTask);//Invalid creation - same name
            TestBoardCreation(email2, "Tomer's Board", noLimTask);//Invalid creation - not register email
        }
        public void TestBoardCreation(string email, string boardName, int[] maxTasks)
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
            int[] maxTasks = { 25,25, 25 };
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName);
            TestBoardDeletion(email, boardName);//Valid deletion 
            boardName = "Shira's Board"; 
            BS.CreateBoard(email,boardName2 );
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
            string email = "MayaLich@post.bgu.ac.il";
            string email2 = "Shira@post.bgu.ac.il";
            string boardName1 = "Maya's Board";
            string boardName2 = "Shira's Board";
            string boardName3 = "Or's Board";
            int[] maxTasks = { 25, 25, 25 };
            US.Register(email, "Mm212178");
            US.Register(email2, "Shira123");
            BS.CreateBoard(email, boardName1);
            BS.CreateBoard(email, boardName2);
            TestGetAllBoards(email, 2);//Valid
            TestGetAllBoards(email2, 0);//Invalid - user with no boards
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
    }
}
