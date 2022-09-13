using HC.Patient.Entity;
using HC.Patient.Service.IServices.Organizations;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("OrganizationDatabaseDetail")]
    [ActionFilter]
    public class OrganizationDatabaseDetailController : BaseController
    {

        private readonly IOrganizationService _organizationService;

        #region Construtor of the class
        public OrganizationDatabaseDetailController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }
        #endregion

        // GET: api/OrganizationDatabaseDetail
        [HttpGet]
        public JsonResult Get(string databaseName, string organizationName, int organizationID, string sortColumn, string sortOrder, int pageNumber, int pageSize)
        {
            return Json(_organizationService.ExecuteFunctions(() => _organizationService.GetOrganizationDatabaseDetails(databaseName, organizationName, organizationID, sortColumn, sortOrder, pageNumber, pageSize)));
        }

        // GET: api/OrganizationDatabaseDetail/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OrganizationDatabaseDetail
        [HttpPost]
        public JsonResult Post([FromBody]OrganizationDatabaseDetail organizationDatabaseDetail)
        {
            return Json(_organizationService.ExecuteFunctions(() => _organizationService.SaveOrganizationDatabaseDetail(organizationDatabaseDetail)));
        }


        // PATCH: api/OrganizationDatabaseDetail/5
        [HttpPatch("{id}")]
        public JsonResult Patch(int id, [FromBody]OrganizationDatabaseDetail organizationDatabaseDetail)
        {
            organizationDatabaseDetail.UpdatedBy = GetToken(HttpContext).UserID;
            return Json(_organizationService.ExecuteFunctions(() => _organizationService.UpdateOrganizationDatabaseDetail(id, organizationDatabaseDetail)));
        }

        // DELETE: api/OrganizationDatabaseDetail/5
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            return Json(_organizationService.ExecuteFunctions(() => _organizationService.DeleteOrganizationDatabaseDetail(id, GetToken(HttpContext).UserID)));
        }
    }
}
