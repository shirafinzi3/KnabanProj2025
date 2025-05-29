using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controllers
{
    internal class ColumnController
    {
        private readonly string connectionString;
        public const string TableName = "Columns";
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ColumnController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            connectionString = $"Data Source={path}; Version=3;";
        }

        public List<ColumnDTO> SelectAll()
        {
            List<ColumnDTO> results = new List<ColumnDTO>();
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
                        results.Add(ConvertReaderToColumn(dataReader));
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

        public List<ColumnDTO> SelectColumnByBoard(long boardID)
        {
            List<ColumnDTO> results = new List<ColumnDTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {TableName} WHERE {ColumnDTO.BoardIDColumnName} = @boardID";
                command.Parameters.AddWithValue("@boardID", boardID);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToColumn(dataReader));
                    }
                }
                catch
                {
                    Log.Error($"Failed to select column");
                    throw new InvalidOperationException($"Failed to selecrt column");
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

        public bool Insert(ColumnDTO col)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({ColumnDTO.ColumnIDColumnName} ,{ColumnDTO.BoardIDColumnName}, " +
                                                $" {ColumnDTO.ColumnNameColumnName}, {ColumnDTO.MaxTasksColumnName}) " +
                        $"VALUES (@columnID,@boardID, @colName, @maxTasks);";

                    SQLiteParameter colIdParam = new SQLiteParameter(@"columnID", col.ColumnID);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardID", col.BoardID);
                    SQLiteParameter colNameParam = new SQLiteParameter(@"colName", col.ColName);
                    SQLiteParameter maxTasksParam = new SQLiteParameter(@"maxTasks", col.MaxTasks);

                    command.Parameters.Add(colIdParam);
                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(colNameParam);
                    command.Parameters.Add(maxTasksParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    Log.Error($"Failed to insert col {col.ColumnID} to DB");
                    throw new InvalidOperationException($"Failed to insert col {col.ColumnID} to DB");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        public bool Delete(ColumnDTO cToBeDeleted)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {TableName} WHERE {ColumnDTO.ColumnIDColumnName}={cToBeDeleted.ColumnID}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    Log.Error($"Failed to delete column {cToBeDeleted.ColumnID} from DB");
                    throw new InvalidOperationException($"Failed to delete {cToBeDeleted.ColumnID} from DB");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        public bool UpdateMaxTasks(long columnID, string attributeName, int attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {TableName} SET [{attributeName}]=@attributeValue WHERE {ColumnDTO.ColumnIDColumnName}= {columnID}"
                };
                try
                {
                    command.Parameters.AddWithValue("@attributeValue", attributeValue);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    Log.Error($"Failed to update {attributeName} for task {columnID} in DB");
                    throw new InvalidOperationException($"Failed to update {attributeName} for task {columnID} in DB");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        public long SelectMaxColumnID()
        {
            long result = 0;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT MAX({ColumnDTO.ColumnIDColumnName}) FROM {TableName};";
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
                    Log.Error("Failed to select max column id from database");
                    throw new Exception("Failed to select max column id from database");
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

        private ColumnDTO ConvertReaderToColumn(SQLiteDataReader reader)
        {
            return new ColumnDTO(reader.GetInt64(0), reader.GetInt64(1), reader.GetString(2), reader.GetInt32(3));

        }
    }
}

