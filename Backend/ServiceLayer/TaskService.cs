using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.BuisnessLayer;
using Microsoft.VisualBasic;

namespace Backend.ServiceLayer
{
    public class TaskService
    {
        private BoardFacade BF;
        /// <summary>
        /// This constructor initiates a BoardFacade object
        /// </summary>
        public TaskService()
        {
            BF = new BoardFacade();
        }
        /// <summary>
        /// This method adds a new task to a specific board
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">The unique board name to which the task needs to be added</param>
        /// <param name="title">The title of the new task</param>
        /// <param name="desc">The description of the task</param>
        /// <param name="dueDate">The due date set by the user for the task</param>
        /// <returns></returns>
        public string AddTask(String email, String boardName, String title, String desc, DateTime dueDate)
        {
            try
            {
                TaskBL taskBL = BF.AddTask(email, boardName, title, desc, dueDate);
                Response<TaskSL> res = new Response<TaskSL>(null, new TaskSL(taskBL.Title, taskBL.Desc, taskBL.Column, taskBL.DueDate, taskBL.CTime, taskBL.TaskID));
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<TaskSL>(e.Message));
            }
        }
        /// <summary>
        /// This method updates the task title
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">The unique board name in which the task is</param>
        /// <param name="taskID">A unique id for the task</param>
        /// <param name="title">The title of the task</param>
        /// <param name="column">The column in which the task is</param>
        /// <returns>A TaskSL or an error</returns>
        public string UpdateTitle(string email, string boardName, long taskID, string title, string column)
        {
            try
            {
                TaskBL taskBL = BF.UpdateTitle(email, boardName, taskID, title, column);
                Response<TaskSL> res = new Response<TaskSL>(null, new TaskSL(taskBL.Title, taskBL.Desc, taskBL.Column, taskBL.DueDate, taskBL.CTime, taskBL.TaskID));
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<TaskSL>(e.Message));
            }
        }
        /// <summary>
        /// This method updates the task description
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">The unique board name in which the task is</param>
        /// <param name="taskID">A unique id for the task</param>
        /// <param name="desc">The description of the task</param>
        /// <param name="column">The column in which the task is</param>
        /// <returns>A TaskSL or an error</returns>
        public string UpdateDesc(string email, string boardName, long taskID, string desc, string column)
        {
            try
            {
                TaskBL taskBL = BF.UpdateDesc(email, boardName, taskID, desc, column);
                Response<TaskSL> res = new Response<TaskSL>(null, new TaskSL(taskBL.Title, taskBL.Desc, taskBL.Column, taskBL.DueDate, taskBL.CTime, taskBL.TaskID));
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<TaskSL>(e.Message));
            }
        }
        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">The unique board name in which the task is</param>
        /// <param name="taskID">A unique id for the task</param>
        /// <param name="dueDate">The due date of the task</param>
        /// <param name="column">The column in which the task is</param>
        /// <returns>A TaskSL or an error</returns>
        public string UpdateDueDate(string email, string boardName, long taskID, DateTime dueDate, string column)
        {
            try
            {
                TaskBL taskBL = BF.UpdateDueDate(email, boardName, taskID, dueDate, column);
                Response<TaskSL> res = new Response<TaskSL>(null, new TaskSL(taskBL.Title, taskBL.Desc, taskBL.Column , taskBL.DueDate, taskBL.CTime, taskBL.TaskID));
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<TaskSL>(e.Message)); ;
            }
        }
        /// <summary>
        /// This method moves a task to the next column if exists
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">The unique board name to which the task needs to be added</param>
        /// <param name="Id">A unique id for the task</param>
        /// <returns>A boolean - true if the move was succesful and false othewise ot an error</returns>
        public string MoveTask(String email, String boardName, long taskID) 
        {
            try
            {
                TaskBL taskBL = BF.MoveTask(email, boardName, taskID);
                Response<TaskSL> res = new Response<TaskSL>(null ,new TaskSL(taskBL.Title, taskBL.Desc, taskBL.Column, taskBL.DueDate, taskBL.CTime, taskBL.TaskID));
                return JsonSerializer.Serialize(res);  
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<TaskSL>(e.Message));
            }
        }
        /// <summary>
        /// This method return all in progress tasks of a user from all of his\hers board
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>A list conatining all the inprogress task of a user or an error</returns>

        public string InProgressList(String email)
        {
            try
            {
                List<TaskBL> inProgressList = BF.InProgressList(email);
                Response<List<TaskSL>> res = new Response<List<TaskSL>>(null, null);//add task list instaed of null
                return JsonSerializer.Serialize(res); 
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<TaskSL>(e.Message));
            }
        }
    }
}
