using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.AuditLog;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Repositories.Interfaces;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomJsonApiController<T>
    : CustomJsonApiController<T, int> where T : class, IIdentifiable<int>
    {        
        public CustomJsonApiController(
            IJsonApiContext jsonApiContext,
            IResourceService<T, int> resourceService,
            ILoggerFactory loggerFactory, IUserCommonRepository userCommonRepository)
            : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            //_context = context;
        }

    }

    public class CustomJsonApiController<T, TId>
    : JsonApiController<T, TId> where T : class, IIdentifiable<TId>
    {        
        private readonly IDbContextResolver _dbContextResolver;
        public readonly IJsonApiContext _jsonApiContext;
        public readonly IUserCommonRepository _userCommonRepository;

        public CustomJsonApiController(
            IJsonApiContext jsonApiContext,
            IResourceService<T, TId> resourceService,
            ILoggerFactory loggerFactory, 
            IUserCommonRepository userCommonRepository)
        : base(jsonApiContext, resourceService)
        {
            _dbContextResolver = jsonApiContext.GetDbContextResolver();
            _jsonApiContext = jsonApiContext;
            _userCommonRepository = userCommonRepository;


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

        //[HttpGet]
        //public override async Task<IActionResult> GetAsync()
        //{
        //    try
        //    {
        //        return await base.GetAsync();
        //        //var asyncData = await base.GetAsync();
        //        //if (((dynamic)asyncData) != null && (((dynamic)asyncData).StatusCode != 404))
        //        //{
        //        //    return asyncData;
        //        //}
        //        //else
        //        //{
        //        //    return Json(new
        //        //    {
        //        //        data = new object(),
        //        //        Message = StatusMessage.NotFound,
        //        //        StatusCode = (int)HttpStatusCodes.NotFound
        //        //    });
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //        //return Json(new
        //        //{
        //        //    data = new object(),
        //        //    Message = StatusMessage.UnknownError,
        //        //    StatusCode = (int)HttpStatusCodes.UnprocessedEntity
        //        //});
        //    }

        //}

        [HttpGet]
        public override async Task<IActionResult> GetAsync() => await base.GetAsync();

        //[HttpGet("{id}")]
        //public override async Task<IActionResult> GetAsync(TId id)
        //{
        //    try
        //    {
        //        return await ExcludeDeletedRecords(id);
        //        //var asyncData = await ExcludeDeletedRecords(id);
        //        //if (((dynamic)asyncData) != null && (((dynamic)asyncData).StatusCode != 404))
        //        //{
        //        //    return asyncData;
        //        //}
        //        //else
        //        //{
        //        //    return Json(new
        //        //    {
        //        //        data = new object(),
        //        //        Message = StatusMessage.NotFound,
        //        //        StatusCode = (int)HttpStatusCodes.NotFound
        //        //    });
        //        //}

        //        //return await base.GetAsync(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        //return Json(new
        //        //{
        //        //    data = new object(),
        //        //    Message = StatusMessage.UnknownError,
        //        //    StatusCode = (int)HttpStatusCodes.UnprocessedEntity
        //        //});
        //        return await base.GetAsync(id);
        //    }


        //}

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(TId id)
        {
            try
            {
                return await ExcludeDeletedRecords(id);
                //return await base.GetAsync(id);
            }
            catch (Exception)
            {
                return await base.GetAsync(id);
            }

        }

        //private async Task<IActionResult> ExcludeDeletedRecords(TId id)
        //{
        //    CommonMethods commonMethods = new CommonMethods();
        //    var asyncData = await base.GetAsync(id);
        //    if (((dynamic)asyncData)!=null && (((dynamic)asyncData).StatusCode != 404))
        //    {
        //        var genericData = ((ObjectResult)asyncData).Value;
        //        if ((genericData.GetType()).Name == typeof(Staffs).Name)
        //        {
        //            try
        //            {

        //                ((Staffs)genericData).Password = CommonMethods.Decrypt(((Staffs)genericData).Password);

        //            }
        //            catch (Exception ex)
        //            {


        //            }
        //        }

        //        var listProperties = GetProperties(genericData);
        //        foreach (var property in listProperties)
        //        {
        //            if (((dynamic)((dynamic)property).PropertyType).Name == "List`1")
        //            {
        //                var propertyValue = property.GetValue(genericData, null);
        //                if (propertyValue != null)
        //                {
        //                    dynamic result = (Activator.CreateInstance(propertyValue.GetType()));
        //                    foreach (var item in (dynamic)propertyValue)
        //                    {
        //                        if (item.GetType().GetProperty("IsDeleted") != null && item.IsDeleted == false)
        //                        {
        //                            result.Add((dynamic)item);
        //                        }
        //                    }
        //                    property.SetValue(genericData, result, null);
        //                }
        //            }
        //        }


        //    ((ObjectResult)asyncData).Value = genericData;
        //        return asyncData;
        //    }
        //    else
        //    {
        //        return asyncData;
        //    }

        //}


        private async Task<IActionResult> ExcludeDeletedRecords(TId id)
        {
            var asyncData = await base.GetAsync(id);

            var genericData = ((ObjectResult)asyncData).Value;
            if ((genericData.GetType()).Name == typeof(Staffs).Name)
            {
                try
                {

                    ((Staffs)genericData).Password = CommonMethods.Decrypt(((Staffs)genericData).Password);

                }
                catch (Exception)
                {


                }
            }

            var listProperties = GetProperties(genericData);
            foreach (var property in listProperties)
            {
                if (((dynamic)((dynamic)property).PropertyType).Name == "List`1")
                {
                    var propertyValue = property.GetValue(genericData, null);
                    if (propertyValue != null)
                    {
                        dynamic result = (Activator.CreateInstance(propertyValue.GetType()));
                        foreach (var item in (dynamic)propertyValue)
                        {
                            if (item.IsDeleted == false)
                            {
                                result.Add((dynamic)item);
                            }
                        }
                        property.SetValue(genericData, result, null);
                    }
                }
            }
       ((ObjectResult)asyncData).Value = genericData;
            return asyncData;
        }

        private List<PropertyInfo> GetProperties(object obj)
        {
            return obj.GetType().GetProperties().ToList();
        }

        [HttpGet("{id}/relationships/{relationshipName}")]
        public override async Task<IActionResult> GetRelationshipsAsync(TId id, string relationshipName)
            => await base.GetRelationshipsAsync(id, relationshipName);

        [HttpGet("{id}/{relationshipName}")]
        public override async Task<IActionResult> GetRelationshipAsync(TId id, string relationshipName)
            => await base.GetRelationshipAsync(id, relationshipName);

        [HttpPost]
        public override async Task<IActionResult> PostAsync([FromBody]T entity)
        {
            try
            {
                
                SaveUserInfo(entity);
                TokenModel tokenModel = GetIPFromRequst();//get ip from request header
                await AuditLogEntityPost(entity, tokenModel.IPAddress);

                String PreString = ControllerContext.ActionDescriptor.ControllerTypeInfo.Name.Replace("Controller", "").Replace("Master", "").Replace("Patient", "Client").Replace("EDI", "Claim").Replace("PatientLabTest", "PatientLabOrder");
                string returnString = PreString;

                switch (returnString.ToUpper())
                {
                    case "ICD":
                        returnString = "Diagnosis";
                        break;
                }
                System.Text.StringBuilder SB = new System.Text.StringBuilder();
                foreach (Char C in returnString)
                {
                    if (Char.IsUpper(C))
                    {
                        SB.Append(' ');
                    }

                    if (Char.IsUpper(C) && returnString.IndexOf(C) != 0)
                    {
                        SB.Append(C.ToString().ToLower());
                    }
                    else
                    {
                        SB.Append(C);
                    }
                }


                _jsonApiContext.MetaBuilder.Add("message", StatusMessage.APISavedSuccessfully.Replace("[controller]", SB.ToString()=="Tags"?"Tag":SB.ToString()));

                _jsonApiContext.MetaBuilder.Add("statusCode", (int)HttpStatusCodes.OK);

                return await base.PostAsync(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SaveUserInfo(T entity)
        {
            StringValues authorizationToken;
            var authHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
            var authToken = authorizationToken.ToString().Replace("Bearer", "").Trim();

            var encryptData = CommonMethods.GetDataFromToken(authToken);
            int userID = 0;
            int organizationID = 0;
            if (encryptData != null && encryptData.Claims != null)
            {
                if (encryptData.Claims.Count > 0)
                {
                    userID = Convert.ToInt32(encryptData.Claims[0].Value);
                    organizationID = Convert.ToInt32(encryptData.Claims[3].Value);
                    try
                    {
                        if (ControllerContext.ActionDescriptor.ActionName == "PostAsync")
                        {
                            ((dynamic)entity).CreatedBy = userID;
                            ((dynamic)entity).CreatedDate = DateTime.UtcNow;
                            if (doesPropertyExist(((dynamic)entity), "OrganizationID"))
                            {
                                ((dynamic)entity).OrganizationID = organizationID;
                            }
                        }
                        else if (ControllerContext.ActionDescriptor.ActionName == "PatchAsync")
                        {
                            ((dynamic)entity).UpdatedBy = userID;
                            ((dynamic)entity).UpdatedDate = DateTime.UtcNow;
                            _jsonApiContext.AttributesToUpdate.Add(new AttrAttribute("UpdatedBy", "UpdatedBy"), userID);
                            _jsonApiContext.AttributesToUpdate.Add(new AttrAttribute("UpdatedDate", "UpdatedDate"), DateTime.UtcNow);
                        }
                        else if (ControllerContext.ActionDescriptor.ActionName == "DeleteAsync")
                        {
                            _jsonApiContext.AttributesToUpdate.Add(new AttrAttribute("DeletedBy", "DeletedBy"), userID);
                            _jsonApiContext.AttributesToUpdate.Add(new AttrAttribute("DeletedDate", "DeletedDate"), DateTime.UtcNow);

                        }
                    }
                    catch (Exception)
                    {


                    }
                }
            }
        }

        [HttpPatch("{id}")]
        public override async Task<IActionResult> PatchAsync(TId id, [FromBody]T entity)
        {
            try
            {
                SaveUserInfo(entity);
                //code for audit log of json api
                TokenModel tokenModel = GetIPFromRequst();//get ip from request header
                await AuditLogEntity(id, entity, tokenModel.IPAddress);

                String PreString = ControllerContext.ActionDescriptor.ControllerTypeInfo.Name.Replace("Controller", "").Replace("Master", "").Replace("Patient", "Client").Replace("EDI", "Claim").Replace("PatientLabTest", "PatientLabOrder");
                string returnString = PreString;

                switch (returnString.ToUpper())
                {
                    case "ICD":
                        returnString = "Diagnosis";
                        break;
                }
                System.Text.StringBuilder SB = new System.Text.StringBuilder();
                foreach (Char C in returnString)
                {
                    if (Char.IsUpper(C))
                    {
                        SB.Append(' ');
                    }

                    if (Char.IsUpper(C) && returnString.IndexOf(C) != 0)
                    {
                        SB.Append(C.ToString().ToLower());
                    }
                    else
                    {
                        SB.Append(C);
                    }
                }

                _jsonApiContext.MetaBuilder.Add("message", StatusMessage.APIUpdatedSuccessfully.Replace("[controller]", SB.ToString() == "Tags" ? "Tag" : SB.ToString()));
                _jsonApiContext.MetaBuilder.Add("statusCode", (int)HttpStatusCodes.OK);


                return await base.PatchAsync(id, entity);
            }
            catch (Exception)
            {
                return null;

            }
        }


        private async Task AuditLogEntityPost(T entity, string ipAddress = "")
        {
            var attrToUpdate = _jsonApiContext.AttributesToUpdate;
            //var oldEntity = _dbContextResolver.GetDbSet<T>().Where(m => m.Id.Equals(id)).FirstOrDefault();            
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            string ScreenName = null;
            string Action = null;


            String PreString = ControllerContext.ActionDescriptor.ControllerTypeInfo.Name.Replace("Controller", "");


            System.Text.StringBuilder SB = new System.Text.StringBuilder();


            foreach (Char C in PreString)
            {
                if (Char.IsUpper(C))
                {
                    SB.Append(' ');
                }

                SB.Append(C);
            }
            //switch ((ControllerNames)Enum.Parse(typeof(ControllerNames), ControllerContext.ActionDescriptor.ControllerTypeInfo.Name.ToUpper()))
            // {
            // case ControllerNames.STAFFCONTROLLER:
            if (ControllerContext.ActionDescriptor.ActionName == "PostAsync")
            {
                ScreenName = "Create " + SB.ToString();
                Action = AuditLogAction.Create;
            }
            else if (ControllerContext.ActionDescriptor.ActionName == "PatchAsync")
            {
                ScreenName = "Update " + SB.ToString();
                Action = AuditLogAction.Modify;
            }
            else if (ControllerContext.ActionDescriptor.ActionName == "DeleteAsync")
            {
                ScreenName = "Delete " + SB.ToString();
                Action = AuditLogAction.Delete;

            }

            StringValues authorizationToken;
            var authHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
            var authToken = authorizationToken.ToString().Replace("Bearer", "").Trim();

            var encryptData = CommonMethods.GetDataFromToken(authToken);
            int userID = 0;

            if (encryptData != null && encryptData.Claims != null)
            {
                if (encryptData.Claims.Count > 0)
                {
                    userID = Convert.ToInt32(encryptData.Claims[0].Value);
                }
            }


            List<AuditLogs> auditLogs = GetAuditLogValuesPost(entity, typeof(T).Name, attrToUpdate)
        .Select(q => new AuditLogs()
        {
            NewValue = q.NewValue,
            OldValue = q.OldValue,
            Action = Action,
            ScreenName = ScreenName,
            AuditLogColumnId = q.ColumnID,
            CreatedDate = DateTime.UtcNow,
            IPAddress = ipAddress,
            CreatedById = userID == 0 ? 1 : userID,
            LocationID = token.LocationID,
            OrganizationID = token.OrganizationID
        }).ToList();
            await _dbContextResolver.GetDbSet<AuditLogs>().AddRangeAsync(auditLogs);
        }

        private async Task AuditLogEntity(TId id, T entity, string ipAddress = "")
        {
            var attrToUpdate = _jsonApiContext.AttributesToUpdate;
            var oldEntity = _dbContextResolver.GetDbSet<T>().Where(m => m.Id.Equals(id)).FirstOrDefault();            
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            string ScreenName = null;
            string Action = null;


            String PreString = ControllerContext.ActionDescriptor.ControllerTypeInfo.Name.Replace("Controller", "");


            System.Text.StringBuilder SB = new System.Text.StringBuilder();


            foreach (Char C in PreString)
            {
                if (Char.IsUpper(C))
                {
                    SB.Append(' ');
                }

                SB.Append(C);
            }
            //switch ((ControllerNames)Enum.Parse(typeof(ControllerNames), ControllerContext.ActionDescriptor.ControllerTypeInfo.Name.ToUpper()))
            // {
            // case ControllerNames.STAFFCONTROLLER:
            if (ControllerContext.ActionDescriptor.ActionName == "PostAsync")
            {
                ScreenName = "Create " + SB.ToString();
                Action = AuditLogAction.Create;
            }
            else if (ControllerContext.ActionDescriptor.ActionName == "PatchAsync")
            {
                ScreenName = "Update " + SB.ToString();
                Action = AuditLogAction.Modify;
            }
            else if (ControllerContext.ActionDescriptor.ActionName == "DeleteAsync")
            {
                ScreenName = "Delete " + SB.ToString();
                Action = AuditLogAction.Delete;
            }

            StringValues authorizationToken;
            var authHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
            var authToken = authorizationToken.ToString().Replace("Bearer", "").Trim();

            var encryptData = CommonMethods.GetDataFromToken(authToken);
            int userID = 0;

            if (encryptData != null && encryptData.Claims != null)
            {
                if (encryptData.Claims.Count > 0)
                {
                    userID = Convert.ToInt32(encryptData.Claims[0].Value);
                }
            }


            List<AuditLogs> auditLogs = GetAuditLogValues(oldEntity, entity, typeof(T).Name, attrToUpdate)
        .Select(q => new AuditLogs()
        {
            NewValue = q.NewValue,
            OldValue = q.OldValue,
            Action = Action,
            ScreenName = ScreenName,
            AuditLogColumnId = q.ColumnID,
            CreatedDate = DateTime.UtcNow,
            IPAddress=ipAddress,
            CreatedById = userID == 0 ? 1 : userID,
            LocationID = token.LocationID,
            OrganizationID = token.OrganizationID

        }).ToList();
            await _dbContextResolver.GetDbSet<AuditLogs>().AddRangeAsync(auditLogs);
        }

        private async Task AuditLogEntityDelete(TId id, T entity, string ipAddress = "")
        {
            var attrToUpdate = _jsonApiContext.AttributesToUpdate;
            var oldEntity = _dbContextResolver.GetDbSet<T>().Where(m => m.Id.Equals(id)).FirstOrDefault();            
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            string ScreenName = null;
            string Action = null;


            String PreString = ControllerContext.ActionDescriptor.ControllerTypeInfo.Name.Replace("Controller", "");


            System.Text.StringBuilder SB = new System.Text.StringBuilder();


            foreach (Char C in PreString)
            {
                if (Char.IsUpper(C))
                {
                    SB.Append(' ');
                }

                SB.Append(C);
            }
            //switch ((ControllerNames)Enum.Parse(typeof(ControllerNames), ControllerContext.ActionDescriptor.ControllerTypeInfo.Name.ToUpper()))
            // {
            // case ControllerNames.STAFFCONTROLLER:
            if (ControllerContext.ActionDescriptor.ActionName == "PostAsync")
            {
                ScreenName = "Create " + SB.ToString();
                Action = AuditLogAction.Create;
            }
            else if (ControllerContext.ActionDescriptor.ActionName == "PatchAsync")
            {
                ScreenName = "Update " + SB.ToString();
                Action = AuditLogAction.Modify;
            }
            else if (ControllerContext.ActionDescriptor.ActionName == "DeleteAsync")
            {
                ScreenName = "Delete " + SB.ToString();
                Action = AuditLogAction.Delete;
            }

            StringValues authorizationToken;
            var authHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
            var authToken = authorizationToken.ToString().Replace("Bearer", "").Trim();

            var encryptData = CommonMethods.GetDataFromToken(authToken);
            int userID = 0;

            if (encryptData != null && encryptData.Claims != null)
            {
                if (encryptData.Claims.Count > 0)
                {
                    userID = Convert.ToInt32(encryptData.Claims[0].Value);
                }
            }

            //((dynamic)oldEntity).IsDeleted = true;
            //var oldN = oldEntity;
            //((dynamic)oldEntity).IsDeleted = false;
            //var oldE = oldEntity;

            //    List<AuditLogs> auditLogs = GetAuditLogValues(oldE, oldN, typeof(T).Name, attrToUpdate)
            //.Select(q => new AuditLogs()
            //{
            //    NewValue = q.NewValue,
            //    OldValue = q.OldValue,
            //    Action = Action,
            //    ScreenName = ScreenName,
            //    AuditLogColumnId = q.ColumnID,
            //    CreatedDate = DateTime.UtcNow,
            //    IPAddress = ipAddress,
            //    CreatedById = userID
            //}).ToList();

            List<AuditLogs> auditLogs = new List<AuditLogs>();

            string tableName = typeof(T).Name;
            if (tableName == "MasterInsuranceType")
            {
                tableName = "MasterInsuranceTypes";
            }

            var auditTable = _dbContextResolver.GetDbSet<AuditLogTable>().AsQueryable().Where(j => j.TableName == tableName && j.IsActive == true).FirstOrDefault();
            if (auditTable != null)
            {
                foreach (var oProperty in attrToUpdate.Keys)
                {

                    var auditColumn = _dbContextResolver.GetDbSet<AuditLogColumn>().AsQueryable().Where(j => j.ColumnName == oProperty.PublicAttributeName && j.IsActive == true
                    && j.AuditLogTableId == auditTable.Id).FirstOrDefault();

                    if (auditColumn != null)
                    {
                        auditLogs.Add(new AuditLogs()
                        {
                            NewValue = "true",
                            OldValue = "false",
                            Action = Action,
                            ScreenName = ScreenName,
                            AuditLogColumnId = auditColumn.Id,
                            CreatedDate = DateTime.UtcNow,
                            IPAddress = ipAddress,
                            CreatedById = userID == 0 ? 1 : userID,
                            LocationID = token.LocationID,
                            OrganizationID = token.OrganizationID
                        });
                    }
                }
            }
            await _dbContextResolver.GetDbSet<AuditLogs>().AddRangeAsync(auditLogs);
        }

        /// <summary>
        /// audit logs
        /// </summary>
        /// <param name="oOldRecord"></param>
        /// <param name="oNewRecord"></param>
        /// <param name="tableName"></param>
        /// <param name="attrToUpdate"></param>
        /// <returns></returns>
        [NonAction]
        public List<AuditLog> GetAuditLogValues(dynamic oOldRecord, dynamic oNewRecord, string tableName, Dictionary<AttrAttribute, object> attrToUpdate)
        {
            if (tableName == "MasterInsuranceType")
            {
                tableName = "MasterInsuranceTypes";
            }
            List<AuditLog> auditLog = new List<AuditLog>();

            var oType = oOldRecord.GetType();

            try
            {
                //var auditLogTable = (String)oType.Name.Trim();
                var auditTable = _dbContextResolver.GetDbSet<AuditLogTable>().AsQueryable().Where(j => j.TableName == tableName && j.IsActive==true).FirstOrDefault();
                if (auditTable != null)
                {
                    foreach (var oProperty in attrToUpdate.Keys)
                    {
                        //var oProperty = oType.
                        var oOldValue = oProperty.GetValue(oOldRecord);
                        var oNewValue = oProperty.GetValue(oNewRecord);

                        var auditColumn = _dbContextResolver.GetDbSet<AuditLogColumn>().AsQueryable().Where(j => j.ColumnName == oProperty.PublicAttributeName && j.IsActive == true
                        && j.AuditLogTableId == auditTable.Id).FirstOrDefault();
                        string sOldValue = null;
                        string sNewValue = null;
                        // this will handle the scenario where either value is null
                        if (auditColumn != null && !object.Equals(oOldValue, oNewValue))
                        {


                            if (!string.IsNullOrEmpty(auditColumn.Query))
                            {
                                GetForeignTableData dt = new GetForeignTableData();
                                int n;
                                bool isNumeric = int.TryParse(Convert.ToString(oNewValue), out n);
                                if (oNewValue != null)
                                {
                                    string query = auditColumn.Query.Replace("@Id", isNumeric == true ? Convert.ToString(oNewValue) : "'" + Convert.ToString(oNewValue) + "'");

                                    var _context = _dbContextResolver.GetContext();
                                    //dynamic result = new ExpandoObject();
                                    var connection = _context.Database.GetDbConnection();
                                    try
                                    {

                                        //open the connection for use
                                        if (connection.State == ConnectionState.Closed) { connection.Open(); }
                                        //create a command object
                                        using (var cmd = connection.CreateCommand())
                                        {
                                            //command to execute
                                            cmd.CommandText = query;
                                            cmd.CommandType = CommandType.Text;
                                            cmd.CommandTimeout = 1000;
                                            using (var reader = cmd.ExecuteReader())
                                            {
                                                dt = DataReaderMapToList<GetForeignTableData>(reader).FirstOrDefault();
                                                reader.NextResult();
                                            }

                                        }
                                    }
                                    catch (Exception)
                                    { }
                                    sNewValue = dt.Value;
                                }
                                else
                                {
                                    sNewValue = null;
                                }

                                //isNumeric = int.TryParse(Convert.ToString(oOldValue), out n);
                                if (oOldValue != null)
                                {
                                    string query = auditColumn.Query.Replace("@Id", isNumeric == true ? Convert.ToString(oOldValue) : "'" + Convert.ToString(oOldValue) + "'");

                                    var _context = _dbContextResolver.GetContext();
                                    //dynamic result = new ExpandoObject();
                                    var connection = _context.Database.GetDbConnection();
                                    try
                                    {

                                        //open the connection for use
                                        if (connection.State == ConnectionState.Closed) { connection.Open(); }
                                        //create a command object
                                        using (var cmd = connection.CreateCommand())
                                        {
                                            //command to execute
                                            cmd.CommandText = query;
                                            cmd.CommandType = CommandType.Text;
                                            cmd.CommandTimeout = 1000;
                                            using (var reader = cmd.ExecuteReader())
                                            {
                                                dt = DataReaderMapToList<GetForeignTableData>(reader).FirstOrDefault();
                                                reader.NextResult();
                                            }

                                        }
                                    }
                                    catch (Exception)
                                    { }
                                    sOldValue = dt.Value;
                                }
                                else
                                {
                                    sOldValue = null;
                                }
                            }

                            else
                            {
                                // Handle the display values when the underlying value is null
                                sOldValue = oOldValue == null ? null : oOldValue.ToString();
                                sNewValue = oNewValue == null ? null : oNewValue.ToString();
                            }
                            auditLog.Add(new AuditLog() { ColumnID = auditColumn.Id, TableName = tableName, PropertyName = oProperty.InternalAttributeName, OldValue = sOldValue, NewValue = sNewValue });
                        }



                    }
                }

                return auditLog;
            }
            catch (Exception )
            {
                throw;
            }


        }

        [NonAction]
        public List<AuditLog> GetAuditLogValuesPost(dynamic oNewRecord, string tableName, Dictionary<AttrAttribute, object> attrToUpdate)
        {
            if(tableName== "MasterInsuranceType")
            {
                tableName = "MasterInsuranceTypes";
            }
            List<AuditLog> auditLog = new List<AuditLog>();

            var oType = oNewRecord.GetType();

            try
            {
                //var auditLogTable = (String)oType.Name.Trim();
                var auditTable = _dbContextResolver.GetDbSet<AuditLogTable>().AsQueryable().Where(j => j.TableName == tableName && j.IsActive == true).FirstOrDefault();
                if (auditTable != null)
                {
                    foreach (var oProperty in attrToUpdate.Keys)
                    {
                        //var oProperty = oType.
                        object oOldValue = null;
                        object oNewValue = oProperty.GetValue(oNewRecord);

                        var auditColumn = _dbContextResolver.GetDbSet<AuditLogColumn>().AsQueryable().Where(j => j.ColumnName == oProperty.PublicAttributeName && j.IsActive == true
                        && j.AuditLogTableId == auditTable.Id).FirstOrDefault();
                        string sOldValue = null;
                        string sNewValue = null;
                        // this will handle the scenario where either value is null
                        if (auditColumn != null && !object.Equals(oOldValue, oNewValue))
                        {


                            if (!string.IsNullOrEmpty(auditColumn.Query))
                            {
                                GetForeignTableData dt = new GetForeignTableData();
                                int n;
                                bool isNumeric = int.TryParse(Convert.ToString(oNewValue), out n);
                                if (oNewValue != null)
                                {
                                    string query = auditColumn.Query.Replace("@Id", isNumeric == true ? Convert.ToString(oNewValue) : "'" + Convert.ToString(oNewValue) + "'");

                                    var _context = _dbContextResolver.GetContext();
                                    //dynamic result = new ExpandoObject();
                                    var connection = _context.Database.GetDbConnection();
                                    try
                                    {

                                        //open the connection for use
                                        if (connection.State == ConnectionState.Closed) { connection.Open(); }
                                        //create a command object
                                        using (var cmd = connection.CreateCommand())
                                        {
                                            //command to execute
                                            cmd.CommandText = query;
                                            cmd.CommandType = CommandType.Text;
                                            cmd.CommandTimeout = 1000;
                                            using (var reader = cmd.ExecuteReader())
                                            {
                                                dt = DataReaderMapToList<GetForeignTableData>(reader).FirstOrDefault();
                                                reader.NextResult();
                                            }

                                        }
                                    }
                                    catch (Exception)
                                    { }

                                    try
                                    {
                                        sNewValue = dt.Value;
                                    }
                                    catch (Exception)
                                    {
                                    }

                                }
                                else
                                {
                                    sNewValue = null;
                                }

                                //isNumeric = int.TryParse(Convert.ToString(oOldValue), out n);
                                if (oOldValue != null)
                                {
                                    string query = auditColumn.Query.Replace("@Id", isNumeric == true ? Convert.ToString(oOldValue) : "'" + Convert.ToString(oOldValue) + "'");

                                    var _context = _dbContextResolver.GetContext();
                                    //dynamic result = new ExpandoObject();
                                    var connection = _context.Database.GetDbConnection();
                                    try
                                    {

                                        //open the connection for use
                                        if (connection.State == ConnectionState.Closed) { connection.Open(); }
                                        //create a command object
                                        using (var cmd = connection.CreateCommand())
                                        {
                                            //command to execute
                                            cmd.CommandText = query;
                                            cmd.CommandType = CommandType.Text;
                                            cmd.CommandTimeout = 1000;
                                            using (var reader = cmd.ExecuteReader())
                                            {
                                                dt = DataReaderMapToList<GetForeignTableData>(reader).FirstOrDefault();
                                                reader.NextResult();
                                            }

                                        }
                                    }
                                    catch (Exception)
                                    { }
                                    sOldValue = dt.Value;
                                }
                                else
                                {
                                    sOldValue = null;
                                }
                            }

                            else
                            {
                                // Handle the display values when the underlying value is null
                                sOldValue = oOldValue == null ? null : oOldValue.ToString();
                                sNewValue = oNewValue == null ? null : oNewValue.ToString();
                            }
                            auditLog.Add(new AuditLog() { ColumnID = auditColumn.Id, TableName = tableName, PropertyName = oProperty.InternalAttributeName, OldValue = sOldValue, NewValue = sNewValue });
                        }



                    }
                }

                return auditLog;
            }
            catch (Exception )
            {
                throw;
            }


        }

        public static IList<T> DataReaderMapToList<T>(IDataReader dr)
        {
            IList<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())               //Solution - Check if property is there in the reader and then try to remove try catch code
                {
                    try
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    catch { continue; }
                }
                list.Add(obj);
            }
            return list;
        }

        [HttpPatch("{id}/relationships/{relationshipName}")]
        public override async Task<IActionResult> PatchRelationshipsAsync(
            TId id, string relationshipName, [FromBody] List<DocumentData> relationships)
            => await base.PatchRelationshipsAsync(id, relationshipName, relationships);

        [HttpDelete("{id}")]        
        public override async Task<IActionResult> DeleteAsync(TId id)
        {

            var asyncEntity = await base.GetAsync(id);
            var entity = (T)((ObjectResult)asyncEntity).Value;
            TokenModel tokenModel = GetIPFromRequst();//get ip from request header
            string entityToString = Convert.ToString(entity);
            string entityName = entityToString.Substring((entityToString.LastIndexOf('.') + 1), entityToString.Length - (entityToString.LastIndexOf('.') + 1));
            int Id = Convert.ToInt32(id);
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(Id, DatabaseTables.DatabaseEntityName(entityName), false, tokenModel).FirstOrDefault();
            if (recordDependenciesModel == null || (recordDependenciesModel != null && recordDependenciesModel.TotalCount == 0))
            {
                _jsonApiContext.AttributesToUpdate.Add(new AttrAttribute("IsDeleted", "IsDeleted"), true);
                await AuditLogEntityDelete(id, entity);
                ((dynamic)entity).IsDeleted = true;
                SaveUserInfo(null);
                _jsonApiContext.AttributesToUpdate.Remove(new AttrAttribute("IsDeleted", "IsDeleted"));
                String PreString = ControllerContext.ActionDescriptor.ControllerTypeInfo.Name.Replace("Controller", "").Replace("Master", "").Replace("Patient", "Client");


                string returnString = PreString;

                switch (returnString.ToUpper())
                {
                    case "ICD":
                        returnString = "Diagnosis";
                        break;
                }
                System.Text.StringBuilder SB = new System.Text.StringBuilder();
                foreach (Char C in returnString)
                {
                    if (Char.IsUpper(C))
                    {
                        SB.Append(' ');
                    }

                    if (Char.IsUpper(C) && returnString.IndexOf(C) != 0)
                    {
                        SB.Append(C.ToString().ToLower());
                    }
                    else
                    {
                        SB.Append(C);
                    }
                }
                return Json(new
                {
                    data = await base.PatchAsync(entity.Id, entity),
                    Message = StatusMessage.DeletedSuccessfully.Replace("[controller]", SB.ToString() == "Tags" ? "Tag" : SB.ToString()),
                    StatusCode = (int)HttpStatusCodes.NoContent//(Invalid credentials)
                });
                //return 
            }
            else
            {
                //Response.StatusCode = (int)HttpStatusCodes.NoContent;
                return Json(new
                {
                    Message = StatusMessage.AlreadyExists,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Already Exists)
                });
            }


        }

        public static bool doesPropertyExist(dynamic obj, string property)
        {
            return ((Type)obj.GetType()).GetProperties().Where(p => p.Name.Equals(property)).Any();
        }
        private TokenModel GetIPFromRequst()
        {
            StringValues ipAddress;
            TokenModel token = new TokenModel();
            HttpContext.Request.Headers.TryGetValue("IPAddress", out ipAddress);
            if (!string.IsNullOrEmpty(ipAddress)) { token.IPAddress = ipAddress; } else { token.IPAddress = "203.129.220.76"; }
            return token;
        }
    }
}
