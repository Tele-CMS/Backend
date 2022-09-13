
using HC.Model;
using HC.Patient.Model.Patient;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientAlertRepository : IRepositoryBase<Entity.PatientAlerts>
    {
        IQueryable<T> GetPatientAlerts<T>(PatientAlertFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetAllPatientAlertsUsers<T>(PatientAlertFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
