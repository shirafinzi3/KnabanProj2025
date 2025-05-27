using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;
using log4net;

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
        private readonly List<string> users;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BoardController boardController { get; set; }
        private bool isPersistent = false;

        public BoardDTO(long ID, string name, string ownerEmail)
        {
            boardID = ID;
            this.name = name;
            this.ownerEmail = OwnerEmail;
            boardController = new BoardController();
            users = new List<string>();
            users.Add(ownerEmail);
        }

        public string OwnerEmail
        {
            get => ownerEmail;
            set
            {
                if (isPersistent)
                {
                    boardController.UpdateBoardName(boardID, boardNameColumnName, value);
                }
                ownerEmail = value;
            }
        }
        public string Name
        {
            get => name;
        }
        public long BoardID
        {
            get => boardID;
        }

        public List<string> Users
        {
            get => users; //need to do using SELECT i think?
        }
        public void AddUser(string userEmail)
        {
            if (!users.Contains(userEmail))//need to check if we can just add it without if
            {
                users.Add(userEmail);
                
            }
        }
        public void RemoveUser(string userEmail)
        {
            if (!users.Contains(userEmail)) //need to check if we can just remove it without if
            { 
                users.Remove(userEmail);
            }
        }
        public void AddColumn(ColumnDTO column) //NEED TO CHECK THE UML
        {
            column.Save(this.BoardID);
        }
        public void Save()
        {
            if (isPersistent) throw new InvalidOperationException("Cannot save persisted object");
            if (boardController.Insert(this))
            {
                isPersistent = true;
                Log.Info("board data saved to database");
            }
            else
            {
                Log.Error("Failed to insert board into DB");
                throw new InvalidOperationException("Failed to insert user into DB");
            }
        }
    }
}