using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model
{
    public class MasterServicesModel
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int GlobalCodeId { get; set; }
        public int TotalRecords {get;set;}
    }
}
