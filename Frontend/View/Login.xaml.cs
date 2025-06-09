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
using IntroSE.Kanban.Frontend.ViewModel;

namespace IntroSE.Kanban.Frontend.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private LoginVM loginVM;
        public Login()
        {
            InitializeComponent();
            this.loginVM = new LoginVM();
            this.DataContext = loginVM;
        }
        // Handles connecion with the window (similiar to service layer)
        // Holds a loginVM object to use its methods
        // Holds button click logic
        // On successful login will send the usermodel to the new main window it creates
        // The error that the service throws should be shown when trying to register ot login with non valid data
        // For each text box we should use binding with mode.
        // In binding - specify pass (mostly fields) in the data context (view model) using the setter.
        // If we want to display defultive data in textbox use Mode - TwoWay
        // For error message or any there label- use mode One Way 
        
    }
}
