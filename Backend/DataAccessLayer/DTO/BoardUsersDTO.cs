using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.Controllers;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    internal class BoardUsersDTO
    {
        private long boardID;
        private string userEmail;
        public const string boardIDColumnName = "boardID";
        public const string userEmailColumnName = "userEmail";
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool isPersistent = false;
        private readonly BoardUsersController boardUserController;

        public BoardUsersDTO(long boardID, string userEmail) 
        { 
            this.boardID = boardID;
            this.userEmail = userEmail;
            boardUserController = new BoardUsersController();
        }

        public long BoardID
        {
            get => boardID;
        }

        public string UserEmail
        {
            get=> userEmail;
        }

        public void Save()
        {
            if(isPersistent)
            {
                throw new InvalidOperationException("Cannot save persisted object");
            }
            boardUserController.Insert(this);
            Log.Info("Board-user pair saved to database");
            isPersistent = true;
        }

        
    }
}
