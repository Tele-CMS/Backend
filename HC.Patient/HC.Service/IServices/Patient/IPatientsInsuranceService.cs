using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientsInsuranceService :IBaseService
    {
        JsonModel SavePatientInsurance(List<PatientInsuranceModel> patientInsuranceListModel, TokenModel tokenModel);
        JsonModel GetPatientInsurances(PatientFilterModel filterModel, TokenModel tokenModel);
    }
}
