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
        public string Desc { get; set; }
        public string Column { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }

    }
}
