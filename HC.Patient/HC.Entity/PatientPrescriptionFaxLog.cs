using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientPrescriptionFaxLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
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
    }
}
