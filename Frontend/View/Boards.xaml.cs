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

namespace IntroSE.Kanban.Frontend.View
{
    /// <summary>
    /// Interaction logic for Boards.xaml
    /// </summary>
    public partial class Boards : Window
    {
        public Boards()
        {
            InitializeComponent();
        }
    }
    // Will hold a UserModel and a BoardsVM
    // In the xaml file might want to use DataGrid to present boards - allows clicking on a row
    // After login main window opens - should show the name of the user and his list of boards
    // This window should allow creating and deleting boards
    // This window will send to a board window when double clicked on a row in the list of boards
}
