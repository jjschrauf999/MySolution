using Budget.Data;
using Budget.Objects;
using System.Collections.Generic;

namespace Budget.Services
{
    public static class UsersService
    {
        public static List<Users> GetUsers()
        {
            return UsersData.GetAllUsers();
        }

        public static Users GetUser(string username)
        {
            return UsersData.GetUser(username);
        }

        public static Users GetUserByEmail(string email)
        {
            return UsersData.GetUserByEmail(email);
        }

        public static Users GetUser(int userId)
        {
            return UsersData.GetUser(userId);
        }

        public static bool CreateNewUser(string firstName, string lastName, string email, string primaryContactPref, string username, string password)
        {
            return UsersData.CreateNewUser(firstName, lastName, email, primaryContactPref, username, password);
        }

        public static UsersContactInformation GetAllUserContactInformationByEmail(string email)
        {
            return UsersData.GetAllUserContactInformationByEmail(email);
        }

        public static bool ResetPassword(int userId, string newPassword, bool systemGenerated)
        {
            return UsersData.ResetPassword(userId, newPassword, systemGenerated);
        }
    }
}
