using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.ServiceLayer;

namespace IntroSE.Kanban.Frontend.Model
{
    internal class BoardModel
    {
        public string Owner { get; set; }
        public string BoardName {  get; set; }
        public long BoardID { get; set; }
        public List<string> users;
        public BoardModel(BoardSL boardSL) 
        { 
            this.BoardID=boardSL.boardID;
            this.BoardName=boardSL.Name;
            this.Owner = boardSL.Owner;
            this.users = boardSL.Users;
          
        }
    }

   
}
