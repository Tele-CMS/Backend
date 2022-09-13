using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.PatientCommon.Interfaces
{
    public interface IPatientCommonService : IBaseService
    {
        Patients PatientExist(Patients patientInfo);
        Patients UpdatePatientData(int id,Patients patientInfo);
        PatientAddress UpdatePatientAddressData(int id, PatientAddress patientInfo);
        PatientInsuranceDetails UpdatePatientInsuranceData(int id, PatientInsuranceDetails patientInfo);
        PhoneNumbers UpdatePhoneNumbersData(int id, PhoneNumbers patientInfo);        
        PatientVitals UpdatePatientVitalseData(int id, PatientVitals patientInfo);        
        bool UpdatePatientMedicalFamilyHistoryData(int id, PatientMedicalFamilyHistory patientInfo);
        PatientImmunization UpdatePatientImmunizationData(int id, PatientImmunization patientInfo);
        PatientSocialHistory UpdatePatientSocialHistoryData(int id, PatientSocialHistory patientInfo);
        List<Authorization> GetAuthorizationByPatientID(int Id);
        List<Patients> GetPatientByTag(List<string> tag);
        List<PatientModel> GetFilteredPatients(string searchKey, string operation, string startWith, string tags, string locationIDs,string isActive,int pageSize, TokenModel tokenModel);
    }
}

