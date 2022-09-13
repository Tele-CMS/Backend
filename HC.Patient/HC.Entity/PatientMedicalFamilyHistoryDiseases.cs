using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientMedicalFamilyHistoryDiseases : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("PatientMedicalFamilyHistory")]
        public int MedicalFamilyHistoryId { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("MasterICD")]
        public int DiseaseID { get; set; }
        [NotMapped]
        public string Disease1
        {
            get
            {
                try
                {
                    return MasterICD.Code;
                    //return ((MasterICD != null && string.IsNullOrEmpty(MasterICD.Code)) ? MasterICD.Code : string.Empty);
                }
                catch (Exception)
                {

                    return null;
                }
            }
        }
        public bool? DiseaseStatus { get; set; }
        public int? AgeOfDiagnosis { get; set; }
        /// <summary>
        /// Following are the relation of the table
        /// </summary>
        [Obsolete]
        public virtual Patients Patient { get; set; }
        public virtual PatientMedicalFamilyHistory PatientMedicalFamilyHistory { get; set; }
        public virtual MasterICD MasterICD { get; set; }
    }
}
