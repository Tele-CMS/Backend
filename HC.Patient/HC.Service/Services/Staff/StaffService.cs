using HC.Common;
using HC.Common.HC.Common;
using HC.Common.Model.Staff;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Payment;
using HC.Patient.Model.Staff;
using HC.Patient.Model.Users;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Service.IServices.Images;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.User;
using HC.Service;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Patient
{
    public class StaffService : BaseService, IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IImageService _imageService;
        private readonly IUserRepository _userRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        private readonly HCOrganizationContext _context;
        private readonly IStaffTagRepository _staffTagRepository;
        private readonly IUserTimesheetByAppointmentTypeRepository _userTimesheetByAppointmentTypeRepository;
        private readonly IUserDetailedDriveTimeRepository _userDetailedDriveTimeRepository;
        private readonly IUserPasswordHistoryService _userPasswordHistoryService;
        private JsonModel response = new JsonModel();
        private readonly IStaffSpecialityRepository _staffSpecialityRepository;
        private readonly IStaffTaxonomyRepository _staffTaxonomyRepository;
        private readonly IStaffServicesRepository _staffServicesRepository;
        private readonly IStaffCareCategoryRepository _staffCareCategoryRepository;
        public StaffService(
            IStaffRepository staffRepository,
            IImageService imageService,
            IUserRepository userRepository,
            IUserCommonRepository userCommonRepository,
            IStaffTagRepository staffTagRepository,
            HCOrganizationContext context,
            IUserTimesheetByAppointmentTypeRepository userTimesheetByAppointmentTypeRepository,
            IUserDetailedDriveTimeRepository userDetailedDriveTimeRepository,
            IUserPasswordHistoryService userPasswordHistoryService,
            IStaffSpecialityRepository staffSpecialityRepository,
            IStaffTaxonomyRepository staffTaxonomyRepository,
            IStaffServicesRepository staffServicesRepository,
            IStaffCareCategoryRepository staffCareCategoryRepository
            )
        {
            _staffRepository = staffRepository;
            _imageService = imageService;
            _userRepository = userRepository;
            _userCommonRepository = userCommonRepository;
            _staffTagRepository = staffTagRepository;
            _context = context;
            _userTimesheetByAppointmentTypeRepository = userTimesheetByAppointmentTypeRepository;
            _userDetailedDriveTimeRepository = userDetailedDriveTimeRepository;
            _userPasswordHistoryService = userPasswordHistoryService;
            _staffSpecialityRepository = staffSpecialityRepository;
            _staffTaxonomyRepository = staffTaxonomyRepository;
            _staffServicesRepository = staffServicesRepository;
            _staffCareCategoryRepository = staffCareCategoryRepository;
        }

        public JsonModel GetStaffByTags(ListingFiltterModel listingFiltterModel, TokenModel tokenModel)
        {
            try
            {
                List<StaffModel> staffModels = _staffRepository.GetStaffByTags<StaffModel>(listingFiltterModel, tokenModel).ToList();
                if (staffModels != null && staffModels.Count > 0)
                {
                    staffModels.ForEach(a =>
                    {
                        if (!string.IsNullOrEmpty(a.PhotoThumbnailPath))
                        {
                            a.PhotoThumbnailPath = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, a.PhotoThumbnailPath);
                        }
                    });
                    response = new JsonModel(staffModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                }
                else { response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound); }
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCode.InternalServerError, e.Message);
            }
            return response;
        }

        public JsonModel GetStaffs(ListingFiltterModel listingFiltterModel, TokenModel token)
        {
            try
            {
                //not found
                response.data = new object();
                response.Message = StatusMessage.NotFound;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                /////

                List<StaffModels> staffModels = _staffRepository.GetStaff<StaffModels>(listingFiltterModel, token).ToList();
                if (staffModels != null && staffModels.Count > 0)
                {
                    response.data = staffModels;
                    response.Message = StatusMessage.FetchMessage;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.meta = new Meta(staffModels, listingFiltterModel);
                }
            }
            catch (Exception e)
            {
                //not error
                response.data = new object();
                response.Message = StatusMessage.ErrorOccured;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.AppError = e.Message;
                /////
            }
            return response;
        }

        public JsonModel CreateUpdateStaff(Staffs staff, TokenModel token)
        {
           // using (var transaction = _context.Database.BeginTransaction())
            {
                DateTime Currentdate = DateTime.UtcNow;
                try
                {
                    //encrypt password
                    if (staff!=null && !string.IsNullOrEmpty(staff.Password)) { staff.Password = CommonMethods.Encrypt(staff.Password); }
                    //

                    if (staff.Id == 0) //new case
                    {
                        //check duplicate staff
                        Staffs staffDB = _staffRepository.Get(m => (m.Email == staff.Email || m.UserName == staff.UserName) && m.OrganizationID == token.OrganizationID);
                        if (staffDB != null) //if user try to enter duplicate records
                        {
                            response = new JsonModel(new object(), StatusMessage.StaffAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                        }
                        else // insert new staff
                        {
                            staff.OrganizationID = token.OrganizationID;
                            staff.CreatedBy = token.UserID;
                            staff.CreatedDate = Currentdate;
                            staff.IsActive = true;
                            staff.IsDeleted = false;
                            _imageService.ConvertBase64ToImageForUser(staff);

                            //Save User
                            Entity.User requestUser = SaveUser(staff, token, Currentdate);
                            staff.UserID = requestUser.Id;

                            //Map Staff Entity
                            MapStaffEntity(staff, token, Currentdate);

                            //save staff
                            _context.Staffs.Add(staff);
                            _context.SaveChanges();
                            response = new JsonModel(staff, StatusMessage.StaffCreated, (int)HttpStatusCode.OK);
                        }
                    }
                    else
                    {
                        Staffs staffDB = _context.Staffs.Where(a => a.Id == staff.Id).FirstOrDefault();

                        if (staffDB != null)
                        {
                            Entity.User user = _context.User.Where(a => a.Id == staffDB.UserID).FirstOrDefault();
                            bool isPwdUpdated = CommonMethods.Decrypt(user.Password) == CommonMethods.Decrypt(staff.Password) ? false : true;
                            List<UserPasswordHistoryModel> passwordHistory = _userPasswordHistoryService.GetUserPasswordHistory(staff.UserID);
                            if (isPwdUpdated == true && passwordHistory != null && passwordHistory.Count > 0 && passwordHistory.FindAll(x => x.Password == CommonMethods.Decrypt(staff.Password)).Count() > 0)
                            {
                              //  transaction.Rollback();
                                return new JsonModel(null, UserAccountNotification.PasswordMatch, (int)HttpStatusCodes.UnprocessedEntity, string.Empty);
                            }

                            staffDB = MapStaff(staff, staffDB);

                            //remove staff location
                            List<StaffLocation> staffLocationList = _context.StaffLocation.Where(a => a.StaffId == staffDB.Id).ToList();
                            _context.StaffLocation.RemoveRange(staffLocationList);

                            user.Password = staffDB.Password;
                            user.RoleID = staffDB.RoleID;
                            if (isPwdUpdated)
                                user.PasswordResetDate = DateTime.UtcNow;
                            _context.User.Update(user);

                            //Map Staff Location
                            MapStaffLoaction(staffDB);


                            ///////update staff team////////////////////
                            if (staffDB.StaffTeamList != null && staffDB.StaffTeamList.Count() > 0)
                            { UpdateStaffTeam(staffDB, token, Currentdate); }

                            ///////update staff tag////////////////////
                            if (staffDB.StaffTagsModel != null && staffDB.StaffTagsModel.Count() > 0)
                            { UpdateStaffTags(staffDB, token, Currentdate); }

                            ///////update staff Speciality////////////////////
                            if (staffDB.StaffSpecialityModel != null && staffDB.StaffSpecialityModel.Count() > 0)
                            { UpdateStaffSpeciality(staffDB, token, Currentdate); }

                            ///////update staff CareCategory////////////////////
                            if (staffDB.StaffCareCategoryModel != null && staffDB.StaffCareCategoryModel.Count() > 0)
                            { UpdateStaffCarecategory(staffDB, token, Currentdate); }

                            ///////update staff Taxonomy////////////////////
                            if (staffDB.StaffTaxonomyModel != null && staffDB.StaffTaxonomyModel.Count() > 0)
                            { UpdateStaffTaxonomy(staffDB, token, Currentdate); }

                            ///////update staff services////////////////////
                            if (staffDB.StaffServicesModels != null && staffDB.StaffServicesModels.Count() > 0)
                            { UpdateStaffServices(staffDB, token, Currentdate); }

                            _context.Staffs.Update(staffDB);
                            _context.SaveChanges();
                            if (isPwdUpdated)
                                _userPasswordHistoryService.SaveUserPasswordHistory(staffDB.UserID, DateTime.UtcNow, staffDB.Password);
                            response = new JsonModel(staffDB, StatusMessage.StaffUpdated, (int)HttpStatusCode.OK);
                        }
                        else
                        {
                            response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
                        }
                    }
                   // transaction.Commit();
                }
                catch (Exception e)
                {
                  //  transaction.Rollback();
                    response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCode.InternalServerError, e.Message);
                }
                return response;
            }
        }

        private Staffs MapStaff(Staffs staff, Staffs staffDB)
        {
            staffDB.FirstName = staff.FirstName;
            staffDB.LastName = staff.LastName;
            staffDB.MiddleName = staff.MiddleName;
            staffDB.Address = staff.Address;
            staffDB.CountryID = staff.CountryID;
            staffDB.City = staff.City;
            staffDB.StateID = staff.StateID;
            staffDB.Zip = staff.Zip;
            staffDB.Latitude = staff.Latitude;
            staffDB.Longitude = staff.Longitude;
            staffDB.PhoneNumber = staff.PhoneNumber;
            staffDB.NPINumber = staff.NPINumber;
            staffDB.TaxId = staff.TaxId;
            staffDB.DOB = staff.DOB;
            staffDB.DOJ = staff.DOJ;
            staffDB.RoleID = staff.RoleID;
            staffDB.Email = staff.Email;
            staffDB.Gender = staff.Gender;
            staffDB.ApartmentNumber = staff.ApartmentNumber;
            staffDB.CAQHID = staff.CAQHID;
            staffDB.Language = staff.Language;
            staffDB.DegreeID = staff.DegreeID;
            staffDB.EmployeeID = staff.EmployeeID;
            staffDB.Password = staff.Password;
            staffDB.PhotoBase64 = staff.PhotoBase64;
            staffDB.StaffTeamList = staff.StaffTeamList;
            staffDB.StaffLocationList = staff.StaffLocationList;
            staffDB.StaffTagsModel = staff.StaffTagsModel;
            staffDB.StaffSpecialityModel = staff.StaffSpecialityModel;
            staffDB.StaffCareCategoryModel = staff.StaffCareCategoryModel;
            staffDB.StaffTaxonomyModel = staff.StaffTaxonomyModel;
            staffDB.StaffServicesModels = staff.StaffServicesModels;
            //staffDB.PayRate = staff.PayRate;
            //staffDB.FTFpayRate = staff.FTFpayRate;
            staffDB.PayrollGroupID = staff.PayrollGroupID;
            staffDB.IsRenderingProvider = staff.IsRenderingProvider;
            staffDB.IsUrgentCare = staff.IsUrgentCare;
            staffDB.AboutMe = staff.AboutMe;
            if (!string.IsNullOrEmpty(staffDB.PhotoBase64))
            {
                staffDB = _imageService.ConvertBase64ToImageForUser(staffDB);
            }

            return staffDB;
        }
        public Staffs GetStaffByUserId(int userid, TokenModel token)
        {
            return _context.Staffs.Where(a => a.UserID == userid && a.IsDeleted == false)
                                        .FirstOrDefault();
        }
        public JsonModel GetStaffById(int id, TokenModel token)
        {
            try
            {
                Staffs staffs = _context.Staffs
                                        .Include(z => z.StaffTeam)
                                        .Include(z => z.StaffLocation)
                                        .Include(z => z.StaffTags)
                                        .Include(z => z.MasterGender)
                                        .Include(z => z.UserRoles)
                                        .Include(z => z.StaffSpecialities)
                                        .Include(z => z.StaffCareCategories)
                                        .Include(z => z.StaffTaxonomies)
                                        .Include(z => z.StaffAwards)
                                        .Include(z => z.StaffExperiences)
                                        .Include(z => z.StaffQualifications)
                                        .Include(z => z.StaffServices)
                                        .Include(z => z.ProviderCancellationRules)
                                        .Where(a => a.Id == id && a.IsActive == true && a.IsDeleted == false)
                                        .FirstOrDefault();

                if (staffs != null)
                {
                    // with include we can only include the records, but can't execute any condition so condition are separately perform
                    staffs.StaffTeamList = staffs.StaffTeam.Where(z => z.IsDeleted == false).Select(a => new StaffTeamModel { id = a.Id, staffid = a.StaffId, staffteamid = a.StaffTeamID, isdeleted = a.IsDeleted }).ToList();
                    staffs.StaffTeam = null;
                    staffs.StaffLocationList = staffs.StaffLocation.Select(z => new StaffLocationModel { Id = z.LocationID, IsDefault = z.IsDefault }).ToList();
                    staffs.StaffLocation = null;
                    staffs.StaffTagsModel = staffs.StaffTags.Where(a => a.IsDeleted == false).Select(x => new StaffTagsModel { Id = x.Id, IsDeleted = x.IsDeleted, StaffID = x.StaffID, TagID = x.TagID }).ToList();
                    staffs.StaffTags = null;
                    staffs.GenderName = staffs.MasterGender != null ? staffs.MasterGender.Gender : "";
                    staffs.MasterGender = null;
                    staffs.RoleName = staffs.UserRoles != null ? staffs.UserRoles.RoleName : "";
                    staffs.UserRoles = null;
                    //
                    staffs.StaffSpecialityModel = staffs.StaffSpecialities
                         .Join(_context.GlobalCode,
                        st => st.GlobalCodeId,
                        gc => gc.Id,
                        (st, gc) => new { st, gc })
                        .Join(_context.GlobalCodeCategory,
                        comb => comb.gc.GlobalCodeCategoryID,
                        gcc => gcc.Id,
                        (comb, gcc) => new { comb })
                        .Where(z => z.comb.st.IsDeleted == false).Select(a => new StaffSpecialityModel { Id = a.comb.st.Id, StaffID = a.comb.st.StaffID, SpecialityId = a.comb.st.GlobalCodeId, IsDeleted = a.comb.st.IsDeleted, Speciality = a.comb.gc.GlobalCodeName }).Distinct().ToList();
                    staffs.StaffSpecialities = null;

                    staffs.StaffTaxonomyModel = staffs.StaffTaxonomies
                        .Join(_context.GlobalCode,
                        st => st.GlobalCodeId,
                        gc => gc.Id,
                        (st, gc) => new { st, gc })
                        .Join(_context.GlobalCodeCategory,
                        comb => comb.gc.GlobalCodeCategoryID,
                        gcc => gcc.Id,
                        (comb, gcc) => new { comb })
                        .Where(z => z.comb.st.IsDeleted == false).Select(a => new StaffTaxonomyModel { Id = a.comb.st.Id, StaffID = a.comb.st.StaffID, TaxonomyId = a.comb.st.GlobalCodeId, IsDeleted = a.comb.st.IsDeleted, TaxonomyName = a.comb.gc.GlobalCodeName, TaxonomyValue = a.comb.gc.GlobalCodeValue }).Distinct().ToList();
                    staffs.StaffTaxonomies = null;

                    staffs.StaffServicesModels = staffs.StaffServices
                        .Join(_context.MasterServices,
                        st => st.ServiceId,
                        ms => ms.Id,
                        (st, ms) => new { st, ms })
                        .Where(z => z.st.IsDeleted == false).Select(a => new StaffServicesModel { Id = CommonMethods.Encrypt(a.st.Id.ToString()), StaffId = CommonMethods.Encrypt(a.st.StaffId.ToString()), ServiceId = a.ms.Id.ToString(), IsDeleted = a.st.IsDeleted, ServiceName = a.ms.ServiceName }).Distinct().ToList();
                    staffs.StaffServices = null;

                    staffs.StaffCareCategoryModel = staffs.StaffCareCategories.Where(z => z.StaffID == id && z.IsDeleted == false && z.IsActive == true).Select(a => new StaffCareCategoryModel { Id = a.Id, StaffID = a.StaffID, healthcarecategoryID = a.CareCategoryId, IsDeleted = a.IsDeleted }).Distinct().ToList();
                    staffs.StaffCareCategories = null;

                    #region Profile
                    staffs.StaffQualificationModels = staffs.StaffQualifications.Where(z => z.IsDeleted == false).Select(a => new StaffQualificationModel { Id = CommonMethods.Encrypt(a.Id.ToString()), Course = a.Course, University = a.University, StartDate = a.StartDate.ToString(), EndDate = a.EndDate.ToString(), IsDeleted = a.IsDeleted }).ToList();
                    staffs.StaffQualifications = null;

                    staffs.StaffAwardModels = staffs.StaffAwards.Where(z => z.IsDeleted == false).Select(a => new StaffAwardModel { Id = CommonMethods.Encrypt(a.Id.ToString()), AwardType = a.AwardType, AwardDate = a.AwardDate.ToString(), Description = a.Description, IsDeleted = a.IsDeleted }).ToList();
                    staffs.StaffAwards = null;

                    staffs.StaffExperienceModels = staffs.StaffExperiences.Where(z => z.IsDeleted == false).Select(a => new StaffExperienceModel { Id = CommonMethods.Encrypt(a.Id.ToString()), OrganizationName = a.OrganizationName, StartDate = a.StartDate.ToString(), EndDate = a.EndDate.ToString(), TotalExperience = CommonMethods.getYearMonthDayBetweenDates(a.StartDate, (DateTime)a.EndDate), IsDeleted = a.IsDeleted }).ToList();
                    staffs.StaffExperiences = null;
                    #endregion

                    if (staffs != null && !string.IsNullOrEmpty(staffs.PhotoPath) && !string.IsNullOrEmpty(staffs.PhotoThumbnailPath))
                    {
                        staffs.PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffPhotos, staffs.PhotoPath);
                        staffs.PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, staffs.PhotoThumbnailPath);
                    }

                    HC.Patient.Entity.User user = _context.User
                                   .Where(a => a.Id == staffs.UserID && a.IsActive == true && a.IsDeleted == false)
                                   .FirstOrDefault();

                    if (!string.IsNullOrEmpty(user.UserName)) { staffs.UserName = user.UserName; }
                    //Decrypt password                    
                    if (!string.IsNullOrEmpty(user.Password)) { staffs.Password = CommonMethods.Decrypt(user.Password); }
                    //

                    response = new JsonModel(staffs, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
                }
                else
                {
                    response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
                }
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, e.Message);
            }
            return response;
        }

        public JsonModel DeleteStaff(int id, TokenModel token)
        {
            try
            {
                List<RecordDependenciesModel> recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.Staffs, true, token).ToList();
                if (recordDependenciesModel != null && recordDependenciesModel.Exists(a => a.TotalCount > 0))
                { response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized); }
                else
                {
                    Staffs staff = _staffRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
                    if (staff != null)
                    {
                        staff.IsDeleted = true;
                        staff.DeletedBy = token.UserID;
                        staff.UpdatedBy = token.UserID;
                        staff.DeletedDate = DateTime.UtcNow;
                        _staffRepository.Update(staff);
                        _staffRepository.SaveChanges();
                        response = new JsonModel(new object(), StatusMessage.StaffDelete, (int)HttpStatusCodes.OK);
                    }
                    else { response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound); }
                }
            }
            catch (Exception e)
            { response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCode.InternalServerError, e.Message); }
            return response;
        }

        public JsonModel UpdateStaffActiveStatus(int staffId, bool isActive, TokenModel token)
        {
            try
            {
                bool status = _userCommonRepository.UpdateStaffActiveStatus(staffId, isActive);
                if (isActive)
                {
                    response = new JsonModel(status, StatusMessage.UserActivation, (int)HttpStatusCodes.OK);
                }
                else
                {
                    response = new JsonModel(status, StatusMessage.UserDeactivation, (int)HttpStatusCodes.OK);
                }
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, e.Message);
            }
            return response;
        }

        public JsonModel GetStaffFeeSettings(string id, TokenModel token)
        {
            try
            {
                int staffId = 0;
                bool decripted = int.TryParse(id, out staffId);
                if (!decripted)
                {
                    Int32.TryParse(Common.CommonMethods.Decrypt(id.Replace(" ", "+")), out staffId);
                }
                Staffs staffs = _context.Staffs.Include(s => s.ProviderCancellationRules).FirstOrDefault(x => x.Id == staffId);
                var feesSettings = new ManageFeesRefundsModel()
                {
                    CancelationRules = staffs.ProviderCancellationRules != null ? staffs.ProviderCancellationRules.Select(x => new CancelationRuleModel() {
                        RefundPercentage = x.RefundPercentage,
                        UptoHours = x.UptoHour
                    }).ToList() : new List<CancelationRuleModel>(),
                    F2fFee = staffs.FTFpayRate,
                    FolowupDays = staffs.FollowUpDays,
                    FolowupFees = staffs.FollowUpPayRate,
                    NewOnlineFee = staffs.PayRate,
                    Providers = new List<int>() { staffId}
                };
                response = new JsonModel(staffs, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, e.Message);
            }

            return response;
        }


        public JsonModel UpdateProviderUrgentCareStatus(int staffId, bool isUrgentCare, TokenModel token)
        {
            try
            {
                
                Staffs staffs = _context.Staffs.Where(x => x.Id == staffId && x.IsDeleted == false && x.IsActive == true).FirstOrDefault();
                staffs.IsUrgentCare = isUrgentCare;
                staffs.UpdatedBy = token.UserID;
                staffs.UpdatedDate = DateTime.UtcNow;
                _staffRepository.Update(staffs);
                _staffRepository.SaveChanges();
                response = new JsonModel(staffs, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, e.Message);
            }

            return response;
        }

        public JsonModel UpdateProviderTimeInterval(string id, TokenModel token)
        {
            try
            {
                int timeinterval = 0;
                bool decripted = int.TryParse(id, out timeinterval);
                if (!decripted)
                {
                    Int32.TryParse(Common.CommonMethods.Decrypt(id.Replace(" ", "+")), out timeinterval);
                }
                Staffs staffs = _context.Staffs.Where(x => x.Id == token.StaffID && x.IsDeleted == false && x.IsActive == true).FirstOrDefault();
                staffs.TimeInterval = timeinterval;
                staffs.UpdatedBy = token.UserID;
                staffs.UpdatedDate = DateTime.UtcNow;
                _staffRepository.Update(staffs);
                _staffRepository.SaveChanges();
                response = new JsonModel(staffs, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, e.Message);
            }

            return response;
        }

        public JsonModel GetCancelationRules(List<int> staffIds)
        {
            var data = _staffRepository.GetCancelationRules(staffIds);
            return new JsonModel(data.ToList(), StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
        }


        #region Helping Method
        private Entity.User SaveUser(Staffs entity, TokenModel token, DateTime Currentdate)
        {
            Entity.User requestUser = new Entity.User();
            requestUser.UserName = entity.UserName;
            requestUser.Password = entity.Password;
            requestUser.RoleID = entity.RoleID; // TO DO it will be dynamic
            requestUser.CreatedBy = token.UserID;
            requestUser.CreatedDate = Currentdate;
            requestUser.IsActive = true;
            requestUser.IsDeleted = false;
            requestUser.OrganizationID = entity.OrganizationID;
            requestUser.PasswordResetDate = DateTime.UtcNow;
            _userRepository.Create(requestUser);
            _userRepository.SaveChanges();
            return requestUser;
        }
        private void MapStaffEntity(Staffs entity, TokenModel token, DateTime Currentdate)
        {
            MapStaffLoaction(entity);

            if (entity.StaffTeamList != null && entity.StaffTeamList.Count() > 0)
            {
                #region Map Staff Team
                List<StaffTeam> staffTeamList = new List<StaffTeam>();
                foreach (StaffTeamModel staffteam in entity.StaffTeamList)
                {
                    StaffTeam team = new StaffTeam();
                    //team.StaffId = savedStaff.Id;
                    team.StaffTeamID = staffteam.staffteamid;
                    team.CreatedBy = token.UserID;
                    team.CreatedDate = Currentdate;
                    staffTeamList.Add(team);
                }
                entity.StaffTeam = staffTeamList;
                #endregion
            }

            if (entity.StaffTagsModel != null && entity.StaffTagsModel.Count > 0)
            {

                #region Map Staff Tags
                List<StaffTags> staffTagList = new List<StaffTags>();
                foreach (StaffTagsModel staffTag in entity.StaffTagsModel)
                {
                    StaffTags tag = new StaffTags();
                    tag.IsActive = true;
                    tag.IsDeleted = false;
                    tag.CreatedBy = token.UserID;
                    tag.CreatedDate = Currentdate;
                    tag.TagID = staffTag.TagID;
                    tag.CreatedBy = token.UserID;
                    tag.CreatedDate = Currentdate;
                    staffTagList.Add(tag);
                }
                entity.StaffTags = staffTagList;
                #endregion}
            }

            if (entity.StaffSpecialityModel != null && entity.StaffSpecialityModel.Count > 0)
            {

                #region Map Staff Speciality
                List<StaffSpeciality> staffSpecialityList = new List<StaffSpeciality>();
                foreach (StaffSpecialityModel staffSpecialityModel in entity.StaffSpecialityModel)
                {
                    StaffSpeciality tag = new StaffSpeciality
                    {
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = token.UserID,
                        CreatedDate = Currentdate,
                        GlobalCodeId = staffSpecialityModel.SpecialityId
                    };
                    staffSpecialityList.Add(tag);
                }
                entity.StaffSpecialities = staffSpecialityList;
                #endregion
            }

            if (entity.StaffCareCategoryModel != null && entity.StaffCareCategoryModel.Count > 0)
            {

                #region Map Staff CareCategory
                List<StaffCareCategories> staffCarecategoryList = new List<StaffCareCategories>();
                foreach (StaffCareCategoryModel StaffCareCategoryModel in entity.StaffCareCategoryModel)
                {
                    StaffCareCategories tag = new StaffCareCategories
                    {
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = token.UserID,
                        CreatedDate = Currentdate,
                        CareCategoryId = StaffCareCategoryModel.healthcarecategoryID
                    };
                    staffCarecategoryList.Add(tag);
                }
                entity.StaffCareCategories = staffCarecategoryList;
                #endregion
            }

            if (entity.StaffTaxonomyModel != null && entity.StaffTaxonomyModel.Count > 0)
            {

                #region Map Staff taxonomy
                List<StaffTaxonomy> staffTaxonomyList = new List<StaffTaxonomy>();
                foreach (StaffTaxonomyModel staffTaxonomyModel in entity.StaffTaxonomyModel)
                {
                    StaffTaxonomy tag = new StaffTaxonomy
                    {
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = token.UserID,
                        CreatedDate = Currentdate,
                        GlobalCodeId = staffTaxonomyModel.TaxonomyId
                    };
                    staffTaxonomyList.Add(tag);
                }
                entity.StaffTaxonomies = staffTaxonomyList;
                #endregion
            }

            if (entity.StaffServicesModels != null && entity.StaffServicesModels.Count > 0)
            {

                #region Map Staff taxonomy
                List<StaffServices> staffServiceList = new List<StaffServices>();
                foreach (StaffServicesModel staffServicesModel in entity.StaffServicesModels)
                {
                    StaffServices tag = new StaffServices
                    {
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = token.UserID,
                        CreatedDate = Currentdate,
                        ServiceId = Convert.ToInt16(staffServicesModel.ServiceId)
                    };
                    staffServiceList.Add(tag);
                }
                entity.StaffServices = staffServiceList;
                #endregion
            }
        }
        private void MapStaffLoaction(Staffs entity)
        {
            if (entity.StaffLocationList != null && entity.StaffLocationList.Count() > 0)
            {
                #region Map Staff Location
                List<StaffLocation> staffLocationList = new List<StaffLocation>();
                foreach (StaffLocationModel staffLoc in entity.StaffLocationList)
                {
                    StaffLocation staffLocation = new StaffLocation();
                    //staffLocation.StaffId = savedStaff.Id;
                    staffLocation.LocationID = staffLoc.Id;
                    staffLocation.IsDefault = staffLoc.IsDefault;
                    staffLocation.OrganizationID = entity.OrganizationID;
                    staffLocationList.Add(staffLocation);
                }
                entity.StaffLocation = staffLocationList;
                #endregion
            }
        }
        private Staffs UpdateStaffTeam(Staffs entity, TokenModel token, DateTime Currentdate)
        {
            List<StaffTeam> staffTeamList = new List<StaffTeam>();
            foreach (StaffTeamModel staffteam in entity.StaffTeamList)
            {
                StaffTeam team = new StaffTeam();
                if (staffteam.id > 0)
                {
                    team = _context.StaffTeam.Where(a => a.Id == staffteam.id).FirstOrDefault();
                    //team.StaffId = staffID;
                    //team.StaffTeamID = staffteam.staffteamid;
                    if (staffteam.isdeleted)
                    {
                        team.IsDeleted = staffteam.isdeleted;
                        team.DeletedBy = token.UserID;
                        team.DeletedDate = Currentdate;
                    }
                    team.UpdatedBy = token.UserID;
                    team.UpdatedDate = Currentdate;
                }
                else
                {
                    team.StaffId = entity.Id;
                    team.StaffTeamID = staffteam.staffteamid;
                    team.CreatedBy = token.UserID;
                    team.CreatedDate = Currentdate;
                }
                staffTeamList.Add(team);
            }
            entity.StaffTeam = staffTeamList;
            return entity;
        }
        private Staffs UpdateStaffTags(Staffs entity, TokenModel token, DateTime Currentdate)
        {
            List<StaffTags> staffTagList = new List<StaffTags>();
            StaffTags team = null;
            foreach (StaffTagsModel stafftag in entity.StaffTagsModel)
            {
                team = new StaffTags();
                if (stafftag.Id > 0)
                {
                    team = _staffTagRepository.Get(a => a.Id == stafftag.Id && a.IsDeleted == false && a.IsActive == true);
                    if (stafftag.IsDeleted)
                    {
                        team.IsDeleted = stafftag.IsDeleted;
                        team.DeletedBy = token.UserID;
                        team.DeletedDate = Currentdate;
                    }
                    team.UpdatedBy = token.UserID;
                    team.UpdatedDate = Currentdate;
                }
                else
                {
                    team.CreatedBy = token.UserID;
                    team.CreatedDate = Currentdate;
                    team.IsActive = true;
                    team.IsDeleted = false;
                    team.StaffID = entity.Id;
                    team.TagID = stafftag.TagID;
                }
                staffTagList.Add(team);
            }
            entity.StaffTags = staffTagList;
            return entity;
        }

        private Staffs UpdateStaffSpeciality(Staffs entity, TokenModel token, DateTime Currentdate)
        {
            List<StaffSpeciality> staffSpecialityList = new List<StaffSpeciality>();
            StaffSpeciality speciality = null;
            foreach (StaffSpecialityModel staffSpecialityModel in entity.StaffSpecialityModel)
            {
                speciality = new StaffSpeciality();
                if (staffSpecialityModel.Id > 0)
                {
                    speciality = _staffSpecialityRepository.Get(a => a.Id == staffSpecialityModel.Id && a.IsDeleted == false && a.IsActive == true);
                    if (staffSpecialityModel.IsDeleted)
                    {
                        speciality.IsDeleted = staffSpecialityModel.IsDeleted;
                        speciality.DeletedBy = token.UserID;
                        speciality.DeletedDate = Currentdate;
                    }
                    speciality.UpdatedBy = token.UserID;
                    speciality.UpdatedDate = Currentdate;
                }
                else
                {
                    speciality.CreatedBy = token.UserID;
                    speciality.CreatedDate = Currentdate;
                    speciality.IsActive = true;
                    speciality.IsDeleted = false;
                    speciality.StaffID = entity.Id;
                    speciality.Id = staffSpecialityModel.Id;
                    speciality.GlobalCodeId = staffSpecialityModel.SpecialityId;
                }
                staffSpecialityList.Add(speciality);
            }
            entity.StaffSpecialities = staffSpecialityList;
            return entity;
        }

        private Staffs UpdateStaffCarecategory(Staffs entity, TokenModel token, DateTime Currentdate)
        {
            List<StaffCareCategories> staffCarecategoryList = new List<StaffCareCategories>();
            List<StaffCareCategories> carecategoryList = new List<StaffCareCategories>();

            carecategoryList = _context.StaffCareCategories.Where(a => a.StaffID == entity.Id && a.IsDeleted == false && a.IsActive == true).ToList();
            foreach (StaffCareCategories item in carecategoryList)
            {
                item.IsDeleted = true;
                item.IsActive = false;
                item.DeletedBy = token.UserID;
                item.DeletedDate = Currentdate;

                //_context.StaffCareCategories.Update(item);
                // _context.SaveChanges();
                staffCarecategoryList.Add(item);
            }

            carecategoryList = _context.StaffCareCategories.Where(a => a.StaffID == entity.Id).ToList();
            StaffCareCategories carecategory = null;
            foreach (StaffCareCategoryModel staffCarecategoryModel in entity.StaffCareCategoryModel)
            {
                carecategory = new StaffCareCategories();
                //if (staffCarecategoryModel.Id > 0)
                //{
                //    carecategory = _staffCareCategoryRepository.Get(a => a.Id == staffCarecategoryModel.Id && a.IsDeleted == false && a.IsActive == true);
                //    if (staffCarecategoryModel.IsDeleted)
                //    {
                //        carecategory.IsDeleted = staffCarecategoryModel.IsDeleted;
                //        carecategory.DeletedBy = token.UserID;
                //        carecategory.DeletedDate = Currentdate;
                //    }
                //    carecategory.UpdatedBy = token.UserID;
                //    carecategory.UpdatedDate = Currentdate;
                //}
                //else
                {
                    carecategory.CreatedBy = token.UserID;
                    carecategory.CreatedDate = Currentdate;
                    carecategory.IsActive = true;
                    carecategory.IsDeleted = false;
                    carecategory.StaffID = entity.Id;
                    //carecategory.Id = staffSpecialityModel.Id;
                    carecategory.CareCategoryId = staffCarecategoryModel.healthcarecategoryID;
                }
                staffCarecategoryList.Add(carecategory);
            }
            entity.StaffCareCategories = staffCarecategoryList;
            return entity;
        }

        private Staffs UpdateStaffTaxonomy(Staffs entity, TokenModel token, DateTime Currentdate)
        {
            List<StaffTaxonomy> staffTaxonomyList = new List<StaffTaxonomy>();
            StaffTaxonomy taxonomy = null;
            foreach (StaffTaxonomyModel staffTaxonomyModel in entity.StaffTaxonomyModel)
            {
                taxonomy = new StaffTaxonomy();
                if (staffTaxonomyModel.Id > 0)
                {
                    taxonomy = _staffTaxonomyRepository.Get(a => a.Id == staffTaxonomyModel.Id && a.IsDeleted == false && a.IsActive == true);
                    if (staffTaxonomyModel.IsDeleted)
                    {
                        taxonomy.IsDeleted = staffTaxonomyModel.IsDeleted;
                        taxonomy.DeletedBy = token.UserID;
                        taxonomy.DeletedDate = Currentdate;
                    }
                    taxonomy.UpdatedBy = token.UserID;
                    taxonomy.UpdatedDate = Currentdate;
                }
                else
                {
                    taxonomy.CreatedBy = token.UserID;
                    taxonomy.CreatedDate = Currentdate;
                    taxonomy.IsActive = true;
                    taxonomy.IsDeleted = false;
                    taxonomy.StaffID = entity.Id;
                    taxonomy.Id = staffTaxonomyModel.Id;
                    taxonomy.GlobalCodeId = staffTaxonomyModel.TaxonomyId;
                }
                staffTaxonomyList.Add(taxonomy);
            }
            entity.StaffTaxonomies = staffTaxonomyList;
            return entity;
        }

        private Staffs UpdateStaffServices(Staffs entity, TokenModel token, DateTime Currentdate)
        {
            List<StaffServices> staffServicesList = new List<StaffServices>();
            StaffServices staffServices = null;
            foreach (StaffServicesModel staffServicesModel in entity.StaffServicesModels)
            {

                staffServices = new StaffServices();
                if (!string.IsNullOrEmpty(staffServicesModel.Id))
                {
                    staffServices = _staffServicesRepository.Get(a => a.Id == Convert.ToInt16(CommonMethods.Decrypt(staffServicesModel.Id.Replace(" ", "+"))) && a.IsDeleted == false && a.IsActive == true);
                    if (staffServicesModel.IsDeleted)
                    {
                        staffServices.IsDeleted = staffServicesModel.IsDeleted;
                        staffServices.DeletedBy = token.UserID;
                        staffServices.DeletedDate = Currentdate;
                    }
                    staffServices.UpdatedBy = token.UserID;
                    staffServices.UpdatedDate = Currentdate;
                }
                else
                {
                    staffServices.CreatedBy = token.UserID;
                    staffServices.CreatedDate = Currentdate;
                    staffServices.IsActive = true;
                    staffServices.IsDeleted = false;
                    staffServices.StaffId = entity.Id;
                    staffServices.ServiceId = Convert.ToInt16(staffServicesModel.ServiceId);
                }
                staffServicesList.Add(staffServices);
            }
            entity.StaffServices = staffServicesList;
            return entity;
        }

        public JsonModel GetDoctorDetailsFromNPI(string npiNumber, string enumerationType)
        {
            //string html = string.Empty;
            //string city = "baltimore";
            //string enumeration_type = "";   //NPI-1 or NPI-2
            //string limit = "200";  //default=10 , max= 200
            //string skip = "";
            //string state = "";
            //string country = "US";
            // var query = "enumeration_type="+enumeration_type + "&limit=" + limit+"&skip="+skip+ "&state="+state+ "&country=" + country+"&city="+city;
            var query = "number=" + npiNumber + "&enumeration_type=" + enumerationType;//1083617021;
            string url = "https://npiregistry.cms.hhs.gov/api?" + query;
            string response = CommonMethods.CreateHTTPRequest(url, null, "GET", "application/json");
            NPIDetailsRootObject root = JsonConvert.DeserializeObject<NPIDetailsRootObject>(response);
            return new JsonModel(root, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, "");
        }

        public JsonModel GetStaffProfileData(int staffId, TokenModel token)
        {
            StaffProfileModel staffProfileModel = _staffRepository.GetStaffProfileData(staffId, token);
            staffProfileModel.PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, staffProfileModel.PhotoThumbnailPath);
            staffProfileModel.PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffPhotos, staffProfileModel.PhotoPath);
            return new JsonModel(staffProfileModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
        }




        public JsonModel GetAssignedLocationsById(int staffId, TokenModel tokenModel)
        {
            List<StaffAssignedLocationsModel> staffLocations = _staffRepository.GetAssignedLocationsById<StaffAssignedLocationsModel>(staffId, tokenModel).ToList();
            if (staffLocations != null && staffLocations.Count > 0)
            {
                response = new JsonModel(staffLocations, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }

        public JsonModel GetStaffHeaderData(int staffId, TokenModel tokenModel)
        {
            StaffHeaderDataModel staffHeaderDataModels = _staffRepository.GetStaffHeaderData<StaffHeaderDataModel>(staffId, tokenModel).FirstOrDefault();
            if (staffHeaderDataModels != null)
            {
                staffHeaderDataModels.ProfileImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, staffHeaderDataModels.ProfileImage);
                response = new JsonModel(staffHeaderDataModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }
        public JsonModel GetStaffExperience(int staffId, TokenModel tokenModel)
        {
            StaffHeaderDataModel staffHeaderDataModels = _staffRepository.GetStaffHeaderData<StaffHeaderDataModel>(staffId, tokenModel).FirstOrDefault();
            if (staffHeaderDataModels != null)
            {
                staffHeaderDataModels.ProfileImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, staffHeaderDataModels.ProfileImage);
                response = new JsonModel(staffHeaderDataModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }
        public JsonModel CheckStaffProfile(int staffId, TokenModel tokenModel)
        {
            var staffProfile = _staffRepository.CheckStaffProfile<StaffProfileSetupModel>(staffId, tokenModel);
            if (staffProfile != null)
            {

                response = new JsonModel(staffProfile, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);

            }
            else
                response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
            return response;
        }
        public JsonModel GetStaffByName(string name, TokenModel tokenModel)
        {
            var staffProfile = _staffRepository.GetStaffByName<StaffByNameModel>(name, tokenModel).ToList();
            if (staffProfile != null && staffProfile.Count > 0)
            {
                staffProfile.ForEach(x =>
                {
                    x.PhotoThumbnailPath = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, x.PhotoThumbnailPath);
                });
                response = new JsonModel(staffProfile, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            else
                response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
            return response;
        }


        public JsonModel GetStaffByEmail(string email, TokenModel tokenModel)
        {
            var staffProfile = _staffRepository.GetStaffByEmail(email, tokenModel).ToList();
            if (staffProfile != null && staffProfile.Count > 0)
            {
                response = new JsonModel(staffProfile, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            else
                response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
            return response;
        }

        #endregion
    }
}