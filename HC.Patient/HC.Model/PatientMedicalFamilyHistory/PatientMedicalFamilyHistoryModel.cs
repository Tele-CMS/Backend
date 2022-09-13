using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientMedicalFamilyHistory
{
    public class PatientMedicalFamilyHistoryModel
    {   
        public int Id { get; set; }        
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string FirstName { get; set; }        
        public string LastName { get; set; }        
        public int GenderID { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }        
        public int RelationshipID { get; set; }
        public string RelationShipName{ get; set; }
        public DateTime? DateOfDeath { get; set; }        
        public string CauseOfDeath { get; set; }        
        public string Observation { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Others { get; set; }
        public string OtherRelationshipName { get; set; }
        public int LinkedEncounterId { get; set; }
        public List<PatientMedicalFamilyHistoryDiseasesModel> PatientMedicalFamilyHistoryDiseases { get; set; }
    }

    public class PatientMedicalFamilyHistoryDiseasesModel
    {   
        public  int Id { get; set; }     
        public int PatientID { get; set; }        
        public int MedicalFamilyHistoryId { get; set; }
        public int DiseaseID { get; set; }
        public bool? DiseaseStatus { get; set; }        
        public int? AgeOfDiagnosis { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
