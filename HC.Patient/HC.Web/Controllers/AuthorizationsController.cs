using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Authorization;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("Authorizations")]
    [ActionFilter]
    public class AuthorizationsController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;
        public AuthorizationsController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Description  : get all authorization for patient
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="authType"></param>
        /// <returns></returns>
        [HttpGet("GetAllAuthorizationsForPatient")]
        public JsonResult GetAllAuthorizationsForPatient(int patientId, int pageNumber=1, int pageSize=10, string authType="")
        {
            return Json(_authorizationService.ExecuteFunctions(() => _authorizationService.GetAllAuthorizationsForPatient(patientId, pageNumber, pageSize, authType, GetToken(HttpContext))));
        }

        /// <summary>        
        /// Description  : save and Update authorization for patient
        /// </summary>
        /// <param name="authModel"></param>
        /// <returns></returns>
        [HttpPost("SaveAuthorization")]
        public JsonResult SaveAuthorization([FromBody]AuthModel authModel)
        {
            return Json(_authorizationService.ExecuteFunctions(() => _authorizationService.SaveAuthorization(authModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get authorization for patient by id
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetAuthorizationById")]
        public JsonResult GetAuthorizationById(SearchFilterModel searchFilterModel)
        {   
            return Json(_authorizationService.ExecuteFunctions(() => _authorizationService.GetAuthorizationById(searchFilterModel, GetToken(HttpContext))));
        }


        /// <summary>        
        /// Description  : delete authorization for patient by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteAutorization")]
        public JsonResult DeleteAutorization(int id)
        {
            return Json(_authorizationService.ExecuteFunctions(() => _authorizationService.DeleteAutorization(id, GetToken(HttpContext))));
        }
    }
}