using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public const int DESC_LIM = 300;
        public const int TITLE_LIM = 50;
        public TaskBL(string title, DateTime dueDate, string desc, long id)
        { 
            this.Title = title;
            this.DueDate = dueDate;
            this.Desc = desc;
            this.taskID = id;
            this.CTime = DateTime.Now;
            this.Assignee = null;
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
            set {this.assignee = value; }
        }
    }
}
