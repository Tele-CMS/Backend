using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class AlertsDataModel
    {
        public string LoadDate { get; set; }
        public string AlertType { get; set; }
        public string Details { get; set; }
        public string PatientName { get; set; }
        public DateTime DOB { get; set; }
        public string MedicalId { get; set; }
        public string EligibilityStatus { get; set; }
        public string Risk { get; set; }
        public int? Age { get; set; }
        public string CareTeamNames { get; set; }
        public string ProgramNames { get; set; }
        public string Gender { get; set; }
        public string Relationship { get; set; }
        public string PrimaryCondition { get; set; }
        public string ComorbidConditions { get; set; }
        public string NextAppointmentDate { get; set; }
    }
}
