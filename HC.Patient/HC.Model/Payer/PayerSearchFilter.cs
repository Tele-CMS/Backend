using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Payer
{
    public class PayerSearchFilter
    { 
        public string ID { get; set; }
        public int? ID2 { get; set; }
        public string Name { get; set; }
        public int OrganizationID { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public bool? IsPayerInformation { get; set; }
        public bool? IsMasterServiceCode { get; set; }
        public bool? IsPayerActivity { get; set; }
        public bool? IsPayerServiceCode { get; set; }
        public bool? IsPayerActivityCode { get; set; }
        
    }
}
