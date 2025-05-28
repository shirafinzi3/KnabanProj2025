using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using log4net;

namespace Backend.BuisnessLayer
{
    internal class TaskBL
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly long taskID;
        private readonly DateTime cTime;
        private string title;
        private DateTime dueDate;
        private string desc;
        private string assignee;
        private TaskDTO tDTO;
        public const int DESC_LIM = 300;
        public const int TITLE_LIM = 50;
        public TaskBL(string title, DateTime dueDate, string desc, long id)
        {
            tDTO = new TaskDTO(id, title, desc, dueDate, cTime, assignee);
            this.Title = title;
            this.DueDate = dueDate;
            this.Desc = desc;
            this.taskID = id;
            this.CTime = DateTime.Now;
            tDTO.CTime = this.cTime; 
            this.Assignee = null;
            tDTO.Save();
        }

        public TaskBL(TaskDTO tDTO)
        {
            this.tDTO = tDTO;
            this.Title = tDTO.Title;
            this.dueDate = tDTO.DueDate;
            this.desc = tDTO.Desc;
            this.taskID = tDTO.TaskID;
            this.CTime = tDTO.CTime;
            this.assignee = tDTO.Assignee;

        }
        public TaskDTO GetTaskDTO()
        {
            return this.tDTO;
        }
        public long TaskID { get { return this.taskID; }}
        public DateTime CTime { get; }
        public string Desc
        {
            get => desc;
            set
            {
                if (value.Length > DESC_LIM)
                {
                    Log.Error("Provided description exceeds character limit");
                    throw new Exception("Provided description exceeds character limit");
                }
                else if (value==null)
                {
                    Log.Error("Provided description is null");
                    throw new Exception("Provided descritpion is null");
                }
                else
                {
                    this.GetTaskDTO().Desc= value;
                    this.desc = value;
                }
            }
        }
        public string Title
        {
            get => title;
            set
            {
                if (value.Length > TITLE_LIM)
                {
                    Log.Error("Provided title exceeds charachter limit");
                    throw new Exception("Provided titlr exceeds character limit");
                }
                else if (string.IsNullOrEmpty(value))
                {
                    Log.Error("Provided title is null or empty");
                    throw new Exception("Provided title is null or empty");
                }
                else
                {
                    this.GetTaskDTO().Title = value;
                    this.title = value;
                }
            }
        }
        public DateTime DueDate
        {
            get => this.dueDate;
            set
            {
                if (value >= DateTime.Today)
                {
                    this.GetTaskDTO().DueDate = value;
                    this.dueDate = value;
                }
                else
                {
                    Log.Error("Provided duedate is invalid - not a future date");
                    throw new Exception("Provided duedate is invalid - not a future date");
                }

            }

        }
        public string Assignee
        {
            get => assignee;
            set 
            {
                this.GetTaskDTO().Assignee = value;
                this.assignee = value; 
            }
        }
    }
}
