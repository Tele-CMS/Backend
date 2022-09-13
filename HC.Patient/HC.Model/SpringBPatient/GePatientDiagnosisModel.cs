using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.SpringBPatient
{
    public class GePatientDiagnosisModel
    {
        public int Id { get; set; }
        public string PrimaryInsuredMemberId { get; set; }
        public DateTime ServiceBeginDate { get; set; }
        public string DiagnosisName { get; set; }
        public string DiagnosisCode { get; set; }
        public string Source { get; set; }
        public int TotalRecords { get; set; }

        public bool Flag { get; set; }

        public string AlertType { get; set; }

    }
}
