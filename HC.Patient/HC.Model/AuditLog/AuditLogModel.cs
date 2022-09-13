using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.AuditLog
{
    public class AuditLogModel
    {
        public int Id { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Action { get; set; }
        public DateTime LogDate { get; set; }

        public string ColumnName { get; set; }
        public string ScreenName { get; set; }

        public string TableName { get; set; }
        public string EncryptionCode { get; set; }
        public Nullable<bool> AccessedToDelete { get; set; }
        public string PatientName { get; set; }
        public string CreatedByName { get; set; }
        public string IPAddress { get; set; }

        public decimal TotalRecords { get; set; }

        public string ParentInfo { get; set; }

        public int ColumnID { get; set; }
        public string LoginAttempt { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
    }


    public class GetForeignTableData
    {
        public string Value { get; set; }
    }


}
