using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using Backend.ServiceLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using log4net;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    internal class BoardFacade
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<string, Dictionary<string, long>> boardsByEmail;
        private readonly AuthenticationFacade auth;
        private readonly Dictionary<long, BoardBL> boardsById;
        private long nextBoardID;
        private long nextColumnID;
        private long nextTaskID;

        public BoardFacade (AuthenticationFacade auth)
        {
            this.auth = auth;
            this.boardsByEmail = new Dictionary<string, Dictionary<string, long>>();
            this.boardsById = new Dictionary<long, BoardBL>();
            this.nextBoardID = 1;
            this.nextColumnID = 1;
            this.nextTaskID = 1;
        }
        public BoardBL CreateBoard(string email, string boardName)
        {
            if (!auth.IsLoggedIn(email))
            {
                Log.Error($"User {email} is not logged in");
                throw new InvalidOperationException($"User {email} is not logged in");
            }
            if (!boardsByEmail.ContainsKey(email))
            {
                boardsByEmail[email]= new Dictionary<string, long>();
            }
            if (boardsByEmail[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} already exists for user {email}");
                throw new InvalidOperationException($"Board {boardName} already exists for user {email}");
            }
            
            BoardBL newBoard = new BoardBL(boardName,email,nextBoardID++, nextColumnID);
            nextColumnID = nextColumnID + 3;
            boardsByEmail[email][boardName] = newBoard.BoardID;
            boardsById[newBoard.BoardID] = newBoard;
            return newBoard;

        }
        public void DeleteBoard(string email, string boardName)
        {
            emailAuth(email);
            long id = boardsByEmail[email][boardName];
            BoardBL toDelete = boardsById[id];
            bool isRemoveFromEmail = boardsByEmail[email].Remove(boardName);
            bool isRemoveFromID = boardsById.Remove(id);
            if (!isRemoveFromEmail && !isRemoveFromID) 
            {
                Log.Error($"cant delete {boardName} because does not exist");
                throw new InvalidOperationException($"cant delete {boardName} because does not exist");
            }

            toDelete.GetBoardDTO().Delete();
            boardsById.Remove(toDelete.BoardID);
            Log.Info($"Board {boardName} deleted for user {email} - {isRemoveFromID}");
            

        }
        public List<TaskBL> InProgressList(string email)
        {
            emailAuth(email);
            List<TaskBL> inProgressList = new List<TaskBL>();
            foreach (long id in boardsByEmail[email].Values)
            {
                BoardBL board = boardsById[id];
                foreach (TaskBL task in board.Columns[BoardBL.IN_PROGRESS].GetTasks().Values)
                {
                    if(task.Assignee == email)
                    {
                        inProgressList.Add(task);
                    }
                }
            }
            return inProgressList;
        }

        public TaskBL AddTask(string email, string boardName, string title, string desc, DateTime dueDate)
        {
            emailAuth(email);
            if (!boardsByEmail[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            long id = boardsByEmail[email][boardName];
            BoardBL board = boardsById[id];
            if (!board.Users.Contains(email))
            {
                Log.Error($"User {email} is not a member of board {boardName}");
                throw new InvalidOperationException($"User {email} is not a member of board {boardName}");
            }
            return board.addTask(title, dueDate, desc, nextTaskID++);
        }

        public bool DeleteTask(String email, String boardName, long taskID)
        {
            emailAuth(email);
            if (!boardsByEmail[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            long id = boardsByEmail[email][boardName];
            BoardBL board = boardsById[id];
            if (!board.Users.Contains(email))
            {
                Log.Error($"User {email} is not a member of board {boardName}");
                throw new InvalidOperationException($"User {email} is not a member of board {boardName}");
            }
            return board.deleteTask(taskID);
        } 
        public TaskBL UpdateTitle(string email, string boardName, long taskID, string title) 

        {
            TaskBL toChange = GetEditableTask(email,boardName,taskID, "title");
            if(toChange.Assignee != email && toChange.Assignee != null)
            {
                Log.Error($"User {email} is not the assignee for task {toChange.Title}");
                throw new InvalidOperationException($"User {email} is not the assignee for task {toChange.Title}");
            }
            toChange.Title= title;
            return toChange;

        }
        public TaskBL UpdateDesc(string email, string boardName, long taskID, string desc)
        {
            TaskBL toChange = GetEditableTask(email, boardName, taskID, "description");
            if (toChange.Assignee != email && toChange.Assignee!=null && toChange != null )
            {
                Log.Error($"User {email} is not the assignee for task {toChange.Title}");
                throw new InvalidOperationException($"User {email} is not the assignee for task {toChange.Title}");
            }
            toChange.Desc = desc;
            return toChange;
        }
        public TaskBL UpdateDueDate(string email, string boardName, long taskID, DateTime dueDate)
        {
            TaskBL toChange = GetEditableTask(email, boardName, taskID, "due date");
            if (toChange.Assignee != email && toChange.Assignee != null)
            {
                Log.Error($"User {email} is not the assignee for task {toChange.Title}");
                throw new InvalidOperationException($"User {email} is not the assignee for task {toChange.Title}");
            }
            toChange.DueDate = dueDate;
            return toChange;
        }
        public void MoveTask(string email, string boardName, long taskID) 
        {
            emailAuth(email);
            if (!boardsByEmail[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            TaskBL toMove = null;
            long id = boardsByEmail[email][boardName];
            BoardBL board = boardsById[id];
            foreach (Column c in board.Columns.Values)
            {
                if (c.GetTasks().ContainsKey(taskID))
                {
                    toMove = c.GetTasks()[taskID];
                    break;
                }
            }
            if (toMove == null)
            {
                Log.Error($"task {taskID} does not exist");
                throw new InvalidOperationException($"task {taskID} does not exist");
            }
            if (toMove.Assignee != email && toMove.Assignee != null)
            {
                Log.Error($"User {email} is not the assignee for task {toMove.Title}");
                throw new InvalidOperationException($"User {email} is not the assignee for task {toMove.Title}");
            }
            board.moveTask( taskID);
        }

        public void ChangeMaxTasks(string email, string boardName, int colIdx, int newLim)
        {
            emailAuth(email);
            if (!boardsByEmail[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            long id = boardsByEmail[email][boardName];
            BoardBL board = boardsById[id];
            board.changeMaxTasks(colIdx, newLim);
        }
        public int GetColumnLimit(string email, string boardName, int colIdx)
        {
            emailAuth(email);
            if (!boardsByEmail[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            long id = boardsByEmail[email][boardName];
            BoardBL board = boardsById[id];
            return board.GetColumnLimit(colIdx);
        }
        public string GetColumnName(string email, string boardName, int colIdx)
        {
            emailAuth(email);
            if (!boardsByEmail[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            long id = boardsByEmail[email][boardName];
            BoardBL board = boardsById[id];
            return board.GetColumnName(colIdx);
        }
        public Dictionary<long,TaskBL> GetColumn(string email, string boardName, int colIdx)
        {
            emailAuth(email);
            if (!boardsByEmail[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            long id = boardsByEmail[email][boardName];
            BoardBL board = boardsById[id];
            return board.GetColumn(colIdx);
        }
        public void JoinBoard(string email, long boardID)
        {
            emailAuth(email);
            if (!boardsById.ContainsKey(boardID))
            {
                Log.Error($"Board deos not exists for user {email}");
                throw new InvalidOperationException($"Board does not exists for user {email}");
            }
            BoardBL toJoin = boardsById[boardID];
            if (boardsByEmail[email].ContainsKey(toJoin.BoardName))
            {
                Log.Error($"Board {toJoin.BoardName} already exists for user {email}");
                throw new InvalidOperationException($"Board {toJoin.BoardName} already exists for user {email}");
            }
            toJoin.AddUser(email);
            boardsByEmail[email].Add(toJoin.BoardName, toJoin.BoardID);
            Log.Info($"User {email} joined board {toJoin.BoardName}");
        }
        public void LeaveBoard(string email, long boardID)
        {
            emailAuth(email);
            if (!boardsById.ContainsKey(boardID))
            {
                Log.Error($"Board does not exists for user {email}");
                throw new InvalidOperationException($"Board does not exists for user {email}");
            }
            BoardBL toLeave = boardsById[boardID];
            toLeave.RemoveUser(email);
            boardsByEmail[email].Remove(toLeave.BoardName);
            Log.Info($"User {email} joined board {toLeave.BoardName}");

        }
        public void TransferOwnership(string email, string boardName, string otherEmail)
        {
            emailAuth(email);
            if (!boardsByEmail[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            long id = boardsByEmail[email][boardName];
            BoardBL toTransfer = boardsById[id];
            toTransfer.Owner = otherEmail;
            Log.Info($"Ownership for board {toTransfer.BoardName} transferd from {email} to {otherEmail}");
        }
        public void AssignTask(string email, string boardName, int colIDX, long taskID, string emailAssignee)
        {
            emailAuth(email);
            if (!boardsByEmail[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            long id = boardsByEmail[email][boardName];
            BoardBL board = boardsById[id];
            board.AssignTask(email, colIDX, taskID, emailAssignee);
            Log.Info($"User {email} assigned task to user {emailAssignee}");
        }
        public string GetBoardName(long boardID)
        {
            if (!boardsById.ContainsKey(boardID))
            {
                Log.Error($"Board does not exists");
                throw new InvalidOperationException($"Board does not exist");
            }
            BoardBL board = boardsById[boardID];
            return board.BoardName;
        }
        public List<long> GetUserBoards(string email)
        {
            emailAuth(email);
            List<long> ids = new List<long>();
            foreach (long id in boardsByEmail[email].Values)
            {
                ids.Add(id);
            }
            return ids;
        }
        private TaskBL GetEditableTask(string email, string boardName, long taskID, string field)
        {
            TaskBL toReturn = null;
            Column column = null;
            emailAuth(email);
            if (!boardsByEmail[email].ContainsKey(boardName))
            {
                Log.Error($"Board {boardName} does not exist");
                throw new KeyNotFoundException($"Board {boardName} does not exist");
            }
            long id = boardsByEmail[email][boardName];
            BoardBL board = boardsById[id];
            foreach (Column c in board.Columns.Values)
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
            if (!boardsByEmail.ContainsKey(email))
            {
                Log.Error($"User {email} does not exist, or has no boardsByEmail");
                throw new KeyNotFoundException($"User {email} does not exist or has no boardsByEmail");
            }
        }

        public void LoadAllBoards()
        {
            BoardController boardController = new BoardController();
            ColumnController columnController = new ColumnController();
            TaskController taskController = new TaskController();

            List<BoardDTO> bDTOs = boardController.SelectAll();

            //Load Boards
            foreach (BoardDTO bDTO in bDTOs)
            {
                foreach(string email in bDTO.Users)
                {
                    if (!boardsByEmail.ContainsKey(email))
                    {
                        boardsByEmail[email] = new Dictionary<string, long>();
                    }
                    boardsByEmail[email][bDTO.Name] = bDTO.BoardID;
                }
                BoardBL board = new BoardBL(bDTO);
                boardsById[bDTO.BoardID] = board;
            }
            //Load Columns
            foreach(BoardDTO bDTO in bDTOs)
            {
                BoardBL board = boardsById[bDTO.BoardID];
                List<ColumnDTO> cDtos = columnController.SelectColumnByBoard(bDTO.BoardID);

                foreach(ColumnDTO cDTO in cDtos)
                {
                    board.AddLoadedColumn(new Column(cDTO));
                }
            }

            foreach(BoardBL board in boardsById.Values)
            {
                foreach(Column col in board.Columns.Values)
                {
                    List<TaskDTO> tDTOs = taskController.SelectTaskByColumn(col.ColumnID);
                    foreach(TaskDTO tDTO in tDTOs)
                    {
                        col.AddLoadedTask(new TaskBL(tDTO));
                    }
                }
            }
            this.nextBoardID = boardController.SelectMaxBoardID() + 1;
            this.nextColumnID = columnController.SelectMaxColumnID() + 1;
            this.nextTaskID = taskController.SelectMaxTaskID() + 1;
            Log.Info("Board data uploaded from database");
        }

        public void DeleteAllBoards()
        {
            /*BoardController boardController = new BoardController();
            List<BoardDTO> bDTOs = boardController.SelectAll(); 
            foreach (BoardDTO boardDTO in bDTOs)
            {
                boardDTO.Delete();
            }
            boardsByEmail.Clear();
            boardsById.Clear();
            Log.Info("Board data deleted from database");*/
            BoardController boardController = new BoardController();
            boardController.DeleteAll();
            boardsById.Clear();
            boardsByEmail.Clear();
            Log.Info("All boards deleted from database");
        }
    }
}
