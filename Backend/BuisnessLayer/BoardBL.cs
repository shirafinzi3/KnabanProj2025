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
        public const String BACKLOG = "Backlog";
        public const String IN_PROGRESS = "In Progress";
        public const String DONE = "Done";
        public Dictionary<string,Column> Columns
        {
            get { return columns; }
        }
      


        public BoardBL(string boardName, int[] maxTasks)
        {
            this.boardName = boardName;
            this.columns = new Dictionary<String, Column>
            {
                 { BACKLOG, new Column(BACKLOG, maxTasks[0]) },
                 { IN_PROGRESS, new Column(IN_PROGRESS, maxTasks[1]) },
                 { DONE, new Column(DONE, maxTasks[2]) }
            };
            this.nextTaskID = 1;
    
        }

        public string BoardName
        {
            get { return boardName; }
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
    }
}
