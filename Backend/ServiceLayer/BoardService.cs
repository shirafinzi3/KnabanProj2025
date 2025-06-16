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

        public string CreateBoard(String email, String boardName)
        {
            try
            {
                BoardBL boardBL = BF.CreateBoard(email, boardName);
                Response<BoardSL> res = new Response<BoardSL>(null, new BoardSL(boardBL.BoardName,boardBL.BoardID,boardBL.Owner)); 
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
                Response<string> res1 = new Response<string>();
                return JsonSerializer.Serialize(res1);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<string>(e.Message));
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
                Response<string> res1 = new Response<string>(null, columnName);
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
                    columnTaskSL.Add(new TaskSL(taskBL.Title, taskBL.Desc, taskBL.DueDate, taskBL.GetCTime(), taskBL.TaskID));
                }
                Response<List<TaskSL>> res1 = new Response<List<TaskSL>>(columnTaskSL);
                return JsonSerializer.Serialize(res1);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<BoardSL>(e.Message));
            }
        }
        /// <summary>
        /// This method allows a user join another board
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardID">A unique board id</param>
        /// <returns>An empty response or an error message </returns>
        public string JoinBoard(String email, long boardID)
        {
            try
            {
                BF.JoinBoard(email, boardID);
                Response<string> res = new Response<string>();
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<string>(e.Message));
            }
        }
        /// <summary>
        /// This method allows a user leave another board
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardID">A unique board id</param>
        /// <returns>An empty response or an error message </returns>
        public string LeaveBoard(String email, long boardID)
        {
            try
            {
                BF.LeaveBoard(email, boardID);
                Response<string> res = new Response<string>();
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<BoardSL>(e.Message));
            }
        }
        /// <summary>
        /// This method alllows a user to transfer the ownership of a board that he/she owns
        /// </summary>
        /// <param name="ownerEmail">The email of the current owner</param>
        /// <param name="boardName">The board name which is beign transferd</param>
        /// <param name="otherEmail">The email to which the board is being transfered</param>
        /// <returns>An empty response or an error message </returns>
        public string TransferOwnership(String ownerEmail, string boardName, string otherEmail)
        {
            try
            {
                BF.TransferOwnership(ownerEmail, boardName, otherEmail);
                Response<string> res = new Response<string>();
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<BoardSL>(e.Message));
            }
        }
        /// <summary>
        /// This method returns a board name
        /// </summary>
        /// <param name="boardID">A unique board id</param>
        /// <returns>A response with the board name or an error message </returns>
        public string GetBoardName(long boardID)
        {
            try
            {
                string boardName = BF.GetBoardName(boardID);
                Response<string> res = new Response<string>(null,boardName);
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<BoardSL>(e.Message));
            }
        }
        /// <summary>
        /// This method returns a list of all the ids of the boards that the user is a member of
        /// </summary>
        /// <param name="email">A unique email of the user</param>
        /// <returns>A response with the list of ids or an error message </returns>
        public string GetUserBoards(string email)
        {
            try
            {
                List<long> ids = BF.GetUserBoards(email);
                Response<List<long>> res = new Response<List<long>>(null, ids);
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<List<long>>(e.Message));
            }
        }
        /// <summary>
        /// This method returns a list of all the boards sl's that the user is a member of
        /// </summary>
        /// <param name="email">A unique email of the user</param>
        /// <returns>A response with the list of board sl's or an error message </returns>
        public string GetAllBoards(string email)
        {
            try
            {
                List<BoardBL> boards = BF.GetAllBoards(email);
                List<BoardSL> list = new List<BoardSL>();
                foreach(BoardBL boardBL in boards)
                {
                    list.Add(new BoardSL(boardBL.BoardName, boardBL.BoardID,boardBL.Owner));
                }
                Response<List<BoardSL>> res = new Response<List<BoardSL>>(null, list);
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<List<BoardSL>>(e.Message));
            }
        }

        /// <summary>
        /// This method loads all board data from the data base
        /// <returns>An empty response or an error message</returns>
        public string LoadAllBoards()
        {
            try
            {
                BF.LoadAllBoards();
                Response<string> res = new Response<string>();
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<string>(e.Message));
            }
        }
        /// <summary>
        /// This method deletes all boar data from the data base
        /// <returns>An empty response or an error message</returns>
        public string DeleteAllBoards()
        {
            try
            {
                BF.DeleteAllBoards();
                Response<string> res = new Response<string>();
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<string>(e.Message));
            }
        }

    }
}
