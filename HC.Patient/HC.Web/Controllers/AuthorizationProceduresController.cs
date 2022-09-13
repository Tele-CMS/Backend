//using Audit.WebApi;
using HC.Common;
using HC.Common.Filters;
using HC.Common.HC.Common;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.Interfaces;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})", IncludeResponseBody = true, IncludeHeaders = true, IncludeModelState = true)]
    public class AuthorizationProceduresController : CustomJsonApiController<AuthorizationProcedures, int>
    {
        private readonly IDbContextResolver _dbContextResolver;



        #region Construtor of the class
        public AuthorizationProceduresController(
       IJsonApiContext jsonApiContext,
       IResourceService<AuthorizationProcedures, int> resourceService,
       ILoggerFactory loggerFactory, IUserCommonRepository userCommonRepository)
    : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _dbContextResolver = jsonApiContext.GetDbContextResolver();
                jsonApiContext.PageManager.DefaultPageSize = (int)CommonAttributes.PageSize;
                if (jsonApiContext.QuerySet != null && !jsonApiContext.QuerySet.Equals(null))
                {
                    //jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsActive", "true", ""));
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));
                }
                else
                {

                    jsonApiContext.QuerySet = new QuerySet();
                    jsonApiContext.QuerySet.Filters = new List<FilterQuery>();
                    //jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsActive", "true", ""));
                    jsonApiContext.QuerySet.Filters.Add(new FilterQuery("IsDeleted", "false", ""));

                }
            }
            catch
            {

            }
        }

        #endregion

        #region Class Methods



        /// <summary>
        /// this method is used for get request for patient
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            return await base.GetAsync();

        }

        /// <summary>
        /// get relationship data
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="relationshipName"></param>
        /// <returns></returns>
        [HttpGet("{type}/{id}/{relationshipName}")]
        public async Task<IActionResult> GetRelationshipAsync(string type, int id, string relationshipName)
        {
            return await base.GetRelationshipAsync(id, relationshipName);
        }

        /// <summary>
        /// get relationship data from relationship name
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="relationshipName"></param>
        /// <returns></returns>
        [HttpGet("{type}/{id}/relationships/{relationshipName}")]
        public async Task<IActionResult> GetRelationshipsAsync(string type, int id, string relationshipName)
        {
            return await base.GetRelationshipsAsync(id, relationshipName);
        }

        /// <summary>
        /// this method is used for save authorization
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [ValidateModel]
        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]AuthorizationProcedures authorizationProcedures)
        {
            return await base.PostAsync(authorizationProcedures);
        }

        /// <summary>
        /// patch data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorizationProcedures"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(int id, [FromBody]AuthorizationProcedures authorizationProcedures)
        {

            return await base.PatchAsync(id, authorizationProcedures);
        }

        /// <summary>
        /// post multiple data in single request with custom modification
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("postMultiple")]
        public async Task<IActionResult> PostAsync([FromBody]JObject entity)
        {
            try
            {
                AuthorizationProcedureList authorizationProcedure = entity.ToObject<AuthorizationProcedureList>();
                int authorizationID = authorizationProcedure.AuthorizationID;
                var dbContext = _dbContextResolver.GetDbSet<AuthorizationProcedures>();
                if (authorizationProcedure.AuthorizationProcedures != null && authorizationProcedure.AuthorizationProcedures.Count > 0)
                {
                    var authorizationProcedures = dbContext.Where(m => m.AuthorizationId == authorizationID).ToList();
                    authorizationProcedures.ForEach(l => _dbContextResolver.GetDbSet<AuthProcedureCPT>().RemoveRange(_dbContextResolver.GetDbSet<AuthProcedureCPT>().Where(i => i.AuthorizationProceduresId == l.Id).ToList()));
                    await _dbContextResolver.GetContext().SaveChangesAsync();
                    _dbContextResolver.GetDbSet<AuthorizationProcedures>().RemoveRange(authorizationProcedures);
                    await _dbContextResolver.GetContext().SaveChangesAsync();
                    foreach (AuthorizationProcedures procedure in authorizationProcedure.AuthorizationProcedures) { await base.PostAsync(procedure); }

                    Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.Success,
                        StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                });
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.Success,
                        StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// patch multiple data in single request with custom modification
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPatch("patchMultiple")]
        public IActionResult PatchAsync([FromBody]JObject entity)
        {
            try
            {

                CommonMethods commonMethods = new CommonMethods();
                StringValues authorizationToken;
                var authHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
                var authToken = authorizationToken.ToString().Replace("Bearer", "").Trim();

                var encryptData = commonMethods.GetDataFromToken(authToken);

                AuthorizationProcedureList authorizationProcedure = entity.ToObject<AuthorizationProcedureList>();
                int authorizationID = authorizationProcedure.AuthorizationID;
                var dbContext = _dbContextResolver.GetDbSet<AuthorizationProcedures>();
                if (authorizationProcedure.AuthorizationProcedures != null && authorizationProcedure.AuthorizationProcedures.Count > 0)
                {
                    //var authorizationProcedures = dbContext.Where(m => m.AuthorizationId == authorizationID).ToList();
                    //authorizationProcedures.ForEach(l => _dbContextResolver.GetDbSet<AuthProcedureCPT>().RemoveRange(_dbContextResolver.GetDbSet<AuthProcedureCPT>().Where(i => i.AuthorizationProcedureId == l.Id).ToList()));
                    //await _dbContextResolver.GetContext().SaveChangesAsync();
                    //_dbContextResolver.GetDbSet<AuthorizationProcedures>().RemoveRange(authorizationProcedures);
                    //await _dbContextResolver.GetContext().SaveChangesAsync();
                    //foreach (AuthorizationProcedures procedure in authorizationProcedure.AuthorizationProcedures) { await base.PostAsync(procedure); }

                    int userID = 0;
                    int organizationID = 0;
                    if (encryptData != null && encryptData.Claims != null)
                    {
                        if (encryptData.Claims.Count > 0)
                        {
                            userID = Convert.ToInt32(encryptData.Claims[0].Value);
                            organizationID = Convert.ToInt32(encryptData.Claims[3].Value);



                            //authProceduresAdded.ForEach(k =>
                            //{
                            //    base.PostAsync(k);
                            //});
                            authorizationProcedure.AuthorizationProcedures.ForEach(k =>
                            {
                                //k.CreatedBy = userID;
                                if (k.IsDeleted == true)
                                {
                                    var dbData = _dbContextResolver.GetDbSet<AuthorizationProcedures>().Where(o => o.Id == k.Id).FirstOrDefault();
                                    k.CreatedBy = dbData.CreatedBy;
                                    k.CreatedDate = dbData.CreatedDate;
                                    
                                    //DetachLocal(_dbContextResolver.GetContext(), dbData, dbData.Id.ToString());
                                    _dbContextResolver.GetContext().Entry(dbData).State = EntityState.Detached;
                                    k.DeletedBy = userID;
                                    k.DeletedDate = DateTime.UtcNow;
                                    if (k.AuthProcedureCPTLink != null)
                                    {
                                        k.AuthProcedureCPTLink.ForEach(h => {
                                            var dbDataCPT = _dbContextResolver.GetDbSet<AuthProcedureCPT>().Where(o => o.Id == h.Id).FirstOrDefault();
                                            h.CreatedDate = dbDataCPT.CreatedDate;
                                            h.CreatedBy = dbDataCPT.CreatedBy;
                                            h.IsActive = dbDataCPT.IsActive;
                                            _dbContextResolver.GetContext().Entry(dbDataCPT).State = EntityState.Detached;
                                            h.IsDeleted = true;
                                            h.DeletedBy = userID;
                                            h.DeletedDate = DateTime.UtcNow;
                                            h.BlockedUnit = dbDataCPT.BlockedUnit;
                                        });
                                    }
                                }
                                else
                                {
                                    if (k.Id != 0)
                                    {
                                        var dbData = _dbContextResolver.GetDbSet<AuthorizationProcedures>().Where(o => o.Id == k.Id).FirstOrDefault();
                                        k.CreatedBy = dbData.CreatedBy;
                                        k.CreatedDate = dbData.CreatedDate;
                                        k.IsActive = dbData.IsActive;
                                        _dbContextResolver.GetContext().Entry(dbData).State = EntityState.Detached;
                                        k.UpdatedBy = userID;
                                        k.UpdatedDate = DateTime.UtcNow;

                                        if (k.AuthProcedureCPTLink != null)
                                        {
                                            k.AuthProcedureCPTLink.ForEach(h =>
                                            {
                                                if (h.IsDeleted == true)
                                                {
                                                    var dbDataCPT = _dbContextResolver.GetDbSet<AuthProcedureCPT>().Where(o => o.Id == h.Id).FirstOrDefault();
                                                    h.CreatedBy = dbDataCPT.CreatedBy;
                                                    h.CreatedDate = dbDataCPT.CreatedDate;
                                                    h.IsActive = dbDataCPT.IsActive;
                                                    _dbContextResolver.GetContext().Entry(dbDataCPT).State = EntityState.Detached;
                                                    h.DeletedBy = userID;
                                                    h.DeletedDate = DateTime.UtcNow;
                                                    h.BlockedUnit = dbDataCPT.BlockedUnit;
                                                }
                                                else
                                                {
                                                    if(h.Id!=0)
                                                    {
                                                        var dbDataCPT = _dbContextResolver.GetDbSet<AuthProcedureCPT>().Where(o => o.Id == h.Id).FirstOrDefault();
                                                        h.CreatedBy = dbDataCPT.CreatedBy;
                                                        h.CreatedDate = dbDataCPT.CreatedDate;
                                                        h.IsActive = dbDataCPT.IsActive;
                                                        _dbContextResolver.GetContext().Entry(dbDataCPT).State = EntityState.Detached;
                                                        h.UpdatedBy = userID;
                                                        h.UpdatedDate = DateTime.UtcNow;
                                                        h.BlockedUnit = dbDataCPT.BlockedUnit;
                                                    }
                                                    else
                                                    {
                                                        h.CreatedBy = userID;
                                                        h.CreatedDate = DateTime.UtcNow;
                                                    }

                                                }
                                            });
                                        }
                                    }
                                    else
                                    {
                                        k.CreatedBy = userID;
                                        k.CreatedDate = DateTime.UtcNow;
                                        if (k.AuthProcedureCPTLink != null)
                                        {
                                            k.AuthProcedureCPTLink.ForEach(h =>
                                            {
                                                if (h.IsDeleted == true)
                                                {
                                                    h.DeletedBy = userID;
                                                    h.DeletedDate = DateTime.UtcNow;
                                                }
                                                else
                                                {
                                                    h.CreatedBy = userID;
                                                    h.CreatedDate = DateTime.UtcNow;
                                                }
                                            });
                                        }
                                    }
                                }
                            });

                            //var authProceduresAdded = authorizationProcedure.AuthorizationProcedures.Where(j => j.Id == 0).ToList();

                            //var authProceduresUpdated = authorizationProcedure.AuthorizationProcedures.Where(j => j.Id != 0).ToList();

                            _dbContextResolver.GetDbSet<AuthorizationProcedures>().UpdateRange(authorizationProcedure.AuthorizationProcedures);
                             _dbContextResolver.GetContext().SaveChanges();

                            //foreach (AuthorizationProcedures procedure in authProceduresAdded)
                            //{
                            //    await base.PostAsync(procedure);
                            //}

                            //foreach (AuthorizationProcedures procedure in authProceduresUpdated)
                            //{
                            //    _dbContextResolver.GetDbSet<AuthorizationProcedures>().Update(procedure);
                            //    await _dbContextResolver.GetContext().SaveChangesAsync();
                            //}


                            Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)

                        }
                    }
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.AuthorizationProcedureSaved,
                        StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                    });
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.Success,
                        StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPatch]
        [Route("DeleteAsync/{id}")]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }

        //    public static void DetachLocal<T>(this DbContext context, T t, string entryId)
        //where T : class, IIdentifier
        //    {
        //        var local = context.Set<T>()
        //            .Local
        //            .FirstOrDefault(entry => entry.Id.Equals(entryId));
        //        if (local !=null)
        //        {
        //            context.Entry(local).State = EntityState.Detached;
        //        }
        //        context.Entry(t).State = EntityState.Modified;
        //    }
        #endregion


    }
}