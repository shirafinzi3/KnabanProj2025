using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal class UserController
    {
        private readonly string connectionString;
        private const string TableName = "Users";

        public UserController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanbanDB.db"));
            connectionString = $"Data Source={path}; Version=3;";
        }

        public List<UserDTO> SelectAll()
        {
            //TODO
        }
    
        public List <UserDTO> Select()
        {
            //TODO
        }

        public bool Insert(UserDTO user)
        {

        }

        public bool Delete(UserDTO user)
        {
            //TODO
        }

        public bool UpdatePassword(string password)
        {
            //Do we need to do this?
        }


    }
}