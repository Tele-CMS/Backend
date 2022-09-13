using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientSocialHistoryModel
    {
        public int Id { get; set; }
        public int AlcohalID { get; set; }
        public int DrugID { get; set; }
        public string Occupation { get; set; }
        public int PatientID { get; set; }
        public int TobaccoID { get; set; }
        public int TravelID { get; set; }
        public bool IsDeleted { get; set; }
        public int LinkedEncounterId { get; set; }
    }
}
