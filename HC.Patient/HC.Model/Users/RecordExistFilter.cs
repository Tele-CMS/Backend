using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Users
{
    public class RecordExistFilter
    {
        public string TableName { get; set; }
        public string  Value { get; set; }
        public string ColmnName { get; set; }
        public int? ID { get; set; }
        public string LabelName { get; set; }
        public int OrganizationID { get; set; }

    }

    public class NoOfRecordsModel
    {
        public int NoOfRecords { get; set; }
    }
}
