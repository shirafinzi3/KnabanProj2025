using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.ServiceLayer;
using IntroSE.Kanban.Frontend.Model;

namespace IntroSE.Kanban.Frontend.Controllers
{
    internal class BoardController
    {
        BoardService bs;
        public BoardController(BoardService bs)
        { 
            this.bs = bs;
        }
        internal ObservableCollection<BoardModel> GetAllBoards(UserModel user)
        {
            Response<List<BoardSL>> response = JsonSerializer.Deserialize<Response< List<BoardSL>>>( bs.GetUserBoards(user.Email));
            if (response.ErrorMessage != null)
            {
                throw new Exception(response.ErrorMessage);
            }
            ObservableCollection<BoardModel> list = new ObservableCollection<BoardModel>();
            foreach(BoardSL board in response.ReturnValue)
            {
                list.Add(new BoardModel(board));
            }
            return list ;
        }
        internal BoardModel CreateBoard(UserModel user, string BoardName)
        {
            Response<BoardSL> response = JsonSerializer.Deserialize<Response<BoardSL>>(bs.CreateBoard(user.Email,BoardName));
            if (response.ErrorMessage != null)
            {
                throw new Exception(response.ErrorMessage);
            }
            
            return new BoardModel(response.ReturnValue);
        }

    }
}
