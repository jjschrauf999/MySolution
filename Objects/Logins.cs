using Budget.Extension;
using System;

namespace Budget.Objects
{
    public class Logins
    {
        public Logins()
        {
        }

        [ColumnMapping(Source = "LoginId")]
        public int LoginId { get; set; }

        [ColumnMapping(Source = "UserId")]
        public int UserId { get; set; }

        [ColumnMapping(Source = "LoginAttemptDate")]
        public DateTime LoginAttemptDate { get; set; }

        [ColumnMapping(Source = "FailedLogins")]
        public int FailedLogins { get; set; }

    }
}
