using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Payer;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Payer
{
    public interface  ISymptomatePatientReportRepository : IRepositoryBase<SymptomatePatientReport>
    {
        IQueryable<T> GetSymptomateReportListing<T>(PatientSymptomateFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
