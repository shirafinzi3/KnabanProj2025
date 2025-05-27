using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    internal class ColumnDTO
    {
        private long columnID;
        private long boardID;
        private string colName;
        private int maxTasks;
        public const string ColumnIDColumnName = "ColumnID";
        public const string BoardIDColumnName = "BoardID";
        public const string ColumnNameColumnName = "ColumnName";
        public const string MaxTasksColumnName = "MaxTasks";
        public ColumnController columnController { get; set; }
        private bool isPersistent = false;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public long ColumnID { get => columnID; }
        public long BoardID { get => boardID; }
        public string ColName { get => colName; }
      
        public ColumnDTO(long columnID, string colName, int maxTasks)
        {
            this.columnID = columnID;
            this.colName = colName;
            this.maxTasks = MaxTasks;
            columnController = new ColumnController();
        }

        public ColumnDTO(long columnID, long boardID, string colName, int maxTasks)
        {
            this.columnID = columnID;
            this.boardID = boardID;
            this.colName = colName;
            this.maxTasks = MaxTasks;
            columnController = new ColumnController();
        }

        public int MaxTasks
        {
            get => maxTasks;
            set
            {
                if (isPersistent)
                {
                    columnController.UpdateMaxTasks(columnID, MaxTasksColumnName, value);
                }
                maxTasks = value;
            }
        }
        public void AddTask(TaskDTO task)
        {
            task.Save(this.columnID);
        }

        public void Save()
        {
            if (isPersistent)
            {
                throw new InvalidOperationException("Cannot save persisted object");
            }
        }
        public void Save(long boardID)
        {
            if (isPersistent)
            {
                throw new InvalidOperationException("Cannot save persisted object");
            }
            this.boardID = boardID;
            columnController.Insert(this);
            Log.Info("Column data saved to database");
            isPersistent = true;
        }
    }
}
