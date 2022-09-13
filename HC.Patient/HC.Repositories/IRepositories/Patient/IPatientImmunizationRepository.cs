using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientImmunizationRepository : IRepositoryBase<PatientImmunization>
    {
        IQueryable<T> GetImmunization<T>(PatientFilterModel filterModel, TokenModel tokenModel) where T : class, new();
    }
}
