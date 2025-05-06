using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using log4net;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    internal class Column
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public String columnName { get; }
        private int maxTasks;
        public readonly Dictionary<long, TaskBL> tasks = new Dictionary<long, TaskBL>();

        public Column(String columnName, int maxTasks)
        {
            this.columnName = columnName;
            MaxTasks = maxTasks;
        }

        public int MaxTasks
        {
            get => maxTasks; 
            set
            {
                if(value>0 && tasks.Count > value)
                {
                    Log.Error($"Cant lower max tasks of {columnName} as it currently holds more than {value}");
                    throw new InvalidOperationException($"Cant lower max tasks of {columnName} as it currently holds more than {value}");
                }
                this.maxTasks = value;
            }
        }
        public void Add(TaskBL task)
        {
            if (maxTasks>=0 && tasks.Count >= maxTasks) //if -1 (maxTasks<0) no limit, if not, check column isnt at capacity
            {
                Log.Error($"Column {columnName} is full");
                throw new InvalidOperationException($"Column {columnName} is full");
            }
            tasks.Add(task.TaskID, task);
        }

        public bool Remove(long taskId)
        {
           return tasks.Remove(taskId);
        }
        public Dictionary<long, TaskBL> GetTasks() { return tasks; }
    }
}
