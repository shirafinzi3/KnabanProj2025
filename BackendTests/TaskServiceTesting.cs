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
    internal class TaskServiceTesting
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
        public void AddTaskTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string boardName = "Maya's Board";
            int[] maxTasks = { 25, 25, 25 };
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName, maxTasks);
            Console.Write("Expected: Success, Actual: ");
            TestAddTask(email, boardName, "Task1", "desc", DateTime.Now.AddDays(3));// Valid add
            Console.Write("Expected: Failed, Actual: ");
            TestAddTask(email, boardName, "", "desc", DateTime.Now.AddDays(3));//Invalid - Empty title
            Console.Write("Expected: Failed, Actual: ");
            TestAddTask(email, boardName, new string('A', 51), "desc", DateTime.Now.AddDays(3)); // Invalid- Title exceeds max length
            Console.Write("Expected: Failed, Actual: ");
            TestAddTask(email, boardName, "Task2", new string('D', 301), DateTime.Now.AddDays(3)); // Invaid -Description exceeds max length
            Console.Write("Expected: Failed, Actual: ");
            TestAddTask(email, "NonExistentBoard", "Task4", "desc", DateTime.Now.AddDays(3));// Invalid - Add task to non-existent board

            Console.Write("Expected: Success, Actual: ");
            TestAddTask(email, boardName, "Task2", "", DateTime.Now.AddDays(3));// Valid add, empty desc
            Console.Write("Expected: Failed, Actual: ");
            TestAddTask(email, boardName, "Task2", "desc", DateTime.Today.AddDays(-3));// Fail, Invalid due date
            Console.Write("Expected: Failed, Actual: ");
            BS.ChangeMaxTasks(email, boardName, new int[] { 2, 25, 25 });
            TestAddTask(email, boardName, "Task3", "desc", DateTime.Today.AddDays(3));// Fail, more than maxTasks
            BS.ChangeMaxTasks(email, boardName, new int[] { 25, 25, 25 }); //need to check if this fails!!
        }
        public void TestAddTask(string email, string boardName, string title, string desc, DateTime dueDate)
        {
            string str = TS.AddTask(email,boardName,title,desc,dueDate);
            Response<TaskSL>? res = JsonSerializer.Deserialize<Response<TaskSL>>(str);
            if (res.ErrorMsg == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void UpdateTitleTestCases() //maybe we should have the updates get TaskSLs in contract?? 
        {                                   //either way how do we know it during runtime? from user?
            string email = "MayaLich@post.bgu.ac.il";
            string boardName = "Maya's Board";
            int[] maxTasks = { 25, 25, 25 };
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName, maxTasks);
            TS.AddTask(email, boardName, "Task1", "desc", DateTime.Now.AddDays(3));
            TestUpdateTitle(email, boardName, 1, "New Title"); // Valid update - TODO check id synchronizing
            TestUpdateTitle(email, boardName, 1, "");//Invalid - Empty title
            TestUpdateTitle(email, boardName, 1, new string('A', 51));// Invalid - Title exceeds max length
            TestUpdateTitle(email, boardName, 999, "Another Title"); // Invalid - Non-existent task - TODO check id synchronizing
        }

        public void TestUpdateTitle(string email, string boardName, long taskId, string newTitle)
        {
            string str = TS.UpdateTitle(email, boardName, taskId, newTitle);
            Response<TaskSL>? res = JsonSerializer.Deserialize<Response<TaskSL>>(str);
            if (res.ErrorMsg == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");

        }
        public void UpdateDueDateTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string boardName = "Maya's Board";
            int[] maxTasks = { 25, 25, 25 };
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName, maxTasks);
            TS.AddTask(email, boardName, "Task1", "desc", DateTime.Now.AddDays(3));
            TestUpdateDueDate(email, boardName, 0, DateTime.Now.AddDays(5)); // Valid update
            TestUpdateDueDate(email, boardName, 999, DateTime.Now.AddDays(5));// Invalid - Non-existent task - TODO check id synchronizing
        }

        public void TestUpdateDueDate(string email, string boardName, long taskId, DateTime newDueDate)
        {
            string str = TS.UpdateDueDate(email, boardName, taskId, newDueDate);
            Response<TaskSL>? res = JsonSerializer.Deserialize<Response<TaskSL>>(str);
            if (res.ErrorMsg == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void UpdateDescTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string boardName = "Maya's Board";
            int[] maxTasks = { 25, 25, 25 };
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName,maxTasks);
            TS.AddTask(email, boardName, "Task1", "desc", DateTime.Now.AddDays(3));
            TestUpdateDesc(email, boardName, 1, "New Description");// Valid update
            TestUpdateDesc(email, boardName, 1, "");// Valid update - no description
            TestUpdateDesc(email, boardName, 1, new string('D', 301));// Invalid - Description exceeds max length
            TestUpdateDesc(email, boardName, 999, "desc");// Non-existent task - TODO check id synchronizing
        }

        public void TestUpdateDesc(string email, string boardName, long taskId, string newDesc)
        {
            string str = TS.UpdateDesc(email, boardName, taskId, newDesc);
            Response<TaskSL>? res = JsonSerializer.Deserialize<Response<TaskSL>>(str);
            if (res.ErrorMsg == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void MoveTaskTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string boardName = "Maya's Board";
            int[] maxTasks = { 25, 25, 25 };
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName,maxTasks);
            TS.AddTask(email, boardName, "Task1", "desc", DateTime.Now.AddDays(3));
            TestMoveTask(email, boardName, 1);// Valid move from Backlog to In Progress
            TestMoveTask(email, boardName, 1);// Valid move from In Progress to Done
            TestMoveTask(email, boardName, 1);// Invalid - move from Done (cannot move further)
            TestMoveTask(email, boardName, 999); // Invalid move non-existent task - TODO check id synchronizing
            TestMoveTask(email, "NonExistentBoard", 0);// Invalid - move non-existent board
        }

        public void TestMoveTask(string email, string boardName, long taskId)
        {
            string str = TS.MoveTask(email, boardName, taskId);
            Response<string>? res = JsonSerializer.Deserialize<Response<string>>(str);
            if (res.ErrorMsg == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void InProgressListTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string boardName1 = "Board1";
            string boardName2 = "Board2";
            int[] maxTasks = { 25, 25, 25 };
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName1,maxTasks);
            BS.CreateBoard(email, boardName2,maxTasks);
            TS.AddTask(email, boardName1, "Task1", "desc", DateTime.Now.AddDays(3));
            TS.AddTask(email, boardName2, "Task2", "desc", DateTime.Now.AddDays(3));
            TS.MoveTask(email, boardName1, 1);// Move Task1 to In Progress
            TS.MoveTask(email, boardName2, 2);
            TS.MoveTask(email, boardName2, 2);// Move Task2 to Done (should not appear in In Progress list)
            TestInProgressList(email);// Only Task1 should be In Progress
            TS.AddTask(email, boardName1, "Task3", "desc", DateTime.Now.AddDays(3));
            TS.MoveTask(email, boardName2, 3);
            TestInProgressList(email);//Task1 and Task3 should be In Progress
            TS.MoveTask(email, boardName1, 1);//Move Task1 to done
            TS.MoveTask(email, boardName2, 3);//Move Task3 to done
            TestInProgressList(email);//Expect an empty list

        }
        public void TestInProgressList(string email)
        {
            string str = TS.InProgressList(email);
            Response<List<TaskSL>>? res = JsonSerializer.Deserialize<Response<List<TaskSL>>>(str);
            if (res.ErrorMsg == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
    }
}
