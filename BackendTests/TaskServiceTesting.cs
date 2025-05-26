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
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName);
            Console.Write("Expected: Success, Actual: ");
            TestAddTask(email, boardName, "Task1", "desc", DateTime.Now.AddDays(3));// Valid add
            Console.Write("Expected: Fail, Actual: ");
            TestAddTask(email, boardName, "", "desc", DateTime.Now.AddDays(3));//Invalid - Empty title
            Console.Write("Expected: Fail, Actual: ");
            TestAddTask(email, boardName, new string('A', 51), "desc", DateTime.Now.AddDays(3)); // Invalid- Title exceeds max length
            Console.Write("Expected: Fail, Actual: ");
            TestAddTask(email, boardName, "Task2", new string('D', 301), DateTime.Now.AddDays(3)); // Invaid -Description exceeds max length
            Console.Write("Expected: Fail, Actual: ");
            TestAddTask(email, "NonExistentBoard", "Task4", "desc", DateTime.Now.AddDays(3));// Invalid - Add task to non-existent board
            Console.Write("Expected: Success, Actual: ");
            TestAddTask(email, boardName, "Task2", "", DateTime.Now.AddDays(3));// Valid add, empty desc
            Console.Write("Expected: Fail, Actual: ");
            TestAddTask(email, boardName, "Task2", "desc", DateTime.Today.AddDays(-3));// Fail, Invalid due date
            BS.ChangeMaxTasks(email, boardName, 0, 2);
            Console.Write("Expected: Sucqcess, Actual: ");
            TestAddTask(email, boardName, "Task3", "desc", DateTime.Today.AddDays(3));// Fail, more than maxTasks
            BS.ChangeMaxTasks(email, boardName, 0, 25); //need to check if this fails!!



        }
        public void TestAddTask(string email, string boardName, string title, string desc, DateTime dueDate)
        {
            string str = TS.AddTask(email, boardName, title, desc, dueDate);
            Response<TaskSL>? res = JsonSerializer.Deserialize<Response<TaskSL>>(str);
            if (res.ErrorMessage == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void UpdateTitleTestCases() //maybe we should have the updates get TaskSLs in contract?? 
        {                                   //either way how do we know it during runtime? from user?
            string email = "MayaLich@post.bgu.ac.il";
            string boardName = "Maya's Board";
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName);
            TS.AddTask(email, boardName, "Task1", "desc", DateTime.Now.AddDays(3));
            Console.Write("Expected: Success, Actual: ");
            TestUpdateTitle(email, boardName, 1, "New Title"); // Valid update 
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateTitle(email, boardName, 1, "");//Invalid - Empty title
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateTitle(email, boardName, 1, new string('A', 51));// Invalid - Title exceeds max length
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateTitle(email, boardName, 999, "Another Title"); // Invalid - Non-existent task 
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateTitle("wrong@post.bgu.ac.il", boardName, 1, "ValidTitle"); // Invalid - Non-existent user
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateTitle(email, "FakeBoard", 1, "ValidTitle"); // Invalid - Non-existent board
            US.Logout(email);
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateTitle(email, boardName, 1, "New Title"); // Invalid - not logged in user

        }

        public void TestUpdateTitle(string email, string boardName, long taskId, string newTitle)
        {
            string str = TS.UpdateTitle(email, boardName, taskId, newTitle);
            Response<TaskSL>? res = JsonSerializer.Deserialize<Response<TaskSL>>(str);
            if (res.ErrorMessage == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");

        }
        public void UpdateDueDateTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string boardName = "Maya's Board";
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName);
            TS.AddTask(email, boardName, "Task1", "desc", DateTime.Now.AddDays(3));
            Console.Write("Expected: Success, Actual: ");
            TestUpdateDueDate(email, boardName, 1, DateTime.Now.AddDays(5)); // Valid update
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateDueDate(email, boardName, 1, DateTime.Now.AddDays(-1)); // Invalid - Due date in the past
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateDueDate("fake@post.bgu.ac.il", boardName, 1, DateTime.Now.AddDays(2)); //Invalid -  Non-existent user
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateDueDate(email, "UnknownBoard", 1, DateTime.Now.AddDays(2)); // Invalid - Non-existent board
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateDueDate(email, boardName, 999, DateTime.Now.AddDays(5));// Invalid - Non-existent task - TODO check id synchronizing
            US.Logout(email);
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateDueDate(email, boardName, 1, DateTime.Now.AddDays(5)); // Invalid - not logged in user
        }

        public void TestUpdateDueDate(string email, string boardName, long taskId, DateTime newDueDate)
        {
            string str = TS.UpdateDueDate(email, boardName, taskId, newDueDate);
            Response<TaskSL>? res = JsonSerializer.Deserialize<Response<TaskSL>>(str);
            if (res.ErrorMessage == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void UpdateDescTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string boardName = "Maya's Board";
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName);
            TS.AddTask(email, boardName, "Task1", "desc", DateTime.Now.AddDays(3));
            Console.Write("Expected: Success, Actual: ");
            TestUpdateDesc(email, boardName, 1, "New Description");// Valid update
            Console.Write("Expected: Success, Actual: ");
            TestUpdateDesc(email, boardName, 1, "");// Valid update - no description
            Console.Write("Expected: Success, Actual: ");
            TestUpdateDesc(email, boardName, 1, new string('D', 300)); // Valid - Exactly max length
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateDesc(email, boardName, 1, new string('D', 301));// Invalid - Description exceeds max length
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateDesc(email, boardName, 999, "desc");// Non-existent task - TODO check id synchronizing
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateDesc("fake@post.bgu.ac.il", boardName, 1, "New Desc"); // Invalid - Non-existent user
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateDesc(email, "WrongBoard", 1, "New Desc"); // Invalid - Non-existent board
            US.Logout(email);
            Console.Write("Expected: Fail, Actual: ");
            TestUpdateDesc(email, boardName, 1, "New Description"); // Invalid - not logged in user

        }

        public void TestUpdateDesc(string email, string boardName, long taskId, string newDesc)
        {
            string str = TS.UpdateDesc(email, boardName, taskId, newDesc);
            Response<TaskSL>? res = JsonSerializer.Deserialize<Response<TaskSL>>(str);
            if (res.ErrorMessage == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void MoveTaskTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string boardName = "Maya's Board";
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName);
            TS.AddTask(email, boardName, "Task1", "desc", DateTime.Now.AddDays(3));
            Console.Write("Expected: Success, Actual: ");
            TestMoveTask(email, boardName, 1);// Valid move from Backlog to In Progress
            Console.Write("Expected: Success, Actual: ");
            TestMoveTask(email, boardName, 1);// Valid move from In Progress to Done
            Console.Write("Expected: Fail, Actual: ");
            TestMoveTask(email, boardName, 1);// Invalid - move from Done (cannot move further)
            Console.Write("Expected: Fail, Actual: ");
            TestMoveTask(email, boardName, 999); // Invalid move non-existent task - TODO check id synchronizing
            Console.Write("Expected: Fail, Actual: ");
            TestMoveTask(email, "NonExistentBoard", 0);// Invalid - move non-existent board
        }

        public void TestMoveTask(string email, string boardName, long taskId)
        {
            string str = TS.MoveTask(email, boardName, taskId);
            Response<string>? res = JsonSerializer.Deserialize<Response<string>>(str);
            if (res.ErrorMessage == null)
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
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName1);
            BS.CreateBoard(email, boardName2);
            TS.AddTask(email, boardName1, "Task1", "desc", DateTime.Now.AddDays(3));
            TS.AddTask(email, boardName2, "Task2", "desc", DateTime.Now.AddDays(3));
            TS.AssignTask(email, boardName1, 0, 1, email);
            TS.AssignTask(email, boardName2, 0, 1, email);
            TS.MoveTask(email, boardName1, 1);// Move Task1 to In Progress
            TS.MoveTask(email, boardName2, 1);
            TS.MoveTask(email, boardName2, 1);// Move Task2 to Done (should not appear in In Progress list)
            TestInProgressList(email);// Only Task1 should be In Progress
            Console.WriteLine("Only Task1 should be in List, Actual: " + TS.InProgressList(email));
            TS.AddTask(email, boardName1, "Task3", "desc", DateTime.Now.AddDays(3));
            TS.AssignTask(email, boardName1, 0, 2, email);
            TS.MoveTask(email, boardName1, 2);
            TestInProgressList(email);//Task1 and Task3 should be In Progress
            Console.WriteLine("Task1 and Task 3 should be in List, Actual: " + TS.InProgressList(email));
            TS.MoveTask(email, boardName1, 1);//Move Task1 to done
            TS.MoveTask(email, boardName2, 3);//Move Task3 to done
            TestInProgressList(email);//Expect an empty list
            Console.WriteLine("Empry List, Actual: " + TS.InProgressList(email));
            Console.Write("Expected: Fail, Actual: ");
            TestInProgressList("fake@post.bgu.ac.il"); // Invalid - non-existent user
            US.Logout(email);
            Console.Write("Expected: Fail, Actual: ");
            TestInProgressList(email); // Invalid - not logged in user
        }
        public void TestInProgressList(string email)
        {
            string str = TS.InProgressList(email);
            Response<List<TaskSL>>? res = JsonSerializer.Deserialize<Response<List<TaskSL>>>(str);
            if (res.ErrorMessage == null)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void DeleteTaskTestCases()
        {
            string email = "MayaLich@post.bgu.ac.il";
            string boardName = "Maya's Board";
            US.Register(email, "Mm212178");
            BS.CreateBoard(email, boardName);
            TS.AddTask(email, boardName, "Task1", "desc", DateTime.Now.AddDays(3));
            TS.AddTask(email, boardName, "Task2", "desc", DateTime.Now.AddDays(3));
            Console.Write("Expected: Success, Actual: ");
            TestDeleteTask(email, boardName, 1);// valid - delete first task
            Console.Write("Expected: Fail, Actual: ");
            TestDeleteTask(email, boardName, 1);// Invalid - delete second task
            Console.Write("Expected: Fail, Actual: ");
            TestDeleteTask(email, boardName, 100);// Invalid - delete no exist task
            Console.Write("Expected: Fail, Actual: ");
            TestDeleteTask("notvalid@post.bgu.ac.il", boardName, 1);// Invalid - delete from not valid user
            Console.Write("Expected: Fail, Actual: ");
            TestDeleteTask(email, "noBoard", 100);// Invalid - delete no exist board

        }
        public void TestDeleteTask(string email, string boardName, long TaskId )
        {
            string str = TS.DeleteTask(email, boardName, TaskId);
            Response<bool>? res = JsonSerializer.Deserialize<Response<bool>>(str);
            if (res.ErrorMessage == null && res.ReturnValue == true)
            {
                Console.WriteLine("Success");
            }
            else Console.WriteLine("Failed");
        }
        public void AssignTaskTestCases()
        {
            string email1 = "MayaLich@post.bgu.ac.il";
            string email2 = "OtherUser@post.bgu.ac.il";
            string boardName = "Maya's Board";
            US.Register(email1, "Mm212178");
            US.Register(email2, "OtherPass123");
            string str =BS.CreateBoard(email1, boardName);
            Response<BoardSL> res = JsonSerializer.Deserialize<Response<BoardSL>>(str);
            long TestBoardId = res.ReturnValue.boardID;
            BS.JoinBoard(email2, TestBoardId);  
            string taskResponse = TS.AddTask(email1, boardName, "Task1", "desc", DateTime.Now.AddDays(3));
            Response<TaskSL>? taskRes = JsonSerializer.Deserialize<Response<TaskSL>>(taskResponse);
            long testTaskID = taskRes.ReturnValue.Id;
            Console.Write("Expected: Success, Actual: ");
            TestAssignTask(email1, boardName, 0, testTaskID, email2); // Valid - assign to user that is a member of the board
            Console.Write("Expected: Success, Actual: ");
            TestAssignTask(email1, boardName, 0, testTaskID, email1); // Valid - reassign to original creator
            Console.Write("Expected: Fail, Actual: ");
            TestAssignTask(email1, boardName, 0, testTaskID, "nonexistent@user.com"); // Invalid - user doesn't exist
            Console.Write("Expected: Fail, Actual: ");
            TestAssignTask("nonexistent@user.com", boardName, 0, testTaskID, email2); // Invalid - assigner doesn't exist
            Console.Write("Expected: Fail, Actual: ");
            TestAssignTask(email1, "FakeBoard", 0, testTaskID, email2); // Invalid - board doesn't exist
            Console.Write("Expected: Fail, Actual: ");
            TestAssignTask(email1, boardName, 0,99999, email2); // Invalid - task doesn't exist
            US.Logout(email1);
            Console.Write("Expected: Fail, Actual: ");
            TestAssignTask(email1, boardName, 0, testTaskID, email2); // Invalid - user not logged in
        }
        public void TestAssignTask(string email,string boardName,int col, long TaskID, string emailAssignee)
        {
            string str = TS.AssignTask(email, boardName,col, TaskID, emailAssignee);
            Response<TaskSL>? res = JsonSerializer.Deserialize<Response<TaskSL>>(str);
            if (res != null && res.ErrorMessage == null)
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
