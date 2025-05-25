using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    internal class TaskDTO
    {
        private long taskID;
        private long boardID;
        private string title;
        private string desc;
        private DateTime dueDate;
        private DateTime cTime;
        private string asignee;
        private string taskStatus; // maybe change with columnDTO
        public const string TaskIDColumnName = "TaskID";
        public const string BoardIDColumnName = "BoardID";
        public const string TitleColumnName = "Title";
        public const string DescColumnName = "Desc";
        public const string DueDateColumnName = "DueDate";
        public const string CTimeColumnName = "CTime";
        public const string AssigneeColumnName = "Assignee";
        public const string TaskStatusColumnName = "TaskStatus";
        public TaskController taskController { get; set; }
        private bool isPersistent = false;

        //TODO Getters and setters as needed

        public TaskDTO(long taskID, long boardID, string title, DateTime dueDate, DateTime cTime, string desc, string taskStatus) 
        {
            //TODO
        }
    }
}
