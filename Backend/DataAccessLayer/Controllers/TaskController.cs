using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal class TaskController
    {
        private readonly string connectionString;
        private const string TableName = "Tasks";
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TaskController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            connectionString = $"Data Source={path}; Version=3;";
        }

        public List<TaskDTO> SelectAll()
        {
            List<TaskDTO> results = new List<TaskDTO>();
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
                        results.Add(ConvertReaderToTask(dataReader));
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

        public List<TaskDTO> SelectTaskByColumn(long columnID)
        {
            List<TaskDTO> results = new List<TaskDTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {TableName} WHERE {TaskDTO.ColumnIDColumnName} = @columnID";
                command.Parameters.AddWithValue("@columnID", columnID);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToTask(dataReader));
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

        public bool Insert(TaskDTO task)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({TaskDTO.TaskIDColumnName} ,{TaskDTO.ColumnIDColumnName}, {TaskDTO.TitleColumnName}," +
                                                $"  {TaskDTO.DueDateColumnName}, {TaskDTO.CTimeColumnName}, {TaskDTO.AssigneeColumnName}) " +
                        $"VALUES (@taskID,@columnID, @title, @desc, @dueDate, @cTime,@assignee);";

                    SQLiteParameter taskIdParam = new SQLiteParameter(@"taskID", task.TaskID);
                    SQLiteParameter columnIdParam = new SQLiteParameter(@"columnID", task.ColumnID);
                    SQLiteParameter titleParam = new SQLiteParameter(@"title", task.Title);
                    SQLiteParameter descParam = new SQLiteParameter(@"desc", task.Desc);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDate", task.DueDate);
                    SQLiteParameter cTimeParam = new SQLiteParameter(@"cTime", task.CTime);
                    SQLiteParameter assigneeParam = new SQLiteParameter(@"assignee", task.Assignee);


                    command.Parameters.Add(taskIdParam);
                    command.Parameters.Add(columnIdParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(cTimeParam);
                    command.Parameters.Add(assigneeParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    Log.Error($"Failed to insert task {task.TaskID} to DB");
                    throw new InvalidOperationException($"Failed to insert task {task.TaskID} to DB");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        public bool Delete(TaskDTO bToBeDeleted)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {TableName} WHERE {TaskDTO.TaskIDColumnName}={bToBeDeleted.TaskID}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    Log.Error($"Failed to delete task {bToBeDeleted.TaskID} from DB");
                    throw new InvalidOperationException($"Failed to delete task {bToBeDeleted.TaskID} from DB");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        public bool Update(long taskID, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,                                                       
                    CommandText = $"UPDATE {TableName} SET [{attributeName}]=@attributeValue WHERE {TaskDTO.TaskIDColumnName}= {taskID}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    Log.Error($"Failed to uodate {attributeName} for task {taskID} in DB");
                    throw new InvalidOperationException($"Failed to uodate {attributeName} for task {taskID} in DB");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        public bool Update(long taskID, string attributeName, DateTime attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,                                                        
                    CommandText = $"UPDATE {TableName} SET [{attributeName}]=@attributeValue WHERE {TaskDTO.TaskIDColumnName}={taskID}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    Log.Error($"Failed to uodate {attributeName} for task {taskID} in DB");
                    throw new InvalidOperationException($"Failed to uodate {attributeName} for task {taskID} in DB");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        /* public bool MoveTask(long taskID, string columnIDColumnName, long newColumnID)
         {
             int res = -1;
             using (var connection = new SQLiteConnection(connectionString))
             {
                 SQLiteCommand command = new SQLiteCommand
                 {
                     Connection = connection,                                                        
                     CommandText = $"UPDATE {TableName} SET [{columnIDColumnName}]= @newColumnID WHERE {TaskDTO.TaskIDColumnName}={taskID}"
                 };
                 try
                 {
                     command.Parameters.Add(new SQLiteParameter(columnIDColumnName, newColumnID));
                     connection.Open();
                     command.ExecuteNonQuery();
                 }
                 catch
                 {
                     Log.Error($"Failed to uodate {columnIDColumnName} for task {taskID} in DB");
                     throw new InvalidOperationException($"Failed to uodate {columnIDColumnName} for task {taskID} in DB");
                 }
                 finally
                 {
                     command.Dispose();
                     connection.Close();
                 }

             }
             return res > 0;
         }*/
        public long SelectMaxTaskID()
        {
            long result = 0;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT MAX({TaskDTO.TaskIDColumnName}) FROM {TableName};";
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
                    Log.Error("Failed to select max task id from database");
                    throw new Exception("Failed to select max task id from database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return result;
        }
        private TaskDTO ConvertReaderToTask(SQLiteDataReader reader)
        {
            return new TaskDTO(reader.GetInt64(0), reader.GetInt64(1), reader.GetString(2), reader.GetString(3),
                                                            reader.GetDateTime(4),  reader.GetDateTime(5), reader.GetString(6));

        }
    }

}
