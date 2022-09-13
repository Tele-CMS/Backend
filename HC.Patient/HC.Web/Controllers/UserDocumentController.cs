using HC.Common;
using HC.Common.HC.Common;
using HC.Patient.Model.Users;
using HC.Patient.Service.IServices.User;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("UserDocument")]
    [ActionFilter]
    public class UserDocumentController : BaseController
    {
        private readonly IUserService _userService;
        #region Construtor of the class
        public UserDocumentController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// upload user documents
        /// </summary>
        /// <param name="userDocuments"></param>
        /// <returns></returns>
        [HttpPost("UploadUserDocuments")]
        public JsonResult UploadUserDocuments([FromBody]UserDocumentsModel userDocuments)
        {
            return Json(_userService.ExecuteFunctions(() => _userService.UploadUserDocuments(userDocuments, GetToken(HttpContext))));
        }

        /// <summary>
        /// get user's all documents
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet("GetUserDocuments")]
        public JsonResult GetPatientDocuments(int userID, DateTime? from, DateTime? to)
        {
            return Json(_userService.ExecuteFunctions(() => _userService.GetUserDocuments(userID, from, to, GetToken(HttpContext))));
        }

        /// <summary>
        /// <Description> get user's single document in base 64</Description>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUserDocument")]
        public IActionResult GetUserDocument(int id)
        {
            UserDocumentsResponseModel response = _userService.GetUserDocument(id, GetToken(HttpContext));
            if (response != null)
            {
                string applicationType = CommonMethods.GetMimeType(response.Extenstion);
                return File(response.File, applicationType, response.FileName);
                //string applicationType = CommonMethods.GetMimeType(response.Extenstion);
               // return File(response.File, "application/octet-stream", response.FileName);
            }
            else
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.DocumentNotExist,
                    StatusCode = (int)HttpStatusCodes.NotFound
                });
            }

        }


        /// <summary>
        /// <Description> This API for delete user's document </Description>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteUserDocument")]
        public JsonResult DeleteUserDocument(int id)
        {
            return Json(_userService.DeleteUserDocument(id, GetToken(HttpContext)));
        }
        #endregion

        [HttpGet("GetPateintAppointmenttDocuments")]
        public JsonResult GetPateintAppointmenttDocuments(int apptId)
        {
            return Json(_userService.ExecuteFunctions(() => _userService.GetPateintAppointmenttDocuments(apptId, GetToken(HttpContext))));
        }


        [HttpPatch("UpdateProviderEducationalDocumentStatus/{id}/{documentstatus}")]
        public JsonResult UpdateProviderEducationalDocumentStatus(int id, bool documentstatus)
        {
            return Json(_userService.UpdateProviderEducationalDocumentStatus(id, documentstatus, GetToken(HttpContext)));
        }

        [HttpGet("GetProviderEducationalDocuments")]
        public JsonResult GetProviderEducationalDocuments(int userID, DateTime? from, DateTime? to)
        {
            return Json(_userService.ExecuteFunctions(() => _userService.GetProviderEducationalDocuments(userID, from, to, GetToken(HttpContext))));
        }

        [HttpGet("GetprovidereductaionalDocumentsForPatientCheckin")]
        public JsonResult GetprovidereductaionalDocumentsForPatientCheckin(int staffid)
        {
            return Json(_userService.ExecuteFunctions(() => _userService.GetprovidereductaionalDocumentsForPatientCheckin(staffid, GetToken(HttpContext))));
        }

        #region Helping Methods
        /////////////////////////
        //helping methods
        #endregion
    }
}