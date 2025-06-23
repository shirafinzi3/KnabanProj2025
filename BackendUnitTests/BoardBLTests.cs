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
        public void Setup()
        {
            AuthenticationFacade auth = new AuthenticationFacade();
            auth.Login("MayaL@post.bgu.ac.il");
            BoardFacade facade = new BoardFacade(auth);
            facade.DeleteAllBoards();
            board = new BoardBL("TestBoard", "MayaL@post.bgu.ac.il", 1, 1);
        }

        [Test]
        public void BoardName_BoardNameIsCorrect()
        {
            Assert.AreEqual("TestBoard", board.BoardName);
        }

        [Test]
        public void Owner_GetOwnerEmail_ReturnsCorrectEmail()
        {
            Assert.AreEqual("MayaL@post.bgu.ac.il", board.Owner);
        }

        [Test]
        public void AddUser_UserIsAddedSuccessfully()
        {
            board.AddUser("newuser@post.bgu.ac.il");
            Assert.Contains("newuser@post.bgu.ac.il", board.Users);
        }

        [Test]
        public void AddUser_AddDuplicateUser_ThrowsException()
        {
            board.AddUser("Duplicate@post.bgu.ac.il");
            var ex = Assert.Throws<InvalidOperationException>(() => board.AddUser("Duplicate@post.bgu.ac.il"));
            Assert.That(ex.Message, Is.EqualTo("User Duplicate@post.bgu.ac.il is already a member of TestBoard"));
        }

        [Test]
        public void RemoveUser_ExistingUser_IsRemovedSuccessfully()
        {
            board.AddUser("toRemove@post.bgu.ac.il");
            board.RemoveUser("toRemove@post.bgu.ac.il");
            Assert.IsFalse(board.Users.Contains("toRemove@post.bgu.ac.il"));
        }

        [Test]
        public void RemoveUser_NonExistingUser_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => board.RemoveUser("nonexist@post.bgu.ac.il"));
        }

        [Test]
        public void RemoveUser_OwnerCannotBeRemoved_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => board.RemoveUser("MayaL@post.bgu.ac.il"));
        }

        [Test]
        public void TransferOwnership_ValidUser_Succeeds()
        {
            board.AddUser("newOwner@post.bgu.ac.il");
            board.Owner = "newOwner@post.bgu.ac.il";
            Assert.AreEqual("newOwner@post.bgu.ac.il", board.Owner);
        }

        [Test]
        public void TransferOwnership_NonMemberUser_ThrowsException()
        {
            var ex = Assert.Throws<Exception>(() => board.Owner = "not@post.bgu.ac.il");
            Assert.That(ex.Message, Does.Contain("not@post.bgu.ac.il is not a member"));
        }

        [Test]
        public void TransferOwnership_SameAsCurrentOwner_ThrowsException()
        {
            var ex = Assert.Throws<Exception>(() => board.Owner = "MayaL@post.bgu.ac.il");
            Assert.That(ex.Message, Is.EqualTo("MayaL@post.bgu.ac.il is already the owner of TestBoard"));
        }

        [Test]
        public void AddTask_TaskAppearsInBacklog()
        {
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            Assert.IsTrue(board.Columns[BoardBL.BACKLOG].GetTasks().ContainsKey(task.TaskID));
        }

        [Test]
        public void MoveTask_FromBacklogToInProgress_Succeeds()
        {
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.moveTask(task.TaskID);
            Assert.IsTrue(board.Columns[BoardBL.IN_PROGRESS].GetTasks().ContainsKey(task.TaskID));
        }

        [Test]
        public void MoveTask_FromInProgressToDone_Succeeds()
        {
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.moveTask(task.TaskID);
            board.moveTask(task.TaskID);
            Assert.IsTrue(board.Columns[BoardBL.DONE].GetTasks().ContainsKey(task.TaskID));
        }

        [Test]
        public void MoveTask_FromDoneColumn_ThrowsException()
        {
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.moveTask(task.TaskID);
            board.moveTask(task.TaskID);
            var ex = Assert.Throws<Exception>(() => board.moveTask(task.TaskID));
            Assert.That(ex.Message, Does.Contain("Cant move task forward from Done column"));
        }

        [Test]
        public void MoveTask_NonExistentTask_ThrowsException()
        {
            var ex = Assert.Throws<Exception>(() => board.moveTask(999));
            Assert.That(ex.Message, Does.Contain("Task does not exist"));
        }

        [Test]
        public void DeleteTask_ExistingTask_Succeeds()
        {
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            var result = board.deleteTask(task.TaskID);
            Assert.IsTrue(result);
        }

        [Test]
        public void DeleteTask_NonExistentTask_ReturnsFalse()
        {
            var result = board.deleteTask(999);
            Assert.IsTrue(!result);
        }

        [Test]
        public void ChangeMaxTasks_ValidLimit_UpdatesSuccessfully()
        {
            board.changeMaxTasks(0, 10);
            Assert.AreEqual(10, board.GetColumnLimit(0));
        }

        [Test]
        public void ChangeMaxTasks_LimitBelowCurrentCount_ThrowsException()
        {
            board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.addTask("task2", DateTime.Now.AddDays(3), "desc", 2);
            var ex = Assert.Throws<InvalidOperationException>(() => board.changeMaxTasks(0, 1));
            Assert.That(ex.Message, Does.Contain("Cant lower max tasks of backlog as it currently holds more than 1"));
        }

        [Test]
        public void AssignTask_ValidAssignee_Succeeds()
        {
            board.AddUser("assignee@post.bgu.ac.il");
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.AssignTask("MayaL@post.bgu.ac.il", 0, task.TaskID, "assignee@post.bgu.ac.il");
            Assert.AreEqual("assignee@post.bgu.ac.il", task.Assignee);
        }

        [Test]
        public void AssignTask_AssigneeNotMember_ThrowsException()
        {
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            var ex = Assert.Throws<InvalidOperationException>(() => board.AssignTask("MayaL@post.bgu.ac.il", 0, task.TaskID, "nonAssignee@post.bgu.ac.il"));
            Assert.That(ex.Message, Does.Contain("User nonAssignee@post.bgu.ac.il is a member of TestBoard"));
        }

        [Test]
        public void AssignTask_TaskDoesNotExist_ThrowsException()
        {
            board.AddUser("assignee@post.bgu.ac.il");
            var ex = Assert.Throws<InvalidOperationException>(() => board.AssignTask("MayaL@post.bgu.ac.il", 0, 999, "nonAssignee@post.bgu.ac.il"));
            Assert.That(ex.Message, Does.Contain("User nonAssignee@post.bgu.ac.il is a member of TestBoard"));
        }

        [Test]
        public void AssignTask_AssignerNotAssignee_ThrowsException()
        {
            board.AddUser("assignee1@post.bgu.ac.il");
            board.AddUser("assignee2@post.bgu.ac.il");
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.AssignTask("MayaL@post.bgu.ac.il", 0, task.TaskID, "assignee1@post.bgu.ac.il");
            var ex = Assert.Throws<Exception>(() => board.AssignTask("assignee2@post.bgu.ac.il", 0, 999, "assignee2@post.bgu.ac.il"));
            Assert.That(ex.Message, Does.Contain("Task does not exist"));
        }

        [Test]
        public void TaskFlow_AssignmentAndMovement_WorksCorrectly()
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
        public void TaskFlow_AssignmentAndReassignment_WorksCorrectly()
        {
            board.AddUser("assignee1@post.bgu.ac.il");
            board.AddUser("assignee2@post.bgu.ac.il");
            var task = board.addTask("task1", DateTime.Now.AddDays(3), "desc", 1);
            board.AssignTask("MayaL@post.bgu.ac.il", 0, task.TaskID, "assignee1@post.bgu.ac.il");
            board.AssignTask("assignee1@post.bgu.ac.il", 0, task.TaskID, "assignee2@post.bgu.ac.il");
            Assert.AreEqual("assignee2@post.bgu.ac.il", task.Assignee);
        }

        [Test]
        public void TaskFlow_MaxTasksLimit_ThrowsWhenFull()
        {
            board.changeMaxTasks(0, 2);
            board.addTask("task1", DateTime.Now.AddDays(2), "desc", 1);
            board.addTask("task2", DateTime.Now.AddDays(3), "desc", 2);
            var ex = Assert.Throws<InvalidOperationException>(() => board.addTask("task3", DateTime.Now.AddDays(4), "desc", 3));
            Assert.That(ex.Message, Does.Contain("Column backlog is full"));
        }

        [Test]
        public void OwnershipFlow_TransferAndAddUser_WorksCorrectly()
        {
            board.AddUser("assignee1@post.bgu.ac.il");
            board.Owner = "assignee1@post.bgu.ac.il";
            board.AddUser("assignee2@post.bgu.ac.il");
            Assert.Contains("assignee2@post.bgu.ac.il", board.Users);
        }


    }

}
