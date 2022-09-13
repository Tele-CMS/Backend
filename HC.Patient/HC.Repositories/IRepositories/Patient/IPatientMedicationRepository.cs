using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientMedicationRepository : IRepositoryBase<PatientMedication>
    {
        IQueryable<T> GetMedication<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetMedicationForPrint<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
