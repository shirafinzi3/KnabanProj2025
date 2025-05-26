using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal class ColumnController
    {
        private readonly string connectionString;
        private const string TableName = "Columns";

        public ColumnController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanbanDB.db"));
            connectionString = $"Data Source={path}; Version=3;";
        }

        public List<ColumnDTO> SelectAll()
        {
            //TODO
        }

        public List<ColumnDTO> Select()
        {
            //TODO
        }

        public bool Insert(ColumnDTO board)
        {
            //TODO
        }

        public bool Delete(ColumnDTO board)
        {
            //TODO
        }

        public bool UpdateMaxTasks(int newMaxTasks)
        {
            //TODO
        }
    }
}

