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

        private readonly BoardController boardController;
        private bool isPersistent = false;
        private readonly BoardUsersController buController;
        public BoardDTO(long ID, string name, string ownerEmail)
        {
            this.boardID = ID;
            this.name = name;
            this.ownerEmail = ownerEmail;
            this.boardController = new BoardController();
            this.users = new List<string>();
            this.buController = new BoardUsersController();
        }

        public string OwnerEmail
        {
            get => ownerEmail;
            set
            {
                if (isPersistent)
                {
                    boardController.Update(boardID, ownerEmailColumnName, value);
                }
                ownerEmail = value;
            }
        }
        public string Name
        {
            get => name;
            set
            {
                if (isPersistent)
                {
                    boardController.Update(boardID, boardNameColumnName, value);
                }
                name = value;
            }
        }
        public long BoardID
        {
            get => boardID;
        }
        
        public BoardUsersController GetBuController()
        {
            return this.buController;
        }
        public BoardController GetBoardController()
        {
            return this.boardController;
        }
        
        public List<string> Users
        {
            get => users; 
        }
        public void AddUser(string userEmail)
        {
            if (!users.Contains(userEmail))
            {
                BoardUsersDTO buDTO = new BoardUsersDTO(boardID, userEmail);
                buDTO.Save();
                users.Add(userEmail);
                
            }
        }
        public void RemoveUser(string userEmail)
        {
            if (users.Contains(userEmail)) 
            {
                BoardUsersDTO buDTO = new BoardUsersDTO(boardID, userEmail);
                buDTO.Delete();
                users.Remove(userEmail);
            }
        }
        public void AddColumn(ColumnDTO column) 
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

        public void Delete()
        {
            if (!isPersistent)
            {
                throw new InvalidOperationException("Cannot delete non persisted object");
            }
            boardController.Delete(this);
            Log.Info("Board data deleted from DB");
            isPersistent = false;
        }
    }
}