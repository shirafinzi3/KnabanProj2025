using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal class BoardUsersController
    {
        private readonly string connectionString;
        private const string TableName = "BoardUsers";
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BoardUsersController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            connectionString = $"Data Source={path}; Version=3;";
        }

        public List<BoardUsersDTO> SelectByBoard(long boardID)
        {
            List<BoardUsersDTO> results = new List<BoardUsersDTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT boardID, userEmail FROM {TableName} WHERE {BoardUsersDTO.boardIDColumnName} = @boardID";
                command.Parameters.AddWithValue("@boardID", boardID);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToBoardUsers(dataReader));
                    }
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
            return results;
        }

        public List<BoardUsersDTO> SelectByUser(string userEmail)
        {
            List<BoardUsersDTO> results = new List<BoardUsersDTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT boardID, userEmail FROM {TableName} WHERE {BoardUsersDTO.userEmailColumnName} = @userEmail";
                command.Parameters.AddWithValue("@userEmail", userEmail);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToBoardUsers(dataReader));
                    }
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
            return results;
        }

        public bool Insert(BoardUsersDTO buDTO)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({BoardUsersDTO.boardIDColumnName} ,{BoardUsersDTO.userEmailColumnName})" +
                        $"VALUES (@boardID, @userEmail);";

                    SQLiteParameter ubBoardIDParam = new SQLiteParameter(@"boardID", buDTO.BoardID);
                    SQLiteParameter ubUserEmailParam = new SQLiteParameter(@"userEmail", buDTO.UserEmail);

                    command.Parameters.Add(ubBoardIDParam);
                    command.Parameters.Add(ubUserEmailParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    Log.Error($"Failed to insert boardUser pair into DB");
                    throw new InvalidOperationException($"Failed to insert boardUser pair into DB");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        public List<BoardUsersDTO> SelectAll()
        {
            List<BoardUsersDTO> results = new List<BoardUsersDTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {TableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToBoardUsers(dataReader));
                    }
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
            return results;
        }


        public bool Delete(BoardUsersDTO buToBeDeleted)
             {
            int res = -1;

            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {TableName} WHERE (({BoardUsersDTO.boardIDColumnName}={buToBeDeleted.BoardID}) AND ({BoardUsersDTO.userEmailColumnName}={buToBeDeleted.UserEmail}))"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    Log.Error($"Failed to delete Board User pair from DB");
                    throw new InvalidOperationException($"Failed to delete Board User pair from DB");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }
        private BoardUsersDTO ConvertReaderToBoardUsers(SQLiteDataReader reader)
        {
            return new BoardUsersDTO(reader.GetInt64(0), reader.GetString(1));

        }
    }
}
