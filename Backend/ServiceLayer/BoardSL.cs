using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.ServiceLayer
{
    public class BoardSL
    {
        public string name;
        public Dictionary<string, List<TaskSL>> tasks;
        /// <summary>
        /// This constructor initiates a BoardSL object with a unique name and a list of tasks
        /// </summary>
        /// <param name="name">The unique board name (per user)</param>
        /// <param name="tasks">A dictionary of the tasks in each column of the board</param>
        public BoardSL(string name, Dictionary<string, List<TaskSL>> tasks)
        {
            this.name = name;
            this.tasks = tasks;
        }
        public string Name { get; set; }
        public Dictionary<String,TaskSL> Tasks { get;}
    }
}
