using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    internal class BoardFacade
    {
        Dictionary<string, Dictionary<string, BoardBL>> boards = new Dictionary<string, Dictionary<string, BoardBL>>();
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
            return null;
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
