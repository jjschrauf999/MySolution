using System;
using Budget.Objects;
using Budget.Services;

namespace Budget.BusinessLogic
{
    public class Login
    {
        //TODO move const strings to resource file
        const string INVALIDLOGIN = "Invalid login.";
        const string USERISLOCKED = "User is locked.";
        const string USERISLOCKEDTOOMANYFAILEDLOGINS = "User is locked because of too many failed logins.";

        public static Tuple<bool, string> UserLogin(string username, string password)
        {
            Users User = UsersService.GetUser(username);

            Tuple<bool, string> userValid = ValidateUser(User);
            if (!userValid.Item1)
            {
                return userValid;
            }

            Tuple<bool, string> userActive = IsUserActive(User);
            if (!userActive.Item1)
            {
                return userActive;
            }

            Tuple<bool, string> userNotLocked = IsUserNotLocked(User);
            if (!userNotLocked.Item1)
            {
                return userNotLocked;
            }

            Tuple<bool, string> userPasswordValid = ValidateUserPassword(User, password);
            if (!userPasswordValid.Item1)
            {
                FailedLoginAttempt(User.UserId);

                if (UsersService.GetUser(User.UserId).IsLocked)
                {
                    return new Tuple<bool, string>(false, USERISLOCKEDTOOMANYFAILEDLOGINS);
                }

                return userPasswordValid;
            }

            SuccessfulLoginAttempt(User.UserId);

            return new Tuple<bool, string>(true, String.Empty);
        }

        public static bool UserPasswordResetRequired(string username)
        {
            return UsersService.GetUser(username).PasswordChangeRequired;
        }

        private static Tuple<bool, string> ValidateUser(Users User)
        {
            if (User.Username == null)
            {
                return new Tuple<bool, string>(false, INVALIDLOGIN);
            }

            return new Tuple<bool, string>(true, String.Empty);
        }

        private static Tuple<bool, string> IsUserNotLocked(Users User)
        {
            if (User.IsLocked)
            {
                return new Tuple<bool, string>(false, USERISLOCKED);
            }

            return new Tuple<bool, string>(true, String.Empty);
        }

        private static Tuple<bool, string> IsUserActive(Users User)
        {
            if (!User.IsActive)
            {
                return new Tuple<bool, string>(false, INVALIDLOGIN);
            }

            return new Tuple<bool, string>(true, String.Empty);
        }

        private static Tuple<bool, string> ValidateUserPassword(Users User, string password)
        {
            if (User.Password != password)
            {
                return new Tuple<bool, string>(false, INVALIDLOGIN);
            }

            return new Tuple<bool, string>(true, String.Empty);
        }

        private static void SuccessfulLoginAttempt(int userId)
        {
            LoginsService.Login(userId, false);
        }

        private static void FailedLoginAttempt(int userId)
        {
            LoginsService.Login(userId, true);
        }
    }
}
