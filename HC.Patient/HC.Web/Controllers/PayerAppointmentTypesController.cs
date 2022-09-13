using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Patient.Entity;
using HC.Patient.Repositories.Interfaces;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    public class PayerAppointmentTypesController : CustomJsonApiController<Entity.PayerAppointmentTypes, int>
    {   
        private readonly IDbContextResolver _dbContextResolver;

        #region Construtor of the class
        public PayerAppointmentTypesController(
       IJsonApiContext jsonApiContext,
       IResourceService<Entity.PayerAppointmentTypes, int> resourceService,
       ILoggerFactory loggerFactory, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {   
                _dbContextResolver = jsonApiContext.GetDbContextResolver();
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;

                if (jsonApiContext.QuerySet != null && !jsonApiContext.QuerySet.Equals(null))
                {
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));
                }
                else
                {
                    jsonApiContext.QuerySet = new QuerySet();
                    jsonApiContext.QuerySet.Filters = new List<FilterQuery>();
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Class Methods
        
        #endregion

        #region Class Methods
        [HttpPost]
        [ValidateModel]
        public override async Task<IActionResult> PostAsync([FromBody]PayerAppointmentTypes payerAppointmentTypes)
        {
            if (ModelState.IsValid)
            {
                return await base.PostAsync(payerAppointmentTypes);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCodes.NotFound;//(Not Found)
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound//(Not Found)
                });
            }

        }
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]PayerAppointmentTypes payerAppointmentTypes)
        {
            return await base.PatchAsync(id, payerAppointmentTypes);
        }

        [HttpPatch]
        [Route("DeleteAsync/{id}")]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }
        #endregion

        #region Helping Methods
        #endregion

        // [HttpGet("")]
    }
}