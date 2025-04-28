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
        const String BACKLOG = "Backlog";
        const String IN_PROGRESS = "In Progress";
        const String DONE = "Done";

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

    }
}
