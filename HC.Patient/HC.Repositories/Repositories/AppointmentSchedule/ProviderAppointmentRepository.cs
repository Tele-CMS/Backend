using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories
{
    public class ProviderAppointmentRepository : IProviderAppointmentRepository
    {
        private HCOrganizationContext _context;
        public ProviderAppointmentRepository(HCOrganizationContext context)
        {
            this._context = context;
        }
        public List<AppointmentModel> GetProviderListToMakeAppointment(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel)
        {
            //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date); 
            string day = "";
            if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
            {
                var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
                day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
            }
            if (appointmentSearchModel.Gender=="") 
            {
                appointmentSearchModel.Gender = "0";
            }
            SqlParameter[] parameters = {
                                          new SqlParameter("@organizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
                                          new SqlParameter("@locationId",appointmentSearchModel.Locations),
                                          new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
                                          new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
                                          new SqlParameter("@serviceId",appointmentSearchModel.Services),
                                          new SqlParameter("@rating",appointmentSearchModel.Rates),
                                          new SqlParameter("@gender",appointmentSearchModel.Gender),
                                          new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
                                          new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
                                          new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
                                          new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
                                          new SqlParameter("@ProviderId",appointmentSearchModel.ProviderId),
                                           
            };

            var result = _context.GetProviderListToMakeAppointment(SQLObjects.GetProviderListToBookAppointment, parameters.Length, parameters);
            List<AppointmentModel> appointmentModels = new List<AppointmentModel>();
            var AppointmentModelResult = result.AppointmentModels;
            //if (!string.IsNullOrEmpty(appointmentSearchModel.ProviderId))
            //{
            //    int staffId = Convert.ToInt32(appointmentSearchModel.ProviderId);
            //    AppointmentModelResult = AppointmentModelResult.Where(x => x.StaffId == staffId).ToList();
            //}
            AppointmentModelResult.ForEach(app =>
            {
                AppointmentModel appointment = new AppointmentModel();
                appointment.FollowUpDays = appointment.FollowUpDays ?? 0;
                appointment.FollowUpPayRate = appointment.FollowUpPayRate ?? 0;
                appointment.keyword = app.keyword;
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
            #region Old Code


            //if (string.IsNullOrEmpty(appointmentSearchModel.sortColumn))
            //    appointmentSearchModel.sortColumn = "ProviderFirstName";

            //if (string.IsNullOrEmpty(appointmentSearchModel.sortOrder) || appointmentSearchModel.sortOrder == "asc")
            //    appointmentSearchModel.sortOrder = "true";
            //else
            //    appointmentSearchModel.sortOrder = "false";

            //var providers = (CommonMethods.OrderByField((from s in _context.Staffs
            //                                             join u in _context.User on s.UserID equals u.Id
            //                                             join st in _context.StaffTaxonomies on s.Id equals st.StaffID
            //                                             join gc in _context.GlobalCode on st.GlobalCodeId equals gc.Id
            //                                             join avail in _context.StaffAvailability on s.Id equals avail.StaffID
            //                                             join loc in _context.StaffLocation on s.Id equals loc.StaffId
            //                                             join country in _context.MasterCountry on s.CountryID equals country.Id
            //                                             join d in _context.MasterWeekDays on avail.DayId equals d.Id
            //                                             where u.OrganizationID == tokenModel.OrganizationID
            //                                             && (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd").Contains(d.Day) 
            //                                             && appointmentSearchModel.LocationIds.Contains(loc.LocationID.ToString())
            //                                             //&& appointmentSearchModel.Specialities.ToLower().Contains(gc.Id.ToString())
            //                                             && u.IsDeleted == false && s.IsDeleted == false && st.IsDeleted == false && avail.IsDeleted == false
            //                                             && u.IsActive == true & s.IsActive == true && st.IsActive == true && avail.IsActive == true
            //                                             orderby ("" + appointmentSearchModel.sortColumn + " " + appointmentSearchModel.sortOrder + "")
            //                                             select new AppointmentModel()
            //                                             {
            //                                                 ProviderId = CommonMethods.Encrypt(s.Id.ToString()),
            //                                                 ProviderFirstName = s.FirstName,
            //                                                 ProviderMiddleName = s.MiddleName,
            //                                                 ProviderLastName = s.LastName,
            //                                                 ProviderFullName = string.IsNullOrEmpty(s.MiddleName) ? (string.IsNullOrEmpty(s.LastName) ? s.FirstName : s.FirstName + " " + s.LastName) : s.FirstName + " " + s.MiddleName + " " + s.LastName,
            //                                                 PayRate = s.PayRate,
            //                                                 Address = s.Address,
            //                                                 Country = country.CountryName,
            //                                                 ProviderImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffPhotos, s.PhotoPath),
            //                                                 ProviderImageThumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, s.PhotoThumbnailPath),
            //                                                 Phone = s.PhoneNumber,
            //                                                 Email = s.Email,
            //                                                 Taxonomies = (
            //                                                 from stax in _context.StaffTaxonomies
            //                                                 join gc in _context.GlobalCode on stax.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where stax.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && stax.IsDeleted == false && stax.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentTaxonomyModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(stax.Id.ToString()),
            //                                                     Taxonomy = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Specialities = (
            //                                                 from ss in _context.StaffSpecialities
            //                                                 join gc in _context.GlobalCode on ss.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where ss.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && ss.IsDeleted == false && ss.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentSpecialitiesModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(ss.Id.ToString()),
            //                                                     Speciality = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Experiences = (
            //                                                 from exp in _context.StaffExperiences
            //                                                 where exp.StaffId == s.Id
            //                                                 && exp.IsDeleted == false && exp.IsActive == true
            //                                                
            //                                                 select new AppointmentExperienceModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(exp.Id.ToString()),
            //                                                     StartDate = exp.StartDate,
            //                                                     EndDate = exp.EndDate,
            //                                                     Organization = exp.OrganizationName,
            //                                                     TotalExperience = CommonMethods.getYearMonthDayBetweenDates(exp.StartDate, (DateTime)exp.EndDate)

            //                                                 }
            //                                                 ).ToList()
            //                                                 //InvitationSendDate = inv.InvitationSendDate != null ? CommonMethods.ConvertFromUtcTimeWithOffset(inv.InvitationSendDate, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel) : inv.InvitationSendDate,
            //                                                 //InvitationStatus = ((Common.Enums.CommonEnum.UserInvitationStatus)inv.InvitationStatus).ToString()

            //                                             }).GroupBy(p => new { p.ProviderId }).Select(g => g.FirstOrDefault()),
            //                                          appointmentSearchModel.sortColumn, Convert.ToBoolean(appointmentSearchModel.sortOrder))).AsQueryable();
            ////var sql = CommonMethods.ToSql(providers);
            //var result = providers.Skip((appointmentSearchModel.pageNumber - 1) * appointmentSearchModel.pageSize).Take(appointmentSearchModel.pageSize).ToList();
            //result[0].TotalRecords = providers.ToList().Count();
            //return result;
            #endregion Old Code
        }

        //public List<AppointmentModel> GetProviderListToMakeAppointmentForRate(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel,string v1,string v2)
        //{
        //    //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date); 
        //    string day = "";
        //    if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
        //    {
        //        var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
        //        day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
        //    }
        //    if (appointmentSearchModel.Gender == "")
        //    {
        //        appointmentSearchModel.Gender = "0";
        //    }
        //    SqlParameter[] parameters = {
        //                                  new SqlParameter("@organizationId",tokenModel.OrganizationID),
        //                                  new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
        //                                  new SqlParameter("@locationId",appointmentSearchModel.Locations),
        //                                  new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
        //                                  new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
        //                                  new SqlParameter("@serviceId",appointmentSearchModel.Services),
        //                                  new SqlParameter("@v1",v1),
        //                                   new SqlParameter("@v2",v2),
        //                                  new SqlParameter("@gender",appointmentSearchModel.Gender),
        //                                  new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
        //                                  new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
        //                                  new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
        //                                  new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
        //    };

        //    var result = _context.GetProviderListToMakeAppointment(SQLObjects.GetProviderListToBookAppointmentForRate, parameters.Length, parameters);
        //    List<AppointmentModel> appointmentModels = new List<AppointmentModel>();
        //    var AppointmentModelResult = result.AppointmentModels;
        //    AppointmentModelResult.ForEach(app =>
        //    {
        //        AppointmentModel appointment = new AppointmentModel();
        //        appointment = app;
        //        appointment.StaffId = app.StaffId;
        //        appointment.ProviderId = CommonMethods.Encrypt(app.StaffId.ToString());
        //        appointment.FullName = CommonMethods.getFullName(app.FirstName, app.MiddleName, app.LastName);
        //        appointment.ProviderImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffPhotos, app.ProviderImage);
        //        appointment.ProviderImageThumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, app.ProviderImageThumbnail);
        //        appointment.Taxonomies = result.Taxonomies.Where(t => t.StaffId == app.StaffId).ToList();
        //        appointment.Experiences = result.Experiences.Where(t => t.StaffId == app.StaffId).ToList();
        //        appointment.Specialities = result.Specialities.Where(t => t.StaffId == app.StaffId).ToList();
        //        appointment.Availabilities = result.Availabilities.Where(t => t.StaffId == app.StaffId).ToList();
        //        appointmentModels.Add(appointment);
        //    });
        //    return appointmentModels;
        //    #region Old Code


        //    //if (string.IsNullOrEmpty(appointmentSearchModel.sortColumn))
        //    //    appointmentSearchModel.sortColumn = "ProviderFirstName";

        //    //if (string.IsNullOrEmpty(appointmentSearchModel.sortOrder) || appointmentSearchModel.sortOrder == "asc")
        //    //    appointmentSearchModel.sortOrder = "true";
        //    //else
        //    //    appointmentSearchModel.sortOrder = "false";

        //    //var providers = (CommonMethods.OrderByField((from s in _context.Staffs
        //    //                                             join u in _context.User on s.UserID equals u.Id
        //    //                                             join st in _context.StaffTaxonomies on s.Id equals st.StaffID
        //    //                                             join gc in _context.GlobalCode on st.GlobalCodeId equals gc.Id
        //    //                                             join avail in _context.StaffAvailability on s.Id equals avail.StaffID
        //    //                                             join loc in _context.StaffLocation on s.Id equals loc.StaffId
        //    //                                             join country in _context.MasterCountry on s.CountryID equals country.Id
        //    //                                             join d in _context.MasterWeekDays on avail.DayId equals d.Id
        //    //                                             where u.OrganizationID == tokenModel.OrganizationID
        //    //                                             && (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd").Contains(d.Day) 
        //    //                                             && appointmentSearchModel.LocationIds.Contains(loc.LocationID.ToString())
        //    //                                             //&& appointmentSearchModel.Specialities.ToLower().Contains(gc.Id.ToString())
        //    //                                             && u.IsDeleted == false && s.IsDeleted == false && st.IsDeleted == false && avail.IsDeleted == false
        //    //                                             && u.IsActive == true & s.IsActive == true && st.IsActive == true && avail.IsActive == true
        //    //                                             orderby ("" + appointmentSearchModel.sortColumn + " " + appointmentSearchModel.sortOrder + "")
        //    //                                             select new AppointmentModel()
        //    //                                             {
        //    //                                                 ProviderId = CommonMethods.Encrypt(s.Id.ToString()),
        //    //                                                 ProviderFirstName = s.FirstName,
        //    //                                                 ProviderMiddleName = s.MiddleName,
        //    //                                                 ProviderLastName = s.LastName,
        //    //                                                 ProviderFullName = string.IsNullOrEmpty(s.MiddleName) ? (string.IsNullOrEmpty(s.LastName) ? s.FirstName : s.FirstName + " " + s.LastName) : s.FirstName + " " + s.MiddleName + " " + s.LastName,
        //    //                                                 PayRate = s.PayRate,
        //    //                                                 Address = s.Address,
        //    //                                                 Country = country.CountryName,
        //    //                                                 ProviderImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffPhotos, s.PhotoPath),
        //    //                                                 ProviderImageThumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, s.PhotoThumbnailPath),
        //    //                                                 Phone = s.PhoneNumber,
        //    //                                                 Email = s.Email,
        //    //                                                 Taxonomies = (
        //    //                                                 from stax in _context.StaffTaxonomies
        //    //                                                 join gc in _context.GlobalCode on stax.GlobalCodeId equals gc.Id
        //    //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
        //    //                                                 where stax.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
        //    //                                                 && stax.IsDeleted == false && stax.IsActive == true
        //    //                                                 orderby gc.GlobalCodeName
        //    //                                                 select new AppointmentTaxonomyModel()
        //    //                                                 {
        //    //                                                     Id = CommonMethods.Encrypt(stax.Id.ToString()),
        //    //                                                     Taxonomy = gc.GlobalCodeName
        //    //                                                 }
        //    //                                                 ).ToList(),
        //    //                                                 Specialities = (
        //    //                                                 from ss in _context.StaffSpecialities
        //    //                                                 join gc in _context.GlobalCode on ss.GlobalCodeId equals gc.Id
        //    //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
        //    //                                                 where ss.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
        //    //                                                 && ss.IsDeleted == false && ss.IsActive == true
        //    //                                                 orderby gc.GlobalCodeName
        //    //                                                 select new AppointmentSpecialitiesModel()
        //    //                                                 {
        //    //                                                     Id = CommonMethods.Encrypt(ss.Id.ToString()),
        //    //                                                     Speciality = gc.GlobalCodeName
        //    //                                                 }
        //    //                                                 ).ToList(),
        //    //                                                 Experiences = (
        //    //                                                 from exp in _context.StaffExperiences
        //    //                                                 where exp.StaffId == s.Id
        //    //                                                 && exp.IsDeleted == false && exp.IsActive == true
        //    //                                                
        //    //                                                 select new AppointmentExperienceModel()
        //    //                                                 {
        //    //                                                     Id = CommonMethods.Encrypt(exp.Id.ToString()),
        //    //                                                     StartDate = exp.StartDate,
        //    //                                                     EndDate = exp.EndDate,
        //    //                                                     Organization = exp.OrganizationName,
        //    //                                                     TotalExperience = CommonMethods.getYearMonthDayBetweenDates(exp.StartDate, (DateTime)exp.EndDate)

        //    //                                                 }
        //    //                                                 ).ToList()
        //    //                                                 //InvitationSendDate = inv.InvitationSendDate != null ? CommonMethods.ConvertFromUtcTimeWithOffset(inv.InvitationSendDate, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel) : inv.InvitationSendDate,
        //    //                                                 //InvitationStatus = ((Common.Enums.CommonEnum.UserInvitationStatus)inv.InvitationStatus).ToString()

        //    //                                             }).GroupBy(p => new { p.ProviderId }).Select(g => g.FirstOrDefault()),
        //    //                                          appointmentSearchModel.sortColumn, Convert.ToBoolean(appointmentSearchModel.sortOrder))).AsQueryable();
        //    ////var sql = CommonMethods.ToSql(providers);
        //    //var result = providers.Skip((appointmentSearchModel.pageNumber - 1) * appointmentSearchModel.pageSize).Take(appointmentSearchModel.pageSize).ToList();
        //    //result[0].TotalRecords = providers.ToList().Count();
        //    //return result;
        //    #endregion Old Code
        //}

        public List<AppointmentModel> GetProviderListToMakeAppointmentForRate(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel, string payrate, string minrate)
        {
            //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date); 
            string day = "";
            if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
            {
                var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
                day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
            }
            if (appointmentSearchModel.Gender == "")
            {
                appointmentSearchModel.Gender = "0";
            }
            SqlParameter[] parameters = {
                                          new SqlParameter("@organizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
                                          new SqlParameter("@locationId",appointmentSearchModel.Locations),
                                          new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
                                          new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
                                          new SqlParameter("@serviceId",appointmentSearchModel.Services),
                                          new SqlParameter("@payrate",payrate),
                                          new SqlParameter("@v1",minrate),

                                          new SqlParameter("@gender",appointmentSearchModel.Gender),
                                          new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
                                          new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
                                          new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
                                          new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
            };

            var result = _context.GetProviderListToMakeAppointment(SQLObjects.GetProviderListToBookAppointmentForRate_v2, parameters.Length, parameters);
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
            #region Old Code


            //if (string.IsNullOrEmpty(appointmentSearchModel.sortColumn))
            //    appointmentSearchModel.sortColumn = "ProviderFirstName";

            //if (string.IsNullOrEmpty(appointmentSearchModel.sortOrder) || appointmentSearchModel.sortOrder == "asc")
            //    appointmentSearchModel.sortOrder = "true";
            //else
            //    appointmentSearchModel.sortOrder = "false";

            //var providers = (CommonMethods.OrderByField((from s in _context.Staffs
            //                                             join u in _context.User on s.UserID equals u.Id
            //                                             join st in _context.StaffTaxonomies on s.Id equals st.StaffID
            //                                             join gc in _context.GlobalCode on st.GlobalCodeId equals gc.Id
            //                                             join avail in _context.StaffAvailability on s.Id equals avail.StaffID
            //                                             join loc in _context.StaffLocation on s.Id equals loc.StaffId
            //                                             join country in _context.MasterCountry on s.CountryID equals country.Id
            //                                             join d in _context.MasterWeekDays on avail.DayId equals d.Id
            //                                             where u.OrganizationID == tokenModel.OrganizationID
            //                                             && (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd").Contains(d.Day) 
            //                                             && appointmentSearchModel.LocationIds.Contains(loc.LocationID.ToString())
            //                                             //&& appointmentSearchModel.Specialities.ToLower().Contains(gc.Id.ToString())
            //                                             && u.IsDeleted == false && s.IsDeleted == false && st.IsDeleted == false && avail.IsDeleted == false
            //                                             && u.IsActive == true & s.IsActive == true && st.IsActive == true && avail.IsActive == true
            //                                             orderby ("" + appointmentSearchModel.sortColumn + " " + appointmentSearchModel.sortOrder + "")
            //                                             select new AppointmentModel()
            //                                             {
            //                                                 ProviderId = CommonMethods.Encrypt(s.Id.ToString()),
            //                                                 ProviderFirstName = s.FirstName,
            //                                                 ProviderMiddleName = s.MiddleName,
            //                                                 ProviderLastName = s.LastName,
            //                                                 ProviderFullName = string.IsNullOrEmpty(s.MiddleName) ? (string.IsNullOrEmpty(s.LastName) ? s.FirstName : s.FirstName + " " + s.LastName) : s.FirstName + " " + s.MiddleName + " " + s.LastName,
            //                                                 PayRate = s.PayRate,
            //                                                 Address = s.Address,
            //                                                 Country = country.CountryName,
            //                                                 ProviderImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffPhotos, s.PhotoPath),
            //                                                 ProviderImageThumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, s.PhotoThumbnailPath),
            //                                                 Phone = s.PhoneNumber,
            //                                                 Email = s.Email,
            //                                                 Taxonomies = (
            //                                                 from stax in _context.StaffTaxonomies
            //                                                 join gc in _context.GlobalCode on stax.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where stax.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && stax.IsDeleted == false && stax.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentTaxonomyModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(stax.Id.ToString()),
            //                                                     Taxonomy = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Specialities = (
            //                                                 from ss in _context.StaffSpecialities
            //                                                 join gc in _context.GlobalCode on ss.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where ss.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && ss.IsDeleted == false && ss.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentSpecialitiesModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(ss.Id.ToString()),
            //                                                     Speciality = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Experiences = (
            //                                                 from exp in _context.StaffExperiences
            //                                                 where exp.StaffId == s.Id
            //                                                 && exp.IsDeleted == false && exp.IsActive == true
            //                                                
            //                                                 select new AppointmentExperienceModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(exp.Id.ToString()),
            //                                                     StartDate = exp.StartDate,
            //                                                     EndDate = exp.EndDate,
            //                                                     Organization = exp.OrganizationName,
            //                                                     TotalExperience = CommonMethods.getYearMonthDayBetweenDates(exp.StartDate, (DateTime)exp.EndDate)

            //                                                 }
            //                                                 ).ToList()
            //                                                 //InvitationSendDate = inv.InvitationSendDate != null ? CommonMethods.ConvertFromUtcTimeWithOffset(inv.InvitationSendDate, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel) : inv.InvitationSendDate,
            //                                                 //InvitationStatus = ((Common.Enums.CommonEnum.UserInvitationStatus)inv.InvitationStatus).ToString()

            //                                             }).GroupBy(p => new { p.ProviderId }).Select(g => g.FirstOrDefault()),
            //                                          appointmentSearchModel.sortColumn, Convert.ToBoolean(appointmentSearchModel.sortOrder))).AsQueryable();
            ////var sql = CommonMethods.ToSql(providers);
            //var result = providers.Skip((appointmentSearchModel.pageNumber - 1) * appointmentSearchModel.pageSize).Take(appointmentSearchModel.pageSize).ToList();
            //result[0].TotalRecords = providers.ToList().Count();
            //return result;
            #endregion Old Code
        }

        public List<AppointmentModel> GetProviderListToMakeAppointmentLessRate(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel)
        {
            //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date); 
            string day = "";
            if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
            {
                var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
                day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
            }
            if (appointmentSearchModel.Gender == "")
            {
                appointmentSearchModel.Gender = "0";
            }
            SqlParameter[] parameters = {
                                          new SqlParameter("@organizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
                                          new SqlParameter("@locationId",appointmentSearchModel.Locations),
                                          new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
                                          new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
                                          new SqlParameter("@serviceId",appointmentSearchModel.Services),
                                          new SqlParameter("@rating",appointmentSearchModel.Rates),
                                          new SqlParameter("@gender",appointmentSearchModel.Gender),
                                          new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
                                          new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
                                          new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
                                          new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
            };

            var result = _context.GetProviderListToMakeAppointment(SQLObjects.GetProviderListToBookAppointmentLessRate, parameters.Length, parameters);
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
            #region Old Code


            //if (string.IsNullOrEmpty(appointmentSearchModel.sortColumn))
            //    appointmentSearchModel.sortColumn = "ProviderFirstName";

            //if (string.IsNullOrEmpty(appointmentSearchModel.sortOrder) || appointmentSearchModel.sortOrder == "asc")
            //    appointmentSearchModel.sortOrder = "true";
            //else
            //    appointmentSearchModel.sortOrder = "false";

            //var providers = (CommonMethods.OrderByField((from s in _context.Staffs
            //                                             join u in _context.User on s.UserID equals u.Id
            //                                             join st in _context.StaffTaxonomies on s.Id equals st.StaffID
            //                                             join gc in _context.GlobalCode on st.GlobalCodeId equals gc.Id
            //                                             join avail in _context.StaffAvailability on s.Id equals avail.StaffID
            //                                             join loc in _context.StaffLocation on s.Id equals loc.StaffId
            //                                             join country in _context.MasterCountry on s.CountryID equals country.Id
            //                                             join d in _context.MasterWeekDays on avail.DayId equals d.Id
            //                                             where u.OrganizationID == tokenModel.OrganizationID
            //                                             && (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd").Contains(d.Day) 
            //                                             && appointmentSearchModel.LocationIds.Contains(loc.LocationID.ToString())
            //                                             //&& appointmentSearchModel.Specialities.ToLower().Contains(gc.Id.ToString())
            //                                             && u.IsDeleted == false && s.IsDeleted == false && st.IsDeleted == false && avail.IsDeleted == false
            //                                             && u.IsActive == true & s.IsActive == true && st.IsActive == true && avail.IsActive == true
            //                                             orderby ("" + appointmentSearchModel.sortColumn + " " + appointmentSearchModel.sortOrder + "")
            //                                             select new AppointmentModel()
            //                                             {
            //                                                 ProviderId = CommonMethods.Encrypt(s.Id.ToString()),
            //                                                 ProviderFirstName = s.FirstName,
            //                                                 ProviderMiddleName = s.MiddleName,
            //                                                 ProviderLastName = s.LastName,
            //                                                 ProviderFullName = string.IsNullOrEmpty(s.MiddleName) ? (string.IsNullOrEmpty(s.LastName) ? s.FirstName : s.FirstName + " " + s.LastName) : s.FirstName + " " + s.MiddleName + " " + s.LastName,
            //                                                 PayRate = s.PayRate,
            //                                                 Address = s.Address,
            //                                                 Country = country.CountryName,
            //                                                 ProviderImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffPhotos, s.PhotoPath),
            //                                                 ProviderImageThumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, s.PhotoThumbnailPath),
            //                                                 Phone = s.PhoneNumber,
            //                                                 Email = s.Email,
            //                                                 Taxonomies = (
            //                                                 from stax in _context.StaffTaxonomies
            //                                                 join gc in _context.GlobalCode on stax.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where stax.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && stax.IsDeleted == false && stax.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentTaxonomyModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(stax.Id.ToString()),
            //                                                     Taxonomy = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Specialities = (
            //                                                 from ss in _context.StaffSpecialities
            //                                                 join gc in _context.GlobalCode on ss.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where ss.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && ss.IsDeleted == false && ss.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentSpecialitiesModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(ss.Id.ToString()),
            //                                                     Speciality = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Experiences = (
            //                                                 from exp in _context.StaffExperiences
            //                                                 where exp.StaffId == s.Id
            //                                                 && exp.IsDeleted == false && exp.IsActive == true
            //                                                
            //                                                 select new AppointmentExperienceModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(exp.Id.ToString()),
            //                                                     StartDate = exp.StartDate,
            //                                                     EndDate = exp.EndDate,
            //                                                     Organization = exp.OrganizationName,
            //                                                     TotalExperience = CommonMethods.getYearMonthDayBetweenDates(exp.StartDate, (DateTime)exp.EndDate)

            //                                                 }
            //                                                 ).ToList()
            //                                                 //InvitationSendDate = inv.InvitationSendDate != null ? CommonMethods.ConvertFromUtcTimeWithOffset(inv.InvitationSendDate, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel) : inv.InvitationSendDate,
            //                                                 //InvitationStatus = ((Common.Enums.CommonEnum.UserInvitationStatus)inv.InvitationStatus).ToString()

            //                                             }).GroupBy(p => new { p.ProviderId }).Select(g => g.FirstOrDefault()),
            //                                          appointmentSearchModel.sortColumn, Convert.ToBoolean(appointmentSearchModel.sortOrder))).AsQueryable();
            ////var sql = CommonMethods.ToSql(providers);
            //var result = providers.Skip((appointmentSearchModel.pageNumber - 1) * appointmentSearchModel.pageSize).Take(appointmentSearchModel.pageSize).ToList();
            //result[0].TotalRecords = providers.ToList().Count();
            //return result;
            #endregion Old Code
        }

        public List<AppointmentModel> GetProviderListToMakeAppointmentMoreRate(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel)
        {
            //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date); 
            string day = "";
            if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
            {
                var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
                day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
            }
            if (appointmentSearchModel.Gender == "")
            {
                appointmentSearchModel.Gender = "0";
            }
            SqlParameter[] parameters = {
                                          new SqlParameter("@organizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
                                          new SqlParameter("@locationId",appointmentSearchModel.Locations),
                                          new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
                                          new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
                                          new SqlParameter("@serviceId",appointmentSearchModel.Services),
                                          new SqlParameter("@rating",appointmentSearchModel.Rates),
                                          new SqlParameter("@gender",appointmentSearchModel.Gender),
                                          new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
                                          new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
                                          new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
                                          new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
            };

            var result = _context.GetProviderListToMakeAppointment(SQLObjects.GetProviderListToBookAppointmentMoreRate, parameters.Length, parameters);
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
            #region Old Code


            //if (string.IsNullOrEmpty(appointmentSearchModel.sortColumn))
            //    appointmentSearchModel.sortColumn = "ProviderFirstName";

            //if (string.IsNullOrEmpty(appointmentSearchModel.sortOrder) || appointmentSearchModel.sortOrder == "asc")
            //    appointmentSearchModel.sortOrder = "true";
            //else
            //    appointmentSearchModel.sortOrder = "false";

            //var providers = (CommonMethods.OrderByField((from s in _context.Staffs
            //                                             join u in _context.User on s.UserID equals u.Id
            //                                             join st in _context.StaffTaxonomies on s.Id equals st.StaffID
            //                                             join gc in _context.GlobalCode on st.GlobalCodeId equals gc.Id
            //                                             join avail in _context.StaffAvailability on s.Id equals avail.StaffID
            //                                             join loc in _context.StaffLocation on s.Id equals loc.StaffId
            //                                             join country in _context.MasterCountry on s.CountryID equals country.Id
            //                                             join d in _context.MasterWeekDays on avail.DayId equals d.Id
            //                                             where u.OrganizationID == tokenModel.OrganizationID
            //                                             && (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd").Contains(d.Day) 
            //                                             && appointmentSearchModel.LocationIds.Contains(loc.LocationID.ToString())
            //                                             //&& appointmentSearchModel.Specialities.ToLower().Contains(gc.Id.ToString())
            //                                             && u.IsDeleted == false && s.IsDeleted == false && st.IsDeleted == false && avail.IsDeleted == false
            //                                             && u.IsActive == true & s.IsActive == true && st.IsActive == true && avail.IsActive == true
            //                                             orderby ("" + appointmentSearchModel.sortColumn + " " + appointmentSearchModel.sortOrder + "")
            //                                             select new AppointmentModel()
            //                                             {
            //                                                 ProviderId = CommonMethods.Encrypt(s.Id.ToString()),
            //                                                 ProviderFirstName = s.FirstName,
            //                                                 ProviderMiddleName = s.MiddleName,
            //                                                 ProviderLastName = s.LastName,
            //                                                 ProviderFullName = string.IsNullOrEmpty(s.MiddleName) ? (string.IsNullOrEmpty(s.LastName) ? s.FirstName : s.FirstName + " " + s.LastName) : s.FirstName + " " + s.MiddleName + " " + s.LastName,
            //                                                 PayRate = s.PayRate,
            //                                                 Address = s.Address,
            //                                                 Country = country.CountryName,
            //                                                 ProviderImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffPhotos, s.PhotoPath),
            //                                                 ProviderImageThumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, s.PhotoThumbnailPath),
            //                                                 Phone = s.PhoneNumber,
            //                                                 Email = s.Email,
            //                                                 Taxonomies = (
            //                                                 from stax in _context.StaffTaxonomies
            //                                                 join gc in _context.GlobalCode on stax.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where stax.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && stax.IsDeleted == false && stax.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentTaxonomyModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(stax.Id.ToString()),
            //                                                     Taxonomy = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Specialities = (
            //                                                 from ss in _context.StaffSpecialities
            //                                                 join gc in _context.GlobalCode on ss.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where ss.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && ss.IsDeleted == false && ss.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentSpecialitiesModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(ss.Id.ToString()),
            //                                                     Speciality = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Experiences = (
            //                                                 from exp in _context.StaffExperiences
            //                                                 where exp.StaffId == s.Id
            //                                                 && exp.IsDeleted == false && exp.IsActive == true
            //                                                
            //                                                 select new AppointmentExperienceModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(exp.Id.ToString()),
            //                                                     StartDate = exp.StartDate,
            //                                                     EndDate = exp.EndDate,
            //                                                     Organization = exp.OrganizationName,
            //                                                     TotalExperience = CommonMethods.getYearMonthDayBetweenDates(exp.StartDate, (DateTime)exp.EndDate)

            //                                                 }
            //                                                 ).ToList()
            //                                                 //InvitationSendDate = inv.InvitationSendDate != null ? CommonMethods.ConvertFromUtcTimeWithOffset(inv.InvitationSendDate, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel) : inv.InvitationSendDate,
            //                                                 //InvitationStatus = ((Common.Enums.CommonEnum.UserInvitationStatus)inv.InvitationStatus).ToString()

            //                                             }).GroupBy(p => new { p.ProviderId }).Select(g => g.FirstOrDefault()),
            //                                          appointmentSearchModel.sortColumn, Convert.ToBoolean(appointmentSearchModel.sortOrder))).AsQueryable();
            ////var sql = CommonMethods.ToSql(providers);
            //var result = providers.Skip((appointmentSearchModel.pageNumber - 1) * appointmentSearchModel.pageSize).Take(appointmentSearchModel.pageSize).ToList();
            //result[0].TotalRecords = providers.ToList().Count();
            //return result;
            #endregion Old Code
        }

        public List<AppointmentModel> GetProviderListToMakeAppointmentForReviewrating(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel, string rating)
        {
            //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date); 
            string day = "";
            if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
            {
                var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
                day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
            }
            if (appointmentSearchModel.Gender == "")
            {
                appointmentSearchModel.Gender = "0";
            }
            SqlParameter[] parameters = {
                                          new SqlParameter("@organizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
                                          new SqlParameter("@locationId",appointmentSearchModel.Locations),
                                          new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
                                          new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
                                          new SqlParameter("@serviceId",appointmentSearchModel.Services),
                                          new SqlParameter("@rating",rating),
                                           
                                          new SqlParameter("@gender",appointmentSearchModel.Gender),
                                          new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
                                          new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
                                          new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
                                          new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
            };

            var result = _context.GetProviderListToMakeAppointment(SQLObjects.GetProviderListToBookAppointmentRviewRating, parameters.Length, parameters);
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
            #region Old Code


            //if (string.IsNullOrEmpty(appointmentSearchModel.sortColumn))
            //    appointmentSearchModel.sortColumn = "ProviderFirstName";

            //if (string.IsNullOrEmpty(appointmentSearchModel.sortOrder) || appointmentSearchModel.sortOrder == "asc")
            //    appointmentSearchModel.sortOrder = "true";
            //else
            //    appointmentSearchModel.sortOrder = "false";

            //var providers = (CommonMethods.OrderByField((from s in _context.Staffs
            //                                             join u in _context.User on s.UserID equals u.Id
            //                                             join st in _context.StaffTaxonomies on s.Id equals st.StaffID
            //                                             join gc in _context.GlobalCode on st.GlobalCodeId equals gc.Id
            //                                             join avail in _context.StaffAvailability on s.Id equals avail.StaffID
            //                                             join loc in _context.StaffLocation on s.Id equals loc.StaffId
            //                                             join country in _context.MasterCountry on s.CountryID equals country.Id
            //                                             join d in _context.MasterWeekDays on avail.DayId equals d.Id
            //                                             where u.OrganizationID == tokenModel.OrganizationID
            //                                             && (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd").Contains(d.Day) 
            //                                             && appointmentSearchModel.LocationIds.Contains(loc.LocationID.ToString())
            //                                             //&& appointmentSearchModel.Specialities.ToLower().Contains(gc.Id.ToString())
            //                                             && u.IsDeleted == false && s.IsDeleted == false && st.IsDeleted == false && avail.IsDeleted == false
            //                                             && u.IsActive == true & s.IsActive == true && st.IsActive == true && avail.IsActive == true
            //                                             orderby ("" + appointmentSearchModel.sortColumn + " " + appointmentSearchModel.sortOrder + "")
            //                                             select new AppointmentModel()
            //                                             {
            //                                                 ProviderId = CommonMethods.Encrypt(s.Id.ToString()),
            //                                                 ProviderFirstName = s.FirstName,
            //                                                 ProviderMiddleName = s.MiddleName,
            //                                                 ProviderLastName = s.LastName,
            //                                                 ProviderFullName = string.IsNullOrEmpty(s.MiddleName) ? (string.IsNullOrEmpty(s.LastName) ? s.FirstName : s.FirstName + " " + s.LastName) : s.FirstName + " " + s.MiddleName + " " + s.LastName,
            //                                                 PayRate = s.PayRate,
            //                                                 Address = s.Address,
            //                                                 Country = country.CountryName,
            //                                                 ProviderImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffPhotos, s.PhotoPath),
            //                                                 ProviderImageThumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, s.PhotoThumbnailPath),
            //                                                 Phone = s.PhoneNumber,
            //                                                 Email = s.Email,
            //                                                 Taxonomies = (
            //                                                 from stax in _context.StaffTaxonomies
            //                                                 join gc in _context.GlobalCode on stax.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where stax.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && stax.IsDeleted == false && stax.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentTaxonomyModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(stax.Id.ToString()),
            //                                                     Taxonomy = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Specialities = (
            //                                                 from ss in _context.StaffSpecialities
            //                                                 join gc in _context.GlobalCode on ss.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where ss.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && ss.IsDeleted == false && ss.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentSpecialitiesModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(ss.Id.ToString()),
            //                                                     Speciality = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Experiences = (
            //                                                 from exp in _context.StaffExperiences
            //                                                 where exp.StaffId == s.Id
            //                                                 && exp.IsDeleted == false && exp.IsActive == true
            //                                                
            //                                                 select new AppointmentExperienceModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(exp.Id.ToString()),
            //                                                     StartDate = exp.StartDate,
            //                                                     EndDate = exp.EndDate,
            //                                                     Organization = exp.OrganizationName,
            //                                                     TotalExperience = CommonMethods.getYearMonthDayBetweenDates(exp.StartDate, (DateTime)exp.EndDate)

            //                                                 }
            //                                                 ).ToList()
            //                                                 //InvitationSendDate = inv.InvitationSendDate != null ? CommonMethods.ConvertFromUtcTimeWithOffset(inv.InvitationSendDate, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel) : inv.InvitationSendDate,
            //                                                 //InvitationStatus = ((Common.Enums.CommonEnum.UserInvitationStatus)inv.InvitationStatus).ToString()

            //                                             }).GroupBy(p => new { p.ProviderId }).Select(g => g.FirstOrDefault()),
            //                                          appointmentSearchModel.sortColumn, Convert.ToBoolean(appointmentSearchModel.sortOrder))).AsQueryable();
            ////var sql = CommonMethods.ToSql(providers);
            //var result = providers.Skip((appointmentSearchModel.pageNumber - 1) * appointmentSearchModel.pageSize).Take(appointmentSearchModel.pageSize).ToList();
            //result[0].TotalRecords = providers.ToList().Count();
            //return result;
            #endregion Old Code
        }

        public List<AppointmentModel> SortedProviderAvailableList(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel)
        {
            //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date); 
            string day = "";
            if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
            {
                var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
                day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
            }
            if (appointmentSearchModel.Gender == "")
            {
                appointmentSearchModel.Gender = "0";
            }
            SqlParameter[] parameters = {
                                          new SqlParameter("@organizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@v1",appointmentSearchModel.SortType),
                                          new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
                                          new SqlParameter("@locationId",appointmentSearchModel.Locations),
                                          new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
                                          new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
                                          new SqlParameter("@serviceId",appointmentSearchModel.Services),
                                          new SqlParameter("@rating",appointmentSearchModel.Rates),
                                          new SqlParameter("@gender",appointmentSearchModel.Gender),
                                          new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
                                          new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
                                          new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
                                          new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
            };

            var result = _context.GetProviderListToMakeAppointment(SQLObjects.GetSortedProviderListToBookAppointment, parameters.Length, parameters);
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
            #region Old Code


            //if (string.IsNullOrEmpty(appointmentSearchModel.sortColumn))
            //    appointmentSearchModel.sortColumn = "ProviderFirstName";

            //if (string.IsNullOrEmpty(appointmentSearchModel.sortOrder) || appointmentSearchModel.sortOrder == "asc")
            //    appointmentSearchModel.sortOrder = "true";
            //else
            //    appointmentSearchModel.sortOrder = "false";

            //var providers = (CommonMethods.OrderByField((from s in _context.Staffs
            //                                             join u in _context.User on s.UserID equals u.Id
            //                                             join st in _context.StaffTaxonomies on s.Id equals st.StaffID
            //                                             join gc in _context.GlobalCode on st.GlobalCodeId equals gc.Id
            //                                             join avail in _context.StaffAvailability on s.Id equals avail.StaffID
            //                                             join loc in _context.StaffLocation on s.Id equals loc.StaffId
            //                                             join country in _context.MasterCountry on s.CountryID equals country.Id
            //                                             join d in _context.MasterWeekDays on avail.DayId equals d.Id
            //                                             where u.OrganizationID == tokenModel.OrganizationID
            //                                             && (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd").Contains(d.Day) 
            //                                             && appointmentSearchModel.LocationIds.Contains(loc.LocationID.ToString())
            //                                             //&& appointmentSearchModel.Specialities.ToLower().Contains(gc.Id.ToString())
            //                                             && u.IsDeleted == false && s.IsDeleted == false && st.IsDeleted == false && avail.IsDeleted == false
            //                                             && u.IsActive == true & s.IsActive == true && st.IsActive == true && avail.IsActive == true
            //                                             orderby ("" + appointmentSearchModel.sortColumn + " " + appointmentSearchModel.sortOrder + "")
            //                                             select new AppointmentModel()
            //                                             {
            //                                                 ProviderId = CommonMethods.Encrypt(s.Id.ToString()),
            //                                                 ProviderFirstName = s.FirstName,
            //                                                 ProviderMiddleName = s.MiddleName,
            //                                                 ProviderLastName = s.LastName,
            //                                                 ProviderFullName = string.IsNullOrEmpty(s.MiddleName) ? (string.IsNullOrEmpty(s.LastName) ? s.FirstName : s.FirstName + " " + s.LastName) : s.FirstName + " " + s.MiddleName + " " + s.LastName,
            //                                                 PayRate = s.PayRate,
            //                                                 Address = s.Address,
            //                                                 Country = country.CountryName,
            //                                                 ProviderImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffPhotos, s.PhotoPath),
            //                                                 ProviderImageThumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, s.PhotoThumbnailPath),
            //                                                 Phone = s.PhoneNumber,
            //                                                 Email = s.Email,
            //                                                 Taxonomies = (
            //                                                 from stax in _context.StaffTaxonomies
            //                                                 join gc in _context.GlobalCode on stax.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where stax.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && stax.IsDeleted == false && stax.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentTaxonomyModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(stax.Id.ToString()),
            //                                                     Taxonomy = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Specialities = (
            //                                                 from ss in _context.StaffSpecialities
            //                                                 join gc in _context.GlobalCode on ss.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where ss.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && ss.IsDeleted == false && ss.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentSpecialitiesModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(ss.Id.ToString()),
            //                                                     Speciality = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Experiences = (
            //                                                 from exp in _context.StaffExperiences
            //                                                 where exp.StaffId == s.Id
            //                                                 && exp.IsDeleted == false && exp.IsActive == true
            //                                                
            //                                                 select new AppointmentExperienceModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(exp.Id.ToString()),
            //                                                     StartDate = exp.StartDate,
            //                                                     EndDate = exp.EndDate,
            //                                                     Organization = exp.OrganizationName,
            //                                                     TotalExperience = CommonMethods.getYearMonthDayBetweenDates(exp.StartDate, (DateTime)exp.EndDate)

            //                                                 }
            //                                                 ).ToList()
            //                                                 //InvitationSendDate = inv.InvitationSendDate != null ? CommonMethods.ConvertFromUtcTimeWithOffset(inv.InvitationSendDate, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel) : inv.InvitationSendDate,
            //                                                 //InvitationStatus = ((Common.Enums.CommonEnum.UserInvitationStatus)inv.InvitationStatus).ToString()

            //                                             }).GroupBy(p => new { p.ProviderId }).Select(g => g.FirstOrDefault()),
            //                                          appointmentSearchModel.sortColumn, Convert.ToBoolean(appointmentSearchModel.sortOrder))).AsQueryable();
            ////var sql = CommonMethods.ToSql(providers);
            //var result = providers.Skip((appointmentSearchModel.pageNumber - 1) * appointmentSearchModel.pageSize).Take(appointmentSearchModel.pageSize).ToList();
            //result[0].TotalRecords = providers.ToList().Count();
            //return result;
            #endregion Old Code
        }

        public List<AppointmentModel> GetSpecialitySearchTextProviderListToMakeAppointment(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel)
        {
            //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date); 
            string day = "";
            if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
            {
                var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
                day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
            }
            if (appointmentSearchModel.Gender == "")
            {
                appointmentSearchModel.Gender = "0";
            }
            SqlParameter[] parameters = {
                                          new SqlParameter("@organizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
                                          new SqlParameter("@locationId",appointmentSearchModel.Locations),
                                          new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
                                          new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
                                          new SqlParameter("@serviceId",appointmentSearchModel.Services),
                                          new SqlParameter("@rating",appointmentSearchModel.Rates),
                                          new SqlParameter("@gender",appointmentSearchModel.Gender),
                                          new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
                                          new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
                                          new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
                                          new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
                                       
            };

            var result = _context.GetProviderListToMakeAppointment(SQLObjects.GetProviderListToBookAppointment, parameters.Length, parameters);
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
            #region Old Code


            //if (string.IsNullOrEmpty(appointmentSearchModel.sortColumn))
            //    appointmentSearchModel.sortColumn = "ProviderFirstName";

            //if (string.IsNullOrEmpty(appointmentSearchModel.sortOrder) || appointmentSearchModel.sortOrder == "asc")
            //    appointmentSearchModel.sortOrder = "true";
            //else
            //    appointmentSearchModel.sortOrder = "false";

            //var providers = (CommonMethods.OrderByField((from s in _context.Staffs
            //                                             join u in _context.User on s.UserID equals u.Id
            //                                             join st in _context.StaffTaxonomies on s.Id equals st.StaffID
            //                                             join gc in _context.GlobalCode on st.GlobalCodeId equals gc.Id
            //                                             join avail in _context.StaffAvailability on s.Id equals avail.StaffID
            //                                             join loc in _context.StaffLocation on s.Id equals loc.StaffId
            //                                             join country in _context.MasterCountry on s.CountryID equals country.Id
            //                                             join d in _context.MasterWeekDays on avail.DayId equals d.Id
            //                                             where u.OrganizationID == tokenModel.OrganizationID
            //                                             && (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd").Contains(d.Day) 
            //                                             && appointmentSearchModel.LocationIds.Contains(loc.LocationID.ToString())
            //                                             //&& appointmentSearchModel.Specialities.ToLower().Contains(gc.Id.ToString())
            //                                             && u.IsDeleted == false && s.IsDeleted == false && st.IsDeleted == false && avail.IsDeleted == false
            //                                             && u.IsActive == true & s.IsActive == true && st.IsActive == true && avail.IsActive == true
            //                                             orderby ("" + appointmentSearchModel.sortColumn + " " + appointmentSearchModel.sortOrder + "")
            //                                             select new AppointmentModel()
            //                                             {
            //                                                 ProviderId = CommonMethods.Encrypt(s.Id.ToString()),
            //                                                 ProviderFirstName = s.FirstName,
            //                                                 ProviderMiddleName = s.MiddleName,
            //                                                 ProviderLastName = s.LastName,
            //                                                 ProviderFullName = string.IsNullOrEmpty(s.MiddleName) ? (string.IsNullOrEmpty(s.LastName) ? s.FirstName : s.FirstName + " " + s.LastName) : s.FirstName + " " + s.MiddleName + " " + s.LastName,
            //                                                 PayRate = s.PayRate,
            //                                                 Address = s.Address,
            //                                                 Country = country.CountryName,
            //                                                 ProviderImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffPhotos, s.PhotoPath),
            //                                                 ProviderImageThumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, s.PhotoThumbnailPath),
            //                                                 Phone = s.PhoneNumber,
            //                                                 Email = s.Email,
            //                                                 Taxonomies = (
            //                                                 from stax in _context.StaffTaxonomies
            //                                                 join gc in _context.GlobalCode on stax.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where stax.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && stax.IsDeleted == false && stax.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentTaxonomyModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(stax.Id.ToString()),
            //                                                     Taxonomy = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Specialities = (
            //                                                 from ss in _context.StaffSpecialities
            //                                                 join gc in _context.GlobalCode on ss.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where ss.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && ss.IsDeleted == false && ss.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentSpecialitiesModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(ss.Id.ToString()),
            //                                                     Speciality = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Experiences = (
            //                                                 from exp in _context.StaffExperiences
            //                                                 where exp.StaffId == s.Id
            //                                                 && exp.IsDeleted == false && exp.IsActive == true
            //                                                
            //                                                 select new AppointmentExperienceModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(exp.Id.ToString()),
            //                                                     StartDate = exp.StartDate,
            //                                                     EndDate = exp.EndDate,
            //                                                     Organization = exp.OrganizationName,
            //                                                     TotalExperience = CommonMethods.getYearMonthDayBetweenDates(exp.StartDate, (DateTime)exp.EndDate)

            //                                                 }
            //                                                 ).ToList()
            //                                                 //InvitationSendDate = inv.InvitationSendDate != null ? CommonMethods.ConvertFromUtcTimeWithOffset(inv.InvitationSendDate, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel) : inv.InvitationSendDate,
            //                                                 //InvitationStatus = ((Common.Enums.CommonEnum.UserInvitationStatus)inv.InvitationStatus).ToString()

            //                                             }).GroupBy(p => new { p.ProviderId }).Select(g => g.FirstOrDefault()),
            //                                          appointmentSearchModel.sortColumn, Convert.ToBoolean(appointmentSearchModel.sortOrder))).AsQueryable();
            ////var sql = CommonMethods.ToSql(providers);
            //var result = providers.Skip((appointmentSearchModel.pageNumber - 1) * appointmentSearchModel.pageSize).Take(appointmentSearchModel.pageSize).ToList();
            //result[0].TotalRecords = providers.ToList().Count();
            //return result;
            #endregion Old Code
        }

        public List<AppointmentModel> GetSpecialitySearchTextProviderListToMakeAppointmentKeySearch(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel)
        {
            //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date); 
            string day = "";
            if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
            {
                var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
                day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
            }
            if (appointmentSearchModel.Gender == "")
            {
                appointmentSearchModel.Gender = "0";
            }
            SqlParameter[] parameters = {
                                          new SqlParameter("@organizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
                                          new SqlParameter("@locationId",appointmentSearchModel.Locations),
                                          new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
                                          new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
                                          new SqlParameter("@serviceId",appointmentSearchModel.Services),
                                          new SqlParameter("@rating",appointmentSearchModel.Rates),
                                          new SqlParameter("@gender",appointmentSearchModel.Gender),
                                          new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
                                          new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
                                          new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
                                          new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
                                          new SqlParameter("@ProvidersearchText",appointmentSearchModel.ProvidersearchText),
                                         // new SqlParameter("@ProviderId",appointmentSearchModel.ProviderId),

            };

            var result = _context.GetProviderListToMakeAppointment(SQLObjects.getStaffAvailableForProfileListingKeySearch, parameters.Length, parameters);
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

        public List<AppointmentModel> GetUrgentCareProviderListToMakeAppointment(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel)
        {
            //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date); 
            string day = "";
            if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
            {
                var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
                day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
            }
            if (appointmentSearchModel.Gender == "")
            {
                appointmentSearchModel.Gender = "0";
            }
            SqlParameter[] parameters = {
                                          new SqlParameter("@organizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
                                          new SqlParameter("@locationId",appointmentSearchModel.Locations),
                                          new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
                                          new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
                                          new SqlParameter("@serviceId",appointmentSearchModel.Services),
                                          new SqlParameter("@rating",appointmentSearchModel.Rates),
                                          new SqlParameter("@gender",appointmentSearchModel.Gender),
                                          new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
                                          new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
                                          new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
                                          new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
                                          new SqlParameter("@ProviderId",appointmentSearchModel.ProviderId),

            };

            var result = _context.GetProviderListToMakeAppointment(SQLObjects.GetUrgentCareProviderListToBookAppointment, parameters.Length, parameters);
            List<AppointmentModel> appointmentModels = new List<AppointmentModel>();
            var AppointmentModelResult = result.AppointmentModels;
            //if (!string.IsNullOrEmpty(appointmentSearchModel.ProviderId))
            //{
            //    int staffId = Convert.ToInt32(appointmentSearchModel.ProviderId);
            //    AppointmentModelResult = AppointmentModelResult.Where(x => x.StaffId == staffId).ToList();
            //}
            AppointmentModelResult.ForEach(app =>
            {
                AppointmentModel appointment = new AppointmentModel();
                appointment.FollowUpDays = appointment.FollowUpDays ?? 0;
                appointment.FollowUpPayRate = appointment.FollowUpPayRate ?? 0;
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
            #region Old Code


            //if (string.IsNullOrEmpty(appointmentSearchModel.sortColumn))
            //    appointmentSearchModel.sortColumn = "ProviderFirstName";

            //if (string.IsNullOrEmpty(appointmentSearchModel.sortOrder) || appointmentSearchModel.sortOrder == "asc")
            //    appointmentSearchModel.sortOrder = "true";
            //else
            //    appointmentSearchModel.sortOrder = "false";

            //var providers = (CommonMethods.OrderByField((from s in _context.Staffs
            //                                             join u in _context.User on s.UserID equals u.Id
            //                                             join st in _context.StaffTaxonomies on s.Id equals st.StaffID
            //                                             join gc in _context.GlobalCode on st.GlobalCodeId equals gc.Id
            //                                             join avail in _context.StaffAvailability on s.Id equals avail.StaffID
            //                                             join loc in _context.StaffLocation on s.Id equals loc.StaffId
            //                                             join country in _context.MasterCountry on s.CountryID equals country.Id
            //                                             join d in _context.MasterWeekDays on avail.DayId equals d.Id
            //                                             where u.OrganizationID == tokenModel.OrganizationID
            //                                             && (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd").Contains(d.Day) 
            //                                             && appointmentSearchModel.LocationIds.Contains(loc.LocationID.ToString())
            //                                             //&& appointmentSearchModel.Specialities.ToLower().Contains(gc.Id.ToString())
            //                                             && u.IsDeleted == false && s.IsDeleted == false && st.IsDeleted == false && avail.IsDeleted == false
            //                                             && u.IsActive == true & s.IsActive == true && st.IsActive == true && avail.IsActive == true
            //                                             orderby ("" + appointmentSearchModel.sortColumn + " " + appointmentSearchModel.sortOrder + "")
            //                                             select new AppointmentModel()
            //                                             {
            //                                                 ProviderId = CommonMethods.Encrypt(s.Id.ToString()),
            //                                                 ProviderFirstName = s.FirstName,
            //                                                 ProviderMiddleName = s.MiddleName,
            //                                                 ProviderLastName = s.LastName,
            //                                                 ProviderFullName = string.IsNullOrEmpty(s.MiddleName) ? (string.IsNullOrEmpty(s.LastName) ? s.FirstName : s.FirstName + " " + s.LastName) : s.FirstName + " " + s.MiddleName + " " + s.LastName,
            //                                                 PayRate = s.PayRate,
            //                                                 Address = s.Address,
            //                                                 Country = country.CountryName,
            //                                                 ProviderImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffPhotos, s.PhotoPath),
            //                                                 ProviderImageThumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, s.PhotoThumbnailPath),
            //                                                 Phone = s.PhoneNumber,
            //                                                 Email = s.Email,
            //                                                 Taxonomies = (
            //                                                 from stax in _context.StaffTaxonomies
            //                                                 join gc in _context.GlobalCode on stax.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where stax.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && stax.IsDeleted == false && stax.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentTaxonomyModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(stax.Id.ToString()),
            //                                                     Taxonomy = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Specialities = (
            //                                                 from ss in _context.StaffSpecialities
            //                                                 join gc in _context.GlobalCode on ss.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where ss.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && ss.IsDeleted == false && ss.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentSpecialitiesModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(ss.Id.ToString()),
            //                                                     Speciality = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Experiences = (
            //                                                 from exp in _context.StaffExperiences
            //                                                 where exp.StaffId == s.Id
            //                                                 && exp.IsDeleted == false && exp.IsActive == true
            //                                                
            //                                                 select new AppointmentExperienceModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(exp.Id.ToString()),
            //                                                     StartDate = exp.StartDate,
            //                                                     EndDate = exp.EndDate,
            //                                                     Organization = exp.OrganizationName,
            //                                                     TotalExperience = CommonMethods.getYearMonthDayBetweenDates(exp.StartDate, (DateTime)exp.EndDate)

            //                                                 }
            //                                                 ).ToList()
            //                                                 //InvitationSendDate = inv.InvitationSendDate != null ? CommonMethods.ConvertFromUtcTimeWithOffset(inv.InvitationSendDate, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel) : inv.InvitationSendDate,
            //                                                 //InvitationStatus = ((Common.Enums.CommonEnum.UserInvitationStatus)inv.InvitationStatus).ToString()

            //                                             }).GroupBy(p => new { p.ProviderId }).Select(g => g.FirstOrDefault()),
            //                                          appointmentSearchModel.sortColumn, Convert.ToBoolean(appointmentSearchModel.sortOrder))).AsQueryable();
            ////var sql = CommonMethods.ToSql(providers);
            //var result = providers.Skip((appointmentSearchModel.pageNumber - 1) * appointmentSearchModel.pageSize).Take(appointmentSearchModel.pageSize).ToList();
            //result[0].TotalRecords = providers.ToList().Count();
            //return result;
            #endregion Old Code
        }

        public List<AppointmentModel> GetUrgentCareProviderListToMakeAppointmentForRate(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel, string payrate, string minrate)
        {
            //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date); 
            string day = "";
            if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
            {
                var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
                day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
            }
            if (appointmentSearchModel.Gender == "")
            {
                appointmentSearchModel.Gender = "0";
            }
            SqlParameter[] parameters = {
                                          new SqlParameter("@organizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
                                          new SqlParameter("@locationId",appointmentSearchModel.Locations),
                                          new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
                                          new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
                                          new SqlParameter("@serviceId",appointmentSearchModel.Services),
                                          new SqlParameter("@payrate",payrate),
                                          new SqlParameter("@v1",minrate),

                                          new SqlParameter("@gender",appointmentSearchModel.Gender),
                                          new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
                                          new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
                                          new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
                                          new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
            };

            var result = _context.GetProviderListToMakeAppointment(SQLObjects.GetUrgentCareProviderListToBookAppointmentForRate, parameters.Length, parameters);
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
            #region Old Code


            //if (string.IsNullOrEmpty(appointmentSearchModel.sortColumn))
            //    appointmentSearchModel.sortColumn = "ProviderFirstName";

            //if (string.IsNullOrEmpty(appointmentSearchModel.sortOrder) || appointmentSearchModel.sortOrder == "asc")
            //    appointmentSearchModel.sortOrder = "true";
            //else
            //    appointmentSearchModel.sortOrder = "false";

            //var providers = (CommonMethods.OrderByField((from s in _context.Staffs
            //                                             join u in _context.User on s.UserID equals u.Id
            //                                             join st in _context.StaffTaxonomies on s.Id equals st.StaffID
            //                                             join gc in _context.GlobalCode on st.GlobalCodeId equals gc.Id
            //                                             join avail in _context.StaffAvailability on s.Id equals avail.StaffID
            //                                             join loc in _context.StaffLocation on s.Id equals loc.StaffId
            //                                             join country in _context.MasterCountry on s.CountryID equals country.Id
            //                                             join d in _context.MasterWeekDays on avail.DayId equals d.Id
            //                                             where u.OrganizationID == tokenModel.OrganizationID
            //                                             && (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd").Contains(d.Day) 
            //                                             && appointmentSearchModel.LocationIds.Contains(loc.LocationID.ToString())
            //                                             //&& appointmentSearchModel.Specialities.ToLower().Contains(gc.Id.ToString())
            //                                             && u.IsDeleted == false && s.IsDeleted == false && st.IsDeleted == false && avail.IsDeleted == false
            //                                             && u.IsActive == true & s.IsActive == true && st.IsActive == true && avail.IsActive == true
            //                                             orderby ("" + appointmentSearchModel.sortColumn + " " + appointmentSearchModel.sortOrder + "")
            //                                             select new AppointmentModel()
            //                                             {
            //                                                 ProviderId = CommonMethods.Encrypt(s.Id.ToString()),
            //                                                 ProviderFirstName = s.FirstName,
            //                                                 ProviderMiddleName = s.MiddleName,
            //                                                 ProviderLastName = s.LastName,
            //                                                 ProviderFullName = string.IsNullOrEmpty(s.MiddleName) ? (string.IsNullOrEmpty(s.LastName) ? s.FirstName : s.FirstName + " " + s.LastName) : s.FirstName + " " + s.MiddleName + " " + s.LastName,
            //                                                 PayRate = s.PayRate,
            //                                                 Address = s.Address,
            //                                                 Country = country.CountryName,
            //                                                 ProviderImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffPhotos, s.PhotoPath),
            //                                                 ProviderImageThumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, s.PhotoThumbnailPath),
            //                                                 Phone = s.PhoneNumber,
            //                                                 Email = s.Email,
            //                                                 Taxonomies = (
            //                                                 from stax in _context.StaffTaxonomies
            //                                                 join gc in _context.GlobalCode on stax.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where stax.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && stax.IsDeleted == false && stax.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentTaxonomyModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(stax.Id.ToString()),
            //                                                     Taxonomy = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Specialities = (
            //                                                 from ss in _context.StaffSpecialities
            //                                                 join gc in _context.GlobalCode on ss.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where ss.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && ss.IsDeleted == false && ss.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentSpecialitiesModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(ss.Id.ToString()),
            //                                                     Speciality = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Experiences = (
            //                                                 from exp in _context.StaffExperiences
            //                                                 where exp.StaffId == s.Id
            //                                                 && exp.IsDeleted == false && exp.IsActive == true
            //                                                
            //                                                 select new AppointmentExperienceModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(exp.Id.ToString()),
            //                                                     StartDate = exp.StartDate,
            //                                                     EndDate = exp.EndDate,
            //                                                     Organization = exp.OrganizationName,
            //                                                     TotalExperience = CommonMethods.getYearMonthDayBetweenDates(exp.StartDate, (DateTime)exp.EndDate)

            //                                                 }
            //                                                 ).ToList()
            //                                                 //InvitationSendDate = inv.InvitationSendDate != null ? CommonMethods.ConvertFromUtcTimeWithOffset(inv.InvitationSendDate, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel) : inv.InvitationSendDate,
            //                                                 //InvitationStatus = ((Common.Enums.CommonEnum.UserInvitationStatus)inv.InvitationStatus).ToString()

            //                                             }).GroupBy(p => new { p.ProviderId }).Select(g => g.FirstOrDefault()),
            //                                          appointmentSearchModel.sortColumn, Convert.ToBoolean(appointmentSearchModel.sortOrder))).AsQueryable();
            ////var sql = CommonMethods.ToSql(providers);
            //var result = providers.Skip((appointmentSearchModel.pageNumber - 1) * appointmentSearchModel.pageSize).Take(appointmentSearchModel.pageSize).ToList();
            //result[0].TotalRecords = providers.ToList().Count();
            //return result;
            #endregion Old Code
        }
        public List<AppointmentModel> GetSpecialitySearchTextUrgentCareProviderListToMakeAppointmentKeySearch(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel)
        {
            //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date); 
            string day = "";
            if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
            {
                var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
                day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
            }
            if (appointmentSearchModel.Gender == "")
            {
                appointmentSearchModel.Gender = "0";
            }
            SqlParameter[] parameters = {
                                          new SqlParameter("@organizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
                                          new SqlParameter("@locationId",appointmentSearchModel.Locations),
                                          new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
                                          new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
                                          new SqlParameter("@serviceId",appointmentSearchModel.Services),
                                          new SqlParameter("@rating",appointmentSearchModel.Rates),
                                          new SqlParameter("@gender",appointmentSearchModel.Gender),
                                          new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
                                          new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
                                          new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
                                          new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
                                          new SqlParameter("@ProvidersearchText",appointmentSearchModel.ProvidersearchText),
                                         // new SqlParameter("@ProviderId",appointmentSearchModel.ProviderId),

            };

            var result = _context.GetProviderListToMakeAppointment(SQLObjects.getUrgentCareStaffAvailableListingKeySearch, parameters.Length, parameters);
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

        public List<AppointmentModel> SortedUrgentCareProviderAvailableList(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel)
        {
            //DateTime dt = Convert.ToDateTime(appointmentSearchModel.Date); 
            string day = "";
            if (!string.IsNullOrEmpty(appointmentSearchModel.Date))
            {
                var newDate = CommonMethods.ConvertDDMMYYYYToDateTime(appointmentSearchModel.Date, '-');
                day = (Convert.ToDateTime(newDate)).ToString("dddd");//CommonMethods.ConvertFromUtcTimeWithOffset(dt, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("dddd");
            }
            if (appointmentSearchModel.Gender == "")
            {
                appointmentSearchModel.Gender = "0";
            }
            SqlParameter[] parameters = {
                                          new SqlParameter("@organizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@v1",appointmentSearchModel.SortType),
                                          new SqlParameter("@availableDay",day),// (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd")),
                                          new SqlParameter("@locationId",appointmentSearchModel.Locations),
                                          new SqlParameter("@taxonomyId",appointmentSearchModel.Taxonomies),
                                          new SqlParameter("@specialityid",appointmentSearchModel.Specialities),
                                          new SqlParameter("@serviceId",appointmentSearchModel.Services),
                                          new SqlParameter("@rating",appointmentSearchModel.Rates),
                                          new SqlParameter("@gender",appointmentSearchModel.Gender),
                                          new SqlParameter("@SortColumn",appointmentSearchModel.sortColumn),
                                          new SqlParameter("@SortOrder",appointmentSearchModel.sortOrder),
                                          new SqlParameter("@PageNumber",appointmentSearchModel.pageNumber),
                                          new SqlParameter("@PageSize",appointmentSearchModel.pageSize),
            };

            var result = _context.GetProviderListToMakeAppointment(SQLObjects.GetSortedUrgentCareProviderListToBookAppointment, parameters.Length, parameters);
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
            #region Old Code


            //if (string.IsNullOrEmpty(appointmentSearchModel.sortColumn))
            //    appointmentSearchModel.sortColumn = "ProviderFirstName";

            //if (string.IsNullOrEmpty(appointmentSearchModel.sortOrder) || appointmentSearchModel.sortOrder == "asc")
            //    appointmentSearchModel.sortOrder = "true";
            //else
            //    appointmentSearchModel.sortOrder = "false";

            //var providers = (CommonMethods.OrderByField((from s in _context.Staffs
            //                                             join u in _context.User on s.UserID equals u.Id
            //                                             join st in _context.StaffTaxonomies on s.Id equals st.StaffID
            //                                             join gc in _context.GlobalCode on st.GlobalCodeId equals gc.Id
            //                                             join avail in _context.StaffAvailability on s.Id equals avail.StaffID
            //                                             join loc in _context.StaffLocation on s.Id equals loc.StaffId
            //                                             join country in _context.MasterCountry on s.CountryID equals country.Id
            //                                             join d in _context.MasterWeekDays on avail.DayId equals d.Id
            //                                             where u.OrganizationID == tokenModel.OrganizationID
            //                                             && (Convert.ToDateTime(appointmentSearchModel.Date)).ToString("dddd").Contains(d.Day) 
            //                                             && appointmentSearchModel.LocationIds.Contains(loc.LocationID.ToString())
            //                                             //&& appointmentSearchModel.Specialities.ToLower().Contains(gc.Id.ToString())
            //                                             && u.IsDeleted == false && s.IsDeleted == false && st.IsDeleted == false && avail.IsDeleted == false
            //                                             && u.IsActive == true & s.IsActive == true && st.IsActive == true && avail.IsActive == true
            //                                             orderby ("" + appointmentSearchModel.sortColumn + " " + appointmentSearchModel.sortOrder + "")
            //                                             select new AppointmentModel()
            //                                             {
            //                                                 ProviderId = CommonMethods.Encrypt(s.Id.ToString()),
            //                                                 ProviderFirstName = s.FirstName,
            //                                                 ProviderMiddleName = s.MiddleName,
            //                                                 ProviderLastName = s.LastName,
            //                                                 ProviderFullName = string.IsNullOrEmpty(s.MiddleName) ? (string.IsNullOrEmpty(s.LastName) ? s.FirstName : s.FirstName + " " + s.LastName) : s.FirstName + " " + s.MiddleName + " " + s.LastName,
            //                                                 PayRate = s.PayRate,
            //                                                 Address = s.Address,
            //                                                 Country = country.CountryName,
            //                                                 ProviderImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffPhotos, s.PhotoPath),
            //                                                 ProviderImageThumbnail = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, s.PhotoThumbnailPath),
            //                                                 Phone = s.PhoneNumber,
            //                                                 Email = s.Email,
            //                                                 Taxonomies = (
            //                                                 from stax in _context.StaffTaxonomies
            //                                                 join gc in _context.GlobalCode on stax.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where stax.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && stax.IsDeleted == false && stax.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentTaxonomyModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(stax.Id.ToString()),
            //                                                     Taxonomy = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Specialities = (
            //                                                 from ss in _context.StaffSpecialities
            //                                                 join gc in _context.GlobalCode on ss.GlobalCodeId equals gc.Id
            //                                                 join gcat in _context.GlobalCodeCategory on gc.GlobalCodeCategoryID equals gcat.Id
            //                                                 where ss.StaffID == s.Id && gc.IsDeleted == false && gc.IsActive == true
            //                                                 && ss.IsDeleted == false && ss.IsActive == true
            //                                                 orderby gc.GlobalCodeName
            //                                                 select new AppointmentSpecialitiesModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(ss.Id.ToString()),
            //                                                     Speciality = gc.GlobalCodeName
            //                                                 }
            //                                                 ).ToList(),
            //                                                 Experiences = (
            //                                                 from exp in _context.StaffExperiences
            //                                                 where exp.StaffId == s.Id
            //                                                 && exp.IsDeleted == false && exp.IsActive == true
            //                                                
            //                                                 select new AppointmentExperienceModel()
            //                                                 {
            //                                                     Id = CommonMethods.Encrypt(exp.Id.ToString()),
            //                                                     StartDate = exp.StartDate,
            //                                                     EndDate = exp.EndDate,
            //                                                     Organization = exp.OrganizationName,
            //                                                     TotalExperience = CommonMethods.getYearMonthDayBetweenDates(exp.StartDate, (DateTime)exp.EndDate)

            //                                                 }
            //                                                 ).ToList()
            //                                                 //InvitationSendDate = inv.InvitationSendDate != null ? CommonMethods.ConvertFromUtcTimeWithOffset(inv.InvitationSendDate, locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel) : inv.InvitationSendDate,
            //                                                 //InvitationStatus = ((Common.Enums.CommonEnum.UserInvitationStatus)inv.InvitationStatus).ToString()

            //                                             }).GroupBy(p => new { p.ProviderId }).Select(g => g.FirstOrDefault()),
            //                                          appointmentSearchModel.sortColumn, Convert.ToBoolean(appointmentSearchModel.sortOrder))).AsQueryable();
            ////var sql = CommonMethods.ToSql(providers);
            //var result = providers.Skip((appointmentSearchModel.pageNumber - 1) * appointmentSearchModel.pageSize).Take(appointmentSearchModel.pageSize).ToList();
            //result[0].TotalRecords = providers.ToList().Count();
            //return result;
            #endregion Old Code
        }

    }
}
