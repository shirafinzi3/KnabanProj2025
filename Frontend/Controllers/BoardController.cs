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
        TaskService ts;
        public BoardController(BoardService bs, TaskService ts)
        { 
            this.bs = bs;
            this.ts = ts;
        }

        internal ObservableCollection<TaskModel> GetBackLogTasks(String email, string boardName)
        {
            Response<List<TaskSL>> response = JsonSerializer.Deserialize<Response<List<TaskSL>>>( bs.GetColumn(email, boardName, 0));
            if (response.ErrorMessage != null)
            {
                throw new Exception(response.ErrorMessage);
            }
            return new ObservableCollection<TaskModel>(
                ((List<TaskSL>)response.ReturnValue).Select(tsl => new TaskModel(tsl)).ToList());
        }

        internal ObservableCollection<TaskModel> GetInProgressTasks(String email, string boardName)
        {
            Response<List<TaskSL>> response = JsonSerializer.Deserialize<Response<List<TaskSL>>>(bs.GetColumn(email, boardName, 1));
            if (response.ErrorMessage != null)
            {
                throw new Exception(response.ErrorMessage);
            }
            return new ObservableCollection<TaskModel>(
                ((List<TaskSL>)response.ReturnValue).Select(tsl => new TaskModel(tsl)).ToList());
        }

        internal ObservableCollection<TaskModel> GetDoneTasks(String email, string boardName)
        {
            Response<List<TaskSL>> response = JsonSerializer.Deserialize<Response<List<TaskSL>>>(bs.GetColumn(email, boardName, 2));
            if (response.ErrorMessage != null)
            {
                throw new Exception(response.ErrorMessage);
            }
            return new ObservableCollection<TaskModel>(
                ((List<TaskSL>)response.ReturnValue).Select(tsl => new TaskModel(tsl)).ToList());
        }
        internal ObservableCollection<BoardModel> GetAllBoards(UserModel user)
        {
            Response<List<BoardSL>> response = JsonSerializer.Deserialize<Response< List<BoardSL>>>(bs.GetAllBoards(user.Email));
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
