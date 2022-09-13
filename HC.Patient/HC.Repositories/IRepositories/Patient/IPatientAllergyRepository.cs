using CreateClinicalReport.Model;
using HC.Model;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientAllergyRepository : IRepositoryBase<Entity.PatientAllergies>
    {
        IQueryable<T> GetPatientAllergies<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
