
using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.AuditLog;
using HC.Patient.Model.Payer;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Service.Payer.Interfaces;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    public class PayerInformationController : CustomJsonApiController<PayerServiceCodes, int>
    {
        private readonly IDbContextResolver _dbContextResolver;
        private readonly IPayerInformationService _payerServiceCodesService;
        
        #region Construtor of the class
        public PayerInformationController(
        IJsonApiContext jsonApiContext,
        IResourceService<PayerServiceCodes, int> resourceService,
        ILoggerFactory loggerFactory, IPayerInformationService payerServiceCodesService, IUserCommonRepository userCommonRepository)
        : base(jsonApiContext, resourceService, loggerFactory, userCommonRepository)
        {
            try
            {
                _payerServiceCodesService = payerServiceCodesService;                
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
            catch
            {

            }
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// this method is used to save 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("postMultiple")]
        public List<PayerServiceCodes> PatchAsync([FromBody]JArray entity)
        {
            try
            {
                 List<PayerServiceCodes> payerServiceCodes = entity.ToObject<List<PayerServiceCodes>>();
                return _payerServiceCodesService.UpdatePayerServiceCodesToDB(payerServiceCodes);
                

            }
            catch (Exception )
            {
                throw;
            }
        }


        [HttpPost("UpdatePayerInformation")]
        public List<PayerInformationModel> UpdatePayerInformation([FromBody]PayerInfoUpdateModel payerInfoUpdateModel)
        {   
            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                var attrToUpdate = _jsonApiContext.AttributesToUpdate;
                if (payerInfoUpdateModel.PayerInformationList != null && payerInfoUpdateModel.PayerInformationList.Count > 0)
                {
                    payerInfoUpdateModel.PayerInformationList.ForEach(j =>
                    {

                        var oldEntity = _dbContextResolver.GetDbSet<InsuranceCompanies>().Where(m => m.Id.Equals(j.Id)).FirstOrDefault();

                        string ScreenName = null;
                        string Action = null;

                        if (payerInfoUpdateModel.IsUpdate == true)
                        {
                            ScreenName = AuditLogsScreen.UpdatePayerInfo;
                            Action = AuditLogAction.Modify;
                        }
                        else if (payerInfoUpdateModel.IsInsert == true)
                        {
                            ScreenName = AuditLogsScreen.CreatePayerInfo;
                            Action = AuditLogAction.Create;
                        }
                        else if (payerInfoUpdateModel.IsDelete == true)
                        {
                            ScreenName = AuditLogsScreen.DeletePayerInfo;
                            Action = AuditLogAction.Delete;
                            j.IsDeleted = true;
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


                        List<AuditLogs> auditLogs = GetAuditLogValuesPayer(oldEntity, j, typeof(InsuranceCompanies).Name, attrToUpdate)
                    .Select(q => new AuditLogs()
                    {
                        NewValue = q.NewValue,
                        OldValue = q.OldValue,
                        Action = Action,
                        ScreenName = ScreenName,
                        AuditLogColumnId = q.ColumnID,
                        CreatedDate = DateTime.UtcNow,
                        IPAddress = token.IPAddress,
                        CreatedById = userID,
                        LocationID = token.LocationID,
                        OrganizationID = token.OrganizationID
                    }).ToList();
                        _dbContextResolver.GetDbSet<AuditLogs>().AddRangeAsync(auditLogs);
                    });
                }
                else if (payerInfoUpdateModel.PayerServiceCodesList != null && payerInfoUpdateModel.PayerServiceCodesList.Count > 0)
                {   
                    payerInfoUpdateModel.PayerServiceCodesList.ForEach(j =>
                    {
                        
                        var oldEntity = _dbContextResolver.GetDbSet<PayerServiceCodes>().Where(m => m.Id.Equals(j.Id)).FirstOrDefault();

                        string ScreenName = null;
                        string Action = null;


                        if (payerInfoUpdateModel.IsUpdate == true)
                        {
                            ScreenName = AuditLogsScreen.UpdatePayerServiceCodes;
                            Action = AuditLogAction.Modify;
                        }
                        else if (payerInfoUpdateModel.IsInsert == true)
                        {
                            ScreenName = AuditLogsScreen.CreatePayerServiceCodes;
                            Action = AuditLogAction.Create;
                        }
                        else if (payerInfoUpdateModel.IsDelete == true)
                        {
                            ////TO DO Need to change the return type of this method so message can be send from backend
                            //RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>((int)j.Id, typeof(PayerServiceCodes).Name, false, token).FirstOrDefault();
                            //if (recordDependenciesModel == null || (recordDependenciesModel != null && recordDependenciesModel.TotalCount == 0))
                            //{ }
                                ScreenName = AuditLogsScreen.DeletePayerServiceCodes;
                                Action = AuditLogAction.Delete;
                                j.IsDeleted = true;
                            
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
                        List<AuditLogs> auditLogs = GetAuditLogValuesPayer(oldEntity, j, typeof(PayerServiceCodes).Name, attrToUpdate)
                    .Select(q => new AuditLogs()
                    {
                        NewValue = q.NewValue,
                        OldValue = q.OldValue,
                        Action = Action,
                        ScreenName = ScreenName,
                        AuditLogColumnId = q.ColumnID,
                        CreatedDate = DateTime.UtcNow,
                        IPAddress = token.IPAddress,
                        CreatedById = userID,
                        LocationID = token.LocationID,
                        OrganizationID = token.OrganizationID
                    }).ToList();
                        _dbContextResolver.GetDbSet<AuditLogs>().AddRangeAsync(auditLogs);
                    });
                }
                else if (payerInfoUpdateModel.PayerAppointmentTypeList != null && payerInfoUpdateModel.PayerAppointmentTypeList.Count > 0)
                {
                    payerInfoUpdateModel.PayerAppointmentTypeList.ForEach(j =>
                    {
                        var oldEntity = _dbContextResolver.GetDbSet<PayerAppointmentTypes>().Where(m => m.Id.Equals(j.Id)).FirstOrDefault();

                        string ScreenName = null;
                        string Action = null;
                        if (payerInfoUpdateModel.IsUpdate == true)
                        {
                            ScreenName = AuditLogsScreen.UpdatePayerActivity;
                            Action = AuditLogAction.Modify;
                        }
                        else if (payerInfoUpdateModel.IsInsert == true)
                        {
                            ScreenName = AuditLogsScreen.CreatePayerActivity;
                            Action = AuditLogAction.Create;
                        }
                        else if (payerInfoUpdateModel.IsDelete == true)
                        {
                            ScreenName = AuditLogsScreen.DeletePayerActivity;
                            Action = AuditLogAction.Delete;
                            j.IsDeleted = true;
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
                        List<AuditLogs> auditLogs = GetAuditLogValuesPayer(oldEntity, j, typeof(PayerAppointmentTypes).Name, attrToUpdate)
                    .Select(q => new AuditLogs()
                    {
                        NewValue = q.NewValue,
                        OldValue = q.OldValue,
                        Action = Action,
                        ScreenName = ScreenName,
                        AuditLogColumnId = q.ColumnID,
                        CreatedDate = DateTime.UtcNow,
                        IPAddress = token.IPAddress,
                        CreatedById = userID,
                        LocationID = token.LocationID,
                        OrganizationID = token.OrganizationID
                    }).ToList();
                        _dbContextResolver.GetDbSet<AuditLogs>().AddRangeAsync(auditLogs);

                    });
                }
                 _dbContextResolver.GetContext().SaveChanges();
                return _payerServiceCodesService.UpdatePayerInformation(payerInfoUpdateModel);
                //return null;

            }
            catch (Exception)
            {
                return null;
            }
        }

        [NonAction]
        public List<AuditLog> GetAuditLogValuesPayer(dynamic oOldRecord, dynamic oNewRecord, string tableName, Dictionary<AttrAttribute, object> attrToUpdate)
        {

            List<AuditLog> auditLog = new List<AuditLog>();

            var oType = oNewRecord.GetType();

            try
            {
                //var auditLogTable = (String)oType.Name.Trim();
                var auditTable = _dbContextResolver.GetDbSet<AuditLogTable>().AsQueryable().Where(j => j.TableName == tableName).FirstOrDefault();
                if (auditTable != null)
                {
                    List<AttrToUpdateModel> attrToUpdate1 = EnumeratePropertyDifferences(oOldRecord, oNewRecord);

                    foreach (var oProperty in attrToUpdate1)
                    {
                        //var oProperty = oType.
                        //var oOldValue = oProperty.GetValue(oOldRecord);
                        //var oNewValue = oProperty.GetValue(oNewRecord);

                        var oOldValue = oProperty.OldValue;
                        var oNewValue = oProperty.NewValue;

                        var auditColumn = _dbContextResolver.GetDbSet<AuditLogColumn>().AsQueryable().Where(j => j.ColumnName == oProperty.Name
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
                            auditLog.Add(new AuditLog() { ColumnID = auditColumn.Id, TableName = tableName, PropertyName = oProperty.Name, OldValue = sOldValue, NewValue = sNewValue });
                        }
                    }
                }

                return auditLog;
            }
            catch (Exception)
            {
                throw;
            }


        }


        [HttpPost("GetPayerInformation")]
        public JsonResult GetPayerInformation([FromBody]PayerSearchFilter payerSearchFilter)
        {
            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                payerSearchFilter.OrganizationID = token.OrganizationID;
                return Json(_payerServiceCodesService.GetPayerInformationByFilter(payerSearchFilter));
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("GetMasterServiceCodes")]
        public JsonResult GetMasterServiceCodes([FromBody]PayerSearchFilter payerSearchFilter)
        {
            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                payerSearchFilter.OrganizationID = token.OrganizationID;
                return Json(_payerServiceCodesService.GetMasterServiceCodesByFilter(payerSearchFilter));
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("GetPayerActivity")]
        public JsonResult GetPayerActivity([FromBody]PayerSearchFilter payerSearchFilter)
        {
            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                payerSearchFilter.OrganizationID = token.OrganizationID;
                return Json(_payerServiceCodesService.GetPayerActivityByFilter(payerSearchFilter));
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("GetPayerServiceCodes")]
        public JsonResult GetPayerServiceCodes([FromBody]PayerSearchFilter payerSearchFilter)
        {
            try
            {
                TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
                payerSearchFilter.OrganizationID = token.OrganizationID;
                return Json(_payerServiceCodesService.GetPayerServiceCodesByFilter(payerSearchFilter));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        [NonAction]
        public List<AttrToUpdateModel> EnumeratePropertyDifferences(dynamic obj1, dynamic obj2)
        {
            try
            {

                List<PropertyInfo> properties = GetProperties(obj2);
                List<AttrToUpdateModel> changes = new List<AttrToUpdateModel>();

                foreach (PropertyInfo pi in properties)
                {
                    if (((dynamic)((dynamic)pi).PropertyType).Name != "List`1" && pi.Name != "value")
                    {
                        object value1 = null;
                        if (obj1!=null && obj1.GetType().GetProperty(pi.Name) != null)
                        {
                            value1 = obj1.GetType().GetProperty(pi.Name).GetValue(obj1, null);
                        }

                        object value2 = null;
                        if (obj2 != null && obj2.GetType().GetProperty(pi.Name) != null)
                        {
                            value2 = obj2.GetType().GetProperty(pi.Name).GetValue(obj2, null);
                        }

                        if (value1 != value2 && (!string.IsNullOrEmpty(Convert.ToString(value2))/*  value2 != null && value2 != string.Empty*/) && pi.Name != "CreatedDate" && (value1 == null || !value1.Equals(value2)))
                        {
                            changes.Add(new AttrToUpdateModel() { Name = pi.Name, OldValue = Convert.ToString(value1), NewValue = Convert.ToString(value2) });
                        }
                    }

                }
                return changes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<PropertyInfo> GetProperties(object obj)
        {
            return obj.GetType().GetProperties().ToList();
        }

        [HttpPatch]
        [Route("DeleteAsync/{id}")]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }

        #region Helping Methods
        #endregion
    }
}