using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.PatientEncounters
{
    public interface IPatientEncounterTemplateRepository: IRepositoryBase<PatientEncounterTemplates>
    {
        IQueryable<T> GetPatientEncounterTemplateData<T>(int patientEncounterId, int masterTemplateId, TokenModel tokenModel) where T : class, new();
    }
}
