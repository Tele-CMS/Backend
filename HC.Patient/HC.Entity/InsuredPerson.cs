using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class InsuredPerson : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("InsuredPersonId")]
        public int Id { get; set; }

        [StringLength(10)]
        public string Title { get; set; }

        [StringLength(100)]        
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }
        
        [Required]
        [RequiredNumber]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }

        [Required]
        [RequiredNumber]
        [ForeignKey("PatientInsuranceDetails")]
        public int PatientInsuranceID { get; set; }
        
        [Required]
        [RequiredNumber]
        [ForeignKey("MasterRelationship")]
        public int RelationshipID { get; set; }

        [Column(TypeName="varchar(100)")]
        public string OtherRelationshipName { get; set; }

        public DateTime? Dob { get; set; }

        [StringLength(200)]
        public string Address1 { get; set; }
        
        [StringLength(200)]
        public string Address2 { get; set; }

        [StringLength(100)]
        public string City { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("MasterState")]
        public int StateID { get; set; }

        
        [NotMapped]
        public string StateName
        {
            get { return this.MasterState == null ? null : this.MasterState.StateName; }
        }
        [RequiredNumber]        
        [ForeignKey("MasterCountry")]
        public int CountryID { get; set; }
        
        [StringLength(20)]
        public string Zip { get; set; }

        public double? Latitude { get; set; }
        
        public double? Longitude { get; set; }

        
        [StringLength(20)]
        public string ApartmentNumber { get; set; }

        [Required]
        [RequiredNumber]
        [ForeignKey("MasterGender")]
        public int GenderID { get; set; }
        
        public string GenderName
        {
            get
            {
                try
                {
                    return MasterGender.Gender;
                }
                catch (Exception)
                {

                    return null;
                }
            }
        }
        
        public virtual Patients Patient { get; set; }                
        public virtual PatientInsuranceDetails PatientInsuranceDetails { get; set; }
        public virtual MasterState MasterState { get; set; }
        public virtual MasterCountry MasterCountry { get; set; }
        public virtual MasterGender MasterGender { get; set; }
        public virtual MasterRelationship MasterRelationship { get; set; }        
    }
}