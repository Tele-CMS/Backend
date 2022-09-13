using HC.Model;
using HC.Service.Interfaces;
using System;

namespace HC.Patient.Service.IServices.AdminDashboard
{
    public interface IAdminDashboardService : IBaseService
    {
        JsonModel GetTotalClientCount(TokenModel token);
        JsonModel GetTotalRevenue(TokenModel token);
        JsonModel GetOrganizationAuthorization(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token);
        JsonModel GetOrganizationEncounter(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token);
        JsonModel GetStaffEncounter(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token);
        JsonModel GetRegiesteredClientCount(TokenModel token);
        JsonModel GetAdminDashboardData(TokenModel token);
        /// <summary>
        /// To get appointment graph for admin/provider dashboard
        /// </summary>
        /// <param name="appointmentDataFilterModel"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        JsonModel GetAppointmentsDataForGraph(AppointmentDataFilterModel appointmentDataFilterModel, TokenModel token);
        JsonModel GetOrganizationAppointments(FilterModel filterModel, string locationIds, string staffIds, string patientIds, DateTime fromDate, DateTime toDate, string statusName, TokenModel token);

        #region Angular Code
        JsonModel GetClientStatusChart(TokenModel token);
        JsonModel GetDashboardBasicInfo(TokenModel token);
        #endregion

    }
}
