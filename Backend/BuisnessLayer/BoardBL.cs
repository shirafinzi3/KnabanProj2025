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
        private readonly Dictionary<String, Column> columns; 
        private long nextTaskID;
        const String BACKLOG = "Backlog";
        const String IN_PROGRESS = "In Progress";
        const String DONE = "Done";

        public BoardBL(string boardName, int[] maxTsaks)
        {
            this.boardName = boardName;
            this.columns = new Dictionary<String, Column>
            {
                 { BACKLOG, new Column(BACKLOG, maxTasks[0]) },
                 { IN_PROGRESS, new Column(IN_PROGRESS, maxTasks[1]) },
                 { DONE, new Column(DONE, maxTasks[2]) }
            }
            this.nextTaskID = 1;
            
        }
        
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
