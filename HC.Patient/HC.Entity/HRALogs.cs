using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Entity
{
    public class HRALogs:BaseEntity
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public string FileName { get; set; }
        public string ReportType { get; set; }
        public string ReportDate { get; set; }
        public int ReportTypeId { get; set; }
        public int OrganizationId { get; set; }

    }
}
