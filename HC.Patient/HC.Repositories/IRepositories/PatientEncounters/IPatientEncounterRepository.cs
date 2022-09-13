using HC.Model;
using HC.Patient.Data.ViewModel;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Model.PatientEncounters;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.PatientEncounters
{
    public interface IPatientEncounterRepository :IRepositoryBase<PatientEncounter>
    {
        PatientEncounterModel GetPatientEncounterDetails(int encounterId, TokenModel token);
        IQueryable<T> GetAllEncounters<T>(int? patientID, string appointmentType = "", string staffName = "", string status = "", string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "",TokenModel token =null) where T : class, new();

        IQueryable<T> GetAllPatientEncounters<T>(EncounterFilterModel filterModel, TokenModel token ) where T : class, new();
        IQueryable<T> GetServiceCodeForEncounterByAppointmentType<T>(int encounterId) where T : class, new();
        PatientEncounterModel DownloadEncounter(int encounterId,TokenModel token);
        PatientEncounterSummaryPDFModel PrintEncounterSummaryDetails(int encounterId, string checkListIds, TokenModel tokenModel);
        IQueryable<T> GetAllPatientEncounterUsers<T>(EncounterFilterModel filterModel, TokenModel token) where T : class, new();

        DataTable ExportEncounters(EncounterFilterModel filterModel, TokenModel token);
        IQueryable<T> PrintEncountersPDF<T>(EncounterFilterModel filterModel, TokenModel token) where T : class, new();
    }
}
