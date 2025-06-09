using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.ServiceLayer;

namespace IntroSE.Kanban.Frontend.Controllers
{
    internal class BoardController
    {
        BoardService bs;
        public BoardController(BoardService bs)
        { 
            this.bs = bs;
        }
    }
}
