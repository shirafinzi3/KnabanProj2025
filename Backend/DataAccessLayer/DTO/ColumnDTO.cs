using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    internal class ColumnDTO
    {
        private long columnID { get; }
        private long boardID { get; }
        private string colName { get; }
        private int maxTasks;
        public const string ColumnIDColumnName = "ColumnID";
        public const string BoardIDColumnName = "BoardID";
        public const string ColumnNameColumnName = "ColumnName";
        public const string MaxTasksColumnName = "MaxTasks";
        public ColumnController columnController { get; set; }
        private bool isPersistent = false;

        public int MaxTasks
        {
            get => maxTasks;
            set
            {
                maxTasks = value;
            }
        }

        public ColumnDTO(long columnID, long boardID, string colName, int maxTasks)
        {
            this.columnID = columnID;
            this.boardID = boardID;
            this.colName = colName;
            this.maxTasks = MaxTasks;
            columnController = new ColumnController();
        }

        public void save()
        {
            columnController.Insert(this);
            isPersistent = true;
        }
    }
}
