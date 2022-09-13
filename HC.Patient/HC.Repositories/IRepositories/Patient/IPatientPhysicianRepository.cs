using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientPhysicianRepository : IRepositoryBase<PatientPhysician>
    {
        IQueryable<T> GetPatientPhysicianById<T>(int patientId, FilterModel filterModel, TokenModel token) where T : class, new();

    }
}
