using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using log4net;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    internal class BoardFacade
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<string, Dictionary<string, BoardBL>> boards = new Dictionary<string, Dictionary<string, BoardBL>>();
        private readonly AuthenticationFacade auth = new AuthenticationFacade();
        public BoardBL CreateBoard(string email, string boardName, int? maxTasks)
        {
            return null;
        }
        public bool DeleteBoard(string email, string boardName)
        {
            return false;
        }
        public Dictionary<string, BoardBL> GetAllBoards(string email) 
        {
            return null;
        }
        public List<TaskBL> InProgressList(string email)
        {
            if (!auth.IsLoggedIn(email))
            {
                Log.Error($"User {email} is not logged in");
                throw new InvalidOperationException($"User {email} is not logged in");
            }
            if (!boards.ContainsKey(email))
            {
                Log.Error($"User {email} does not exist");
                throw new KeyNotFoundException($"User {email} does not exist");
            }
            List<TaskBL> inProgressList = new List<TaskBL>();
            foreach (BoardBL board in boards[email].Values)
            {
                foreach (TaskBL task in board.Columns[BoardBL.IN_PROGRESS].tasks.Values)
                {
                    inProgressList.Add(task);
                }
            }
            return inProgressList;
        }

        public TaskBL AddTask(string email, string boardName, string title, string desc, DateTime dueDate)
        {
            if (!auth.IsLoggedIn(email))
            {
                Log.Error($"User {email} is not logged in"); //maybe make method that checks these first two and we can  
                throw new InvalidOperationException($"User {email} is not logged in");//put in front of every method that needs it
            }
            if (!boards.ContainsKey(email))
            {
                Log.Error($"User {email} does not exist");
                throw new KeyNotFoundException($"User {email} does not exist");
            }
            if (!boards[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            BoardBL board = boards[email][boardName];
            return board.addTask(title, dueDate, desc);
        }

        public bool DeleteTask(String email, String boardName, long taskID)
        {
            if (!auth.IsLoggedIn(email))
            {
                Log.Error($"User {email} is not logged in"); 
                throw new InvalidOperationException($"User {email} is not logged in");
            }
            if (!boards.ContainsKey(email))
            {
                Log.Error($"User {email} does not exist");
                throw new KeyNotFoundException($"User {email} does not exist");
            }
            if (!boards[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            BoardBL board = boards[email][boardName];
            return board.deleteTask(taskID);
        } 
        public TaskBL UpdateTitle(string email, string boardName, long taskID, string title) 

        {
            TaskBL toChange = GetEditableTask(email,boardName,taskID, "title");
            toChange.Title= title;
            return toChange;

        }
        public TaskBL UpdateDesc(string email, string boardName, long taskID, string desc)
        {
            TaskBL toChange = GetEditableTask(email, boardName, taskID, "description");
            toChange.Desc = desc;
            return toChange;
        }
        public TaskBL UpdateDueDate(string email, string boardName, long taskID, DateTime dueDate)
        {
            TaskBL toChange = GetEditableTask(email, boardName, taskID, "due date");
            toChange.DueDate = dueDate;
            return toChange;
        }
        public bool MoveTask(string email, string boardName, long taskID) 
        {
            if (!auth.IsLoggedIn(email))
            {
                Log.Error($"User {email} is not logged in");
                throw new InvalidOperationException($"User {email} is not logged in");
            }
            if (!boards.ContainsKey(email))
            {
                Log.Error($"User {email} does not exist");
                throw new KeyNotFoundException($"User {email} does not exist");
            }
            if (!boards[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            BoardBL board = boards[email][boardName];
            return board.moveTask(taskID);
        }
        private TaskBL GetEditableTask(string email, string boardName, long taskID, string field)
        {
            TaskBL toReturn = null;
            Column column = null;
            if (!auth.IsLoggedIn(email))
            {
                Log.Error($"User {email} is not logged in");
                throw new InvalidOperationException($"User {email} is not logged in");
            }
            if (!boards.ContainsKey(email))
            {
                Log.Error($"User {email} does not exist");
                throw new KeyNotFoundException($"User {email} does not exist");
            }
            if (!boards[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            foreach(Column c in boards[email][boardName].Columns.Values)
            {
                if (c.tasks.ContainsKey(taskID))
                {
                    toReturn = c.tasks[taskID];
                    column = c;
                    break;
                }
            }
            if(toReturn == null)
            {
                Log.Error($"TaskID {taskID} does not exist in board {boardName}");
                throw new KeyNotFoundException($"TaskID {taskID} does not exist in board {boardName}");
            }
            if (column.columnName.Equals("Done"))
            {
                Log.Error($"Can not update the {field} of a task that is already done");
                throw new InvalidOperationException($"Can not update the {field} of a task that is already done");
            }
            return toReturn;
        }

    }
}
