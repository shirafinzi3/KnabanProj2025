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
        public string title;
        public DateTime dueDate;
        public string desc;
        public const int DESC_LIM= 300;
        public const int TITLE_LIM = 50;
        public TaskBL(string title, DateTime dueDate, string desc, long id)
        {
            this.title = title;
            this.dueDate = dueDate;
            this.desc = desc;
            this.taskID = id;
            this.cTime = DateTime.Now;
        }
        public long TaskID { get; }
        public DateTime CTime { get; }
        public string Desc {
            get => desc; 
            set
            {
                if (value.Length > DESC_LIM)
                {
                    Log.Error("Provided description exceeds character limit");
                    throw new Exception("Provided description exceeds character limit");
                }
                else if (desc==null)
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
        public string Title {
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
        public DateTime DueDate { get; set; }

    }
}
