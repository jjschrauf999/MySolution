using Budget.Objects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Budget.Data
{
    public static class UsersData
    {
        public static List<Users> GetAllUsers()
        {
            string storedProcedure = "Users_Get";
            List <SqlParameter> parameters = new List<SqlParameter>
            {
            };

            return DatabaseHelper.ExecuteReaderList<Users>(storedProcedure, parameters);
        }

        public static Users GetUser(string username)
        {
            string storedProcedure = "Users_Get_ByUsername";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Username", username)
            };

            return DatabaseHelper.ExecuteReader<Users>(storedProcedure, parameters);
        }

        public static Users GetUserByEmail(string email)
        {
            string storedProcedure = "Users_Get_ByEmail";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", email)
            };

            return DatabaseHelper.ExecuteReader<Users>(storedProcedure, parameters);
        }

        public static Users GetUser(int userId)
        {
            string storedProcedure = "Users_Get_ByUserId";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId)
            };

            return DatabaseHelper.ExecuteReader<Users>(storedProcedure, parameters);
        }

        public static bool CreateNewUser(string firstName, string lastName, string email, string primaryContactPref, string username, string password)
        {
            string storedProcedure = "Users_Create";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@FirstName", firstName),
                new SqlParameter("@LastName", lastName),
                new SqlParameter("@Email", email),
                new SqlParameter("@PrimaryContactPref", primaryContactPref),
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };

            return Convert.ToBoolean(DatabaseHelper.ExecuteScalar(storedProcedure, parameters));
        }

        public static UsersContactInformation GetAllUserContactInformation(int userId)
        {
            string storedProcedure = "Users_ContactInformation_Get";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId)
            };

            return DatabaseHelper.ExecuteReader<UsersContactInformation>(storedProcedure, parameters);
        }

        public static UsersContactInformation GetAllUserContactInformationByEmail(string email)
        {
            string storedProcedure = "UsersContactInformation_Get_ByEmail";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", email)
            };

            return DatabaseHelper.ExecuteReader<UsersContactInformation>(storedProcedure, parameters);
        }

        public static bool ResetPassword(int userId, string newPassword, bool systemGenerated)
        {
            string storedProcedure = "Users_Update";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Password", newPassword),
                new SqlParameter("@PasswordSystemGenerated", systemGenerated)
            };

            return Convert.ToBoolean(DatabaseHelper.ExecuteScalar(storedProcedure, parameters));
        }
    }
}
