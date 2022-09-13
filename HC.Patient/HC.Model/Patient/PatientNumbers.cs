using HC.Patient.Entity;
using System.Collections.Generic;

namespace HC.Patient.Model.Patient
{
    public class PatientNumbers
    {
        public int PatientID { get; set; }
        public List<PhoneNumbers> PhoneNumbers { get; set; }
    }

    public class PatientLabels
    {
        public int PatientID { get; set; }
        public List<PatientCustomLabels> PatientCustomLabels { get; set; }



    }

    public class StaffLabels
    {
        public int StaffID { get; set; }
        public List<StaffCustomLabels> StaffCustomLabels { get; set; }



    }
    public class AuthorizationProcedureList
    {
        public int AuthorizationID { get; set; }
        public List<AuthorizationProcedures> AuthorizationProcedures { get; set; }
    }

    public class PatientTagModel
    {
        public int PatientID { get; set; }
        public List<PatientTags> PatientTags { get; set; }
    }

    public class StaffTagModel
    {
        public int StaffID { get; set; }
        public List<StaffTags> StaffTags { get; set; }
    }
}
