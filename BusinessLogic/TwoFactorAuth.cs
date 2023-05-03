using System.Configuration;
using Budget.Common;
using System.Security.Cryptography;
using System;
//TODO REMOVE
//using Twilio;

namespace Budget.BusinessLogic
{
    public static class TwoFactorAuth
    {
        const string EMAILSUBJECT = "New Validation Key";
        const string EMAILBODY = "Validation key: ";
        const string EMAILPRIORITY = "M";
        const string PASSWORDCHARSNUMERIC = "0123456789";
        const int PASSWORDLENGTH = 10;

        public static bool SendTwoFactorAuthentication(string username)
        {
            //TODO check for two factor auth preference

            string randomPassword = GenerateRandomTwoFactorAuth();
            //TODO save randomPassword to database
            //TODO get email from database for sending emails
            string emailAddress = ConfigurationManager.AppSettings["SMTPServerUsername"];

            Email.EmailDelivery(emailAddress, EMAILSUBJECT, EMAILBODY + randomPassword, EMAILPRIORITY);
            return false;
        }

        private static string GenerateRandomTwoFactorAuth()
        {
            try
            {
                char[][] charGroups = new char[][]
                {
                    PASSWORDCHARSNUMERIC.ToCharArray(),
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
