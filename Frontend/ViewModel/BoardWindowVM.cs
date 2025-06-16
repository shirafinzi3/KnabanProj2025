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
        private string errorMessage;
        public UserModel User { get; }
        public BoardModel Board { get; }
        public string BoardName => Board.BoardName;
        public string UserEmail => User.Email;
        public readonly String BACKLOG;
        public readonly String IN_PROGRESS;
        public readonly String DONE;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }
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
            BACKLOG = ControllerFactory.Instance.BoardController.GetColumnName(User, Board, 0);
            IN_PROGRESS = ControllerFactory.Instance.BoardController.GetColumnName(User, Board, 1);
            DONE = ControllerFactory.Instance.BoardController.GetColumnName(User, Board, 2);
        }

        public void AddTask(string title, string description, DateTime dueDate)
        {
            try
            {
                TaskModel task = ControllerFactory.Instance.BoardController.AddTask(User, Board, title, description, dueDate);
                BacklogTasks.Add(task);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public void DeleteTask(TaskModel task, String col)
        {
            try
            {
                ControllerFactory.Instance.BoardController.DeleteTask(User, Board, task);
                if (col == BACKLOG)
                {
                    BacklogTasks.Remove(task);
                }
                else if (col == IN_PROGRESS)
                {
                    InProgressTasks.Remove(task);
                }
                else if (col == DONE)
                {
                    DoneTasks.Remove(task);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        public void MoveTask(TaskModel task, String col)
        {
            try
            {
                ControllerFactory.Instance.BoardController.MoveTask(User, Board, task);
                if (col == BACKLOG)
                {
                    BacklogTasks.Remove(task);
                    InProgressTasks.Add(task);
                }
                else if (col == IN_PROGRESS)
                {
                    InProgressTasks.Remove(task);
                    DoneTasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

    }
}
