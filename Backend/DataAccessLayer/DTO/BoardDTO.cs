using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    internal class BoardDTO
    {
        private long boardID; 
        private string name;
        private string ownerEmail;
        public const string boardIDColumnName = "BoardID";
        public const string boardNameColumnName = "BoardName";
        public const string ownerEmailColumnName = "OwnerEmail";
        public BoardController boardController { get; set; }
        private bool isPersistent = false;

        public BoardDTO(long ID, string name, string ownerEmail)
        {
            boardID = ID;
            this.name = name;
            this.ownerEmail = OwnerEmail;
            boardController = new BoardController();
        }

        public string OwnerEmail
        {
            get => ownerEmail;
            set
            {
               ownerEmail = value; 
            }
        }
        public void save()
        {
            boardController.Insert(this);
            isPersistent = true;
        }
    }
}
