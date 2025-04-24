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
        public BoardService() 
        { 
            this.BF = new BoardFacade();
        }
        /// <summary>
        /// This method creates a board for a user
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">A unique name for the board that needs to be created</param>
        /// <returns>A BoardSL object or an error</returns>
        public string CreateBoard(String email, String boardName)
        {
            try
            {
                BoardBL boardBL = BF.CreateBoard(email, boardName, null);
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
        /// <returns>A boolean - true if the board was succesfully deleted and false otherwise or an error</returns>
        public string DeleteBoard(String email, String boardName)
        {
            try
            {
                bool res = BF.DeleteBoard(email, boardName);
                Response<bool> res1 = new Response<bool>(null,res);
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
                Dictionary<string,BoardBL> listOfBoards = BF.GetAllBoards(email);
                Response<Dictionary<string, BoardSL>> res = new Response<Dictionary<string, BoardSL>>(null, null); //Converting the dictionary to a boardSL dictionary);
                return JsonSerializer.Serialize(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response<Dictionary<string, BoardSL>>(e.Message));
            }
        }
    }
}
