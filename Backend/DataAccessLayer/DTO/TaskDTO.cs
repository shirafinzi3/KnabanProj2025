using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;
using log4net;
using Microsoft.VisualBasic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    internal class TaskDTO
    {
        private long taskID;
        private long columnID;
        private string title;
        private string desc;
        private DateTime dueDate;
        private DateTime cTime;
        private string assignee;
        
        public const string TaskIDColumnName = "TaskID";
        public const string ColumnIDColumnName = "ColumnID";
        public const string TitleColumnName = "Title";
        public const string DescColumnName = "Desc";
        public const string DueDateColumnName = "DueDate";
        public const string CTimeColumnName = "CTime";
        public const string AssigneeColumnName = "Assignee";
        public TaskController taskController { get; set; }
        private bool isPersistent = false;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public long TaskID
        {
            get => taskID;
        }
        public string Title
        {
            get => title;
            set
            {
                if (isPersistent)
                {
                    taskController.Update(taskID, TitleColumnName, value);
                }
                title = value;
            }
        }

        public string Desc
        {
            get => desc;
            set
            {
                if (isPersistent)
                {
                    taskController.Update(taskID, DescColumnName, value);
                }
                desc = value;
            }
        }

        public string Assignee
        {
            get => assignee;
            set
            {
                if (isPersistent)
                {
                    taskController.Update(taskID, AssigneeColumnName, value);
                }
                assignee = value;
            }
        }

        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                if (isPersistent)
                {
                    taskController.Update(taskID, DueDateColumnName, value);
                }
                dueDate = value;
            }
        }

        public DateTime CTime
        {
            get => cTime;
        }

        public long ColumnID
        {
            get => columnID;
            set
            {
                if (isPersistent)
                {
                    taskController.MoveTask(taskID, ColumnIDColumnName, value);
                }
                columnID = value;
            }
        }

        public TaskDTO(long taskID, string title, string desc, DateTime dueDate, DateTime cTime, string assignee) 
        {
            this.taskID = taskID;
            this.title = Title;
            this.desc = Desc;
            this.dueDate = DueDate;
            this.cTime = cTime;
            this.assignee = Assignee;
            taskController = new TaskController();
        }

        public TaskDTO(long taskID, long columnID, string title, string desc, DateTime dueDate, DateTime cTime, string assignee)
        {
            this.taskID = taskID;
            this.columnID = columnID;
            this.title = Title;
            this.desc = desc;
            this.dueDate = dueDate;
            this.cTime = cTime;
            this.assignee = assignee;
            taskController = new TaskController();
        }

        public void Save()
        {
            if (isPersistent)
            {
                throw new InvalidOperationException("Cannot save persisted object");
            }
        }

        public void Save(long columnID)
        {
            if (isPersistent)
            {
                throw new InvalidOperationException("Cannot save persisted object");
            }
            this.columnID = columnID;
            taskController.Insert(this);
            Log.Info("Task data saved to database");
            isPersistent = true;
        }
    }
}
