using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientPhysician :BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Speciality { get; set; }
        [ForeignKey("MasterSpeciality")]
        public int? SpecialityId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FaxNumber { get; set; }
        public string DEANumber { get; set; }
        public string NPINumber { get; set; }
        [ForeignKey("Patients")]
        public int PatientId { get; set; }
        //[ForeignKey("MasterTaxonomyCodes")]
        //public int? TaxonomyCodesID { get; set; }
        [Column(TypeName ="varchar(200)")]
        public string AddressLine1 { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string AddressLine2 { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string City { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string StateCode { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Country { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string ZipCode { get; set; }

        public virtual Patients Patients { get; set; }
        public virtual GlobalCode MasterSpeciality { get; set; }

       // public virtual MasterTaxonomyCodes MasterTaxonomyCodes { get; set; }

    }
}
