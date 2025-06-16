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

namespace IntroSE.Kanban.Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        private UserModel user;
        private BoardModel board;
        private BoardWindowVM vm;

        internal BoardWindow(UserModel user, BoardModel board)
        {
            InitializeComponent();
            this.user = user;
            this.board = board;
            vm = new BoardWindowVM(user, board);
            this.DataContext = vm;
        }


    }
}
