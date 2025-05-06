using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.BuisnessLayer;

namespace Backend.ServiceLayer
{
    public class BoardService
    {
        private BoardFacade BF;
        /// <summary>
        /// This constructor initiates a new Board Facade object
        /// </summary>
        internal BoardService(BoardFacade BF) 
        {
            this.BF = BF;
        }
        /// <summary>
        /// This method creates a board for a user
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">A unique name for the board that needs to be created</param>
        /// <param name="maxTasks"> Array with maxTasks per column (-1 no restriction) </param>
        /// <returns>A BoardSL object or an error</returns>

        public string CreateBoard(String email, String boardName, int[] maxTasks)
        {
            try
            {
                BoardBL boardBL = BF.CreateBoard(email, boardName, maxTasks);
                Response<BoardSL> res = new Response<BoardSL>(null, new BoardSL(boardBL.BoardName)); 
                return JsonSerializer.Serialize(res);
            }
            catch(Exception e) 
            {
                return JsonSerializer.Serialize(new Response<BoardSL>(e.Message));
            }
        }
        /// <summary>
        /// This method deletes an existing board
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">A unique name of the board that need to be deleted</param>
        /// <returns>void</returns>
        public string DeleteBoard(String email, String boardName)
        {
            try
            {
                BF.DeleteBoard(email, boardName);
                Response<String> res1 = new Response<String>();
                return JsonSerializer.Serialize(res1);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<BoardSL>(e.Message));
            }

        }
        /// <summary>
        /// This method returns all the boards of a specific user
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <returns>All the boards of a given user</returns>
        public string GetAllBoards(string email)
        {
            try
            {
                Dictionary<string,BoardBL> listOfBoardsBL = BF.GetAllBoards(email);
                Dictionary<string, BoardSL> listOfBoardsSL = new Dictionary<string, BoardSL>();
                foreach (BoardBL boardBL in listOfBoardsBL.Values)
                {
                    listOfBoardsSL[boardBL.BoardName] = new BoardSL(boardBL.BoardName);
                }
                Response<Dictionary<string, BoardSL>> res = new Response<Dictionary<string, BoardSL>>(null, listOfBoardsSL); //Converting the dictionary to a boardSL dictionary);
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<Dictionary<string, BoardSL>>(e.Message));
            }
        }
        /// <summary>
        /// This changes a column max tasks
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">A unique name of the board that need to be deleted</param>
        /// <param name="colIdx">The column index </param>
        /// <param name="newLim">New limit for the column</param>
        /// <returns>int - new column limit</returns>
        public string ChangeMaxTasks(String email, String boardName, int colIdx, int newLim)
        {
            try
            {
                BF.ChangeMaxTasks(email, boardName, colIdx, newLim);
                Response<int> res1 = new Response<int>(newLim);
                return JsonSerializer.Serialize(res1);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<BoardSL>(e.Message));
            }

        }
        /// <summary>
        /// This gets a column max tasks
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">A unique name of the board that need to be deleted</param>
        /// <param name="colIdx">The column index</param>
        /// <returns>int - new column limit</returns>
        public string GetColumnLimit(String email, String boardName, int colIdx)
        {
            try
            {
                int columnLim = BF.GetColumnLimit(email, boardName, colIdx);
                Response<int> res1 = new Response<int>(columnLim);
                return JsonSerializer.Serialize(res1);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<BoardSL>(e.Message));
            }

        }
        /// <summary>
        /// This gets a column's name
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">A unique name of the board that need to be deleted</param>
        /// <param name="colIdx">The column index</param>
        /// <returns>string - column name </returns>
        public string GetColumnName(String email, String boardName, int colIdx)
        {
            try
            {
                string columnName = BF.GetColumnName(email, boardName, colIdx);
                Response<string> res1 = new Response<string>(columnName);
                return JsonSerializer.Serialize(res1);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<BoardSL>(e.Message));
            }
        }
        /// <summary>
        /// This gets a column's list of tasks
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">A unique name of the board that need to be deleted</param>
        /// <param name="colIdx">The column index</param>
        /// <returns>List of tasks in the specified column </returns>
        public string GetColumn(String email, String boardName, int colIdx)
        {
            try
            {
                Dictionary<long, TaskBL> columnTaskBL =  BF.GetColumn(email, boardName, colIdx);
                List<TaskSL> columnTaskSL = new List<TaskSL>();
                foreach(TaskBL taskBL in columnTaskBL.Values)
                {
                    columnTaskSL.Add(new TaskSL(taskBL.Title, taskBL.Desc, taskBL.DueDate, taskBL.CTime, taskBL.TaskID));
                }
                Response<List<TaskSL>> res1 = new Response<List<TaskSL>>(columnTaskSL);
                return JsonSerializer.Serialize(res1);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<BoardSL>(e.Message));
            }
        }

    }
}
