using HC.Common.Model.Staff;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Users;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.User;
using HC.Patient.Service.Services;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("Staffs")]
    [ActionFilter]
    public class StaffsController : BaseController
    {
        private readonly IStaffService _staffService;
        private readonly IStaffExperienceService _staffExperienceService;
        private readonly IStaffQualificationService _staffQualificationService;
        private readonly IStaffAwardService _staffAwardService;
        private readonly IUserRoleService _userRoleService;
        private JsonModel response = new JsonModel();

        #region  Construtor of the class
        public StaffsController(IStaffService staffService, IStaffExperienceService staffExperienceService, IStaffQualificationService staffQualificationService, IStaffAwardService staffAwardService, IUserRoleService userRoleService)
        {
            _staffService = staffService;
            _staffExperienceService = staffExperienceService;
            _staffQualificationService = staffQualificationService;
            _staffAwardService = staffAwardService;
            _userRoleService = userRoleService;
        }
        #endregion

        #region Class Methods

        /// <summary>
        /// Description: this method is used to get staffs listing with filters
        /// </summary>
        /// <param name="staffFiltterModel"></param>
        /// <returns></returns>
        [HttpGet("GetStaffs")]
        public JsonResult GetStaffs(ListingFiltterModel staffFiltterModel)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.GetStaffs(staffFiltterModel, GetToken(HttpContext))));
        }

        [HttpGet("GetProviders")]
        public JsonResult GetProviders()
        {
            SearchFilterModel roleFilter = new SearchFilterModel()
            {
                pageNumber = 1,
                pageSize = 100,
            };
            var rolesRes =  _userRoleService.GetRoles(roleFilter, GetToken(HttpContext));
            var roles = (List<UserRoleModel>)rolesRes.data;
            ListingFiltterModel  staffFiltterModel =  new ListingFiltterModel();
            staffFiltterModel.sortColumn = "firstname";
            staffFiltterModel.sortOrder = "asc";
            staffFiltterModel.RoleIds = roles.FirstOrDefault(x => x.UserType.ToUpper() == "PROVIDER").Id.ToString();
            staffFiltterModel.pageNumber = 1;
            staffFiltterModel.pageSize = 99999999;


            return Json(_staffService.ExecuteFunctions(() => _staffService.GetStaffs(staffFiltterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// this method is used to create and update staff
        /// </summary>
        /// <param name="staffs"></param>
        /// <returns></returns>
        [HttpPost("CreateUpdateStaff")]
        public JsonResult CreateUpdateStaff([FromBody]Staffs staffs)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.CreateUpdateStaff(staffs, GetToken(HttpContext))));
        }

        /// <summary>
        /// this method is used to get staff by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetStaffById")]
        public JsonResult GetStaffById(int id)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.GetStaffById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// this method is used to delete staff by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("DeleteStaff")]
        public JsonResult DeleteStaff(int id)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.DeleteStaff(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// this method is used to update staff Status
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpPatch("UpdateStaffActiveStatus")]
        public JsonResult UpdateStaffActiveStatus(int staffId, bool isActive)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.UpdateStaffActiveStatus(staffId, isActive, GetToken(HttpContext))));
        }

        /// <summary>
        /// this method is used to get staff by tags filter
        /// </summary>
        /// <param name="listingFiltterModel"></param>
        /// <returns></returns>
        [HttpGet("GetStaffByTag")]
        public JsonResult GetStaffByTag(ListingFiltterModel listingFiltterModel)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.GetStaffByTags(listingFiltterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get doctor details from NPI number
        /// </summary>
        /// <param name="npiNumber"></param>
        /// <param name="enumerationType"></param>
        /// <returns></returns>
        [HttpGet("GetDoctorDetailsFromNPI")]
        public JsonResult GetDoctorDetailsFromNPI(string npiNumber, string enumerationType)
        {
            return Json(_staffService.ExecuteFunctions<JsonModel>(() => _staffService.GetDoctorDetailsFromNPI(npiNumber, enumerationType)));
        }

        /// <summary>
        /// <Description> Get Staff Profile Data</Description>        
        /// </summary>
        /// <param name="Id">Staff Id</param>
        /// <returns></returns>
        [HttpGet("GetStaffProfileData/{Id}")]
        public JsonResult GetStaffProfileData(int Id)
        {
            return Json(_staffService.ExecuteFunctions<JsonModel>(() => _staffService.GetStaffProfileData(Id, GetToken(HttpContext))));
        }

        /// <summary>
        /// this method is used to get staff lcoation by staff id
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetAssignedLocationsById")]
        public JsonResult GetAssignedLocationsById(int id)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.GetAssignedLocationsById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// this method is used to get staff header data
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetStaffHeaderData")]
        public JsonResult GetStaffHeaderData(int id)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.GetStaffHeaderData(id, GetToken(HttpContext))));
        }


        [HttpGet("GetLoggedinUserDetail")]
        public JsonResult GetLoggedinUserDetail()
        {
            return Json(_userRoleService.ExecuteFunctions(() => _userRoleService.GetLoggedinUserInfo(GetToken(HttpContext))));
        }
        #region Staff Experience


        [HttpGet("GetStaffExperience")]
        public JsonResult GetStaffExperience(string id)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffExperienceService.getStaffExperiences(GetToken(HttpContext), id)));
        }
        [HttpPost("SaveUpdateStaffExperience")]
        public JsonResult SaveUpdateStaffExperience([FromBody] StaffExperienceRequestModel staffExperienceModels)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffExperienceService.SaveUpdateExperience(staffExperienceModels, GetToken(HttpContext))));
        }

        #endregion Staff Experience

        #region Staff Qualification


        [HttpGet("GetStaffQualifications")]
        public JsonResult GetStaffQualification(string id)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffQualificationService.getStaffQualifications(GetToken(HttpContext), id)));
        }
        [HttpPost("SaveUpdateStaffQualifications")]
        public JsonResult SaveUpdateStaffQualification([FromBody] StaffQualificationRequestModel staffQualificationRequestModel)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffQualificationService.SaveUpdateQualifications(staffQualificationRequestModel, GetToken(HttpContext))));
        }

        #endregion Staff Qualification

        #region Staff Award


        [HttpGet("GetStaffAwards")]
        public JsonResult GetStaffAwards(string id)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffAwardService.getStaffAwards(GetToken(HttpContext), id)));
        }
        [HttpPost("SaveUpdateStaffAwards")]
        public JsonResult SaveUpdateStaffAwards([FromBody] StaffAwardRequestModel staffAwardRequestModel)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffAwardService.SaveUpdateAwards(staffAwardRequestModel, GetToken(HttpContext))));
        }

        #endregion Staff Award
        #region StaffProfile
        /// <summary>
        /// this method is used to Check whether user has Updated his complete profile.
        /// </summary>        
        /// <returns></returns>
        [HttpGet("CheckStaffProfile")]
        public JsonResult CheckStaffProfile(int id)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.CheckStaffProfile(id, GetToken(HttpContext))));
        }
        #endregion

        #region All Staff
        [HttpGet("GetStaffsByName")]
        public JsonResult GetStaffsByName(string name)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.GetStaffByName(name, GetToken(HttpContext))));
        }
        #endregion All Staff


        [HttpGet("GetStaffFeeSettings")]
        public JsonResult GetStaffFeeSettings(string id)
        {
            
            return Json(_staffService.ExecuteFunctions(() => _staffService.GetStaffFeeSettings(id, GetToken(HttpContext))));
        }

        [HttpPut]
        [Route("UpdateProviderTimeInterval/{Id}")]
        public JsonResult UpdateProviderTimeInterval(string Id)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.UpdateProviderTimeInterval(Id, GetToken(HttpContext))));
        }

        [HttpPut]
        [Route("UpdateProviderUrgentCareStatus")]
        public JsonResult UpdateProviderUrgentCareStatus(int staffId,bool isUrgentCare)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.UpdateProviderUrgentCareStatus(staffId, isUrgentCare, GetToken(HttpContext))));
        }


        #endregion

    }
}