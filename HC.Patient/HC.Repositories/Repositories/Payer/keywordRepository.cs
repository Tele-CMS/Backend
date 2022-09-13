using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Payer
{
    public class keywordRepository : RepositoryBase<HealthcareKeywords>, IkeywordRepository
    {
        private HCOrganizationContext _context;
        public keywordRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }


        public IQueryable<T> GetKeywordList<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@SearchText",searchFilterModel.SearchText), 
                new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber), 
                new SqlParameter("@PageSize",searchFilterModel.pageSize),
                new SqlParameter("@SortColumn",searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.GetKeywordList, parameters.Length, parameters).AsQueryable();
        }


        public List<AppointmentModel> GetProviderListToMakeAppointmentForMobile(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModelForMobile appointmentSearchModel,int categoryid)
        {
            //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date);
            string day = "";
            if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
            {
                var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
                day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
            }
            SqlParameter[] parameters = {
                                          new SqlParameter("@organizationId",tokenModel.OrganizationID),
                                           new SqlParameter("@CareCategoryId",categoryid),
                                          new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
                                          new SqlParameter("@locationId",appointmentSearchModel.Locations),
                                          new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
                                          new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
                                          new SqlParameter("@serviceId",appointmentSearchModel.Services),
                                          new SqlParameter("@rating",appointmentSearchModel.Rating),
                                          new SqlParameter("@gender",appointmentSearchModel.Gender),
                                          new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
                                          new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
                                          new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
                                          new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
            };

            var result = _context.GetProviderListToMakeAppointment(SQLObjects.GetProviderListToBookAppointmentForMobile, parameters.Length, parameters);
            List<AppointmentModel> appointmentModels = new List<AppointmentModel>();
            var AppointmentModelResult = result.AppointmentModels;
            AppointmentModelResult.ForEach(app =>
            {
                AppointmentModel appointment = new AppointmentModel();
                appointment = app;
                appointment.StaffId = app.StaffId;
                appointment.ProviderId = CommonMethods.Encrypt(app.StaffId.ToString());
                appointment.FullName = CommonMethods.getFullName(app.FirstName, app.MiddleName, app.LastName);
                appointment.ProviderImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffPhotos, app.ProviderImage);
                appointment.ProviderImageThumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, app.ProviderImageThumbnail);
                appointment.Taxonomies = result.Taxonomies.Where(t => t.StaffId == app.StaffId).ToList();
                appointment.Experiences = result.Experiences.Where(t => t.StaffId == app.StaffId).ToList();
                appointment.Specialities = result.Specialities.Where(t => t.StaffId == app.StaffId).ToList();
                appointment.Availabilities = result.Availabilities.Where(t => t.StaffId == app.StaffId).ToList();
                appointmentModels.Add(appointment);
            });
            return appointmentModels;
        }
    }

}
