using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using log4net;

namespace Backend.BuisnessLayer
{
    internal class BoardBL
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string boardName;
        private readonly Dictionary<String, Column> columns;
        private readonly List<string> users = new List<string>();
        private long boardID;
        private string owner { get; set; }
        public const String BACKLOG = "backlog";
        public const String IN_PROGRESS = "in progress";
        public const String DONE = "done";
        private BoardDTO bDTO;
        public Dictionary<string,Column> Columns
        {
            get { return columns; }
        }
        public BoardBL(string boardName, string owner, long boardID, long startColID)
        {
            bDTO = new BoardDTO(boardID, boardName, owner);
            this.BoardName = boardName;
            this.columns = new Dictionary<String, Column>
            {
                 { BACKLOG, new Column(BACKLOG, -1, startColID) },
                 { IN_PROGRESS, new Column(IN_PROGRESS, -1 ,startColID+1) },
                 { DONE, new Column(DONE, -1, startColID+2) }
            };
            bDTO.AddColumn(columns[BACKLOG].ColumnDTO);
            bDTO.AddColumn(columns[IN_PROGRESS].ColumnDTO);
            bDTO.AddColumn(columns[DONE].ColumnDTO);
            bDTO.AddUser(owner);
            this.owner = owner;
            this.users.Add(owner);
            this.boardID = boardID;
            bDTO.Save();
        }

        public BoardBL(BoardDTO bDTO)
        {
            this.bDTO = bDTO;
            this.boardName = bDTO.Name;
            this.boardID = bDTO.BoardID;
            this.owner = bDTO.OwnerEmail;
            this.users = new List<string>(bDTO.Users);
            this.columns= new Dictionary<String, Column>();
        }

        public BoardDTO BoardDTO 
        {
            get => bDTO; 
        }
        public void AddLoadedColumn(Column col)
        {
            columns[col.columnName] = col;
        }
        public long BoardID {  get { return boardID; } }
        public string Owner 
        { 
            get => owner;
            set
            {
                if (value.Equals(this.Owner))
                {
                    Log.Error($"{value} is already the owner of {this.boardName}");
                    throw new Exception($"{value} is already the owner of {this.boardName}");
                }
                if (!this.users.Contains(value))
                {
                    Log.Error($"{value} is not a member of {this.boardName}");
                    throw new Exception($"{value} is not a member of {this.boardName}");
                }
                bDTO.OwnerEmail = value;
                owner = value;
            }
        }
        public List<string> Users { get { return users; } }

        public string BoardName
        {
            get=> boardName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Log.Error("Provided title is null or empty");
                    throw new Exception("Provided title is null or empty");
                }
                bDTO.Name = value;
                boardName = value;
            }

        }
       
       
        public TaskBL addTask(string title, DateTime dueDate, string desc, long taskID)
        {
            TaskBL task = new TaskBL(title, dueDate, desc, taskID);
            columns[BACKLOG].Add(task);
            return task;
        }

        public void moveTask(long taskID) //taskBL or taskID?
        {
            if (columns[BACKLOG].GetTasks().ContainsKey(taskID))
            {
                TaskBL task = columns[BACKLOG].GetTasks()[taskID];
                columns[IN_PROGRESS].Add(task); //will throw exception if no space
                columns[BACKLOG].Remove(task);
                Log.Info($"Task {taskID} was successfully moved to in progress");
            }
            else if (columns[IN_PROGRESS].GetTasks().ContainsKey(taskID))
            {
                TaskBL task = columns[IN_PROGRESS].GetTasks()[taskID];
                columns[DONE].Add(task); //will throw exception if no space
                columns[IN_PROGRESS].Remove(task);
                Log.Info($"Task {taskID} was successfully moved to done");

            }
            else if (columns[DONE].GetTasks().ContainsKey(taskID))
            {
                Log.Error("Cant move task forward from Done column");
                throw new Exception("Cant move task forward from Done column");
            }
            else
            {
                Log.Error("Task does not exist");
                throw new Exception("Task does not exist");
            }
        }
        public bool deleteTask(long taskID)
        {
            foreach (Column col in columns.Values)
            { 
                if (col.GetTasks().ContainsKey(taskID))
                {
                    TaskBL toRemove = col.GetTasks()[taskID];
                    toRemove.TaskDTO.Delete();
                    return col.GetTasks().Remove(taskID);
                }
            }
            return false;
        }

        public void changeMaxTasks(int colIdx, int newLim)
        {
            if (colIdx == 0) 
            {
                columns[BACKLOG].ColumnDTO.MaxTasks = newLim;
                columns[BACKLOG].MaxTasks = newLim; 
            }
            else if (colIdx == 1) 
            {
                columns[IN_PROGRESS].ColumnDTO.MaxTasks = newLim;
                columns[IN_PROGRESS].MaxTasks = newLim; 
            }
            else if (colIdx == 2)
            {
                columns[DONE].ColumnDTO.MaxTasks = newLim;
                columns[DONE].MaxTasks = newLim; 
            }
            else
            {
                Log.Error($"Column index {colIdx} is invalid");
                throw new InvalidOperationException($"Column index {colIdx} is invalid");
            }
        }
        public int GetColumnLimit(int colIdx)
        {
            if (colIdx == 0) { return columns[BACKLOG].MaxTasks;  }
            if (colIdx == 1) { return columns[IN_PROGRESS].MaxTasks; }
            if (colIdx == 2) { return columns[DONE].MaxTasks; }
            Log.Error($"Column index {colIdx} is invalid");
            throw new InvalidOperationException($"Column index {colIdx} is invalid");

        }
        public string GetColumnName(int colIdx)
        {
            if (colIdx == 0) { return BACKLOG; }
            if (colIdx == 1) { return IN_PROGRESS; }
            if (colIdx == 2) { return DONE; }
            Log.Error($"Column index {colIdx} is invalid");
            throw new InvalidOperationException($"Column index {colIdx} is invalid");
        }
        public Dictionary<long,TaskBL> GetColumn(int colIdx)
        {
            if (colIdx == 0) { return columns[BACKLOG].GetTasks(); }
            if (colIdx == 1) { return columns[IN_PROGRESS].GetTasks(); }
            if (colIdx == 2) { return columns[DONE].GetTasks(); }
            Log.Error($"Column index {colIdx} is invalid");
            throw new InvalidOperationException($"Column index {colIdx} is invalid");
        }
        public void AddUser(string email)
        {
            if (this.users.Contains(email))
            {
                Log.Error($"User {email} is already a member of {this.BoardName}");
                throw new InvalidOperationException($"User {email} is already a member of {this.BoardName}");
            }
            bDTO.AddUser(email);
            this.users.Add(email);
        }
        public void RemoveUser(string email)
        {
            if (!this.users.Contains(email))
            {
                Log.Error($"User {email} is not a member of {this.BoardName}");
                throw new InvalidOperationException($"User {email} is not a member of {this.BoardName}");
            }
            if (this.owner == email)
            {
                Log.Error($"User {email} is the owner of {this.BoardName} and can not leave");
                throw new InvalidOperationException($"User {email} is the owner of {this.BoardName} and can not leave");
            }
            bDTO.RemoveUser(email);
            this.users.Remove(email);
        }
        public void AssignTask(string email, int colIDX, long taskID, string emailAssignee)
        {
            if (!this.users.Contains(emailAssignee))
            {
                Log.Error($"User {emailAssignee} is a member of {this.BoardName}");
                throw new InvalidOperationException($"User {emailAssignee} is a member of {this.BoardName}");
            }
            string columnName = null;
            if (colIDX == 0) { columnName = BACKLOG; }
            else if (colIDX == 1) { columnName = IN_PROGRESS; }
            else if (colIDX == 2) { columnName = DONE; }
            else
            {
                Log.Error($"Column index {colIDX} is not a valid column index");
                throw new InvalidOperationException($"Column index {colIDX} is not a valid column index");
            }
            if (!columns[columnName].GetTasks().ContainsKey(taskID))
            {
                Log.Error("Task does not exist");
                throw new Exception("Task does not exist");
            }
            TaskBL toAssign = columns[columnName].GetTasks()[taskID];
            if(toAssign.Assignee != email && toAssign.Assignee != null)
            {
                Log.Error($"User {email} is not the assignee of the task");
                throw new Exception($"User {email} is not the assignee of the task");
            }
            toAssign.Assignee = emailAssignee;
        }
    }
}
