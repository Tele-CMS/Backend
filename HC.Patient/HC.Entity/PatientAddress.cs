using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientAddress : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AddressID")]
        public int Id { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public bool? IsMailingSame { get; set; }
        public byte[] Address1 { get; set; }
        public byte[] Address2 { get; set; }
        public byte[] City { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("MasterState")]
        public int StateID { get; set; }
        [NotMapped]
        public string StateName
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(MasterState.StateName))
                    {
                        return MasterState.StateName;
                    }
                    return string.Empty;
                }
                catch (Exception)
                {

                    return string.Empty;
                }
            }
            set { }
        }

        
        public byte[] Zip { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("MasterCountry")]
        public int CountryID { get; set; }
        [ForeignKey("MasterAddressType")]
        public int? AddressTypeID { get; set; }
        public string Others { get; set; }
        public bool IsPrimary { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("MasterPatientLocation")]
        public int PatientLocationID { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public byte[] ApartmentNumber { get; set; }

        /// <summary>
        /// Following are the relationship tables
        /// </summary>
        public virtual Patients Patient { get; set; }
        public virtual MasterState MasterState { get; set; }
        public virtual MasterCountry MasterCountry { get; set; }
        public virtual MasterPatientLocation MasterPatientLocation { get; set; }
        public virtual GlobalCode MasterAddressType { get; set; }
    }
}
