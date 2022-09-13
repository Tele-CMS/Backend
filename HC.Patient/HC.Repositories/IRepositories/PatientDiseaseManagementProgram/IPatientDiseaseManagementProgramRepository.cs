using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Repositories.IRepositories.PatientDiseaseManagementProgram
{
    public interface IPatientDiseaseManagementProgramRepository :IRepositoryBase<Entity.PatientDiseaseManagementProgram>
    {
        IQueryable<T> GetPatientDiseaseManagementProgramList<T>(int patientId, FilterModel filterModel, TokenModel token) where T : class, new();
        JsonModel CreatePatientDiseaseManagementPlan(Entity.PatientDiseaseManagementProgram patientDiseaseManagementProgram,TokenModel token);
        bool UpdatePrimaryCareManager(int patientId, int? caremanagerId);
       // IQueryable<T> GetAllPatientDiseaseManagementProgramsList<T>(ProgramsFilterModel filterModel, TokenModel token) where T : class, new();
        Dictionary<string, object> GetAllPatientDiseaseManagementProgramsList(ProgramsFilterModel filterModel, TokenModel token);
        IQueryable<T> GetProgramsEnrollPatientsForBulkMessage<T>(ProgramsFilterModel filterModel, TokenModel token) where T : class, new();

       // DataTable ExportPatientDiseaseManagementData(ProgramsFilterModel filterModel, TokenModel token);
        IQueryable<T> GetProgramsEnrollPatientsForPDF<T>(ProgramsFilterModel filterModel, TokenModel token) where T : class, new();
        IQueryable<T> GetReportsHRAPrograms<T>(HRALogFilterModel filterModel, TokenModel token) where T : class, new();

    }
}
