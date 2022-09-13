using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientCurrentMedication: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Patients")]
        public int PatientId { get; set; }
        [ForeignKey("MasterMedication")]
        public int MedicationId { get; set; }
        public string Medication { get; set; }
        public string Dose { get; set; }
        public string DosageForm { get; set; }
        public string Unit { get; set; }
        [ForeignKey("MasterFrequency")]
        public int? FrequencyId { get; set; }
        public string Frequency { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? DaySupply { get; set; }
        public decimal? Refills { get; set; }
        [ForeignKey("MasterChronicCondition")]
        public int? ConditionId { get; set; }
        [ForeignKey("PatientPhysician")]
        public int? ProviderId { get; set; }
        public DateTime? PrescribedDate { get; set; }
        [ForeignKey("MasterSource")]
        public int? SourceId { get; set; }
        public string Notes { get; set; }
        public bool? IsNewMedCareGap { get; set; }
        public bool? IsRefillCareGap { get; set; }
        public bool? IsMedicationAdherenceRefill { get; set; }
        public bool? IsMedicationAdherecneNewMed { get; set; }
        public virtual MasterChronicCondition MasterChronicCondition { get; set; }
        public virtual PatientPhysician PatientPhysician { get; set; }
        public virtual Patients Patients { get; set; }
        public virtual MasterMedication MasterMedication { get; set; }
        public virtual GlobalCode MasterFrequency { get; set; }
        public virtual GlobalCode MasterSource { get; set; }

    }
}