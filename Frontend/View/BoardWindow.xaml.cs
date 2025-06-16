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
using Backend.ServiceLayer;
using IntroSE.Kanban.Frontend.Model;
using IntroSE.Kanban.Frontend.ViewModel;
using Microsoft.VisualBasic;

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

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddTaskWindow { Owner = this };
            var addVm = (AddTaskVM)addWindow.DataContext;

            if (addWindow.ShowDialog() == true)
            {
                vm.AddTask(addVm.Title, addVm.Description, addVm.DueDate.Value);
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            String col = (String)button.Tag;
            var task = ((Button)sender).DataContext as TaskModel;
            if (task != null)
            {
                vm.DeleteTask(task, col);
            }
        }

        private void MoveTask_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            String col = (String)button.Tag;
            var task = ((Button)sender).DataContext as TaskModel;
            if (task != null)
            {
                vm.MoveTask(task, col);
            }
        }
    }
}
