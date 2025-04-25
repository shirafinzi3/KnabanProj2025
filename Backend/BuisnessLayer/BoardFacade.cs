using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using log4net;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    internal class BoardFacade
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<string, Dictionary<string, BoardBL>> boards = new Dictionary<string, Dictionary<string, BoardBL>>();
        private readonly AuthenticationFacade auth = new AuthenticationFacade();
        public BoardBL CreateBoard(string email, string boardName, int? maxTasks)
        {
            return null;
        }
        public bool DeleteBoard(string email, string boardName)
        {
            return false;
        }
        public Dictionary<string, BoardBL> GetAllBoards(string email) 
        {
            return null;
        }
        public List<TaskBL> InProgressList(string email)
        {
            if (!auth.IsLoggedIn(email))
            {
                Log.Error($"User {email} is not logged in");
                throw new InvalidOperationException($"User {email} is not logged in");
            }
            if (!boards.ContainsKey(email))
            {
                Log.Error($"User {email} does not exist");
                throw new KeyNotFoundException($"User {email} does not exist");
            }
            List<TaskBL> inProgressList = new List<TaskBL>();
            foreach (BoardBL board in boards[email].Values)
            {
                if (board.Tasks.ContainsKey("Inprogress"))
                {
                    foreach (TaskBL task in board.Tasks["Inprogress"].Values)
                    {
                        inProgressList.Add(task);
                    }
                }
            }
            return inProgressList;
        }
        public TaskBL AddTask(string email, string boardName, string title, string desc, DateTime dueDate)
        {
            return null;
        }
        public TaskBL UpdateTitle(string email, string boardName, long taskID, string title) 
        {
            return null;
        }
        public TaskBL UpdateDesc(string email, string boardName, long taskID, string desc)
        {
            return null;
        }
        public TaskBL UpdateDueDate(string email, string boardName, long taskID, DateTime dueDate)
        {
            return null;
        }
        public TaskBL MoveTask(string email, string boardName, long taskID) 
        {
            return null;
        }

    }
}
