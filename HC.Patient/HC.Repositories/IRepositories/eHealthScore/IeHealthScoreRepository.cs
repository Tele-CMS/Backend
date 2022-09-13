using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.eHealthScore;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Repositories.IRepositories.eHealthScore
{
    public interface IeHealthScoreRepository : IRepositoryBase<PatientHealtheScore>
    {
        eHealthScoreModel PrinteHealthScoreReport<T>(int patientId, int patientHealtheScoreId, TokenModel token) where T : class, new();
        IQueryable<T> GetMemberHealtheScoreListing<T>(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel) where T : class, new();
        IQueryable<T> AssignHealtheScoreToMember<T>(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetAssignedHealtheScore<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> BulkUpdateHealtheScore<T>(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetHealtheScoreDataForBulkUpdate<T>(string patientHealtheScoreIdArray, TokenModel tokenModel) where T : class, new();
        DataTable ExportHealtheScoreToExcel(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel);

    }
}
