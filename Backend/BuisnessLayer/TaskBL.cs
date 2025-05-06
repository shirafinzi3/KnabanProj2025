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
        public DateTime dueDate;
        public string desc;
        public const int DESC_LIM= 300;
        public const int TITLE_LIM = 50;
        public TaskBL(string title, DateTime dueDate, string desc, long id)
        {
            Title = title;
            DueDate = dueDate;
            Desc = desc;
            this.taskID = id;
            this.cTime = DateTime.Today;
        }
        public long TaskID { 
            get => taskID; }
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
        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                if (value.Date < DateTime.Today)
                {
                    Log.Error($"Invalid DueDate as {dueDate} is earlier then current time");
                    throw new Exception($"Invalid DueDate as {dueDate} is earlier then current time");
                }
                this.dueDate = value;
            }
        }

    }
}
