using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal class TaskController
    {
        private readonly string connectionString;
        private const string TableName = "Tasks";

        public TaskController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanbanDB.db"));
            connectionString = $"Data Source={path}; Version=3;";
        }

        public List<TaskDTO> SelectAll()
        {
            //TODO
        }

        public List<TaskDTO> Select()
        {
            //TODO
        }

        public bool Insert(TaskDTO board)
        {

        }

        public bool Delete(TaskDTO board)
        {
            //TODO
        }



    }
}
}
