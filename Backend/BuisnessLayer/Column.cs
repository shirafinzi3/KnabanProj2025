using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using log4net;
using log4net.Repository.Hierarchy;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    internal class Column
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public String columnName { get; }
        private int maxTasks;
        private long columnID;
        private ColumnDTO cDTO;
        private readonly Dictionary<long, TaskBL> tasks = new Dictionary<long, TaskBL>();

        public Column(String columnName, int maxTasks, long columnID)
        {
            cDTO = new ColumnDTO(columnID, columnName, maxTasks);
            this.columnName = columnName;
            MaxTasks = maxTasks;
            this.columnID = columnID;
            cDTO.Save();
            
        }

        public ColumnDTO ColumnDTO
        {
            get => cDTO;
        }
        public long ColumnID { get => columnID; }
        public int MaxTasks
        {
            get => maxTasks; 
            set
            {
                if(value < -1)
                {
                    Log.Error($"Cant lower max tasks of {columnName} to a negative number");
                    throw new InvalidOperationException($"Cant lower max tasks of {columnName} to a negative number");
                }
                if(value > 0 && tasks.Count > value)
                {
                    Log.Error($"Cant lower max tasks of {columnName} as it currently holds more than {value}");
                    throw new InvalidOperationException($"Cant lower max tasks of {columnName} as it currently holds more than {value}");
                }
                cDTO.MaxTasks = value;
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
            cDTO.AddTask(task.TaskDTO);
            tasks.Add(task.TaskID, task);
        }

        public bool Remove(long taskId)
        {
           //NEED to update in deleting
           return tasks.Remove(taskId);
        }
        public Dictionary<long, TaskBL> GetTasks() { return tasks; }
    }
}
