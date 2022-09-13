using HC.Common.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientMedicalFamilyHistory : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MedicalFamilyHistoryId")]
        public int Id { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        [Required]
        public string LastName { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("MasterGender")]
        public int GenderID { get; set; }
        [NotMapped]
        public string Gender
        {
            get
            {
                try
                {
                    return MasterGender.Gender;
                    //return ((MasterGender != null && string.IsNullOrEmpty(MasterGender.Gender)) ? MasterGender.Gender : string.Empty);
                }
                catch (Exception)
                {

                    return null;
                }
            }
            set { }
        }
        [NotMapped]
        public string PatientName
        {
            get
            {
                try
                {
                    return null;
                    //return Patient.FirstName;
                }
                catch (Exception)
                {

                    return null;
                }
            }
            set { }
        }
        [Required]
        [RequiredDate]
        public DateTime? DOB { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("MasterRelationship")]
        public int RelationshipID { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string OtherRelationshipName { get; set; }
        [NotMapped]
        public string RelationshipName
        {
            get
            {
                try
                {
                    return MasterRelationship.RelationshipName;
                    //return ((MasterRelationship != null && string.IsNullOrEmpty(MasterRelationship.RelationshipName)) ? MasterRelationship.RelationshipName : string.Empty);
                }
                catch (Exception)
                {

                    return null;
                }
            }
            set { }
        }       
        
        public DateTime? DateOfDeath { get; set; }
        public string CauseOfDeath { get; set; }
        public string Observation { get; set; }
        [StringLength(100)]
        public string Others { get; set; }

        /// <summary>
        /// Following are the relation of the table
        /// </summary>
        public virtual Patients Patient { get; set; }
        public virtual MasterGender MasterGender { get; set; }
        public virtual MasterRelationship MasterRelationship { get; set; } 
        public virtual List<PatientMedicalFamilyHistoryDiseases> PatientMedicalFamilyHistoryDiseases { get; set; }
    }
}
