using Budget.Extension;
using System;

namespace Budget.Objects
{
    public class Users
    {
        public Users()
        { 
        }

        [ColumnMapping(Source = "UserId")]
        public int UserId { get; set; }

        [ColumnMapping(Source = "Username")]
        public string Username { get; set; }

        [ColumnMapping(Source = "Password")]
        public string Password { get; set; }

        [ColumnMapping(Source = "PasswordSetDate")]
        public DateTime PasswordSetDate { get; set; }

        [ColumnMapping(Source = "PasswordChangeRequired")]
        public bool PasswordChangeRequired { get; set; }

        [ColumnMapping(Source = "PasswordSystemGenerated")]
        public bool PasswordSystemGenerated { get; set; }

        [ColumnMapping(Source = "InsertedOnDate")]
        public DateTime InsertedOnDate { get; set; }

        [ColumnMapping(Source = "LastModifiedOnDate")]
        public DateTime LastModifiedOnDate { get; set; }

        [ColumnMapping(Source = "IsActive")]
        public bool IsActive { get; set; }

        [ColumnMapping(Source = "IsLocked")]
        public bool IsLocked { get; set; }
    }
}
