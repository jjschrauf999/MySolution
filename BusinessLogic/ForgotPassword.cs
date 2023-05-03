using Budget.Common;
using Budget.Objects;
using Budget.Services;
using System;
using System.Security.Cryptography;

namespace Budget.BusinessLogic
{
    public class ForgotPassword
    {
        const string EMAILINVALID = "Email is invalid.";
        const string EMAILREQUIRED = "Email is required.";
        const string USERNOTFOUND = "User wasn't found with the associated email.";
        const string UNKNOWNERROR = "Something went wrong reseting the user password.";
        const string EMAILSUBJECT = "New Password.";
        const string EMAILBODY = "New temporary password: ";
        const string EMAILPRIORITY = "M";
        const string PASSWORDCHARSLCASE = "abcdefghijklmnopqrstuvwxyz";
        const string PASSWORDCHARSUCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string PASSWORDCHARSNUMERIC = "0123456789";
        const string PASSWORDCHARSSPECIAL = "~!@#$%^&*_-+=:;.?";
        const int PASSWORDLENGTH = 10;

        public static Tuple<bool, string> UserForgotPassword(string email)
        {
            Tuple<bool, string> emailTp = ValidateEmail(email);
            if (!emailTp.Item1)
            {
                return emailTp;
            }

            Tuple<bool, string> resetPassword = ResetUserPassword(email);
            if (!resetPassword.Item1)
            {
                return resetPassword;
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

            return new Tuple<bool, string>(true, String.Empty);
        }
        
        private static Tuple<bool, string> ResetUserPassword(string email)
        {
            Users User = UsersService.GetUserByEmail(email);

            if (User.Username == null)
            {
                Console.WriteLine(USERNOTFOUND);
                return new Tuple<bool, string>(true, String.Empty);
            }

            string newPassword = GenerateRandomPassword();

            bool resetPassword = UsersService.ResetPassword(User.UserId, newPassword, true);
            if (!resetPassword)
            {
                Console.WriteLine(UNKNOWNERROR);
                return new Tuple<bool, string>(true, String.Empty);
            }

            try
            {
                Email.EmailDelivery(email, EMAILSUBJECT, EMAILBODY + newPassword, EMAILPRIORITY);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new Tuple<bool, string>(true, String.Empty);
        }

        private static string GenerateRandomPassword()
        {
            try
            {
                char[][] charGroups = new char[][]
                {
                    PASSWORDCHARSLCASE.ToCharArray(),
                    PASSWORDCHARSUCASE.ToCharArray(),
                    PASSWORDCHARSNUMERIC.ToCharArray(),
                    PASSWORDCHARSSPECIAL.ToCharArray()
                };

                int[] charsLeftInGroup = new int[charGroups.Length];

                for (int i = 0; i < charsLeftInGroup.Length; i++)
                {
                    charsLeftInGroup[i] = charGroups[i].Length;
                }

                int[] leftGroupsOrder = new int[charGroups.Length];

                for (int i = 0; i < leftGroupsOrder.Length; i++)
                {
                    leftGroupsOrder[i] = i;
                }

                byte[] randomBytes = new byte[4];

                new RNGCryptoServiceProvider().GetBytes(randomBytes);

                int seed = (randomBytes[0] & 0x7f << 0x18 |
                            randomBytes[1] << 0x10 |
                            randomBytes[2] << 8 |
                            randomBytes[3]);

                Random random = new Random(seed);

                char[] password = null;
                password = new char[PASSWORDLENGTH];

                int nextCharIdx;
                int nextGroupIdx;
                int nextLeftGroupsOrderIdx;
                int lastCharIdx;
                int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

                for (int i = 0; i < password.Length; i++)
                {
                    if (lastLeftGroupsOrderIdx == 0)
                    {
                        nextLeftGroupsOrderIdx = 0;
                    }
                    else
                    {
                        nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx);
                    }

                    nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                    lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                    if (lastCharIdx == 0)
                    {
                        nextCharIdx = 0;
                    }
                    else
                    {
                        nextCharIdx = random.Next(0, lastCharIdx + 1);
                    }

                    password[i] = charGroups[nextGroupIdx][nextCharIdx];

                    if (lastCharIdx == 0)
                    {
                        charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
                    }
                    else
                    {
                        if (lastCharIdx != nextCharIdx)
                        {
                            char temp = charGroups[nextGroupIdx][lastCharIdx];
                            charGroups[nextGroupIdx][lastCharIdx] = charGroups[nextGroupIdx][nextCharIdx];
                            charGroups[nextGroupIdx][nextCharIdx] = temp;
                        }

                        charsLeftInGroup[nextGroupIdx]--;
                    }

                    if (lastLeftGroupsOrderIdx == 0)
                    {
                        lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                    }
                    else
                    {
                        if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                        {
                            int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                            leftGroupsOrder[lastLeftGroupsOrderIdx] = leftGroupsOrder[nextLeftGroupsOrderIdx];
                            leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                        }

                        lastLeftGroupsOrderIdx--;
                    }
                }

                return new string(password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
