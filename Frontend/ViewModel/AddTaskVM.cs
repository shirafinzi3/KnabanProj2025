using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    internal class AddTaskVM : Notifiable
    {
        private string title;
        public string Title
        {
            get => title;
            set { title = value; }
        }

        private string description;
        public string Description
        {
            get => description;
            set { description = value;}
        }

        private DateTime? dueDate = DateTime.Now;
        public DateTime? DueDate
        {
            get => dueDate;
            set { dueDate = value;  }
        }
        
        public bool Submit()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                return false;
            }
            if (DueDate == null)
            {
                return false;
            }
            return true;
        }
    }
}
