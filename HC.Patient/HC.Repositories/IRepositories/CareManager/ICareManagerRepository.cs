using HC.Model;
using HC.Patient.Model.PatientAppointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.CareManager
{
    public interface ICareManagerRepository
    {
        IQueryable<T> GetCareManagerTeamList<T>(int patientId, CommonFilterModel filterModel, TokenModel token) where T : class, new();

        IQueryable<T> GetCareManagerList<T>(TokenModel token) where T : class, new();

        bool AssignAndRemoveCareManagerToAllPatient(int careManagerID, bool isAttached, TokenModel token);
    }                
}
