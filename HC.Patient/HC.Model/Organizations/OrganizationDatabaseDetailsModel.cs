using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Organizations
{
    public class OrganizationDatabaseDetailsModel
    {
        public int? DatabaseDetailID { get; set; }
        public string Organizations { get; set; }
        public int? NoOfOrg { get; set; }
        public string DatabaseName { get; set; }
        public string Password { get; set; }
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsCentralised { get; set; }
        public bool? IsDeleted { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? RowNumber { get; set; }
        public int? TotalRecords { get; set; }
        public double? TotalPages { get; set; }
    }



}
