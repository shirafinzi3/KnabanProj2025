using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.BuisnessLayer
{
    internal class BoardBL
    {
        private string boardName;
        private readonly Dictionary<string,Dictionary<long,TaskBL>> tasks = new Dictionary<string, Dictionary<long, TaskBL>>();
        public string BoardName
        {
            get;
        }
        public Dictionary<string, Dictionary<long, TaskBL>> Tasks
        {
            get;
        }

    }
}
