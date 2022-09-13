using HC.Patient.Entity;
using System.Collections.Generic;

namespace HC.Patient.Model.Patient
{
    public class EncounterModel
    {   
    }
    public class EncounterCPTModel
    {
        public int EncounterID { get; set; }
        public List<PatientEncounterServiceCodes> PatientEncounterCPT { get; set; }
    }
    public class EncounterICDModel
    {
        public int EncounterID { get; set; }
        public List<PatientEncounterDiagnosisCodes> PatientEncounterICD { get; set; }
    }
    //public class PatientEncounterModel //For multiple post in single object
    //{
    //    public int EncounterID { get; set; }
    //    public PatientEncounter PatientEncounter { get; set; }
    //    public List<PatientEncounterICDCodes> PatientEncounterICD { get; set; }
    //    public List<PatientEncounterServiceCodes> PatientEncounterCPT { get; set; }
    //}
    //public class AvailabilityModel
    //{
    //    public int StaffID { get; set; }
    //    public List<StaffAvailability> StaffAvailability { get; set; }
    //}

}
