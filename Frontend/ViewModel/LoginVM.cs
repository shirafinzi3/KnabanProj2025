using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Frontend.Controllers;
using IntroSE.Kanban.Frontend.Model;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    internal class LoginVM : Notifiable
    {
        UserController uc = ControllerFactory.Instance.UserController;
        private string password;
        public string Password {
            get { return password; }
            set
            {
                this.password = value;
            }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                this.email = value;
            }
        }
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                this.errorMessage = value;
                RaisePropertyChanged(nameof(errorMessage));
            }
        }
        internal LoginVM()
        {
            this.email = string.Empty;
            this.password = string.Empty;
            this.errorMessage= string.Empty;
        }
        internal UserModel? Login()
        {
            try
            {
                return uc.Login(email, password);
            }
            catch (Exception ex) 
            {
                ErrorMessage = ex.Message;
                return null;
            }
        }
        internal UserModel? Register()
        {
            try
            {
                return uc.Register(email, password);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }
        }
    }
}
