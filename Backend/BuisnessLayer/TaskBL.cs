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
        private long taskID;
        private DateTime cTime;
        public string title;
        public DateTime dueDate;
        public string desc;
        public string column;
        public TaskBL(string title, DateTime dueDate, string desc)
        {
            this.title = title;
            this.dueDate = dueDate;
            this.desc = desc;
        }
        public long TaskID { get; }
        public DateTime CTime { get; }
        public string Desc {
            get => desc; 
            set
            {
                if (value.Length > 300)
                {
                    Log.Error("Provided description exceeds character limit");
                    throw new Exception("Provided description exceeds character limit");
                }
                else if (string.IsNullOrEmpty(value))
                {
                    Log.Error("Provided description is null or empty");
                    throw new Exception("Provided descritpion is null or empty");
                }
                else
                {
                    this.desc = value;
                }
            }
        }
        public string Column { get; set; }
        public string Title {
            get => title; 
            set
            {
                if (value.Length > 50)
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
