using HC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.AdminDashboard
{
    public interface IAdminDashboardRepository
    {
        IQueryable<T> GetTotalRevenue<T>(TokenModel token) where T : class, new();
        IQueryable<T> GetOrganizationAuthorization<T>(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token) where T : class, new();
        IQueryable<T> GetOrganizationEncounter<T>(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token) where T : class, new();
        IQueryable<T> GetStaffEncounter<T>(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token) where T : class, new();
        IQueryable<T> GetRegiesteredClientCount<T>(TokenModel token) where T : class, new();
        IQueryable<T> GetClientStatusChart<T>(DateTime fromDate, DateTime toDate, TokenModel token) where T : class, new();
        /// <summary>
        /// To get appointment graph on admin/provider dashboard
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dmpEnrolleesDataFilterModel"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        IQueryable<T> GetAppointmentsDataForGraph<T>(AppointmentDataFilterModel dmpEnrolleesDataFilterModel, TokenModel token) where T : class, new();
    }
}
