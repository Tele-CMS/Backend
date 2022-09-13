using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientFaxModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string SourceFaxNumber { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public int CityID { get; set; }
        public int PharmacyID { get; set; }
        public string PharmacyAddress { get; set; }
        public string PharmacyFaxNumber { get; set; }
        public int CreatedBy { get; set; }
        public DateTime SentDate { get; set; }
        public string PrescriptionId { get; set; }
        public int IsFax { get; set; }
        public int FaxStatus { get; set; }
        public bool Issentprescription { get; set; }
    }
}