using Budget.Extension;
using System;

namespace Budget.Objects
{
    public class UsersDemographic
    {
        public UsersDemographic()
        {
        }

        [ColumnMapping(Source = "UsersDemographicId")]
        public int UsersDemographicId { get; set; }

        [ColumnMapping(Source = "UserId")]
        public int UserId { get; set; }

        [ColumnMapping(Source = "FirstName")]
        public string FirstName { get; set; }

        [ColumnMapping(Source = "LastName")]
        public string LastName { get; set; }

        [ColumnMapping(Source = "InsertedOnDate")]
        public DateTime InsertedOnDate { get; set; }

        [ColumnMapping(Source = "LastModifiedOnDate")]
        public DateTime LastModifiedOnDate { get; set; }
    }
}
