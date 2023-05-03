using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Budget.Data
{
    public class LoginsData
    {
        public static bool Login(int userId, bool loginFailed)
        {
            string storedProcedure = "Logins_Create";
            int maxFailedLogins = Convert.ToInt32(ConfigurationManager.AppSettings["MaxFailedLogins"]);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@LoginFailed", loginFailed),
                new SqlParameter("@MaxFailedLogins", maxFailedLogins)
            };

            return Convert.ToBoolean(DatabaseHelper.ExecuteScalar(storedProcedure, parameters));
        }
    }
}
