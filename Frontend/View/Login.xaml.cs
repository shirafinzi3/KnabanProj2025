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
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Frontend.Model;
using IntroSE.Kanban.Frontend.ViewModel;

namespace IntroSE.Kanban.Frontend.View
{

    public partial class Login : Window
    {
        private LoginVM loginVM;
        public Login()
        {
            InitializeComponent();
            this.loginVM = new LoginVM();
            this.DataContext = loginVM;
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel? ret = loginVM.Login();
            if (ret != null)
            {
                Boards boards = new Boards(ret);
                boards.Show();
                this.Close();
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            UserModel? ret = loginVM.Register();
            if (ret != null)
            {
                Boards boards = new Boards(ret);
                boards.Show();
                this.Close();
            }
        }

    }
}
