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
        public int? maxTasks { get; }
        public readonly Dictionary<long, TaskBL> tasks = new Dictionary<long, TaskBL>();

        public Column(String columnName, int? maxTasks)
        {
            this.columnName = columnName;
            this.maxTasks = maxTasks;
        }

        public void Add(TaskBL task)
        {
            if (maxTasks.HasValue && tasks.Count >= maxTasks)
            {
                Log.Error($"Column {columnName} is full");
                throw new InvalidOperationException($"Column {columnName} is full");
            }
            tasks.Add(task.TaskID, task);
        }

        public void Remove(long taskId)
        {
            tasks.Remove(taskId);
        }
    }
}
