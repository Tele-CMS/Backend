using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Organizations;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Repositories.IRepositories.AdminDashboard;
using HC.Patient.Repositories.IRepositories.Appointment;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices.AdminDashboard;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.PatientAppointment;
using HC.Patient.Service.Token.Interfaces;
using HC.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.AdminDashboard
{
    public class AdminDashboardService : BaseService, IAdminDashboardService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAdminDashboardRepository _adminDashboardRepository;
        private readonly HCOrganizationContext _context;
        private readonly ILocationService _locationService;
        private readonly ITokenService _tokenService;
        private readonly IPatientAppointmentService _patientAppointmentService;
        private readonly IAppointmentPaymentRepository _appointmentPaymentRepository;
        private readonly IAppointmentPaymentRefundRepository _appointmentPaymentRefundRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IConfiguration _configuration;

        public AdminDashboardService(IPatientRepository patientRepository
            , IAdminDashboardRepository adminDashboardRepository
            , HCOrganizationContext context
            , ILocationService locationService
            , ITokenService tokenService
            , IPatientAppointmentService patientAppointmentService
            , IAppointmentPaymentRepository appointmentPaymentRepository
            , IAppointmentPaymentRefundRepository appointmentPaymentRefundRepository
            , IAppointmentRepository appointmentRepository
             , IConfiguration configuration
            )
        {
            _patientRepository = patientRepository;
            _adminDashboardRepository = adminDashboardRepository;
            _context = context;
            _locationService = locationService;
            _tokenService = tokenService;
            _patientAppointmentService = patientAppointmentService;
            _appointmentPaymentRepository = appointmentPaymentRepository;
            _appointmentPaymentRefundRepository = appointmentPaymentRefundRepository;
            _appointmentRepository = appointmentRepository;
            _configuration = configuration;
        }
        public JsonModel GetTotalClientCount(TokenModel token)
        {
            try
            {
                IQueryable<int> TotalCount = _patientRepository.GetAll(a => a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).Select(a => a.Id);
                OrganizationTotalClientsModel orgClientCount = new OrganizationTotalClientsModel();
                orgClientCount.TotalClientCount = TotalCount.Count();
                return new JsonModel()
                {
                    data = orgClientCount,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }
        public JsonModel GetTotalRevenue(TokenModel token)
        {
            try
            {
                OrganizationTotalRevenuModel TotalCount = _adminDashboardRepository.GetTotalRevenue<OrganizationTotalRevenuModel>(token).FirstOrDefault();
                return new JsonModel()
                {
                    data = TotalCount,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }
        public JsonModel GetOrganizationAuthorization(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token)
        {
            try
            {
                List<OrganizationAuthorizationModel> organizationAuthorizationModels = _adminDashboardRepository.GetOrganizationAuthorization<OrganizationAuthorizationModel>(pageNumber, pageSize, sortColumn, sortOrder, token).ToList();
                return new JsonModel()
                {
                    data = organizationAuthorizationModels,
                    Message = StatusMessage.FetchMessage,
                    meta = new Meta()
                    {
                        TotalRecords = organizationAuthorizationModels != null && organizationAuthorizationModels.Count > 0 ? organizationAuthorizationModels[0].TotalRecords : 0,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal((organizationAuthorizationModels != null && organizationAuthorizationModels.Count > 0 ? organizationAuthorizationModels[0].TotalRecords : 0) / pageSize))
                    },
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message

                };
            }
        }
        public JsonModel GetOrganizationEncounter(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token)
        {
            try
            {
                List<OrganizationEncounterModel> organizationEncounterModels = _adminDashboardRepository.GetOrganizationEncounter<OrganizationEncounterModel>(pageNumber, pageSize, sortColumn, sortOrder, token).ToList();
                if (organizationEncounterModels.Count > 0)
                    return new JsonModel()
                    {
                        data = organizationEncounterModels,
                        Message = StatusMessage.FetchMessage,
                        meta = new Meta()
                        {
                            TotalRecords = organizationEncounterModels[0].TotalRecords,
                            CurrentPage = pageNumber,
                            PageSize = pageSize,
                            DefaultPageSize = pageSize,
                            TotalPages = Math.Ceiling(Convert.ToDecimal(organizationEncounterModels[0].TotalRecords / pageSize))
                        },
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                else
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.NotFound,
                        StatusCode = (int)HttpStatusCodes.NotFound
                    };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message

                };
            }
        }

        public JsonModel GetStaffEncounter(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token)
        {
            try
            {
                List<OrganizationEncounterModel> organizationEncounterModels = _adminDashboardRepository.GetStaffEncounter<OrganizationEncounterModel>(pageNumber, pageSize, sortColumn, sortOrder, token).ToList();
                if (organizationEncounterModels != null && organizationEncounterModels.Count > 0)
                    return new JsonModel()
                    {
                        data = organizationEncounterModels,
                        Message = StatusMessage.FetchMessage,
                        meta = new Meta()
                        {
                            TotalRecords = organizationEncounterModels[0].TotalRecords,
                            CurrentPage = pageNumber,
                            PageSize = pageSize,
                            DefaultPageSize = pageSize,
                            TotalPages = Math.Ceiling(Convert.ToDecimal(organizationEncounterModels[0].TotalRecords / pageSize))
                        },
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                else
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.NotFound,
                        StatusCode = (int)HttpStatusCodes.NotFound
                    };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message

                };
            }
        }


        public JsonModel GetRegiesteredClientCount(TokenModel token)
        {
            try
            {
                List<OrganizationRegiesteredClientCountModel> orgRegClientCount = _adminDashboardRepository.GetRegiesteredClientCount<OrganizationRegiesteredClientCountModel>(token).ToList();
                if (orgRegClientCount.Count > 0)
                    return new JsonModel()
                    {
                        data = orgRegClientCount,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                else
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.NotFound,
                        StatusCode = (int)HttpStatusCodes.NotFound
                    };
            }
            catch (Exception)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }
        public JsonModel GetAdminDashboardData(TokenModel token)
        {
            try
            {
                OrganizationDashboardModel organizationDashoabrd = new OrganizationDashboardModel();

                #region  Total Client count
                IQueryable<int> TotalCount = _patientRepository.GetAll(a => a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).Select(a => a.Id);
                organizationDashoabrd.OrganizationTotalClients = TotalCount.Count();
                #endregion region

                #region Organization Total Revenu
                organizationDashoabrd.OrganizationTotalRevenu = _adminDashboardRepository.GetTotalRevenue<OrganizationTotalRevenuModel>(token).FirstOrDefault();
                #endregion

                //#region Organization Authorization
                //organizationDashoabrd.OrganizationAuthorization = _adminDashboardRepository.GetOrganizationAuthorization<OrganizationAuthorizationModel>(1, 10, "", "", token).ToList();
                //#endregion

                //#region Organization Encounters
                //organizationDashoabrd.OrganizationEncounter = _adminDashboardRepository.GetOrganizationEncounter<OrganizationEncounterModel>(1, 10, "", "", token).ToList();
                //#endregion

                #region Online Users
                //To-Do static values                 
                //organizationDashoabrd.OnlineUser = _context.User.Join(_context.Patients, U => U.Id, P => P.UserID, (U, P) => new { U, P }).Where(z => z.P.IsDeleted == false && z.P.IsActive == true && z.U.IsOnline == true).Select(x => x.U.Id).Count();
                organizationDashoabrd.OnlineUser = _context.User.Where(z => z.IsDeleted == false && z.IsActive == true && z.IsOnline == true && z.UserRoles.UserType.ToLower() != UserTypeEnum.ADMIN.ToString().ToLower()).Select(x => x.Id).Count();
                #endregion

                #region Get Client Status Chart
                organizationDashoabrd.ClientStatusChart = _adminDashboardRepository.GetClientStatusChart<ClientStatusChartModel>(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow, token).ToList();
                #endregion

                return new JsonModel()
                {
                    data = organizationDashoabrd,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {

                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message

                };
            }
        }



        #region Angular Code APIs

        public JsonModel GetClientStatusChart(TokenModel token)
        {
            try
            {
                List<ClientStatusChartModel> clientStatusChart = _adminDashboardRepository.GetClientStatusChart<ClientStatusChartModel>(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow, token).ToList();
                return new JsonModel()
                {
                    data = clientStatusChart,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }

        public JsonModel GetDashboardBasicInfo(TokenModel token)
        {
            try
            {
                OrganizationDashboardModel organizationDashoabrd = new OrganizationDashboardModel();
                var user = _tokenService.GetUserById(token);
                
                if (user != null)
                {
                    if (user.UserRoles.RoleName == OrganizationRoles.Admin.ToString())
                    {
                        #region  Total Client count
                        IQueryable<int> TotalCount = _patientRepository.GetAll(a => a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).Select(a => a.Id);
                        organizationDashoabrd.OrganizationTotalClients = TotalCount.Count();
                        #endregion region

                        #region Organization Total Revenu
                        organizationDashoabrd.OrganizationTotalRevenu = _adminDashboardRepository.GetTotalRevenue<OrganizationTotalRevenuModel>(token).FirstOrDefault();
                        #endregion

                        //#region Organization Authorization
                        //organizationDashoabrd.OrganizationAuthorization = _adminDashboardRepository.GetOrganizationAuthorization<OrganizationAuthorizationModel>(1, 10, "", "", token).ToList();
                        //#endregion

                        //#region Organization Encounters
                        //organizationDashoabrd.OrganizationEncounter = _adminDashboardRepository.GetOrganizationEncounter<OrganizationEncounterModel>(1, 10, "", "", token).ToList();
                        //#endregion

                        #region Online Users
                        //To-Do static values                 
                        //organizationDashoabrd.OnlineUser = _context.User.Join(_context.Patients, U => U.Id, P => P.UserID, (U, P) => new { U, P }).Where(z => z.P.IsDeleted == false && z.P.IsActive == true && z.U.IsOnline == true).Select(x => x.U.Id).Count();
                        //organizationDashoabrd.OnlineUser = _context.User.Where(z => z.IsDeleted == false && z.OrganizationID == token.OrganizationID && z.IsActive == true && z.IsOnline == true && z.UserRoles.UserType.ToLower() != UserTypeEnum.ADMIN.ToString().ToLower()).Select(x => x.Id).Count();
                        #endregion
                        organizationDashoabrd.AppointmentTotalRevenue = _appointmentPaymentRepository.GetTotalAppointmentRevenue<AppointmentTotalRevenue>(token).FirstOrDefault();

                        organizationDashoabrd.AppointmentTotalRefund = _appointmentPaymentRefundRepository.GetTotalAppointmentRefund<AppointmentTotalRefund>(token).FirstOrDefault();

                        organizationDashoabrd.AppointmentNetRevenue = organizationDashoabrd.AppointmentTotalRevenue.TotalRevenue - organizationDashoabrd.AppointmentTotalRefund.TotalRevenue;

                        organizationDashoabrd.TodayAppointments = 0;
                        JsonModel appResponse = _patientAppointmentService.GetPatientAppointmentListForDashboard("", "", "", (DateTime?)DateTime.UtcNow, (DateTime?)DateTime.UtcNow, "", "", token);
                        if (appResponse.StatusCode == (int)HttpStatusCodes.OK)
                        {
                            var appData = ((List<PatientAppointmentModel>)appResponse.data).Where(x => x.StatusName.ToLower() == AppointmentStatus.APPROVED.ToLower());
                            organizationDashoabrd.TodayAppointments = appData.Count();
                        }

                        organizationDashoabrd.TotalAppointments = 0;
                        JsonModel appResponseTotal = _patientAppointmentService.GetPatientAppointmentListForDashboard("", "", "", null, null, "", "", token);
                        if (appResponseTotal.StatusCode == (int)HttpStatusCodes.OK)
                        {
                            var appData = ((List<PatientAppointmentModel>)appResponseTotal.data).Where(x => x.StatusName.ToLower() == AppointmentStatus.APPROVED.ToLower());
                            organizationDashoabrd.TotalAppointments = appData.Count();
                        }

                        //#region Get Client Status Chart
                        //organizationDashoabrd.ClientStatusChart = _adminDashboardRepository.GetClientStatusChart<ClientStatusChartModel>(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow, token).ToList();
                        //#endregion
                    }
                    else
                    {
                        #region  Total Client count
                        IQueryable<int> TotalCount = _patientRepository.GetAll(a => a.UserID == token.UserID && a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).Select(a => a.Id);
                        organizationDashoabrd.OrganizationTotalClients = TotalCount.Count();
                        #endregion region

                        #region Organization Total Revenu
                        organizationDashoabrd.OrganizationTotalRevenu = _adminDashboardRepository.GetTotalRevenue<OrganizationTotalRevenuModel>(token).FirstOrDefault();
                        #endregion

                        //#region Organization Authorization
                        //organizationDashoabrd.OrganizationAuthorization = _adminDashboardRepository.GetOrganizationAuthorization<OrganizationAuthorizationModel>(1, 10, "", "", token).ToList();
                        //#endregion

                        //#region Organization Encounters
                        //organizationDashoabrd.OrganizationEncounter = _adminDashboardRepository.GetOrganizationEncounter<OrganizationEncounterModel>(1, 10, "", "", token).ToList();
                        //#endregion

                        #region Online Users
                        //To-Do static values                 
                        //organizationDashoabrd.OnlineUser = _context.User.Join(_context.Patients, U => U.Id, P => P.UserID, (U, P) => new { U, P }).Where(z => z.P.IsDeleted == false && z.P.IsActive == true && z.U.IsOnline == true).Select(x => x.U.Id).Count();
                        organizationDashoabrd.OnlineUser = _context.User.Where(z => z.IsDeleted == false && z.OrganizationID == token.OrganizationID && z.IsActive == true && z.IsOnline == true && z.UserRoles.UserType.ToLower() != UserTypeEnum.ADMIN.ToString().ToLower()).Select(x => x.Id).Count();
                        #endregion
                        organizationDashoabrd.AppointmentTotalRevenue = _appointmentPaymentRepository.GetTotalAppointmentRevenue<AppointmentTotalRevenue>(token, token.StaffID).FirstOrDefault();
                        organizationDashoabrd.AppointmentTotalRefund = _appointmentPaymentRefundRepository.GetTotalAppointmentRefund<AppointmentTotalRefund>(token, token.StaffID).FirstOrDefault();

                        PaymentFilterModel paymentFilterModel = new PaymentFilterModel();
                        DateTime date = DateTime.Now;
                        var startdate = new DateTime(date.Year, date.Month -1, 1);
                        var enddate= startdate.AddMonths(1).AddDays(-1);
                        paymentFilterModel.RangeStartDate= startdate.ToString();
                        paymentFilterModel.RangeEndDate = enddate.ToString();
                        paymentFilterModel.StaffId = (token.StaffID).ToString();
                        var appointmentPayments = _appointmentPaymentRepository.GetAppointmentPayments<AppointmentPaymentListingModel>(paymentFilterModel, token).ToList();
                        organizationDashoabrd.PreviousMonthAppointmentsAmount = appointmentPayments[0].TotalNetAmount;
                        organizationDashoabrd.AppointmentNetRevenue = organizationDashoabrd.AppointmentTotalRevenue.TotalRevenue - organizationDashoabrd.AppointmentTotalRefund.TotalRevenue;
                        organizationDashoabrd.TodayAppointments = 0;
                        //JsonModel appResponse = _patientAppointmentService.GetPatientAppointmentListForDashboard(token.LocationID.ToString(), token.StaffID.ToString(), "", (DateTime?)DateTime.UtcNow, (DateTime?)DateTime.UtcNow, "", "", token);
                        //if (appResponse.StatusCode == (int)HttpStatusCodes.OK)
                        //{
                        //    var appData = ((List<PatientAppointmentModel>)appResponse.data).Where(x => x.StatusName.ToLower() == AppointmentStatus.APPROVED.ToLower() && x.StartDateTime.Month == DateTime.Now.Month - 1);
                        //    organizationDashoabrd.TodayAppointments = appData.Count();
                        //}

                        organizationDashoabrd.TotalAppointments = 0;
                        JsonModel appResponseTotal = _patientAppointmentService.GetPatientAppointmentListForDashboard(token.LocationID.ToString(), token.StaffID.ToString(), "", null, null, "", "", token);
                        if (appResponseTotal.StatusCode == (int)HttpStatusCodes.OK)
                        {
                            var appData = ((List<PatientAppointmentModel>)appResponseTotal.data).Where(x => x.StatusName.ToLower() == AppointmentStatus.APPROVED.ToLower() && x.StartDateTime.Month == DateTime.Now.Month);
                            organizationDashoabrd.TotalAppointments = appData.Count();
                            organizationDashoabrd.ThisMonthAppointmentsAmount = appData.Sum(x => x.PaymentAmount);

                            appData = ((List<PatientAppointmentModel>)appResponseTotal.data).Where(x => x.StatusName.ToLower() == AppointmentStatus.APPROVED.ToLower() && x.StartDateTime.Month == DateTime.Now.Month - 1);
                            organizationDashoabrd.TodayAppointments = appData.Count();
                            //organizationDashoabrd.PreviousMonthAppointmentsAmount = appData.Sum(x => x.PaymentAmount);

                            appData = ((List<PatientAppointmentModel>)appResponseTotal.data).Where(x => x.StatusName.ToLower() == AppointmentStatus.APPROVED.ToLower());
                            organizationDashoabrd.TotalAppointmentsCount = appData.Count();
                            organizationDashoabrd.TotalAppointmentsAmount = appData.Sum(x => x.PaymentAmount);
                            appData = ((List<PatientAppointmentModel>)appResponseTotal.data).Where(x => x.StatusName.ToLower() == AppointmentStatus.APPROVED.ToLower() && x.StartDateTime.Date == DateTime.Now.Date);
                            organizationDashoabrd.TodayAppointmentsCount = appData.Count();
                            organizationDashoabrd.TodaysAppointmentsAmount = appData.Sum(x => x.PaymentAmount);
                        }
                        //#region Get Client Status Chart
                        //organizationDashoabrd.ClientStatusChart = _adminDashboardRepository.GetClientStatusChart<ClientStatusChartModel>(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow, token).ToList();
                        //#endregion
                    }
                }


                return new JsonModel()
                {
                    data = organizationDashoabrd,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {

                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message

                };
            }
        }
        #endregion
        public JsonModel GetAppointmentsDataForGraph(AppointmentDataFilterModel appointmentDataFilterModel, TokenModel token)
        {
            List<AppointmentOnDashboardDataModel> appointmentOnDashboardDataModel = _adminDashboardRepository.GetAppointmentsDataForGraph<AppointmentOnDashboardDataModel>(appointmentDataFilterModel, token).ToList();
            if (!ReferenceEquals(appointmentOnDashboardDataModel, null))
            {
                return new JsonModel()
                {
                    data = appointmentOnDashboardDataModel,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
        }
        public JsonModel GetOrganizationAppointments(FilterModel filterModel, string locationIds, string staffIds, string patientIds, DateTime fromDate, DateTime toDate, string statusName, TokenModel token)
        {
            List<PatientAppointmentModel> list = new List<PatientAppointmentModel>();
            //list = _appointmentRepository.GetAppointmentList<PatientAppointmentModel>(locationIds, staffIds, patientIds, fromDate, toDate, null ,null, token.OrganizationID).ToList();
            list = _appointmentRepository.GetAppointmentListForClientDashboard<PatientAppointmentModel>(filterModel, fromDate, toDate, locationIds, staffIds, patientIds, string.Empty, statusName, token).ToList();
            //if (fromDate != null && toDate != null && fromDate == toDate)
            //{
            //    DateTime today = DateTime.UtcNow;
            //    list = list.Where(x => x.StartDateTime >= today).ToList();
            //}
            var hostingServer = _configuration.GetSection("HostingServer").Value;
            list.ForEach(x =>
            {
                LocationModel locationModal = _locationService.GetLocationOffsets(x.ServiceLocationID, token);

                x.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);
                x.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, token);

                var result = _context.SymptomatePatientReport.Where(s => s.PatientID == x.PatientAppointmentId).Select(s => s.Id).FirstOrDefault();
                if (result != 0)
                {
                    x.IsSymptomateReportExist = true;
                    x.ReportId = result;
                }

                if (!string.IsNullOrEmpty(x.PatientPhotoThumbnailPath)) { x.PatientPhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, x.PatientPhotoThumbnailPath); }
                x.AppointmentStaffs = !string.IsNullOrEmpty(x.XmlString) ? XDocument.Parse(x.XmlString).Descendants("Child").Select(y => new AppointmentStaffs()
                {
                    StaffId = Convert.ToInt32(y.Element("StaffId").Value),
                    StaffName = y.Element("StaffName").Value,
                    Address = y.Element("StaffAddress") == null ? "" : (y.Element("StaffAddress").Value == null ? "" : y.Element("StaffAddress").Value),
                    PhotoThumbnail = y.Element("StaffPhotoThumbnailPath") == null ? "" : (string.IsNullOrEmpty((y.Element("StaffPhotoThumbnailPath").Value)) ? "" : hostingServer + "/Images/StaffPhotos/thumb/" + y.Element("StaffPhotoThumbnailPath").Value)
                }).ToList() : new List<AppointmentStaffs>(); x.XmlString = null;
            });

            return new JsonModel()
            {
                data = list,
                Message = StatusMessage.FetchMessage,
                meta = new Meta(list, filterModel),
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

    }
}
