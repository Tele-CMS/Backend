using HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.AuditLog;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HC.Patient.Repositories.Repositories.AuditLog
{
    public class AuditLogRepository : RepositoryBase<AuditLogs>, IAuditLogRepository
    {
        private HCOrganizationContext _context;
        
        public AuditLogRepository(HCOrganizationContext context):base(context)
        {
            this._context = context;        
        }
        public void SaveChangesWithAuditLogs(string screenName, string action, int? patientId, int? userId,string parentInfo,TokenModel token)
        {
            List<AuditLogs> auditLogs = GetChanges(screenName,action,patientId,userId,parentInfo,token);
            //assign IpAddress OrganizationId and LocationID
            auditLogs.ForEach(a => { a.OrganizationID = token.OrganizationID;a.LocationID = token.LocationID;a.IPAddress = token.IPAddress; });
            _context.AuditLogs.AddRange(auditLogs);
            _context.SaveChanges();
        }
       
        public List<AuditLogs> GetChanges(string screenName,string action, int? patientId, int? userId, string parentInfo,TokenModel token)
        {
            _context.ChangeTracker.DetectChanges();
            var Entities = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Deleted || e.State == EntityState.Modified).ToList();
           
            return TrackChanges(Entities, screenName,action, patientId, userId,parentInfo,token);
        }

        private List<AuditLogs> TrackChanges(List<EntityEntry> changeEntries, string screenName,string action, Nullable<int> patientId, Nullable<int> userId, string parentInfo,TokenModel token)
        {
            List<AuditLogs> auditInfoList = new List<AuditLogs>();
            //foreach (var item in Entities)
            //{
            //    var props = item.Entity.GetType().GetProperties().Where(x => x.PropertyType == typeof(int) || x.PropertyType == typeof(string) || x.PropertyType == typeof(DateTime));
            //    foreach (var prop in props)
            //    {
            //        var oldValue = item.OriginalValues[prop.Name]?.ToString();
            //        var newValue = item.CurrentValues[prop.Name]?.ToString();

            //    }
            //}
                foreach (var entry in changeEntries)
            {
                List<AuditLogs> auditList = new List<AuditLogs>();
                auditList = ApplyAuditLog(entry, screenName, action, patientId,userId,parentInfo,token);
                foreach (AuditLogs info in auditList)
                {
                    auditInfoList.Add(info);
                }
            }
            return auditInfoList;
        }

        private List<AuditLogs> ApplyAuditLog(EntityEntry entry,  string screenName,string action, Nullable<int> patientId,Nullable<int> userId, string parentInfo,TokenModel token)
        {
            List<AuditLogs> auditInfoList = new List<AuditLogs>();

            switch (entry.State)
            {
                case EntityState.Added:
                     auditInfoList = GetAddedProperties(entry, screenName,action, patientId, userId,parentInfo,token);
                    break;
                case EntityState.Deleted:
                     auditInfoList = GetDeletedProperties(entry, screenName, action, patientId, userId,parentInfo,token);
                    break;
                case EntityState.Modified:
                     auditInfoList = GetModifiedProperties(entry,screenName, action, patientId,userId,parentInfo,token);
                    break;
            }
            return auditInfoList;
        }

        private List<AuditLogs> GetModifiedProperties(EntityEntry entry, string screenName, string action, Nullable<int> patientId, Nullable<int> userId, string parentInfo, TokenModel token)
        {
            
            List<AuditLogs> auditInfoList = new List<AuditLogs>();
            string TableName = GetTableName(entry);
            string[] entityName = TableName.Split('_');
            if (entityName.Count() == 2)
                TableName = entityName[0];
            PropertyValues dbValues = entry.GetDatabaseValues();
             foreach (var property in entry.OriginalValues.Properties)
            {
                string newVal =  Convert.ToString(entry.CurrentValues[property]).Trim();
                string oldVal = Convert.ToString(dbValues[property]).Trim();
                AuditLogs auditInfo = new AuditLogs();
                if (oldVal != null && !oldVal.Equals(newVal))
                {
                    int TableId = _context.AuditLogTable.Where(p => p.TableName == TableName && p.IsActive == true && p.OrganizationID == token.OrganizationID).Select(p => p.Id).FirstOrDefault();
                    var ColumnId = _context.AuditLogColumn.Where(p => p.AuditLogTableId == TableId && p.ColumnName == property.Name && p.IsActive == true).Select(p => p.Id).FirstOrDefault();
                    string query = _context.AuditLogColumn.Where(p => p.AuditLogTableId == TableId && p.ColumnName == property.Name && p.IsActive == true).Select(p => p.Query).FirstOrDefault();
                    if (ColumnId != 0)
                    {
                        auditInfo.PatientId = patientId;
                        auditInfo.AuditLogColumnId = ColumnId;
                        auditInfo.ScreenName = screenName;
                        auditInfo.CreatedDate = DateTime.UtcNow; // UTC Time for manage Time according to time zone
                        auditInfo.CreatedById = userId;
                        auditInfo.Action = action;
                        auditInfo.ParentInfo = parentInfo;
                        //if (!string.IsNullOrEmpty(query))
                        //{
                        //    GetForeignTableData dt = null;
                        //    int n;
                        //    bool isNumeric = int.TryParse(Convert.ToString(entry.CurrentValues[property]), out n);
                        //    if (entry.CurrentValues[property] != null)
                        //    {

                        //        dt = _context.ExecuteQuery<GetForeignTableData>(query.Replace("@Id", isNumeric == true ? Convert.ToString(entry.CurrentValues[property]) : "'" + Convert.ToString(entry.CurrentValues[property]) + "'"), 0).FirstOrDefault();
                        //        if (dt != null)
                        //            auditInfo.NewValue = Convert.ToString(dt.Value);
                        //    }
                        //    else
                        //    {
                        //        auditInfo.NewValue = null;
                        //    }

                        //    isNumeric = int.TryParse(Convert.ToString(dbValues[property]), out n);
                        //    if (dbValues[property] != null)
                        //    {
                        //        dt = _context.ExecuteQuery<GetForeignTableData>(query.Replace("@Id", isNumeric == true ? Convert.ToString(dbValues[property]) : "'" + Convert.ToString(dbValues[property]) + "'"), 0).FirstOrDefault();
                        //        if (dt != null)
                        //            auditInfo.OldValue = Convert.ToString(dt.Value);
                        //    }
                        //    else
                        //    {
                        //        auditInfo.OldValue = null;
                        //    }
                        //}
                        //else
                        //{
                            auditInfo.OldValue = dbValues[property] != null && Convert.ToString(dbValues[property]) != string.Empty ? Convert.ToString(dbValues[property]) : null;
                            auditInfo.NewValue = entry.CurrentValues[property] != null && Convert.ToString(entry.CurrentValues[property]) != string.Empty ? Convert.ToString(entry.CurrentValues[property]) : null;
                        //}
                        auditInfo.EncryptionCode =
                           CommonMethods.GetHashValue(
                              screenName.Trim() +
                              action.Trim() +
                              (auditInfo.OldValue != null ? auditInfo.OldValue : string.Empty) +
                              (auditInfo.NewValue != null ? auditInfo.NewValue : string.Empty) +
                              (auditInfo.AuditLogColumnId != null ? Convert.ToString(auditInfo.AuditLogColumnId) : string.Empty) +
                              (auditInfo.CreatedById != null ? Convert.ToString(auditInfo.CreatedById) : string.Empty) +
                              (Convert.ToString(auditInfo.CreatedDate)) +
                              (patientId != null ? Convert.ToString(patientId) : string.Empty) +
                              (token.IPAddress!=null?token.IPAddress.Trim():"") +
                              Convert.ToString(token.OrganizationID) +
                              Convert.ToString(token.LocationID));
                        auditInfoList.Add(auditInfo);
                    }
                }
            }
            return auditInfoList;
        }

        private List<AuditLogs> GetAddedProperties(EntityEntry entry, string screenName, string action, Nullable<int> patientId, Nullable<int> userId, string parentInfo, TokenModel token)
        {
            List<AuditLogs> auditInfoList = new List<AuditLogs>();
            string TableName = GetTableName(entry);
            string[] entityName = TableName.Split('_');
            if (entityName.Count() == 2)
                TableName = entityName[0];
            foreach (var property in entry.CurrentValues.Properties)
            {
                AuditLogs auditInfo = new AuditLogs();
                int TableId = _context.AuditLogTable.Where(p => p.TableName == TableName && p.IsActive == true && p.OrganizationID == token.OrganizationID).Select(p => p.Id).FirstOrDefault();
                var ColumnId = _context.AuditLogColumn.Where(p => p.AuditLogTableId == TableId && p.ColumnName == property.Name && p.IsActive == true).Select(p => p.Id).FirstOrDefault();
                string query = _context.AuditLogColumn.Where(p => p.AuditLogTableId == TableId && p.ColumnName == property.Name && p.IsActive == true).Select(p => p.Query).FirstOrDefault();
                if (ColumnId != 0)
                {
                    auditInfo.PatientId = patientId;
                    auditInfo.Action = action;
                    if (!string.IsNullOrEmpty(query))
                    {
                        int n;
                        bool isNumeric = int.TryParse(Convert.ToString(entry.CurrentValues[property]), out n);
                        if (entry.CurrentValues[property] != null)
                        {
                            GetForeignTableData dt = null;
                           dt = _context.ExecuteSqlQuery<GetForeignTableData>(_context, query.Replace("@Id", isNumeric == true ? Convert.ToString(entry.CurrentValues[property]) : "'" + Convert.ToString(entry.CurrentValues[property]) + "'")).FirstOrDefault();
                           //  dt = _context.ExecuteQuery<GetForeignTableData>(query.Replace("@Id", isNumeric == true ? Convert.ToString(entry.CurrentValues[property]) : "'" + Convert.ToString(entry.CurrentValues[property]) + "'"),0).FirstOrDefault();
                            if (dt != null)
                                auditInfo.NewValue = Convert.ToString(dt.Value);
                        }
                        else { auditInfo.NewValue = null; }
                    }
                    else { auditInfo.NewValue = entry.CurrentValues[property] != null && Convert.ToString(entry.CurrentValues[property]) != string.Empty ? Convert.ToString(entry.CurrentValues[property]) : null; }
                    auditInfo.AuditLogColumnId = ColumnId;
                    auditInfo.CreatedById = userId;
                    auditInfo.ScreenName = screenName;
                    auditInfo.CreatedDate = DateTime.UtcNow;  // UTC Time for manage Time according to time zone
                    auditInfo.ParentInfo = parentInfo;
                    auditInfo.EncryptionCode =
                               CommonMethods.GetHashValue(
                              screenName.Trim() +
                              action.Trim() +
                              (auditInfo.OldValue != null ? auditInfo.OldValue : string.Empty) +
                              (auditInfo.NewValue != null ? auditInfo.NewValue : string.Empty) +
                              (auditInfo.AuditLogColumnId != null ? Convert.ToString(auditInfo.AuditLogColumnId) : string.Empty) +
                              (auditInfo.CreatedById != null ? Convert.ToString(auditInfo.CreatedById) : string.Empty) +
                              Convert.ToString(auditInfo.CreatedDate) +
                              (patientId != null ? Convert.ToString(patientId) : string.Empty) +
                              (string.IsNullOrEmpty(token.IPAddress) ? string.Empty : token.IPAddress.Trim()) +
                              Convert.ToString(token.OrganizationID) +
                              Convert.ToString(token.LocationID));
                    auditInfoList.Add(auditInfo);
                }
            }
            return auditInfoList;
        }

        private List<AuditLogs> GetDeletedProperties(EntityEntry entry, string screenName,string action, Nullable<int> patientId, Nullable<int> userId, string parentInfo,TokenModel token)
        {
            List<AuditLogs> auditInfoList = new List<AuditLogs>();
            string TableName = GetTableName(entry);
            string[] entityName = TableName.Split('_');
            if (entityName.Count() == 2)
                TableName = entityName[0];
        
            PropertyValues dbValues = entry.GetDatabaseValues();
            foreach (var property in entry.OriginalValues.Properties)
            {
                AuditLogs auditInfo = new AuditLogs();
                int TableId = _context.AuditLogTable.Where(p => p.TableName == TableName && p.IsActive==true && p.OrganizationID==token.OrganizationID).Select(p => p.Id).FirstOrDefault();
                var ColumnId = _context.AuditLogColumn.Where(p => p.AuditLogTableId == TableId && p.ColumnName == property.Name && p.IsActive==true).Select(p => p.Id).FirstOrDefault();
                string query = _context.AuditLogColumn.Where(p => p.AuditLogTableId == TableId && p.ColumnName == property.Name && p.IsActive == true).Select(p => p.Query).FirstOrDefault();
                if (ColumnId != 0)
                {
                    auditInfo.Action = action;
                    if (!string.IsNullOrEmpty(query))
                    {
                        int n;
                        bool isNumeric = int.TryParse(Convert.ToString(dbValues[property]), out n);
                        if (dbValues[property] != null)
                        {
                            GetForeignTableData dt = null;
                            dt = _context.ExecuteQuery<GetForeignTableData>(query.Replace("@Id", isNumeric == true ? Convert.ToString(dbValues[property]) : "'" + Convert.ToString(dbValues[property]) + "'"),0).FirstOrDefault();
                            if (dt!=null)
                                auditInfo.OldValue = Convert.ToString(dt.Value);
                        }
                        else
                        {
                            auditInfo.OldValue = null;
                        }
                    }
                    else
                    { auditInfo.OldValue = dbValues[property] != null && Convert.ToString(dbValues[property]) != string.Empty ? Convert.ToString(dbValues[property]) : null;}

                    auditInfo.AuditLogColumnId = ColumnId;
                    auditInfo.CreatedById =userId;
                    auditInfo.ScreenName = screenName;
                    auditInfo.CreatedDate = DateTime.UtcNow; // UTC Time for manage Time according to time zone
                    auditInfo.ParentInfo = parentInfo;
                    auditInfo.EncryptionCode =
                              CommonMethods.GetHashValue(
                              screenName.Trim() +
                              action.Trim() +
                              (auditInfo.OldValue != null ? auditInfo.OldValue : string.Empty) +
                              (auditInfo.NewValue != null ? auditInfo.NewValue : string.Empty) +
                              (auditInfo.AuditLogColumnId != null ? Convert.ToString(auditInfo.AuditLogColumnId) : string.Empty) +
                              (auditInfo.CreatedById != null ? Convert.ToString(auditInfo.CreatedById) : string.Empty) +
                              Convert.ToString(auditInfo.CreatedDate) +
                              (patientId != null ? Convert.ToString(patientId) : string.Empty) +
                              token.IPAddress.Trim() +
                              Convert.ToString(token.OrganizationID) +
                              Convert.ToString(token.LocationID));
                    auditInfoList.Add(auditInfo);
                }
            }
            return auditInfoList;
        }
        private string GetTableName(EntityEntry dbEntry)
        {
            TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
            string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;
            if (tableName.Contains("_"))
            {
                if (tableName.Split('_').Count() > 0)
                    return tableName.Split('_')[0];
                else
                    return tableName;
            }
            else
                return tableName;
        }
        private  string DatabaseDecrypt(string encryptedString)
        {
            var passphrase = "# _@hc@_!product!@SDM!";
            encryptedString = "0x0100000014D204B546A661F6C3B3841C592DC563214377A5B65B9F8506A92EFC8A29EC9C"; // temporarily hard coded


            // setup encryption settings to match decryptbypassphrase
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.Key = UTF8Encoding.UTF8.GetBytes(passphrase).Take(16).ToArray(); // stuck on getting key from passphrase
            provider.KeySize = 128;
            provider.Padding = PaddingMode.Zeros;
            // setup data to be decrypted
            byte[] encryptedStringAsByteArray = Convert.FromBase64String(encryptedString);

            // hack some extra bytes up to a multiple of 8
            encryptedStringAsByteArray = encryptedStringAsByteArray.Concat(new byte[] { byte.MinValue, byte.MinValue, byte.MinValue, byte.MinValue }).ToArray(); // add 4 empty bytes to make 32 bytes
            MemoryStream encryptedStringAsMemoryStream = new MemoryStream(encryptedStringAsByteArray);
            // decrypt
            CryptoStream cryptoStream = new CryptoStream(encryptedStringAsMemoryStream, provider.CreateDecryptor(), CryptoStreamMode.Read);
            // return the result
            StreamReader cryptoStreamReader = new StreamReader(cryptoStream);
            string decryptedString = cryptoStreamReader.ReadToEnd();
            return decryptedString;
        }
        public IQueryable<T> GetAuditLogList<T>(string searchText, string createdBy, string patientName, string action, string fromDate, string toDate,int organizationID,int locationID, int pageNumber, int pageSize, string sortColumn, string sortOrder) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@CreatedBy",createdBy),
                                          new SqlParameter("@PatientName",patientName),
                                          new SqlParameter("@Action",action),
                                          new SqlParameter("@FromDate",fromDate),
                                          new SqlParameter("@ToDate",toDate),
                                          new SqlParameter("@OrganizationId",organizationID),
                                          new SqlParameter("@LocationId",locationID),
                                          new SqlParameter("@PageNumber",pageNumber),
                                          new SqlParameter("@PageSize",pageSize),
                                          new SqlParameter("@SortColumn",sortColumn),
                                          new SqlParameter("@SortOrder",sortOrder),
                                          new SqlParameter("@SearchText",searchText)};
            return _context.ExecStoredProcedureListWithOutput<T>("ADT_GetAuditLogs", parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetLoginLogList<T>(string searchText,string createdBy, string patientName, string action, string fromDate, string toDate, int organizationID, int locationID, int pageNumber, int pageSize, string sortColumn, string sortOrder,int roleID) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@CreatedBy",createdBy),
                                          new SqlParameter("@PatientName",patientName),
                                          new SqlParameter("@Action",action),
                                          new SqlParameter("@FromDate",fromDate),
                                          new SqlParameter("@ToDate",toDate),
                                          new SqlParameter("@OrganizationId",organizationID),
                                          new SqlParameter("@LocationId",locationID),
                                          new SqlParameter("@PageNumber",pageNumber),
                                          new SqlParameter("@PageSize",pageSize),
                                          new SqlParameter("@SortColumn",sortColumn),
                                          new SqlParameter("@SortOrder",sortOrder),
                                          new SqlParameter("@RoleID",roleID),
                                          new SqlParameter("@SearchText",searchText)};
            return _context.ExecStoredProcedureListWithOutput<T>("ADT_GetLoginLogs", parameters.Length, parameters).AsQueryable();
        }

    }
}