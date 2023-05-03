using Budget.Extension;
using System;

namespace Budget.Objects
{
    public class UsersContactInformation
    {
        public UsersContactInformation()
        {
        }

        [ColumnMapping(Source = "UsersContactInformationId")]
        public int UsersContactInformationId { get; set; }

        [ColumnMapping(Source = "UsersDemographicId")]
        public int UserDemographicId { get; set; }

        [ColumnMapping(Source = "UsersContactInformationTypeId")]
        public int UsersContactInformationTypeId { get; set; }

        [ColumnMapping(Source = "ContactValue")]
        public string ContactValue { get; set; }

        [ColumnMapping(Source = "IsPrimary")]
        public bool IsPrimary { get; set; }

        [ColumnMapping(Source = "InsertedOnDate")]
        public DateTime InsertedOnDate { get; set; }

        [ColumnMapping(Source = "LastModifiedOnDate")]
        public DateTime LastModifiedOnDate { get; set; }
    }
}
