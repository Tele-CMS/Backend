using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Repositories.Interfaces
{
    public interface IPatientCommonRepository//: IRepository<MasterDataModel>
    {
        Patients PatientExist(Patients patientInfo);
        Patients UpdatePatientData(int id, Patients patientInfo);
        PatientAddress UpdatePatientAddressData(int id, PatientAddress patientInfo);
        PatientInsuranceDetails UpdatePatientInsuranceData(int id, PatientInsuranceDetails patientInfo);
        PhoneNumbers UpdatePhoneNumbersData(int id, PhoneNumbers patientInfo);        
        PatientVitals UpdatePatientVitalseData(int id, PatientVitals patientInfo);        
        bool UpdatePatientDiagnosisData(int id, PatientDiagnosis patientInfo);
        bool UpdatePatientEncounterData(int id, PatientEncounter patientInfo);
        bool UpdatePatientMedicalFamilyHistoryData(int id, PatientMedicalFamilyHistory patientInfo);
        PatientImmunization UpdatePatientImmunizationData(int id, PatientImmunization patientInfo);
        PatientSocialHistory UpdatePatientSocialHistoryData(int id, PatientSocialHistory patientInfo);
        List<Authorization> GetAuthorizationByPatientID(int Id);
        List<Patients> GetPatientByTag(List<string> tag);
        IQueryable<T> GetFilteredPatients<T>(string searchKey, string operation, string startWith, string tags, string locationIDs,string isActive, int pageSize, TokenModel tokenModel) where T : class, new();        
    }
}
