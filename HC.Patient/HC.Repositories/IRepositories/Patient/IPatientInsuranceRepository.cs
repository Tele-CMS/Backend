using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientInsuranceRepository : IRepositoryBase<PatientInsuranceDetails>
    {
        JsonModel SavePatientInsurance(PatientInsuranceDetails patientInsuranceDetails, bool Updated);
        PatientInsuranceDetails GetInsuranceDetail(int id);
        Dictionary<string, object> GetPatientInsurances(int patientId, TokenModel tokenModel);
        Dictionary<string, object> GetPatientInsurancesWithPagination(PatientFilterModel filterModel, TokenModel tokenModel);
    }
}
