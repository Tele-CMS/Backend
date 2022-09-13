using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.PatientDiseaseManagementProgram;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Repositories.Repositories.PatientDiseaseManagementProgram
{
    public class PatientDiseaseManagementProgramRepository : RepositoryBase<Entity.PatientDiseaseManagementProgram>, IPatientDiseaseManagementProgramRepository
    {
        private HCOrganizationContext _context;
        private JsonModel response;
        public PatientDiseaseManagementProgramRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
        public IQueryable<T> GetPatientDiseaseManagementProgramList<T>(int patientId, FilterModel filterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientId),
                                           new SqlParameter("@PageNumber", filterModel.pageNumber),
                                          new SqlParameter("@PageSize", filterModel.pageSize),
                                          new SqlParameter("@SortOrder", filterModel.sortOrder),
                                          new SqlParameter("@SortColumn", filterModel.sortColumn),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.DMP_GetPatientDiseaseManagementProgramList.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public JsonModel CreatePatientDiseaseManagementPlan(Entity.PatientDiseaseManagementProgram patientDiseaseManagementProgram, TokenModel token)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (patientDiseaseManagementProgram.ID == 0)
                    {
                        _context.PatientDiseaseManagementProgram.Add(patientDiseaseManagementProgram);
                        _context.SaveChanges();
                        response = new JsonModel(null, StatusMessage.PatientDMPCreated, (int)HttpStatusCodes.OK);
                    }
                    else
                    {
                        var diseaseManagementProgramName = _context.DiseaseManagementProgram.Where(p => p.ID == patientDiseaseManagementProgram.DiseaseManagementPlanID).Select(s => s.Description).FirstOrDefault();

                        //var test = patientDiseaseManagementProgram.DiseaseManagementPlanPatientActivity.Join(_context.MasterDescriptionType, DMPPA => DMPPA.DiseaseManagmentPlanActivityID, MDT => MDT.ID, (DMPP, MDT) => new { DMPP, MDT });
                        //test.Select(x => new {Id=x.DMPP.ID,Description=x.MDT.Description });
                        //_context.PatientDiseaseManagementProgram.Update(patientDiseaseManagementProgram);
                        //SaveChangesWithAuditLogs(AuditLogsScreen.PatientDiseaseManagementProgramActivity, AuditLogAction.Modify, patientDiseaseManagementProgram.PatientID, token.UserID, String.Format("{0}/{1}", diseaseManagementProgramName, patientDiseaseManagementProgram.ID), token, null, null);
                        response = new JsonModel(null, StatusMessage.PatientDMPUpdated, (int)HttpStatusCodes.OK);
                    }
                    //_context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    response = new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, ex.Message);
                    transaction.Rollback();
                }
            }
            return response;
        }

        public bool UpdatePrimaryCareManager(int patientId, int? caremanagerId)
        {
            PatientAndCareTeamMapping careManagerDetail = _context.PatientAndCareTeamMapping.Where(x => x.CareTeamMemberID == caremanagerId && x.PatientID == patientId && x.IsDeleted == false).FirstOrDefault();
            Patients patients = _context.Patients.Where(x => x.Id == patientId && x.IsDeleted == false).FirstOrDefault();
            try
            {
                if (careManagerDetail == null)
                {
                    PatientAndCareTeamMapping data = new PatientAndCareTeamMapping()
                    {
                        CareTeamMemberID = caremanagerId.Value,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedDate = DateTime.UtcNow,
                        PatientID = patientId
                    };
                    _context.PatientAndCareTeamMapping.Add(data);
                }

                //if (patients.PrimaryCareManagerId == null)
                //{
                //    patients.PrimaryCareManagerId = caremanagerId;

                //}

                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }

        }

        //public IQueryable<T> GetAllPatientDiseaseManagementProgramsList<T>(ProgramsFilterModel filterModel, TokenModel token) where T : class, new()
        //{
        //    SqlParameter[] parameters = {
        //                                   new SqlParameter("@OrganizationId", token.OrganizationID),
        //                                   new SqlParameter("@UserId", token.UserID),
        //                                   new SqlParameter("@ProgramIds", filterModel.ProgramIds),
        //                                   new SqlParameter("@Status", filterModel.Status),
        //                                   new SqlParameter("@StartDate", filterModel.StartDate),
        //                                   new SqlParameter("@EndDate", filterModel.EndDate),
        //                                   new SqlParameter("@CareManagerIds", filterModel.CareManagerIds),
        //                                   new SqlParameter("@PageNumber", filterModel.pageNumber),
        //                                   new SqlParameter("@PageSize", filterModel.pageSize),
        //                                   new SqlParameter("@SortOrder", filterModel.sortOrder),
        //                                   new SqlParameter("@SortColumn", filterModel.sortColumn),
        //    };
        //    return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.DMP_GetAllPatientDiseaseManagementProgramsList.ToString(), parameters.Length, parameters).AsQueryable();
        //}
        public Dictionary<string, object> GetAllPatientDiseaseManagementProgramsList(ProgramsFilterModel filterModel, TokenModel token)
        {
            SqlParameter[] parameters = {
                                           new SqlParameter("@OrganizationId", token.OrganizationID),
                                           new SqlParameter("@UserId", token.UserID),
                                           new SqlParameter("@ProgramIds", filterModel.ProgramIds),
                                           new SqlParameter("@Status", filterModel.Status),
                                           new SqlParameter("@StartDate", filterModel.StartDate),
                                           new SqlParameter("@EndDate", filterModel.EndDate),
                                           new SqlParameter("@CareManagerIds", filterModel.CareManagerIds),
                                           new SqlParameter("@EnrollmentId", filterModel.EnrollmentId),
                                           new SqlParameter("@IsEligible", filterModel.IsEligible),
                                           new SqlParameter("@ConditionIds", filterModel.ConditionIds),
                                           new SqlParameter("@GenderIds", filterModel.GenderIds),
                                           new SqlParameter("@Relationships", filterModel.Relationships),
                                           new SqlParameter("@StartAgeRange", filterModel.StartAgeRange),
                                           new SqlParameter("@EndAgeRange", filterModel.EndAgeRange),
                                           new SqlParameter("@SearchText", filterModel.SearchText),
                                           new SqlParameter("@PageNumber", filterModel.pageNumber),
                                           new SqlParameter("@PageSize", filterModel.pageSize),
                                           new SqlParameter("@SortOrder", filterModel.sortOrder),
                                           new SqlParameter("@SortColumn", filterModel.sortColumn),
                                           new SqlParameter("@NextAppointmentPresent", filterModel.NextAppointmentPresent),
            };
            return _context.ExecStoredProcedureListWithOutputForPatienProgramEnrolleesList(SQLObjects.DMP_GetAllPatientDiseaseManagementProgramsList.ToString(), parameters.Length, parameters);
        }

        public IQueryable<T> GetProgramsEnrollPatientsForBulkMessage<T>(ProgramsFilterModel filterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                            new SqlParameter("@OrganizationId", token.OrganizationID),
                                           new SqlParameter("@UserId", token.UserID),
                                           new SqlParameter("@ProgramIds", filterModel.ProgramIds),
                                           new SqlParameter("@Status", filterModel.Status),
                                           new SqlParameter("@StartDate", filterModel.StartDate),
                                           new SqlParameter("@EndDate", filterModel.EndDate),
                                           new SqlParameter("@CareManagerIds", filterModel.CareManagerIds),
                                           new SqlParameter("@IsEligible", filterModel.IsEligible),
                                           new SqlParameter("@ConditionIds", filterModel.ConditionIds),
                                           new SqlParameter("@GenderIds", filterModel.GenderIds),
                                           new SqlParameter("@Relationships", filterModel.Relationships),
                                           new SqlParameter("@StartAgeRange", filterModel.StartAgeRange),
                                           new SqlParameter("@EndAgeRange", filterModel.EndAgeRange),
                                           new SqlParameter("@SearchText", filterModel.SearchText),
                                           new SqlParameter("@EnrollmentId", filterModel.EnrollmentId),
                                           new SqlParameter("@PageNumber", filterModel.pageNumber),
                                           new SqlParameter("@PageSize", filterModel.pageSize),
                                           new SqlParameter("@SortOrder", filterModel.sortOrder),
                                           new SqlParameter("@SortColumn", filterModel.sortColumn),
                                           new SqlParameter("@NextAppointmentPresent", filterModel.NextAppointmentPresent),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.DMP_GetProgramsEnrollPatientsForBulkMessage.ToString(), parameters.Length, parameters).AsQueryable();
        }

//        public DataTable ExportPatientDiseaseManagementData(ProgramsFilterModel filterModel, TokenModel token)
//        {
//            SqlParameter[] parameters = {
//                                           new SqlParameter("@OrganizationId", token.OrganizationID),
//                                           new SqlParameter("@UserId", token.UserID),
//                                           new SqlParameter("@ProgramIds", filterModel.ProgramIds),
//                                           new SqlParameter("@Status", filterModel.Status),
//                                           new SqlParameter("@StartDate", filterModel.StartDate),
//                                           new SqlParameter("@EndDate", filterModel.EndDate),
//                                           new SqlParameter("@CareManagerIds", filterModel.CareManagerIds),
//                                           new SqlParameter("@IsEligible", filterModel.IsEligible),
//                                           new SqlParameter("@ConditionIds", filterModel.ConditionIds),
//                                           new SqlParameter("@EnrollmentId", filterModel.EnrollmentId),
//                                           new SqlParameter("@GenderIds", filterModel.GenderIds),
//                                           new SqlParameter("@Relationships", filterModel.Relationships),
//                                           new SqlParameter("@StartAgeRange", filterModel.StartAgeRange),
//                                           new SqlParameter("@EndAgeRange", filterModel.EndAgeRange),
//                                           new SqlParameter("@SearchText", filterModel.SearchText),
//                                           new SqlParameter("@PageNumber", filterModel.pageNumber),
//                                           new SqlParameter("@PageSize", filterModel.pageSize),
//                                           new SqlParameter("@SortOrder", filterModel.sortOrder),
//                                           new SqlParameter("@SortColumn", filterModel.sortColumn),
//                                           new SqlParameter("@NextAppointmentPresent", filterModel.NextAppointmentPresent),
//};

//            return _context.ExecStoredProcedureForDatatableExcel(SQLObjects.DMP_ExportPatientDiseaseManagementProgramsData.ToString(), parameters.Length, parameters);

//        }

        public IQueryable<T> GetProgramsEnrollPatientsForPDF<T>(ProgramsFilterModel filterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                           new SqlParameter("@OrganizationId", token.OrganizationID),
                                           new SqlParameter("@UserId", token.UserID),
                                           new SqlParameter("@ProgramIds", filterModel.ProgramIds),
                                           new SqlParameter("@Status", filterModel.Status),
                                           new SqlParameter("@StartDate", filterModel.StartDate),
                                           new SqlParameter("@EndDate", filterModel.EndDate),
                                           new SqlParameter("@CareManagerIds", filterModel.CareManagerIds),
                                           new SqlParameter("@IsEligible", filterModel.IsEligible),
                                           new SqlParameter("@ConditionIds", filterModel.ConditionIds),
                                           new SqlParameter("@EnrollmentId", filterModel.EnrollmentId),
                                           new SqlParameter("@GenderIds", filterModel.GenderIds),
                                           new SqlParameter("@Relationships", filterModel.Relationships),
                                           new SqlParameter("@StartAgeRange", filterModel.StartAgeRange),
                                           new SqlParameter("@EndAgeRange", filterModel.EndAgeRange),
                                           new SqlParameter("@SearchText", filterModel.SearchText),
                                           new SqlParameter("@PageNumber", filterModel.pageNumber),
                                           new SqlParameter("@PageSize", filterModel.pageSize),
                                           new SqlParameter("@SortOrder", filterModel.sortOrder),
                                           new SqlParameter("@SortColumn", filterModel.sortColumn),
                                           new SqlParameter("@NextAppointmentPresent", filterModel.NextAppointmentPresent),
};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.DMP_PrintPatientDiseaseManagementProgramsData.ToString(), parameters.Length, parameters).AsQueryable();

        }


        public IQueryable<T> GetReportsHRAPrograms<T>(HRALogFilterModel filterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                            new SqlParameter("@OrganizationId", token.OrganizationID),
                                           new SqlParameter("@SearchText", filterModel.SearchText),
                                           new SqlParameter("@ProviderId", filterModel.ProviderId),
                                           new SqlParameter("@ReportTypeId", filterModel.ReportTypeId),
                                           new SqlParameter("@PageNumber", filterModel.pageNumber),
                                           new SqlParameter("@PageSize", filterModel.pageSize),
                                           new SqlParameter("@SortOrder", filterModel.sortOrder),
                                           new SqlParameter("@SortColumn", filterModel.sortColumn),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.Log_GetHraProgramsLogs.ToString(), parameters.Length, parameters).AsQueryable();
        }

    }
}

