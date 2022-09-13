using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.Staff;
using HC.Patient.Model.ThemeSettings;
using HC.Patient.Model.Users;
using HC.Patient.Service.IServices.ThemeSettings;
using HC.Patient.Service.Payer.Interfaces;
using HC.Patient.Service.Token.Interfaces;
using HC.Patient.Service.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    public class CommonController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPayerInformationService _payerServiceCodesService;
        private readonly ITokenService _tokenService;
        private readonly IThemeSettingService _themeSettingService;
        
        public CommonController(IUserService userService, IPayerInformationService payerServiceCodesService, ITokenService tokenService, IThemeSettingService themeSettingService)
        {
            _payerServiceCodesService = payerServiceCodesService;
            _userService = userService;
            _tokenService = tokenService;
            _themeSettingService = themeSettingService;        
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordExistFilter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheckIfRecordExists")]
        public JsonResult CheckIfRecordExists([FromBody]RecordExistFilter recordExistFilter)
        {

            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                int OrganizationID = token.OrganizationID;
                recordExistFilter.OrganizationID = OrganizationID;

                bool IsExists = _userService.CheckIfRecordExists(recordExistFilter);
                if (IsExists)
                {
                    return Json(new
                    {
                        data = recordExistFilter.ColmnName,
                        Message = StatusMessage.RecordAlreadyExists.Replace("[string]", recordExistFilter.LabelName),
                        //.Replace("[value]", recordExistFilter.Value).Replace("[table]",recordExistFilter.TableName),
                        StatusCode = (int)HttpStatusCodes.Conflict
                    });
                }
                else
                {
                    return Json(new
                    {
                        data = recordExistFilter.ColmnName,
                        Message = StatusMessage.RecordNotExists,
                        StatusCode = (int)HttpStatusCodes.Accepted
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = Response.StatusCode,//(Error Code)                    
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordExistFilter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheckIfRecordExistsMasterDB")]
        public JsonResult CheckIfRecordExistsMasterDB([FromBody]RecordExistFilter recordExistFilter)
        {

            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                int OrganizationID = token.OrganizationID;
                recordExistFilter.OrganizationID = OrganizationID;

                bool IsExists = _userService.CheckIfRecordExistsMasterDB(recordExistFilter);
                if (IsExists)
                {
                    return Json(new
                    {
                        data = recordExistFilter.ColmnName,
                        Message = StatusMessage.RecordAlreadyExists.Replace("[string]", recordExistFilter.LabelName),
                        //.Replace("[value]", recordExistFilter.Value).Replace("[table]",recordExistFilter.TableName),
                        StatusCode = (int)HttpStatusCodes.Conflict
                    });
                }
                else
                {
                    return Json(new
                    {
                        data = recordExistFilter.ColmnName,
                        Message = StatusMessage.RecordNotExists,
                        StatusCode = (int)HttpStatusCodes.Accepted
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = Response.StatusCode//(Error Code)
                });
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="PatientID"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayerByPatient")]
        public JsonResult GetPayerByPatient(int PatientID, string Key = "")
        {

            try
            {
                var payers = _userService.GetPayerByPatientID(PatientID, Key);
                if (payers != null && payers.Count != 0)
                {
                    return Json(new
                    {
                        data = payers,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = Response.StatusCode
                    });
                }
                else
                {
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.NotFound,
                        StatusCode = Response.StatusCode
                    });
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = Response.StatusCode//(Error Code)
                });
            }
        }

        [HttpGet]
        [Route("VerifyBusinessName")]
        public JsonResult VerifyBusinessName(string BusinessName = "", string HostName = "")
        {
            DomainToken domainToken = new DomainToken();
            TokenModel token = new TokenModel();
            token.Request = HttpContext;
            domainToken.BusinessToken = BusinessName;
            DomainToken tokenData = _tokenService.GetDomain(domainToken);


            if (tokenData != null)
            {
                tokenData.Organization = _tokenService.GetOrganizationById(tokenData.OrganizationId, token);
                Response.StatusCode = (int)HttpStatusCodes.OK;
                return Json(new
                {
                    data = tokenData,
                    Message = StatusMessage.VerifiedBusinessName,
                    StatusCode = (int)HttpStatusCodes.OK
                });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCodes.Unauthorized;
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.UnVerifiedBusinessName,
                    StatusCode = (int)HttpStatusCodes.Unauthorized
                });
            }
        }



        [HttpGet]
        [Route("GetUserAssignedLocations")]
        public JsonResult GetUserAssignedLocations(int userId)
        {
            //get users assigned locations
            List<UserLocationsModel> userLocation = _tokenService.GetUserLocations(userId);
            if (userLocation != null && userLocation.Count() > 0)
            {
                Response.StatusCode = (int)HttpStatusCodes.OK;
                return Json(new
                {
                    data = userLocation,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCodes.NoContent;
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NoContent
                });
            }
        }

        [HttpGet("GetSaltedPassword")]
        public string GetSaltedPassword(string password, bool isEncrypt)
        {
            if (isEncrypt)
                return CommonMethods.Decrypt(password);
            else
                return CommonMethods.Encrypt(password);
        }

        [HttpPost]
        [Route("CreateSetting")]
        public JsonResult CreateSetting([FromBody]ThemeSettingsModel model)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_themeSettingService.ExecuteFunctions<JsonModel>(() => _themeSettingService.CreateSettings(model, token)));
        }

       
        [HttpGet]
        [Route("GetSettings")]
        public JsonResult GetSettings(int userId)
        {
            return Json(_themeSettingService.ExecuteFunctions<JsonModel>(() => _themeSettingService.GetSettings(userId)));
        }
        //[HttpGet("GetPayerInformation")]
        //public JsonResult GetPayerInformation(string payerName, int? pageNumber, int? pageSize, string sortColumn, string sortOrder)
        //{
        //    try
        //    {
        //        TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
        //        PayerSearchFilter payerSearchFilter = new PayerSearchFilter();
        //        payerSearchFilter.OrganizationID = token.OrganizationID;
        //        payerSearchFilter.Name = payerName;
        //        payerSearchFilter.PageNumber = pageNumber;
        //        payerSearchFilter.PageSize = pageSize;
        //        payerSearchFilter.SortColumn = sortColumn;
        //        payerSearchFilter.SortOrder = sortOrder;
        //        payerSearchFilter.IsPayerInformation = true;
        //        return Json(_payerServiceCodesService.GetPayerInformationByFilter(payerSearchFilter));
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}
    }
}