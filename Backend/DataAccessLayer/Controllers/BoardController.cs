/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal class BoardController
    {
        private readonly string connectionString;
        private const string TableName = "Boards";

        public BoardController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanbanDB.db"));
            connectionString = $"Data Source={path}; Version=3;";
        }

        public List<BoardDTO> SelectAll()
        {
            //TODO
        }

        public List<BoardDTO> Select()
        {
            //TODO
        }

        public bool Insert(BoardDTO board)
        {

        }

        public bool Delete(BoardDTO board)
        {
            //TODO
        }

        

    }
}
*/