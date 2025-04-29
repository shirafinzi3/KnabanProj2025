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
        private readonly Dictionary<String, Column> columns; //Board constructor should initialize this with below const being keys
        private int[] maxTasks; //need to check if need
        private long nextTaskID; //initialize as 1 in constructor
        public const String BACKLOG = "Backlog";
        public const String IN_PROGRESS = "In Progress";
        public const String DONE = "Done";

        //Maya In the constructor pls make sure to intialize columns with the three constants as the keys, and a new Column 
        //for each key :) thats how I wrote columns to work 
        //Also pls intitalize nextTaskID as 1 in the constructor, thank you!! <3
        public string BoardName
        {
            get;
        }
        public Dictionary<string, Column> Columns
        {
            get;
        }

        public TaskBL addTask(string title, DateTime dueDate, string desc)
        {
            TaskBL task = new TaskBL(title, dueDate, desc, nextTaskID++);
            columns[BACKLOG].Add(task);
            return task;
        }

        public bool moveTask(long taskID) //taskBL or taskID?
        {
            if (columns[BACKLOG].tasks.ContainsKey(taskID))
            {
                TaskBL task = columns[BACKLOG].tasks[taskID];
                columns[IN_PROGRESS].Add(task); //will throw exception if no space
                columns[BACKLOG].Remove(taskID);
                return true; //maybe have method be void bc it either throws exception bc no space or moves it.
            }
            else if (columns[IN_PROGRESS].tasks.ContainsKey(taskID))
            {
                TaskBL task = columns[IN_PROGRESS].tasks[taskID];
                columns[DONE].Add(task); //will throw exception if no space
                columns[IN_PROGRESS].Remove(taskID);
                return true;
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
