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
    }
}
