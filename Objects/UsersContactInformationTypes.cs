using Budget.Extension;
using System;

namespace Budget.Objects
{
    public class UsersContactInformationType
    {
        public UsersContactInformationType()
        {
        }

        [ColumnMapping(Source = "UsersContactInformationTypeId")]
        public int UsersContactInformationTypeId { get; set; }

        [ColumnMapping(Source = "ContactInformationType")]
        public string ContactInformationType { get; set; }

        [ColumnMapping(Source = "InsertedOnDate")]
        public DateTime InsertedOnDate { get; set; }

        [ColumnMapping(Source = "LastModifiedOnDate")]
        public DateTime LastModifiedOnDate { get; set; }
    }
}
