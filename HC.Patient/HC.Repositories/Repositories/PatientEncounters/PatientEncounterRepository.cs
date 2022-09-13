using HC.Patient.Data;
using HC.Patient.Data.ViewModel;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.PatientEncounters;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using HC.Patient.Model.PatientEncounters;
using HC.Model;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Model.Patient;
using System.Data;

namespace HC.Patient.Repositories.Repositories.PatientEncounters
{
    public class PatientEncounterRepository:RepositoryBase<PatientEncounter>, IPatientEncounterRepository
    {
        private HCOrganizationContext _context;
        public PatientEncounterRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }

        public PatientEncounterModel GetPatientEncounterDetails(int encounterId, TokenModel token)
        {
            SqlParameter[] parameters = { new SqlParameter ("@OrganizationId", token.OrganizationID ),
                                          new SqlParameter ("@UserId", token.UserID ),
                                          new SqlParameter ("@EncounterId", encounterId )};
            return _context.ExecStoredProcedureForPatientEncounterDetail("ENC_GetPatientEncounterDetails",parameters.Length, false, parameters);
        }

        public IQueryable<T> GetServiceCodeForEncounterByAppointmentType<T>(int appointmentId) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@AppointmentId", appointmentId) };
            return _context.ExecStoredProcedureListWithOutput<T>("ENC_GetServiceCodeForEncounterByAppointmentType", parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetAllEncounters<T>(int? patientID, string appointmentType ="", string staffName = "", string status = "", string fromDate = "", string toDate = "", int pageNumber=1, int pageSize=10, string sortColumn="", string sortOrder="",TokenModel token = null) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter ("@OrganizationId", token.OrganizationID ),
                                          new SqlParameter ("@UserId", token.UserID ),
                                          new SqlParameter("@PatientId", (int?)patientID),                                         
                                          new SqlParameter("@StaffName", staffName),
                                          new SqlParameter("@Status", status),
                                          new SqlParameter("@FromDate", fromDate),
                                          new SqlParameter("@ToDate", toDate),
                                          new SqlParameter("@PageNumber", pageNumber),
                                          new SqlParameter("@PageSize", pageSize),
                                          new SqlParameter("@SortColumn", sortColumn),
                                          new SqlParameter("@SortOrder", sortOrder)};
            return _context.ExecStoredProcedureListWithOutput<T>("ENC_GetAllEncounters", parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetAllPatientEncounters<T>(EncounterFilterModel filtermodel, TokenModel token = null) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter ("@OrganizationId", token.OrganizationID ),
                                          new SqlParameter ("@UserId", token.UserID ),
                                          //new SqlParameter("@PatientId", (int?)patientID),
                                          new SqlParameter("@CareManagerIds", filtermodel.CareManagerIds),
                                          new SqlParameter("@EnrollmentId", filtermodel.EnrollmentId),
                                          new SqlParameter("@EncounterTypeIds", filtermodel.EncounterTypeIds),
                                          new SqlParameter("@Status", filtermodel.Status),
                                          new SqlParameter("@FromDate", filtermodel.FromDate),
                                          new SqlParameter("@ToDate", filtermodel.ToDate),
                                          new SqlParameter("@PageNumber", filtermodel.pageNumber),
                                          new SqlParameter("@PageSize", filtermodel.pageSize),
                                          new SqlParameter("@SortColumn", filtermodel.sortColumn),
                                          new SqlParameter("@SortOrder", filtermodel.sortOrder),
                                          new SqlParameter("@NextAppointmentPresent", filtermodel.NextAppointmentPresent)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.ADM_GetAllPAtientEncounters.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public PatientEncounterModel DownloadEncounter(int encounterId, TokenModel token)
        {
            SqlParameter[] parameters = { new SqlParameter("@EncounterId", encounterId) };
            return _context.ExecStoredProcedureForPatientEncounterDetail(SQLObjects.RPT_DownloadEncounter.ToString() , parameters.Length,true, parameters);
        }
        public PatientEncounterSummaryPDFModel PrintEncounterSummaryDetails(int encounterId, string checkListIds, TokenModel tokenModel)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@EncounterId", encounterId),
                new SqlParameter("@OrganizationID", tokenModel.OrganizationID),
                new SqlParameter("@UserId", tokenModel.UserID),
            };
            return _context.ExecStoredProcedureForPrintEncounterSummary(SQLObjects.PAT_GetPrintEncounterSummaryDetails.ToString(), sqlParameters.Length, sqlParameters);
        }
        public IQueryable<T> GetAllPatientEncounterUsers<T>(EncounterFilterModel filtermodel, TokenModel token = null) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter ("@OrganizationId", token.OrganizationID ),
                                          new SqlParameter ("@UserId", token.UserID ),
                                          new SqlParameter("@CareManagerIds", filtermodel.CareManagerIds),
                                          new SqlParameter("@EnrollmentId", filtermodel.EnrollmentId),
                                          new SqlParameter("@EncounterTypeIds", filtermodel.EncounterTypeIds),
                                          new SqlParameter("@Status", filtermodel.Status),
                                          new SqlParameter("@FromDate", filtermodel.FromDate),
                                          new SqlParameter("@ToDate", filtermodel.ToDate),
                                          new SqlParameter("@PageNumber", filtermodel.pageNumber),
                                          new SqlParameter("@PageSize", filtermodel.pageSize),
                                          new SqlParameter("@SortColumn", filtermodel.sortColumn),
                                          new SqlParameter("@SortOrder", filtermodel.sortOrder),
                                          new SqlParameter("@NextAppointmentPresent", filtermodel.NextAppointmentPresent)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.ADM_GetAllPAtientEncounterUsers.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public DataTable ExportEncounters(EncounterFilterModel filtermodel, TokenModel token)
        {
            SqlParameter[] parameters = {new SqlParameter ("@OrganizationId", token.OrganizationID ),
                                          new SqlParameter ("@UserId", token.UserID ),
                                          //new SqlParameter("@PatientId", (int?)patientID),
                                          new SqlParameter("@CareManagerIds", filtermodel.CareManagerIds),
                                          new SqlParameter("@EnrollmentId", filtermodel.EnrollmentId),
                                          new SqlParameter("@EncounterTypeIds", filtermodel.EncounterTypeIds),
                                          new SqlParameter("@Status", filtermodel.Status),
                                          new SqlParameter("@FromDate", filtermodel.FromDate),
                                          new SqlParameter("@ToDate", filtermodel.ToDate),
                                          new SqlParameter("@PageNumber", filtermodel.pageNumber),
                                          new SqlParameter("@PageSize", filtermodel.pageSize),
                                          new SqlParameter("@SortColumn", filtermodel.sortColumn),
                                          new SqlParameter("@SortOrder", filtermodel.sortOrder),
                                          new SqlParameter("@NextAppointmentPresent", filtermodel.NextAppointmentPresent)
            };

            return _context.ExecStoredProcedureForDatatableExcel(SQLObjects.ADM_GetEncounterExportExcel.ToString(), parameters.Length, parameters);

        }

        public IQueryable<T> PrintEncountersPDF<T>(EncounterFilterModel filterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter ("@OrganizationId", token.OrganizationID ),
                                          new SqlParameter ("@UserId", token.UserID ),
                                          //new SqlParameter("@PatientId", (int?)patientID),
                                          new SqlParameter("@CareManagerIds", filterModel.CareManagerIds),
                                          new SqlParameter("@EnrollmentId", filterModel.EnrollmentId),
                                          new SqlParameter("@EncounterTypeIds", filterModel.EncounterTypeIds),
                                          new SqlParameter("@Status", filterModel.Status),
                                          new SqlParameter("@FromDate", filterModel.FromDate),
                                          new SqlParameter("@ToDate", filterModel.ToDate),
                                          new SqlParameter("@PageNumber", filterModel.pageNumber),
                                          new SqlParameter("@PageSize", filterModel.pageSize),
                                          new SqlParameter("@SortColumn", filterModel.sortColumn),
                                          new SqlParameter("@SortOrder", filterModel.sortOrder),
                                          new SqlParameter("@NextAppointmentPresent", filterModel.NextAppointmentPresent)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.ADM_PrintEncounterPDF.ToString(), parameters.Length, parameters).AsQueryable();

        }
    }
}