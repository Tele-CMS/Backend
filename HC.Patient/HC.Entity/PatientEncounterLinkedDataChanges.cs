using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientEncounterLinkedDataChanges:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("PatientEncounter")]
        public int EncounterId { get; set; }
        [ForeignKey("PatientAllergies")]
        public int? PatientAllergyId { get; set; }
        [ForeignKey("PatientImmunization")]
        public int? PatientImmunizationId { get; set; }
        [ForeignKey("PatientSocialHistory")]
        public int? PatientSocialHistoryId { get; set; }
        [ForeignKey("TrackPatientActivity")]
        public int? TrackPatientActivityId { get; set; }
        public string ChangeDescription { get; set; }
        [ForeignKey("MasterPatientEncLinkedColumn")]
        public int? MasterPatientEncLinkedColumnId { get; set; }
        [ForeignKey("PatientMedicalFamilyHistory")]
        public int? PatientMedicalFamilyHistoryId { get; set; }
        [ForeignKey("PatientPhysician")]
        public int? PatientPhysicianId { get; set; }
        [ForeignKey("PatientLab")]
        public int? PatientLabId { get; set; }
        [ForeignKey("PatientVitals")]
        public int? PatientVitalId { get; set; }
        [ForeignKey("PatientMedication")]
        public int? PatientMedicationId { get; set; }
        [ForeignKey("Barriers")]
        public int? PatientBarrierId { get; set; }
        [ForeignKey("DFA_PatientDocuments")]
        public int? PatientAssessmentId { get; set; }
        [ForeignKey("PatientCareMetric")]
        public int? PatientCareMetricId { get; set; }

        [ForeignKey("MasterCareMetricsQuestion")]
        public int? MasterCareMetricsQuestionId { get; set; }
        [ForeignKey("PatientReferrals")]
        public int? PatientReferralId { get; set; }
        [ForeignKey("Tasks")]
        public int? TaskId { get; set; }
        //[ForeignKey("PatientDiagnosis")]
        public int? PatientDiagnosisId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Action { get; set; }
        //public virtual Tasks Tasks { get; set; }
        //public virtual PatientReferrals PatientReferrals { get; set; }
        public virtual PatientEncounter PatientEncounter { get; set; }
        public virtual PatientAllergies PatientAllergies { get; set; }
        public virtual PatientImmunization PatientImmunization { get; set; }
        public virtual PatientSocialHistory PatientSocialHistory { get; set; }
        //public virtual TrackPatientActivity TrackPatientActivity { get; set; }
        public virtual PatientMedicalFamilyHistory PatientMedicalFamilyHistory { get; set; }
        public virtual PatientMedicalFamilyHistoryDiseases PatientMedicalFamilyHistoryDiseases { get; set; }
        //public virtual PatientPhysician PatientPhysician { get; set; }
        //public virtual PatientLab PatientLab { get; set; }
        public virtual PatientVitals PatientVitals { get; set; }
        public virtual PatientMedication PatientMedication { get; set; }
        //public virtual Barriers Barriers { get; set; }
        public virtual DFA_PatientDocuments DFA_PatientDocuments { get; set; }
       // public virtual PatientDiagnosis PatientDiagnosis { get; set; }
        //public virtual PatientCareMetric PatientCareMetric { get; set; }
        //public virtual MasterCareMetricsQuestion MasterCareMetricsQuestion { get; set; }

        //public virtual MasterPatientEncLinkedColumn MasterPatientEncLinkedColumn { get; set; }
    }
}
