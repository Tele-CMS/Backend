using System;

namespace HC.Patient.Model.Patient
{
    public class SearchInfo
    {
        public string SearchKey { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public int? ClientStatus { get; set; }
        public int? Gender { get; set; }
        public int? PatientID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDOB { get; set; }
        public DateTime? ToDOB { get; set; }
    }
}
