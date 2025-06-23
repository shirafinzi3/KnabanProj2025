using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.ServiceLayer;

namespace IntroSE.Kanban.Frontend.Model
{
    internal class TaskModel
    {
        public long Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public DateTime CTime { get; set; }
        public DateTime DueDate { get; set; }
        public string Assignee { get; set; }
        public TaskModel(TaskSL task)
        {
            this.Id = task.Id;
            this.Title = task.Title;
            if (task.Description == null)
            {
                this.Description = "";
            }
            else
            {
                this.Description = task.Description;
            }
            this.CTime = task.CTime;
            this.DueDate = task.DueDate;
            if (task.assignee == null)
            {
                this.Assignee = "Unassigned";
            }
            else
            {
                this.Assignee = task.assignee;
            }  
        }
    }
}
