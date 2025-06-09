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
    internal class BoardsVM : Notifiable
    {
        private string errorMessage;
        public UserModel User { get; }
        public string WelcomeMessage { get { return $"Welcome, {User.Email}!"; } }
        
        public ObservableCollection<BoardModel> AllBoards { get; }
        public string ErrorMessage
        {
            get { return errorMessage; }
            set 
            {
                errorMessage = value;
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }
        BoardModel selectedBoard;
        public BoardModel SelectedBoard
        {
            get
            {
                return selectedBoard;
            }
            set
            {
                this.selectedBoard = value;
                RaisePropertyChanged(nameof(selectedBoard));
            }
        }
        internal BoardsVM(UserModel user)
        {
            User = user;
            AllBoards = ControllerFactory.Instance.BoardController.GetAllBoards(user);
        }
        public void CreateBoard(string input)
        {
            try
            {
                BoardModel boardModel = ControllerFactory.Instance.BoardController.CreateBoard(User, input);
                AllBoards.Add(boardModel);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        public void DeleteBoard(BoardModel boardModel)
        {
            try
            { 
                ControllerFactory.Instance.BoardController.DeleteBoard(User, boardModel);
                AllBoards.Remove(boardModel);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        public bool Logout()
        {
            try
            {
                bool res = ControllerFactory.Instance.UserController.Logout(User);
                return res;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }
        //Will hold a public user model
        // Use ObservableCollection if we want a list that listens for changes (maybe with tasks or get all boards)
    }
}
