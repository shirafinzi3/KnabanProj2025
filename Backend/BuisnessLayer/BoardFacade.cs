using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using Backend.ServiceLayer;
using log4net;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    internal class BoardFacade
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<string, Dictionary<string, BoardBL>> boards;
        private readonly AuthenticationFacade auth;

        public BoardFacade (AuthenticationFacade auth)
        {
            this.auth = auth;
            this.boards = new Dictionary<string, Dictionary<string, BoardBL>>();
        }
        public BoardBL CreateBoard(string email, string boardName)
        {
            if (!auth.IsLoggedIn(email))
            {
                Log.Error($"User {email} is not logged in");
                throw new InvalidOperationException($"User {email} is not logged in");
            }
            if (!boards.ContainsKey(email))
            {
                boards[email]= new Dictionary<string, BoardBL>();
            }
            if (boards[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} already exists for user {email}");
                throw new InvalidOperationException($"Board {boardName} already exists for user {email}");
            }
            
            BoardBL newBoard = new BoardBL(boardName);
            boards[email][boardName] = newBoard;
            return newBoard;

        }
        public void DeleteBoard(string email, string boardName)
        {
            emailAuth(email);
            
            bool isRemove= boards[email].Remove(boardName);
            if (!isRemove) 
            {
                Log.Error($"cant delete {boardName} because does not exist");
                throw new InvalidOperationException($"cant delete {boardName} because does not exist");
            }
            Log.Info($"Board {boardName} deleted for user {email} - {isRemove}");

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
               return new Dictionary<string, BoardBL>();
            }
            return boards[email];
        }
        public List<TaskBL> InProgressList(string email)
        {
            emailAuth(email);
            List<TaskBL> inProgressList = new List<TaskBL>();
            foreach (BoardBL board in boards[email].Values)
            {
                foreach (TaskBL task in board.Columns[BoardBL.IN_PROGRESS].GetTasks().Values)
                {
                    inProgressList.Add(task);
                }
            }
            return inProgressList;
        }

        public TaskBL AddTask(string email, string boardName, string title, string desc, DateTime dueDate)
        {
            emailAuth(email);
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
            emailAuth(email);
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
        public void MoveTask(string email, string boardName, long taskID) 
        {
            emailAuth(email);
            if (!boards[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            BoardBL board = boards[email][boardName];
            board.moveTask(taskID);
        }

        public void ChangeMaxTasks(string email, string boardName, int colIdx, int newLim)
        {
            emailAuth(email);
            if (!boards[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            BoardBL board = boards[email][boardName];
            board.changeMaxTasks(colIdx, newLim);
        }
        public int GetColumnLimit(string email, string boardName, int colIdx)
        {
            emailAuth(email);
            if (!boards[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            BoardBL board = boards[email][boardName];
            return board.GetColumnLimit(colIdx);
        }
        public string GetColumnName(string email, string boardName, int colIdx)
        {
            emailAuth(email);
            if (!boards[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            BoardBL board = boards[email][boardName];
            return board.GetColumnName(colIdx);
        }
        public Dictionary<long,TaskBL> GetColumn(string email, string boardName, int colIdx)
        {
            emailAuth(email);
            if (!boards[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            BoardBL board = boards[email][boardName];
            return board.GetColumn(colIdx);
        }

        private TaskBL GetEditableTask(string email, string boardName, long taskID, string field)
        {
            TaskBL toReturn = null;
            Column column = null;
            emailAuth(email);
            if (!boards[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            BoardBL board = boards[email][boardName];
            foreach(Column c in board.Columns.Values)
            {
                if (c.GetTasks().ContainsKey(taskID))
                {
                    toReturn = c.GetTasks()[taskID];
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

        private void emailAuth(String email)
        {
            if (!auth.IsLoggedIn(email))
            {
                Log.Error($"User {email} is not logged in");
                throw new InvalidOperationException($"User {email} is not logged in");
            }
            if (!boards.ContainsKey(email))
            {
                Log.Error($"User {email} does not exist, or has no boards");
                throw new KeyNotFoundException($"User {email} does not exist or has no boards");
            }
        }

    }
}
