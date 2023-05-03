using Budget.Objects;
using Budget.Services;
using System;

namespace Budget.BusinessLogic
{
    public class ResetPassword
    {
        const string INVALIDPASSWORD = "Invalid password.";
        const string NEWPASSWORDSDONTMATCH = "New passwords don't match.";
        const string RESETPASSWORDFAILED = "Failed to reset password.";
        const string NEWPASSWORDMATCHESEXISTINGPASSWORD = "New password matches the existing password.";

        public static Tuple<bool, string> ResetUserPassword(string username, string existingPassword, string newPassword, string confirmPassword)
        {
            Users User = UsersService.GetUser(username);

            Tuple<bool, string> existingPasswordTp = ValidateExistingPassword(User, existingPassword);
            if (!existingPasswordTp.Item1)
            {
                return existingPasswordTp;
            }

            Tuple<bool, string> newPasswordTp = UpdatePassword(User, newPassword, confirmPassword);
            if (!newPasswordTp.Item1)
            {
                return newPasswordTp;
            }

            return new Tuple<bool, string>(true, String.Empty);
        }

        private static Tuple<bool, string> ValidateExistingPassword(Users user, string existingPassword)
        {
            if (user.Password != existingPassword)
            {
                return new Tuple<bool, string>(false, INVALIDPASSWORD);
            }

            return new Tuple<bool, string>(true, String.Empty);
        }

        private static Tuple<bool, string> UpdatePassword(Users user, string newPassword, string confirmPassword)
        {
            if (user.Password == newPassword)
            {
                return new Tuple<bool, string>(false, NEWPASSWORDMATCHESEXISTINGPASSWORD);
            }

            Tuple<bool, string> password = CreateUser.ValidatePassword(newPassword);
            if (!password.Item1)
            {
                return password;
            }

            if (newPassword != confirmPassword)
            {
                return new Tuple<bool, string>(false, NEWPASSWORDSDONTMATCH);
            }

            if (UsersService.ResetPassword(user.UserId, newPassword, false))
            {
                return new Tuple<bool, string>(false, RESETPASSWORDFAILED);
            }

            return new Tuple<bool, string>(true, String.Empty);
        }
    }
}
