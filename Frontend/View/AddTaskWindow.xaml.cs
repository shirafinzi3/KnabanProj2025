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
using IntroSE.Kanban.Frontend.ViewModel;

namespace IntroSE.Kanban.Frontend.View
{
    public partial class AddTaskWindow : Window
    {
        private readonly AddTaskVM vm;

        public AddTaskWindow()
        {
            InitializeComponent();
            vm = new AddTaskVM();
            DataContext = vm;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (vm.Submit())
                DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}