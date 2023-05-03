using Budget.Common;
using Budget.Objects;
using Budget.Services;
using System;
using System.Text.RegularExpressions;
using static Budget.Common.EnumTypes;

namespace Budget.BusinessLogic
{
    public class CreateUser
    {
        //TODO move const strings to resource file
        const string FIRSTNAMEREQUIRED = "First name is required.";
        const string FIRSTNAMEMAXLENGTH = "First name max length is 50.";
        const string LASTNAMEREQUIRED = "Last name is required.";
        const string LASTNAMEMAXLENGTH = "Last name max length is 50.";
        const string EMAILINVALID = "Email is invalid.";
        const string EMAILREQUIRED = "Email is required.";
        const string EMAILMAXLENGTH = "Email max length is 50.";
        const string USERNAMEREQUIRED = "Username is required.";
        const string USERNAMEMAXLENGTH = "Username max length is 50 characters.";
        const string PASSWORDREQUIRED = "Password is required.";
        const string PASSWORDNUMBER = "Password must contain a number.";
        const string PASSWORDUPPERCASE = "Password must contain an upper case character.";
        const string PASSWORDLOWERCASE = "Password must contain a lower case character.";
        const string PASSWORDMINLENTH = "Password min length is 8 characters.";
        const string PASSWORDMAXLENTH = "Password max length is 50 characters.";
        const string PASSWORDSPECIALCHARACTER = "Password must cotain a special character.";
        const string USERCREATIONFAILED = "Failed to create user.";
        const string USERALREADYEXISTS = "Username already exists.";
        const string EMAILALREADYEXISTS = "Email already exists";
        const string USERCREATEDBUTUNKNOWNERROR = "The user was created, but some other error occured.";
        const string EMAILSUBJECT = "New User Created - Welcome!";
        const string EMAILBODY = "Thanks for signing up to use our system.";
        const string EMAILPRIORITY = "M";

        public static Tuple<bool, string> CreateNewUser(string firstName, string lastName, string email, string username, string password)
        {
            Users User = UsersService.GetUser(username);
            if (User.Username != null)
            {
                return new Tuple<bool, string>(false, USERALREADYEXISTS);
            }

            UsersContactInformation UserContactInfo = UsersService.GetAllUserContactInformationByEmail(email);
            if (UserContactInfo.ContactValue != null)
            {
                return new Tuple<bool, string>(false, EMAILALREADYEXISTS);
            }

            Tuple<bool, string> firstNameTp = ValidateFirstName(firstName);
            if (!firstNameTp.Item1)
            {
                return firstNameTp;
            }

            Tuple<bool, string> lastNameTp = ValidateLastName(lastName);
            if (!lastNameTp.Item1)
            {
                return lastNameTp;
            }

            Tuple<bool, string> emailTp = ValidateEmail(email);
            if (!emailTp.Item1)
            {
                return emailTp;
            }

            Tuple<bool, string> usernameTp = ValidateUsername(username);
            if (!usernameTp.Item1)
            {
                return usernameTp;
            }

            Tuple<bool, string> passwordTp = ValidatePassword(password);
            if (!passwordTp.Item1)
            {
                return passwordTp;
            }

            Tuple<bool, string> createNewUser = CreateNewUser(firstName, lastName, email, UsersContactInformationTypeEnum.Email.ToString(), username, password);
            if (!createNewUser.Item1)
            {
                return createNewUser;
            }

            Tuple<bool, string> userLogin = Login.UserLogin(username, password);
            if (!userLogin.Item1)
            {
                return new Tuple<bool, string>(false, USERCREATEDBUTUNKNOWNERROR);
            }

            return new Tuple<bool, string>(true, String.Empty);
        }

        private static Tuple<bool, string> CreateNewUser(string firstName, string lastName, string email, string primaryContactPref, string username, string password)
        {
            if (!UsersService.CreateNewUser(firstName, lastName, email, primaryContactPref, username, password))
            {
                return new Tuple<bool, string>(false, USERCREATIONFAILED);
            }

            Email.EmailDelivery(email, EMAILSUBJECT, EMAILBODY, EMAILPRIORITY);

            return new Tuple<bool, string>(true, string.Empty);
        }

        private static Tuple<bool, string> ValidateFirstName(string firstName)
        {
            if (firstName == String.Empty)
            {
                return new Tuple<bool, string>(false, FIRSTNAMEREQUIRED);
            }

            if (firstName.Length > 50)
            {
                return new Tuple<bool, string>(false, FIRSTNAMEMAXLENGTH);
            }

            return new Tuple<bool, string>(true, String.Empty);
        }

        private static Tuple<bool, string> ValidateLastName(string lastName)
        {
            if (lastName == String.Empty)
            {
                return new Tuple<bool, string>(false, LASTNAMEREQUIRED);
            }

            if (lastName.Length > 50)
            {
                return new Tuple<bool, string>(false, LASTNAMEMAXLENGTH);
            }

            return new Tuple<bool, string>(true, String.Empty);
        }

        private static Tuple<bool, string> ValidateEmail(string email)
        {
            if (email == String.Empty)
            {
                return new Tuple<bool, string>(false, EMAILREQUIRED);
            }

            try
            {
                new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                return new Tuple<bool, string>(false, EMAILINVALID);
            }

            if (email.Length > 50)
            {
                return new Tuple<bool, string>(false, EMAILMAXLENGTH);
            }

            return new Tuple<bool, string>(true, String.Empty);
        }

        private static Tuple<bool, string> ValidateUsername(string username)
        {
            if (username == String.Empty)
            {
                return new Tuple<bool, string>(false, USERNAMEREQUIRED);
            }

            if (username.Length > 50)
            {
                return new Tuple<bool, string>(false, USERNAMEMAXLENGTH);
            }

            return new Tuple<bool, string>(true, String.Empty);
        }

        public static Tuple<bool, string> ValidatePassword(string password)
        {
            if (password == String.Empty)
            {
                return new Tuple<bool, string>(false, PASSWORDREQUIRED);
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]");
            var hasSpecialChar = new Regex(@"[!@#\$%\^&\*]");

            if (password.Length < 8)
            {
                return new Tuple<bool, string>(false, PASSWORDMINLENTH);
            }

            if (password.Length > 50)
            {
                return new Tuple<bool, string>(false, PASSWORDMAXLENTH);
            }

            if (!hasNumber.IsMatch(password))
            {
                return new Tuple<bool, string>(false, PASSWORDNUMBER);
            }

            if (!hasUpperChar.IsMatch(password))
            {
                return new Tuple<bool, string>(false, PASSWORDUPPERCASE);
            }

            if (!hasLowerChar.IsMatch(password))
            {
                return new Tuple<bool, string>(false, PASSWORDLOWERCASE);
            }

            if (!hasSpecialChar.IsMatch(password))
            {
                return new Tuple<bool, string>(false, PASSWORDSPECIALCHARACTER);
            }

            return new Tuple<bool, string>(true, String.Empty);
        }
    }
}
