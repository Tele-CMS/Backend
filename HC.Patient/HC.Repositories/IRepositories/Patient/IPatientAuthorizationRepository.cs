using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientAuthorizationRepository : IRepositoryBase<Entity.Authorization>
    {
        Dictionary<string, object> GetAllAuthorizationsForPatient(int patientId, int pageNumber,int pageSize,string authType);
    }
}
