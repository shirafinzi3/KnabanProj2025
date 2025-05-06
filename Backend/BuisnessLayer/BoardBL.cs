using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using log4net;

namespace Backend.BuisnessLayer
{
    internal class BoardBL
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string boardName;
        private Dictionary<String, Column> columns;
        private long nextTaskID;
        public const String BACKLOG = "backlog";
        public const String IN_PROGRESS = "in progress";
        public const String DONE = "done";
        public Dictionary<string,Column> Columns
        {
            get { return columns; }
        }
      
        public BoardBL(string boardName, int[] maxTasks)
        {
            this.BoardName = boardName;
            this.columns = new Dictionary<String, Column>
            {
                 { BACKLOG, new Column(BACKLOG, maxTasks[0]) },
                 { IN_PROGRESS, new Column(IN_PROGRESS, maxTasks[1]) },
                 { DONE, new Column(DONE, maxTasks[2]) }
            };
            this.nextTaskID = 1;
   
        }
        public BoardBL(string boardName)
        {
            this.BoardName = boardName;
            this.columns = new Dictionary<String, Column>
            {
                 { BACKLOG, new Column(BACKLOG, -1) },
                 { IN_PROGRESS, new Column(IN_PROGRESS, -1) },
                 { DONE, new Column(DONE, -1) }
            };
            this.nextTaskID = 1;
        }

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
            }

        }
       
        public TaskBL addTask(string title, DateTime dueDate, string desc)
        {
            TaskBL task = new TaskBL(title, dueDate, desc, nextTaskID++);
            columns[BACKLOG].Add(task);
            return task;
        }

        public void moveTask(long taskID) //taskBL or taskID?
        {
            if (columns[BACKLOG].tasks.ContainsKey(taskID))
            {
                TaskBL task = columns[BACKLOG].tasks[taskID];
                columns[IN_PROGRESS].Add(task); //will throw exception if no space
                columns[BACKLOG].Remove(taskID);
                Log.Info($"Task {taskID} was successfully moved to in progress");
            }
            else if (columns[IN_PROGRESS].tasks.ContainsKey(taskID))
            {
                TaskBL task = columns[IN_PROGRESS].tasks[taskID];
                columns[DONE].Add(task); //will throw exception if no space
                columns[IN_PROGRESS].Remove(taskID);
                Log.Info($"Task {taskID} was successfully moved to done");

            }
            else if (columns[DONE].tasks.ContainsKey(taskID))
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
                if (col.Remove(taskID))
                {
                    return true;
                }
            }
            return false;
        }

        public void changeMaxTasks(int colIdx, int newLim)
        {
            if (colIdx == 0) { columns[BACKLOG].MaxTasks = newLim; }
            else if (colIdx == 1) { columns[IN_PROGRESS].MaxTasks = newLim; }
            else if (colIdx == 2) { columns[DONE].MaxTasks = newLim; }
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
    }
}
