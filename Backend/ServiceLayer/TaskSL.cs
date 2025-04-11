using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.ServiceLayer
{
    public class TaskSL
    {
        public long Id;
        public String title;
        public String description;
        public String column;
        public DateTime cTime;
        public DateTime dueDate;
        /// <summary>
        /// This constuctor initiates a new TaskSL object.
        /// </summary>
        /// <param name="title">The title of the task</param>
        /// <param name="description">The description of the task</param>
        /// <param name="column">The current column in which the task is currently at</param>
        /// <param name="dueDate">The due date set by the user for the task</param>
        /// <param name="cTime">The creation time in which the task was created </param>
        /// <param name="Id">The unique id of the new task</param>
        public TaskSL(string title, string description, string column, DateTime dueDate, DateTime cTime, long Id)
        {
            this.title = title;
            this.description = description;
            this.column = column;
            this.cTime = cTime;
            this.dueDate = dueDate;
            this.Id = Id;
        }
        public string Title { get; set; }
        public string Description { get; set; } 
        public DateTime CTime { get;}
        public DateTime DueDate { get; set; }
        public string Column {  get; set; }
    }
}
