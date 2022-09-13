using HC.Model;
using HC.Patient.Model.eHealthScore;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Service.IServices.eHealthScore
{
    public interface IeHealthScoreService:IBaseService
    {
        MemoryStream PrinteHealthScoreReport(int patientId, int patientHealtheScoreId, TokenModel token);
        JsonModel GetMemberHealtheScoreListing(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel);
        JsonModel AssignHealtheScoreToMember(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel);
        JsonModel GetAssignedHealtheScore(PatientFilterModel patientFilterModel, TokenModel tokenModel);
        JsonModel BulkUpdateHealtheScore(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel);
        JsonModel GetHealtheScoreDataForBulkUpdate(string patientHealtheScoreIdArray, TokenModel tokenModel);
        JsonModel UpdateHealtheScoreForMemberTab(PatientHealtheScoreUpdateModel patientHealtheScoreUpdateModel, TokenModel token);
        MemoryStream ExportHealtheScoreToExcel(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel);
    }
}
