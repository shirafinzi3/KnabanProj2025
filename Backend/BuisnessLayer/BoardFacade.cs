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
        public BoardBL CreateBoard(string email, string boardName, int[] maxTasks)
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
            if (boards[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} already exists for user {email}");
                throw new InvalidOperationException($"Board {boardName} already exists for user {email}");
            }
            
            BoardBL newBoard = new BoardBL(boardName, maxTasks);
            boards[email][boardName] = newBoard;
            return newBoard;

        }
        public bool DeleteBoard(string email, string boardName)
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
            
            bool isRemove= boards[email].Remove(boardName);
            Log.Info($"Board {boardName} deleted for user {email} - {isRemove}");
            return isRemove;
        }
        public Dictionary<string, BoardBL> GetAllBoards(string email) 
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
            return boards[email];
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
                if (board.Tasks.ContainsKey("in progress"))
                {
                    foreach (TaskBL task in board.Tasks["in progress"].Values)
                    {
                        inProgressList.Add(task);
                    }
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
        public TaskBL UpdateTitle(string email, string boardName, long taskID, string title, string column) 
        {
            TaskBL toChange = GetEditableTask(email,boardName,taskID, "title" ,column);
            toChange.Title= title;
            return toChange;

        }
        public TaskBL UpdateDesc(string email, string boardName, long taskID, string desc, string column)
        {
            TaskBL toChange = GetEditableTask(email, boardName, taskID, "description", column);
            toChange.Desc = desc;
            return toChange;
        }
        public TaskBL UpdateDueDate(string email, string boardName, long taskID, DateTime dueDate,string column)
        {
            TaskBL toChange = GetEditableTask(email, boardName, taskID, "due date", column);
            toChange.DueDate = dueDate;
            return toChange;
        }
        public TaskBL MoveTask(string email, string boardName, long taskID) 
        {
            return null;
        }
        private TaskBL GetEditableTask(string email, string boardName, long taskID, string field, string column)
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
            if (column == "done")
            {
                Log.Error($"Can not update the {field} of a task that is already done");
                throw new InvalidOperationException($"Can not update the {field} of a task that is already done");
            }
            return boards[email][boardName].Tasks[column][taskID];

        }

    }
}
