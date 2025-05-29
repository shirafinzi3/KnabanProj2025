using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using System.Data.SQLite;
using log4net;
using System.Reflection.PortableExecutable;
using System.Threading;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal class BoardController
    {
        private readonly string connectionString;
        private const string TableName = "Boards";
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BoardController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            connectionString = $"Data Source={path}; Version=3;";
        }

        public List<BoardDTO> SelectAll()
        {
            return Select();
        }


        public List<BoardDTO> Select()
        {
            List<BoardDTO> result = new List<BoardDTO>();
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            SQLiteCommand command = new SQLiteCommand(null, connection);
            SQLiteDataReader dataReader = null;
            //Boards
            try
            {
                connection.Open();
                command.CommandText = $"SELECT * FROM {TableName}";
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    BoardDTO bDTO = ConvertReaderToBoard(dataReader);
                    result.Add(bDTO);
                }
            }
            catch (Exception e)
            {
                Log.Error("Failed to select boards from database");
                throw new Exception("Failed to select boards from database");
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                command.Dispose();
                connection.Close();
            }
            //UserBoards
            connection = new SQLiteConnection(connectionString);
            connection.Open();
            try
            {
                foreach (BoardDTO bDTO in result)
                {
                    List<BoardUsersDTO> buDTOs = bDTO.GetBuController().SelectByBoard(bDTO.BoardID, connection);
                    foreach (BoardUsersDTO buDTO in buDTOs)
                    {
                        bDTO.Users.Add(buDTO.UserEmail);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return result;
        }


        public bool Insert(BoardDTO board)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} " +
                        $"({BoardDTO.boardIDColumnName}, {BoardDTO.boardNameColumnName}, {BoardDTO.ownerEmailColumnName}) " +
                        $"VALUES (@boardIDVal, @boardNameVal, @ownerEmailVal);";

                    command.Parameters.Add(new SQLiteParameter(@"boardIDVal", board.BoardID));
                    command.Parameters.Add(new SQLiteParameter(@"boardNameVal", board.Name));
                    command.Parameters.Add(new SQLiteParameter(@"ownerEmailVal", board.OwnerEmail));
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to insert board to database", ex);
                    throw new Exception("Failed to insert board to database", ex);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }

        }

        public bool Update(long boardID, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {TableName} SET [{attributeName}]=@attributeValue WHERE {BoardDTO.boardIDColumnName}={boardID}"
                };
                try
                {
                    command.Parameters.AddWithValue("@attributeValue", attributeValue);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    Log.Error($"Failed to update {attributeName} for board {boardID} in DB");
                    throw new InvalidOperationException($"Failed to update {attributeName} for board {boardID} in DB");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        public bool Delete(BoardDTO board)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            { 
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();

                    //delete tasks
                    command.CommandText =$"DELETE FROM Tasks WHERE ColumnID IN  " +
                        $"                  (SELECT ColumnID FROM Columns WHERE {ColumnDTO.BoardIDColumnName} = @boardIDVal);";
                    command.Parameters.Add(new SQLiteParameter("@boardIDVal", board.BoardID));
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                    //delete columns
                    command.CommandText = $"DELETE FROM Columns WHERE {ColumnDTO.BoardIDColumnName} = @boardIDVal;";
                    command.Parameters.Add(new SQLiteParameter("@boardIDVal", board.BoardID));
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                    // delete board user pairs
                    command.CommandText =
                        $"DELETE FROM BoardUsers WHERE {BoardUsersDTO.boardIDColumnName} = @boardIDVal;";
                    command.Parameters.Add(new SQLiteParameter("@boardIDVal", board.BoardID));
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                    // delete board
                    command.CommandText = $"DELETE FROM {TableName} WHERE {BoardDTO.boardIDColumnName} = @boardIDVal;";
                    command.Parameters.Add(new SQLiteParameter("@boardIDVal", board.BoardID));
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Log.Error("Failed to delete board and its data from database");
                    throw new InvalidOperationException("Failed to delete board from database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }
        public long SelectMaxBoardID()
        {
            long result = 0;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT MAX({BoardDTO.boardIDColumnName}) FROM {TableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read() && !dataReader.IsDBNull(0))
                    {
                        result = dataReader.GetInt64(0);
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Failed to select max board id from database");
                    throw new Exception("Failed to select max board id from database");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }
                    command.Dispose();
                    connection.Close();
                }
            }
            return result;
        }
        private BoardDTO ConvertReaderToBoard(SQLiteDataReader reader)
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string creator = reader.GetString(2);
            return new BoardDTO(id, name, creator);
        }
    }
}