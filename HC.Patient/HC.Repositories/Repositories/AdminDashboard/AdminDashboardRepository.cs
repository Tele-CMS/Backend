using HC.Model;
using HC.Patient.Data;
using HC.Patient.Repositories.IRepositories.AdminDashboard;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.AdminDashboard
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private HCOrganizationContext _context;
        public AdminDashboardRepository(HCOrganizationContext context)
        {
            _context = context;
        }
        public IQueryable<T> GetTotalRevenue<T>(TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId", token.OrganizationID) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.ADM_GetTotalRevenue.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetOrganizationAuthorization<T>(int pageNumber, int pageSize, string sortColumn, string sortOrder,TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId", token.OrganizationID),
                                        new SqlParameter("@PageNumber", pageNumber),
                                        new SqlParameter("@PageSize", pageSize),
                                        new SqlParameter("@SortColumn", sortColumn),
                                        new SqlParameter("@SortOrder", sortOrder)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.ADM_GetOrganizationAuthorization.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetOrganizationEncounter<T>(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId", token.OrganizationID),
                                        new SqlParameter("@PageNumber", pageNumber),
                                        new SqlParameter("@PageSize", pageSize),
                                        new SqlParameter("@SortColumn", sortColumn),
                                        new SqlParameter("@SortOrder", sortOrder),                                        
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.ADM_GetOrganizationEncounter.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetStaffEncounter<T>(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId", token.OrganizationID),
                                        new SqlParameter("@StaffId", token.StaffID),
                                        new SqlParameter("@PageNumber", pageNumber),
                                        new SqlParameter("@PageSize", pageSize),
                                        new SqlParameter("@SortColumn", sortColumn),
                                        new SqlParameter("@SortOrder", sortOrder),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.ADM_GetStaffEncounters.ToString(), parameters.Length, parameters).AsQueryable();
        }
        
        public IQueryable<T> GetRegiesteredClientCount<T>(TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId", token.OrganizationID) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.ADM_GetCurrentYearRegisteredClientCount.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetClientStatusChart<T>(DateTime fromDate, DateTime toDate, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId", token.OrganizationID),
                                          new SqlParameter("@LocationId", token.LocationID),
                                          new SqlParameter("@FromDate", fromDate),
                                          new SqlParameter("@ToDate", toDate)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.ADM_GetClientsStatusChart, parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetAppointmentsDataForGraph<T>(AppointmentDataFilterModel dmpEnrolleesDataFilterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                new SqlParameter("@StatusIds", dmpEnrolleesDataFilterModel.StatusIds),
                                new SqlParameter("@CareManagerIds", string.IsNullOrEmpty(dmpEnrolleesDataFilterModel.CareManagerIds) ? string.Empty  : dmpEnrolleesDataFilterModel.CareManagerIds),
                                new SqlParameter("@AppointmentTimeIntervalId", dmpEnrolleesDataFilterModel.AppointmentTimeIntervalId),
                             //   new SqlParameter("@IsCheckTotalEnrollments", dmpEnrolleesDataFilterModel.IsCheckTotalEnrollments),
                                new SqlParameter("@OrganizationId", token.OrganizationID),
                                new SqlParameter("@UserId", token.UserID),
                                new SqlParameter("@PatientId", dmpEnrolleesDataFilterModel.PatientId)
                };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.APT_GetDataForAppointmentsGraph.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
