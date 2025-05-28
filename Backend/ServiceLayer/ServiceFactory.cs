using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BuisnessLayer;
using Microsoft.VisualBasic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        public UserService US { get; }
        public BoardService BS { get; }
        public TaskService TS { get; }

        public ServiceFactory()
        {
            AuthenticationFacade AF = new AuthenticationFacade();
            BoardFacade BF = new BoardFacade(AF);
            UserFacade UF = new UserFacade(AF);

            this.US = new UserService(UF);
            this.BS = new BoardService(BF);
            this.TS = new TaskService(BF);

        }

        public void loadAllData()
        {
             US.LoadAllUsers();
             BS.LoadAllBoards();
        }

        public void deleteAllData()
        {
            US.DeleteAllUsers();
            BS.DeleteAllBoards();
        }
    }
}
