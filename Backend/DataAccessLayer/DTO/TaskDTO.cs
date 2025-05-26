using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;
using Microsoft.VisualBasic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    internal class TaskDTO
    {
        private long taskID { get; }
        private long columnID { get; }
        private string title;
        private string desc;
        private DateTime dueDate;
        private DateTime cTime { get;  }
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

        public string Title
        {
            get => title;
            set
            {
                title = value;
            }
        }

        public string Desc
        {
            get => desc;
            set
            {
                desc = value;
            }
        }

        public string Assignee
        {
            get => assignee;
            set
            {
                assignee = value;
            }
        }

        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value;
            }
        }

        public TaskDTO(long taskID, long columnID, string title, string desc, DateTime dueDate, DateTime cTime, string assignee) 
        {
            this.taskID = taskID;
            this.columnID = columnID;
            this.title = Title;
            this.desc = Desc;
            this.dueDate = DueDate;
            this.cTime = cTime;
            this.assignee = Assignee;
            taskController = new TaskController();
        }

        public void save()
        {
            taskController.Insert(this);
            isPersistent = true;
        }
    }
}
