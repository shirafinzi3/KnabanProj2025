using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Frontend.Controllers;
using IntroSE.Kanban.Frontend.Model;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    internal class BoardWindowVM : Notifiable
    {
        public UserModel User { get; }
        public BoardModel Board { get; }
        public string BoardName => Board.BoardName;
        public string UserEmail => User.Email;

        public ObservableCollection<TaskModel> BacklogTasks { get; }
        public ObservableCollection<TaskModel> InProgressTasks { get; }
        public ObservableCollection<TaskModel> DoneTasks { get; }

        public BoardWindowVM(UserModel user, BoardModel board)
        {
            User = user;
            Board = board;
            BacklogTasks = ControllerFactory.Instance.BoardController.GetBackLogTasks(UserEmail, BoardName);
            InProgressTasks = ControllerFactory.Instance.BoardController.GetInProgressTasks(UserEmail, BoardName);
            DoneTasks = ControllerFactory.Instance.BoardController.GetDoneTasks(UserEmail, BoardName);
        }
    }
}
