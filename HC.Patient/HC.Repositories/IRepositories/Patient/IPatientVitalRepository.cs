using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientVitalRepository : IRepositoryBase<PatientVitals>
    {
        IQueryable<T> GetVitals<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
