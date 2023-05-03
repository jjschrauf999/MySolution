using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Budget.Extension;

namespace Budget
{
    public class DatabaseHelper
    {
        public static List<T> ExecuteReaderList<T>(string storedProcedure, List<SqlParameter> parameters) where T : new()
        {
            SqlConnection dbConnection = OpenDatabaseConnection();

            try
            {
                SqlDataReader reader = ExecuteReader<T>(storedProcedure, parameters, dbConnection);
                List<T> results = reader.ReadList<T>();
                CloseDatabaseConnection(dbConnection);
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                CloseDatabaseConnection(dbConnection);
                throw;
            }
        }

        public static T ExecuteReader<T>(string storedProcedure, List<SqlParameter> parameters) where T : new()
        {
            SqlConnection dbConnection = OpenDatabaseConnection();

            try
            {
                SqlDataReader reader = ExecuteReader<T>(storedProcedure, parameters, dbConnection);
                T results = reader.Read<T>();
                CloseDatabaseConnection(dbConnection);
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                CloseDatabaseConnection(dbConnection);
                throw;
            }
        }

        private static SqlDataReader ExecuteReader<T>(string storedProcedure, List<SqlParameter> parameters, SqlConnection dbConnection) where T : new()
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand();
                PrepareCommand(sqlCommand, dbConnection, storedProcedure, parameters);
                return sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        internal static object ExecuteScalar(string storedProcedure, List<SqlParameter> parameters)
        {
            SqlConnection dbConnection = OpenDatabaseConnection();

            try
            {
                SqlCommand sqlCommand = new SqlCommand();
                PrepareCommand(sqlCommand, dbConnection, storedProcedure, parameters);
                object retrieval = sqlCommand.ExecuteScalar();
                CloseDatabaseConnection(dbConnection);
                return retrieval;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                CloseDatabaseConnection(dbConnection);
                throw;
            }
        }

        private static SqlConnection OpenDatabaseConnection()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["budgetConnection"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                return sqlConnection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static void CloseDatabaseConnection(SqlConnection sqlConnection)
        {
            try
            {
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static void PrepareCommand(SqlCommand sqlCommand, SqlConnection sqlConnection, string storedProc, List<SqlParameter> parameters)
        {
            try
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = storedProc;
                if (parameters != null)
                {
                    AttachParameters(sqlCommand, parameters);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
        }

        private static void AttachParameters(SqlCommand sqlCommand, List<SqlParameter> parameters)
        {
            try
            {
                foreach (SqlParameter p in parameters)
                {
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    sqlCommand.Parameters.Add(p);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
