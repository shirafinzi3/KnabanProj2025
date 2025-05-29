using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using log4net;
using System.Threading;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal class UserController
    {
        private readonly string connectionString;
        private const string tableName = "Users";
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            connectionString = $"Data Source={path}; Version=3;";
        }

        public List<UserDTO> SelectAll()
        {
            return Select();
        }
    
        public List <UserDTO> Select()
        {
            List<UserDTO> result = new List<UserDTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null,connection);
                command.CommandText = $"SELECT * FROM {tableName}";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        result.Add(ConvertReaderToUser(dataReader));
                    }
                }
                catch (Exception ex) 
                {
                    Log.Error("Failed to select users from database");
                    throw new Exception("Failed to select users from database");
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

        public bool Insert(UserDTO user)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {tableName} ({UserDTO.emailColumnName}, {UserDTO.passColumnName}) " + $"VALUES (@emailVal, @passwordVal);";
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.Password);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Log.Error("Failed to insert user to database");
                    throw new Exception("Failed to insert user to database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        public bool Delete(UserDTO user)
        {
            int res = -1;
            using( var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    command.CommandText = $"DELETE FROM {tableName} WHERE {UserDTO.emailColumnName}=@emailVal";
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                    command.Parameters.Add(emailParam);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Log.Error("Failed to delete user from database");
                    throw new Exception("Failed to delete user from database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        public bool UpdatePassword(string email, string password)
        {
            int res = -1;
            using( var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null,connection);
                try
                {
                    command.CommandText = $"UPDATE {tableName} SET {UserDTO.passColumnName} = @passwordVal WHERE {UserDTO.emailColumnName}=@emailVal";
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", password);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Log.Error("Failed to update password in database");
                    throw new Exception("Failed to update password in database");

                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }
        private UserDTO ConvertReaderToUser(SQLiteDataReader reader)
        {
            return new UserDTO(reader.GetString(0),reader.GetString(1));
        }
        public void DeleteAll()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE FROM {tableName};";
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to delete all users");
                    throw new Exception("Failed to delete all users from database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
        }
    }
}