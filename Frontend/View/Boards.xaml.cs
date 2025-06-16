using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IntroSE.Kanban.Frontend.Model;
using IntroSE.Kanban.Frontend.ViewModel;
using Microsoft.VisualBasic;

namespace IntroSE.Kanban.Frontend.View
{
    /// <summary>
    /// Interaction logic for Boards.xaml
    /// </summary>
    public partial class Boards : Window
    {
        private UserModel User;
        private BoardsVM BoardsVM;

        internal Boards(UserModel User)
        {
            InitializeComponent();
            this.User = User;
            this.BoardsVM = new BoardsVM(User);
            this.DataContext = BoardsVM;
        }
        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BoardModel selectedBoard = (BoardModel)((ListBoxItem)sender).Content;
            this.BoardsVM.SelectedBoard = selectedBoard;
            BoardWindow boardWindow = new BoardWindow(User, selectedBoard);
            boardWindow.Show();
            this.Close();
        }
        private void CreateBoard_Click(object sender, RoutedEventArgs e)
        {
            string input = Interaction.InputBox("Enter new board name:", "Add Board", "");  
            BoardsVM.CreateBoard(input);
        }
        private void DeleteBoard_Click(object sender, RoutedEventArgs e)
        {
            var board = ((Button)sender).DataContext as BoardModel;
            if (board != null)
            {
                BoardsVM.DeleteBoard(board);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            bool ret = BoardsVM.Logout();
            if (ret)
            {
                Login login = new Login();
                login.Show();
                this.Close();
            }
        }

    }
    // Will hold a UserModel and a BoardsVM
    // In the xaml file might want to use DataGrid to present boards - allows clicking on a row
    // After login main window opens - should show the name of the user and his list of boards
    // This window should allow creating and deleting boards
    // This window will send to a board window when double clicked on a row in the list of boards
}
