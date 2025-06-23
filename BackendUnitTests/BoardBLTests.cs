using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.BuisnessLayer;
using NUnit.Framework;


namespace BackendUnitTests
{
    internal class BoardBLTests
    {
        private BoardBL board;
        [SetUp]
        public void Setup() {
            AuthenticationFacade auth = new AuthenticationFacade();
            auth.Login("MayaL@post.bgu.ac.il");
            BoardFacade facade = new BoardFacade(auth);
            facade.DeleteAllBoards();
            board = new BoardBL("TestBoard", "MayaL@post.bgu.ac.il", 1, 1);
        }

        [Test]
        public void BoardNameIsCorrect()
        {
            Assert.AreEqual("TestBoard",board.BoardName);
        }

        [Test]
        public void OwnerEmailIsCorrect()
        {
            Assert.AreEqual("MayaL@post.bgu.ac.il", board.Owner);
        }

        [Test]
        public void AddUserIsCorrect()
        {
            board.AddUser("newuser@post.bgu.ac.il");
            Assert.Contains("newuser@post.bgu.ac.il", board.Users);
        }
      
        [Test]
        public void AddUserTwice()
        {
            board.AddUser("Duplicate@post.bgu.ac.il");
            var ex=Assert.Throws<InvalidOperationException>(() => board.AddUser("Duplicate@post.bgu.ac.il"));
            Assert.That(ex.Message, Is.EqualTo("User Duplicate@post.bgu.ac.il is already a member of TestBoard"));
        }

        [Test]
        public void RemoveExistUser()
        {
            board.AddUser("toRemove@post.bgu.ac.il");
            board.RemoveUser("toRemove@post.bgu.ac.il");
            Assert.IsFalse(board.Users.Contains("toRemove@post.bgu.ac.il"));
        }

        [Test]
        public void RemoveNonExistUser()
        {
            Assert.Throws<InvalidOperationException>(() => board.RemoveUser("nonexist@post.bgu.ac.il"));
        }

        [Test] 
        public void RemoveOwner()
        {
            Assert.Throws<InvalidOperationException>(() => board.RemoveUser("MayaL@post.bgu.ac.il"));
        }

        [Test]
        public void TransferOwnership_Valid()
        {
            board.AddUser("newOwner@post.bgu.ac.il");
            board.Owner = "newOwner@post.bgu.ac.il";
            Assert.AreEqual("newOwner@post.bgu.ac.il",board.Owner);
        }

        [Test]  
        public void TransferOwnership_ToNonExist_Invalid()
        {
            var ex = Assert.Throws<Exception>(() => board.Owner = "not@post.bgu.ac.il");
            Assert.That(ex.Message, Does.Contain("not@post.bgu.ac.il is not a member"));
        }

        [Test]
        public void TransferOwnership_ToNonTheOwner_Invalid()
        {
            var ex = Assert.Throws<Exception>(() => board.Owner = "MayaL@post.bgu.ac.il");
            Assert.That(ex.Message,Is.EqualTo("MayaL@post.bgu.ac.il is already the owner of TestBoard"));
        }

        [Test]
        public void AddTask_AddToBacklog()
        {
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            Assert.IsTrue(board.Columns[BoardBL.BACKLOG].GetTasks().ContainsKey(task.TaskID));
        }

        [Test]
        public void MoveTask_BacklogToInProgress()
        {
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.moveTask(task.TaskID);
            Assert.IsTrue(board.Columns[BoardBL.IN_PROGRESS].GetTasks().ContainsKey(task.TaskID));
        }

        [Test]
        public void MoveTask_InProgressToDone()
        {
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.moveTask(task.TaskID);
            board.moveTask(task.TaskID);
            Assert.IsTrue(board.Columns[BoardBL.DONE].GetTasks().ContainsKey(task.TaskID));
        }

        [Test]
        public void MoveTaskFromDone_Invalid()
        {
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.moveTask(task.TaskID);
            board.moveTask(task.TaskID);
            var ex = Assert.Throws<Exception>(() => board.moveTask(task.TaskID));
            Assert.That(ex.Message, Does.Contain("Cant move task forward from Done column"));
        }

        [Test]
        public void MoveNonExistTask_Invalid()
        {
            var ex = Assert.Throws<Exception>(() => board.moveTask(999));
            Assert.That(ex.Message, Does.Contain("Task does not exist"));
        }

        [Test]
        public void DeleteTask_Valid()
        {
            var task=board.addTask("task1",DateTime.Now.AddDays(3), "desc", 1);
            var result=board.deleteTask(task.TaskID);
            Assert.IsTrue(result);
        }

        [Test]
        public void DeleteNotExistTask_Invalid()
        {
            var result = board.deleteTask(999);
            Assert.IsTrue(!result);
        }

        [Test]
        public void ChangeMaxTask_Valid()
        {
            board.changeMaxTasks(0, 10);
            Assert.AreEqual(10,board.GetColumnLimit(0));
        }

        [Test]
        public void ChangeMaxTask_Inalid()
        {
            board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.addTask("task2", DateTime.Now.AddDays(3), "desc", 2);
            var ex = Assert.Throws<InvalidOperationException>(() => board.changeMaxTasks(0,1));
            Assert.That(ex.Message, Does.Contain("Cant lower max tasks of backlog as it currently holds more than 1"));
        }

        [Test]
        public void AssignTask_Valid()
        {
            board.AddUser("assignee@post.bgu.ac.il");
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.AssignTask("MayaL@post.bgu.ac.il", 0, task.TaskID, "assignee@post.bgu.ac.il");
            Assert.AreEqual("assignee@post.bgu.ac.il", task.Assignee);
        }

        [Test]
        public void AssignTaskUserNotExist_Invalid()
        {
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            var ex= Assert.Throws<InvalidOperationException>(() => board.AssignTask("MayaL@post.bgu.ac.il", 0, task.TaskID, "nonAssignee@post.bgu.ac.il"));
            Assert.That(ex.Message, Does.Contain("User nonAssignee@post.bgu.ac.il is a member of TestBoard"));
        }

        [Test]
        public void AssignTaskTaskNotExist_Invalid()
        {
            board.AddUser("assignee@post.bgu.ac.il");
            var ex = Assert.Throws<InvalidOperationException>(() => board.AssignTask("MayaL@post.bgu.ac.il", 0,999, "nonAssignee@post.bgu.ac.il"));
            Assert.That(ex.Message, Does.Contain("User nonAssignee@post.bgu.ac.il is a member of TestBoard"));
        }

        [Test]
        public void AssignerIsNotAssignee_Invalid()
        {
            board.AddUser("assignee1@post.bgu.ac.il");
            board.AddUser("assignee2@post.bgu.ac.il");
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.AssignTask("MayaL@post.bgu.ac.il", 0, task.TaskID, "assignee1@post.bgu.ac.il");
            var ex = Assert.Throws<Exception>(() => board.AssignTask("assignee2@post.bgu.ac.il", 0, 999, "assignee2@post.bgu.ac.il"));
            Assert.That(ex.Message, Does.Contain("Task does not exist"));
        }

        [Test]
        public void FullTaskFlow_AssignmentAndMovement()
        {
            board.AddUser("assignee1@post.bgu.ac.il");
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.AssignTask("MayaL@post.bgu.ac.il", 0, task.TaskID, "assignee1@post.bgu.ac.il");
            board.moveTask(task.TaskID);
            board.moveTask(task.TaskID);
            Assert.IsTrue(board.Columns[BoardBL.DONE].GetTasks().ContainsKey(task.TaskID));
            Assert.AreEqual("assignee1@post.bgu.ac.il", task.Assignee);
        }

        [Test]
        public void FullTaskFlow_AssignmentAndReassign()
        {
            board.AddUser("assignee1@post.bgu.ac.il");
            board.AddUser("assignee2@post.bgu.ac.il");
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.AssignTask("MayaL@post.bgu.ac.il", 0, task.TaskID, "assignee1@post.bgu.ac.il");
            board.AssignTask("assignee1@post.bgu.ac.il", 0, task.TaskID, "assignee2@post.bgu.ac.il");
            Assert.AreEqual("assignee2@post.bgu.ac.il", task.Assignee);
        }

        [Test]
        public void FullTaskFlow_LimitFlow()
        {
            board.changeMaxTasks(0, 2);
            board.addTask("task1", DateTime.Now.AddDays(2), "desc", 1);
            board.addTask("task2", DateTime.Now.AddDays(3), "desc", 2);
            var ex = Assert.Throws<InvalidOperationException>(() => board.addTask("task3", DateTime.Now.AddDays(4), "desc", 3));
            Assert.That(ex.Message, Does.Contain("Column backlog is full"));
        }

        [Test]
        public void FullTransferOwnerFlow()
        {
            board.AddUser("assignee1@post.bgu.ac.il");
            board.Owner = "assignee1@post.bgu.ac.il";
            board.AddUser("assignee2@post.bgu.ac.il");
            Assert.Contains("assignee2@post.bgu.ac.il", board.Users);
        }


    }

}
