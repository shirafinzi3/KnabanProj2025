using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.ServiceLayer
{
    public class BoardSL
    {
        public long boardID{ get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public List<string> Users { get; set; }

        /// <summary>
        /// This constructor initiates a BoardSL object with a unique name and a list of tasks
        /// </summary>
        /// <param name="name">The unique board name (per user)</param>
        /// <param name="tasks">A dictionary of the tasks in each column of the board</param>
        public BoardSL(string name, long boardID,string Owner, List<string> users)
        {
            this.Name = name;
            this.boardID = boardID;
            this.Owner = Owner;
            this.Users = users;
        }
        public BoardSL() { }
        
    }
}
