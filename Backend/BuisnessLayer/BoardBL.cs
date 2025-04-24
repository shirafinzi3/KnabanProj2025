using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Backend.BuisnessLayer
{
    internal class BoardBL
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string boardName;
        private readonly Dictionary<string,Dictionary<long,TaskBL>> tasks = new Dictionary<string, Dictionary<long, TaskBL>>();
        private int[] maxTasks;
        private long nextTaskID;
        public string BoardName
        {
            get;
        }
        public Dictionary<string, Dictionary<long, TaskBL>> Tasks
        {
            get;
        }


    }
}
