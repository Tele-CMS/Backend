using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PrintPatientCurrentMedicationModel
    {
        public int Id { get; set; }
        public int? DaySupply { get; set; }
        public string Dose { get; set; }
        public string Frequency { get; set; }
        public DateTime? PrescribedDate { get; set; }
        public int? Quantity { get; set; }
        public int? Refills { get; set; }
        public string Condition { get; set; }
        public string Medication { get; set; }
        public string ProviderName { get; set; }
        public string Source { get; set; }
        public string DosageForm { get; set; }
    }

    public class PatientCurrentMedicationModel
    {
        public PatientDetailsModel PatientDetailsModel { get; set; }
        public List<PrintPatientCurrentMedicationModel> PrintPatientCurrentMedicationModel { get; set; }
    }
}
