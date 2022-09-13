using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.AuditLog;
using HC.Patient.Model.Patient;
using HC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly HCOrganizationContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public object PatietnTable { get; private set; }

        public RepositoryBase(HCOrganizationContext _context)
        {
            this.context = _context;
            entities = context.Set<T>();
        }


        /// <summary>
        /// Gets the first entity found or default value.
        /// </summary>
        /// <param name="filter">Filter expression for filtering the entities.</param>
        /// <param name="include">Include for eager-loading.</param>
        /// <returns></returns>
        public virtual T GetFirstOrDefault(Expression<Func<T, bool>> filter,
                                          params Expression<Func<T, object>>[] include)
        {
            IQueryable<T> dbQuery = SelectQuery(filter, include);
            return dbQuery.AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Creates the specified entity/entities.
        /// </summary>
        /// <param name="entity">Single entity.</param>
        /// <param name="entities">Multiple entities.</param>
        public virtual void Create(T entity, params T[] entities)
        {
            EntityState state = EntityState.Added;
            SetEntityState(state, entity, entities);
        }


        /// <summary>
        /// Creates the specified entity/entities.
        /// </summary>
        /// <param name="entities">Multiple entities.</param>
        public void Create(T[] entities)
        {
            EntityState state = EntityState.Added;
            SetEntityStateForArray(state, entities);
        }

        /// <summary>
        /// Updates the specified entity/entities.
        /// </summary>
        /// <param name="entity">Single entity.</param>
        /// <param name="entities">Multiple entities.</param>
        public virtual void Update(T entity, params T[] entities)
        {
            EntityState state = EntityState.Modified;
            SetEntityState(state, entity, entities);
        }

        /// <summary>
        /// Updates the specified entity/entities.
        /// </summary>
        /// <param name="entities">Multiple entities.</param>
        public virtual void Update(T[] entities)
        {
            EntityState state = EntityState.Modified;
            SetEntityStateForArray(state, entities);
        }

        /// <summary>
        /// Deletes the specified entity/entities.
        /// </summary>
        /// <param name="entity">Single entity.</param>
        /// <param name="entities">Multiple entities.</param>
        public virtual void Delete(T entity, params T[] entities)
        {
            EntityState state = EntityState.Deleted;
            SetEntityState(state, entity, entities);
        }

        /// <summary>
        /// Deletes the entity by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public virtual void Delete(object id)
        {
            T entity = CreateDbSet<T>().Find(id);
            EntityState state = EntityState.Deleted;
            SetEntityState(state, entity);
        }

        /// <summary>
        /// Deletes multiple entities which are found using filter.
        /// </summary>
        /// <param name="filter">Filter expression for filtering the entities.</param>
        public virtual void Delete(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> dbQuery = SelectQuery(filter);
            dbQuery.AsNoTracking().ToList().ForEach(item => context.Entry(item).State = EntityState.Deleted);
        }

        /// <summary>
        /// Saves the changes to the database.
        /// </summary>
        /// <returns>Number of rows affected.</returns>
        public int SaveChanges()
        {
            int recordsAffected = context.SaveChanges();
            //this.Dispose();  // uncommented by kundan for memeory release 
            return recordsAffected;
        }



        /// <summary>
        /// Fetch all records .
        /// </summary>
        /// <returns></returns>
        /// 
        public IQueryable<T> FetchAll()
        {
            return GetAll();
        }

        public IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> exp)
        {
            return context.Set<T>().Where(exp);
        }
        public T Get(Expression<Func<T, bool>> exp)
        {
            return context.Set<T>().Where(exp).FirstOrDefault(); ;
        }

        public virtual T GetByID(object id)
        {
            return CreateDbSet<T>().Find(id);
        }

        #region Stored Procedures Factory
        //When you expect a model back (async)
        public async Task<IList<T>> ExecWithStoreProcedureAsync(string query, params object[] parameters)
        {
            // EF 6
            //context.Database.SqlQuery<T>(query, parameters).ToListAsync();
            // EF Core
            return await entities.FromSql(query, parameters).ToListAsync();
        }

        //When you expect a model back
        public IEnumerable<T> ExecWithStoreProcedure(string query)
        {
            // EF 6
            //_context.Database.SqlQuery<T>(query);
            // EF Core
            return entities.FromSql(query);
        }

        //When you expect a model back
        public IEnumerable<T> ExecWithStoreProcedureWithParameters(string query, params object[] parameters)
        {
            // EF 6
            //_context.Database.SqlQuery<T>(query, parameters);
            // EF Core
            return entities.FromSql(query, parameters);
        }

        //When you expect a model back
        public T ExecWithStoreProcedureWithParametersForModel(string query, params object[] parameters)
        {
            // EF 6
            //IEnumerable<TResult> dbQuery = _context.Database.SqlQuery<TResult>(query, parameters);
            //return dbQuery.FirstOrDefault();
            // EF Core
            IEnumerable<T> dbQuery = entities.FromSql(query, parameters);
            return dbQuery.FirstOrDefault();
        }

        // Fire and forget (async)
        public async Task ExecuteWithStoreProcedureAsync(string query, params object[] parameters)
        {
            // EF 6
            //await _context.Database.ExecuteSqlCommandAsync(query, parameters);
            // EF Core
            await context.Database.ExecuteSqlCommandAsync(query, default(CancellationToken), parameters);
        }

        // Fire and get no. of row inserted
        public int ExecuteWithStoreProcedure(string query, params object[] parameters)
        {
            return context.Database.ExecuteSqlCommand(query, parameters);
        }
        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="RepositoryBase{TEntity}"/> class.
        /// </summary>
        ~RepositoryBase()
        {
            Dispose(false);
        }
        protected DbSet<TEntity> CreateDbSet<TEntity>() where TEntity : class
        {
            return context.Set<TEntity>();
        }

        #region Private Methods
        private IQueryable<T> SelectQuery(Expression<Func<T, bool>> filter, Expression<Func<T, object>>[] include = null)
        {
            IQueryable<T> dbQuery = CreateDbSet<T>();

            if (filter != null)
            {
                dbQuery = dbQuery.Where(filter);
            }

            if (include != null)
            {
                dbQuery = include.Aggregate(dbQuery, (a, b) => a.Include(b));
            }
            return dbQuery;
        }

        private void SetEntityState(EntityState state, T entity, params T[] entities)
        {
            try
            {
                context.Entry(entity).State = state;
                foreach (T item in entities)
                {
                    context.Entry(item).State = state;
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetEntityStateForArray(EntityState state,T[] entities)
        {
            try
            {
                foreach (T item in entities)
                {
                    context.Entry(item).State = state;
                }
            }
            catch (Exception)
            {
            }
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                }
            }
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }


        #endregion Private Methods
        #region Audit Logs
        //public int SaveChangesWithAuditLogs(string screenName, string action, int? patientId, int? userId, string parentInfo, TokenModel token, PatientDemographicsModel patientDemographicsModel = null, PHIDecryptedModel pHIDecryptedModel = null)
        //{
        //    if (patientDemographicsModel == null || pHIDecryptedModel == null)
        //    {
        //        patientDemographicsModel = new PatientDemographicsModel();
        //        pHIDecryptedModel = new PHIDecryptedModel();
        //    }
        //    List<AuditLogs> auditLogs = GetChanges(screenName, action, patientId, userId, parentInfo, token, patientDemographicsModel, pHIDecryptedModel);
        //    //assign IpAddress OrganizationId and LocationID
        //    auditLogs.ForEach(a => { a.OrganizationID = token.OrganizationID; a.LocationID = token.LocationID; a.IPAddress = token.IPAddress; });
        //    context.AuditLogs.AddRange(auditLogs);
        //    return context.SaveChanges();
        //}
        //public List<AuditLogs> GetChanges(string screenName, string action, int? patientId, int? userId, string parentInfo, TokenModel token, PatientDemographicsModel patientDemographicsModel, PHIDecryptedModel patients)
        //{
        //    var Entities = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Deleted || e.State == EntityState.Modified).ToList();
        //    return TrackChanges(Entities, screenName, action, patientId, userId, parentInfo, token, patientDemographicsModel, patients);
        //}

        //private List<AuditLogs> TrackChanges(List<EntityEntry> Entities, string screenName, string action, Nullable<int> patientId, Nullable<int> userId, string parentInfo, TokenModel token, PatientDemographicsModel patientDemographicsModel, PHIDecryptedModel patients)
        //{
        //    List<AuditLogs> auditInfoList = new List<AuditLogs>();
        //    foreach (var entry in Entities)
        //    {
        //        List<AuditLogs> auditList = new List<AuditLogs>();
        //        auditList = ApplyAuditLog(entry, screenName, action, patientId, userId, parentInfo, token, patientDemographicsModel, patients);
        //        foreach (AuditLogs info in auditList)
        //        {
        //            auditInfoList.Add(info);
        //        }
        //    }
        //    return auditInfoList;
        //}

        //private List<AuditLogs> ApplyAuditLog(EntityEntry entry, string screenName, string action, Nullable<int> patientId, Nullable<int> userId, string parentInfo, TokenModel token, PatientDemographicsModel patientDemographicsModel, PHIDecryptedModel patients)
        //{
        //    List<AuditLogs> auditInfoList = new List<AuditLogs>();

        //    switch (entry.State)
        //    {
        //        case EntityState.Added:
        //            auditInfoList = GetAddedProperties(entry, screenName, action, patientId, userId, parentInfo, token);
        //            break;
        //        //case EntityState.Deleted:
        //        //    auditInfoList = GetDeletedProperties(entry, screenName, action, patientId, userId, parentInfo, token);
        //        //    break;
        //        case EntityState.Modified:
        //            auditInfoList = GetModifiedProperties(entry, screenName, action, patientId, userId, parentInfo, token, patientDemographicsModel, patients);
        //            break;
        //    }
        //    return auditInfoList;
        //}

        //private List<AuditLogs> GetModifiedProperties(EntityEntry entry, string screenName, string action, Nullable<int> patientId, Nullable<int> userId, string parentInfo, TokenModel token, PatientDemographicsModel patientDemographicsModel, PHIDecryptedModel patients)
        //{
        //    List<AuditLogs> auditInfoList = new List<AuditLogs>();
        //    string TableName = GetTableName(entry);
        //    string[] entityName = TableName.Split('_');
        //    if (entityName.Count() == 2)
        //        TableName = entityName[0];

        //    var primaryKey = entry.Metadata.FindPrimaryKey();
        //    var keys = primaryKey.Properties.ToDictionary(x => x.Name);
        //    PropertyValues dbValues = entry.GetDatabaseValues();
        //    int primaryKeyValue = 0;
        //    string newVal = string.Empty;
        //    string oldVal = string.Empty;
        //    foreach (var property in entry.OriginalValues.Properties)
        //    {
        //        newVal = null;
        //        oldVal = null;
        //        if(keys!=null && keys.Count>0)
        //            primaryKeyValue =(int)entry.CurrentValues[keys.FirstOrDefault().Key];

        //        if (TableName == DatabaseTables.PatietnTable)
        //        {

        //            switch (property.Name)
        //            {
        //                case PatientAuditLogColumn.FirstName:
        //                    newVal = patientDemographicsModel.FirstName;
        //                    oldVal = patients.FirstName;
        //                    break;
        //                case PatientAuditLogColumn.MiddleName:
        //                    newVal = patientDemographicsModel.MiddleName;
        //                    oldVal = patients.MiddleName;
        //                    break;
        //                case PatientAuditLogColumn.LastName:
        //                    newVal = patientDemographicsModel.LastName;
        //                    oldVal = patients.LastName;
        //                    break;
        //                case PatientAuditLogColumn.DOB:
        //                    newVal = patientDemographicsModel.DOB != null ? patientDemographicsModel.DOB.ToString("yyyy-MM-dd HH:mm:ss.fffffff") : null;
        //                    oldVal = patients.DateOfBirth;
        //                    break;
        //                case PatientAuditLogColumn.Email:
        //                    newVal = patientDemographicsModel.Email;
        //                    oldVal = patients.EmailAddress;
        //                    break;
        //                case PatientAuditLogColumn.Gender:
        //                    newVal = Convert.ToString(entry.CurrentValues[property]).Trim();
        //                    oldVal = Convert.ToString(dbValues[property]).Trim();
        //                    break;
        //            }
        //        }
        //        else
        //        {
        //            newVal = Convert.ToString(entry.CurrentValues[property]).Trim();
        //            oldVal = Convert.ToString(dbValues[property]).Trim();
        //        }

        //        AuditLogs auditInfo = new AuditLogs();
        //        if (oldVal != null && !oldVal.Equals(newVal))
        //        {
        //            int TableId = context.AuditLogTable.Where(p => p.TableName == TableName && p.IsActive == true && p.OrganizationID == token.OrganizationID).Select(p => p.Id).FirstOrDefault();
        //            var auditLogColumnData = context.AuditLogColumn.Where(p => p.AuditLogTableId == TableId && p.ColumnName == property.Name && p.IsActive == true).Select(p => new
        //            {
        //                ColumnId = p.Id,
        //                MasterEntityName = p.MasterEntityName
        //            }).FirstOrDefault();
        //            //string query = context.AuditLogColumn.Where(p => p.AuditLogTableId == TableId && p.ColumnName == property.Name && p.IsActive == true).Select(p => p.Query).FirstOrDefault();
        //            //string masterEntityName = context.AuditLogColumn.Where(p => p.AuditLogTableId == TableId && p.ColumnName == property.Name && p.IsActive == true).Select(p => p.Query).FirstOrDefault();
        //            if (auditLogColumnData != null && auditLogColumnData.ColumnId != 0)
        //            {
        //                auditInfo.PatientId = patientId;
        //                auditInfo.AuditLogColumnId = auditLogColumnData.ColumnId;
        //                auditInfo.ScreenName = screenName;
        //                auditInfo.CreatedDate = DateTime.UtcNow; // UTC Time for manage Time according to time zone
        //                auditInfo.CreatedById = userId;
        //                auditInfo.Action = action;
        //                auditInfo.ParentInfo = parentInfo;
        //                auditInfo.RecordReferenceNumber = primaryKeyValue;
        //                if (!string.IsNullOrEmpty(auditLogColumnData.MasterEntityName))
        //                {
        //                    int n;
        //                    int masterEntitesId = 0;
        //                    bool isNumeric = int.TryParse(Convert.ToString(entry.CurrentValues[property]), out n);
        //                    if (isNumeric)
        //                    {
        //                        masterEntitesId = int.Parse(entry.CurrentValues[property].ToString());
        //                    }

        //                    GetForeignTableData newData = null, oldData = null;

        //                    newData = GetMasterEntityValue(auditLogColumnData.MasterEntityName, masterEntitesId);
        //                    string oldId = dbValues[property] != null && Convert.ToString(dbValues[property]) != string.Empty ? Convert.ToString(dbValues[property]) : null;

        //                    if (oldId != null)
        //                    {
        //                        oldData = GetMasterEntityValue(auditLogColumnData.MasterEntityName, int.Parse(oldId));
        //                    }
        //                    else
        //                    {
        //                        oldData = null;
        //                    }

        //                    auditInfo.NewValue = newData != null ? Convert.ToString(newData.Value) : null;
        //                    auditInfo.OldValue = oldData != null ? Convert.ToString(oldData.Value) : null;
        //                }
        //                else
        //                {
        //                    auditInfo.OldValue = oldVal;
        //                    auditInfo.NewValue = newVal;

        //                }
        //                auditInfo.EncryptionCode =
        //                   CommonMethods.GetHashValue(
        //                      screenName.Trim() +
        //                      action.Trim() +
        //                      (auditInfo.OldValue != null ? auditInfo.OldValue : string.Empty) +
        //                      (auditInfo.NewValue != null ? auditInfo.NewValue : string.Empty) +
        //                      (auditInfo.AuditLogColumnId != null ? Convert.ToString(auditInfo.AuditLogColumnId) : string.Empty) +
        //                      (auditInfo.CreatedById != null ? Convert.ToString(auditInfo.CreatedById) : string.Empty) +
        //                      (Convert.ToString(auditInfo.CreatedDate)) +
        //                      (patientId != null ? Convert.ToString(patientId) : string.Empty) +
        //                      (token.IPAddress!=null?token.IPAddress.Trim():string.Empty) +
        //                      Convert.ToString(token.OrganizationID) +
        //                      Convert.ToString(token.LocationID)+
        //                      (primaryKeyValue==0?string.Empty:Convert.ToString(primaryKey))
        //                      );
        //                auditInfoList.Add(auditInfo);
        //            }
        //        }
        //    }
        //    return auditInfoList;
        //}

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
                int TableId = context.AuditLogTable.Where(p => p.TableName == TableName && p.IsActive == true && p.OrganizationID == token.OrganizationID).Select(p => p.Id).FirstOrDefault();
                var ColumnId = context.AuditLogColumn.Where(p => p.AuditLogTableId == TableId && p.ColumnName == property.Name && p.IsActive == true).Select(p => p.Id).FirstOrDefault();
                string query = context.AuditLogColumn.Where(p => p.AuditLogTableId == TableId && p.ColumnName == property.Name && p.IsActive == true).Select(p => p.Query).FirstOrDefault();
                if (ColumnId != 0 && !string.IsNullOrEmpty(Convert.ToString(entry.CurrentValues[property])))
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
                            dt = context.ExecuteSqlQuery<GetForeignTableData>(context, query.Replace("@Id", isNumeric == true ? Convert.ToString(entry.CurrentValues[property]) : "'" + Convert.ToString(entry.CurrentValues[property]) + "'")).FirstOrDefault();
                            //dt = _context.ExecuteQuery<GetForeignTableData>(query.Replace("@Id", isNumeric == true ? Convert.ToString(entry.CurrentValues[property]) : "'" + Convert.ToString(entry.CurrentValues[property]) + "'"),0).FirstOrDefault();
                            if (dt != null)
                                auditInfo.NewValue = Convert.ToString(dt.Value);
                        }
                        else { auditInfo.NewValue = null; }
                    }
                    else {
                        auditInfo.NewValue = Convert.ToString(entry.CurrentValues[property]);
                    }
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
                              (token.IPAddress != null ? token.IPAddress.Trim() : string.Empty) +
                              Convert.ToString(token.OrganizationID) +
                              Convert.ToString(token.LocationID));
                    auditInfoList.Add(auditInfo);
                }
            }
            return auditInfoList;
        }

        private List<AuditLogs> GetDeletedProperties(EntityEntry entry, string screenName, string action, Nullable<int> patientId, Nullable<int> userId, string parentInfo, TokenModel token)
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
                int TableId = context.AuditLogTable.Where(p => p.TableName == TableName && p.IsActive == true && p.OrganizationID == token.OrganizationID).Select(p => p.Id).FirstOrDefault();
                var ColumnId = context.AuditLogColumn.Where(p => p.AuditLogTableId == TableId && p.ColumnName == property.Name && p.IsActive == true).Select(p => p.Id).FirstOrDefault();
                string query = context.AuditLogColumn.Where(p => p.AuditLogTableId == TableId && p.ColumnName == property.Name && p.IsActive == true).Select(p => p.Query).FirstOrDefault();
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
                            dt = context.ExecuteQuery<GetForeignTableData>(query.Replace("@Id", isNumeric == true ? Convert.ToString(dbValues[property]) : "'" + Convert.ToString(dbValues[property]) + "'"), 0).FirstOrDefault();
                            if (dt != null)
                                auditInfo.OldValue = Convert.ToString(dt.Value);
                        }
                        else
                        {
                            auditInfo.OldValue = null;
                        }
                    }
                    else
                    { auditInfo.OldValue = dbValues[property] != null && Convert.ToString(dbValues[property]) != string.Empty ? Convert.ToString(dbValues[property]) : null; }

                    auditInfo.AuditLogColumnId = ColumnId;
                    auditInfo.CreatedById = userId;
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
                              (token.IPAddress != null ? token.IPAddress.Trim() : string.Empty) +
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
            //if (tableName.Contains("_"))
            //{
            //    if (tableName.Split('_').Count() > 0)
            //        return tableName.Split('_')[0];
            //    else
            //        return tableName;
            //}
                return tableName;
        }
        //private GetForeignTableData GetMasterEntityValue(string masterEntityName, int masterEntitesId)
        //{
        //    GetForeignTableData data = null;
        //    switch (masterEntityName)
        //    {
        //        case AuditLogMasterEntity.MasterGender:
        //            data = GetGenderValue(masterEntitesId);
        //            break;
        //        case AuditLogMasterEntity.MasterFrequencyTypes:
        //            data = GetMasterFrequencyTypeValue(masterEntitesId);
        //            break;
        //        case AuditLogMasterEntity.MasterMeasureSign:
        //            data = GetMasterMeasureSignValue(masterEntitesId);
        //            break;
        //        case AuditLogMasterEntity.MasterActivityUnitType:
        //            data = GetMasterActivityUnitTypeValue(masterEntitesId);
        //            break;
        //        case AuditLogMasterEntity.MasterNotificationTypes:
        //            data = GetMasterNotificationTypes(masterEntitesId);
        //            break;
        //        case AuditLogMasterEntity.GlobalCode:
        //            data = context.GlobalCode.Where(x => x.Id == masterEntitesId).Select(x => new GetForeignTableData
        //            {
        //                Value = x.GlobalCodeValue
        //            }).FirstOrDefault();
        //            break;
        //        default:
        //            break;
        //    }
        //    return data;
        //}
        //private GetForeignTableData GetGenderValue(int masterTableId)
        //{
        //    GetForeignTableData dt = context.MasterGender.Where(x => x.Id == masterTableId).Select(x => new GetForeignTableData
        //    {
        //        Value = x.Gender
        //    }).FirstOrDefault();
        //    return dt;
        //}
        //private GetForeignTableData GetMasterFrequencyTypeValue(int masterTableId)
        //{
        //    GetForeignTableData dt = context.MasterFrequencyTypes.Where(x => x.ID == masterTableId).Select(x => new GetForeignTableData
        //    {
        //        Value = x.Description
        //    }).FirstOrDefault();
        //    return dt;
        //}
        //private GetForeignTableData GetMasterMeasureSignValue(int masterTableId)
        //{
        //    GetForeignTableData dt = context.MasterMeasureSign.Where(x => x.ID == masterTableId).Select(x => new GetForeignTableData
        //    {
        //        Value = x.Description
        //    }).FirstOrDefault();
        //    return dt;
        //}
        //private GetForeignTableData GetMasterActivityUnitTypeValue(int masterTableId)
        //{
        //    GetForeignTableData dt = context.MasterActivityUnitType.Where(x => x.Id == masterTableId).Select(x => new GetForeignTableData
        //    {
        //        Value = x.UnitType
        //    }).FirstOrDefault();
        //    return dt;
        //}
        //private GetForeignTableData GetMasterNotificationTypes(int masterTableId)
        //{
        //    GetForeignTableData dt = context.MasterNotificationTypes.Where(x => x.Id == masterTableId).Select(x => new GetForeignTableData
        //    {
        //        Value = x.Description
        //    }).FirstOrDefault();
        //    return dt;
        //}

        public IDbContextTransaction StartTransaction()
        {
            return context.Database.BeginTransaction();
        }
        #endregion

        public List<ChangesLog> GetChangesLogData(TokenModel tokenModel, PatientDemographicsModel patientDemographicsModel = null, PHIDecryptedModel patients = null)
        {
            List<ChangesLog> AuditLogs = new List<ChangesLog>();
            var changeTrack = context.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified).ToList();
            int rowIndex = 0;
            int qid = 0;
            foreach (EntityEntry entry in changeTrack)
            {
                qid = 0;
                rowIndex++;
                if (entry.Entity != null)
                {
                    string entityName = GetTableName(entry);
                    string state = string.Empty;
                    // Get the all patient encounter linked columns
                    List<MasterPatientEncLinkedColumn> columns = context.MasterPatientEncLinkedColumn.Where(x => x.MasterPatientEncLinkedEntity.EntityName == entityName && x.MasterPatientEncLinkedEntity.OrganizationId == tokenModel.OrganizationID).ToList();
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            state = entry.State.ToString();

                            foreach (IProperty prop in entry.OriginalValues.Properties.ToList())
                            {
                                if (entityName == "PatientCareMetricsQuestionAnswer" && qid == 0)
                                    qid = (int)entry.CurrentValues[entry.CurrentValues.Properties.Where(x => x.Name == "QuestionId").FirstOrDefault().Name];

                                // check for column exist in MasterPatientEncLinkedColumn
                                MasterPatientEncLinkedColumn encLinkedColumn = columns.Where(x => x.ColumnName == prop.Name).FirstOrDefault();
                                if (!ReferenceEquals(encLinkedColumn, null))
                                {
                                    object currentValue = entry.CurrentValues[prop.Name];
                                    object originalValue = entry.OriginalValues[prop.Name];

                                    if (entityName == DatabaseTables.PatientTable)
                                    {
                                        switch (prop.Name)
                                        {
                                            case PatientAuditLogColumn.FirstName:
                                                currentValue = patientDemographicsModel.FirstName;
                                                originalValue = patients.FirstName;
                                                break;
                                            case PatientAuditLogColumn.MiddleName:
                                                currentValue = patientDemographicsModel.MiddleName;
                                                originalValue = patients.MiddleName;
                                                break;
                                            case PatientAuditLogColumn.LastName:
                                                currentValue = patientDemographicsModel.LastName;
                                                originalValue = patients.LastName;
                                                break;
                                            case PatientAuditLogColumn.DOB:
                                                currentValue = patientDemographicsModel.DOB != null ? patientDemographicsModel.DOB.ToString("yyyy-MM-dd HH:mm:ss.fffffff") : null;
                                                originalValue = patients.DateOfBirth;
                                                break;
                                            case PatientAuditLogColumn.Email:
                                                currentValue = patientDemographicsModel.Email;
                                                originalValue = patients.EmailAddress;
                                                break;
                                            //case PatientAuditLogColumn.Gender:
                                            //    currentValue = Convert.ToString(entry.CurrentValues[prop.Name]).Trim();
                                            //    originalValue = Convert.ToString(dbValues[property]).Trim();
                                            //    break;
                                            case PatientAuditLogColumn.SecondaryEmail:
                                                currentValue = patientDemographicsModel.SecondaryEmail;
                                                originalValue = patients.SecondaryEmailAddress;
                                                break;
                                        }
                                    }
                                    if (!Equals(originalValue, currentValue))
                                    {
                                        if (prop.IsForeignKey())
                                        {
                                            IEntityType[] masterEntities = prop.GetContainingForeignKeys().Select(x => x.PrincipalEntityType).ToArray();

                                            int[] masterIds = new int[] { (int)(currentValue != null ? currentValue : 0), (int)(originalValue != null ? originalValue : 0) };
                                            string[] masterValues = GetMasterEntitiesValue(masterEntities.FirstOrDefault().Name, masterIds);
                                            currentValue = masterValues.FirstOrDefault();
                                            originalValue = masterValues.LastOrDefault();
                                        }
                                        AuditLogs.Add(new ChangesLog
                                        {
                                            TableName = entityName,
                                            State = state,
                                            ColumnName = prop.Name,
                                            ColumnId = encLinkedColumn.Id,
                                            //RecordID = prop.GetContainingPrimaryKey()
                                            OriginalValue = Convert.ToString(originalValue),
                                            NewValue = Convert.ToString(currentValue),
                                            IndexNumber = rowIndex,
                                            RecordID = qid
                                        });
                                    }
                                }
                            }
                            break;
                        case EntityState.Added:
                            state = entry.State.ToString();
                            foreach (IProperty prop in entry.CurrentValues.Properties.ToList())
                            {
                                if (entityName == "PatientCareMetricsQuestionAnswer" && qid == 0)
                                    qid = (int)entry.CurrentValues[entry.CurrentValues.Properties.Where(x => x.Name == "QuestionId").FirstOrDefault().Name];
                                MasterPatientEncLinkedColumn encLinkedColumn = columns.Where(x => x.ColumnName == prop.Name).FirstOrDefault();
                                object currentValue = entry.CurrentValues[prop.Name];
                                if (!ReferenceEquals(encLinkedColumn, null) && !Equals(currentValue, null))
                                {
                                    if (prop.IsForeignKey())
                                    {
                                        IEntityType[] masterEntities = prop.GetContainingForeignKeys().Select(x => x.PrincipalEntityType).ToArray();

                                        int[] masterIds = new int[] { (int)currentValue };
                                        string[] masterValues = GetMasterEntitiesValue(masterEntities.FirstOrDefault().Name, masterIds);
                                        currentValue = masterValues.FirstOrDefault();
                                    }
                                    AuditLogs.Add(new ChangesLog
                                    {
                                        TableName = entityName,
                                        State = state,
                                        ColumnName = prop.Name,
                                        ColumnId = encLinkedColumn.Id,
                                        OriginalValue = string.Empty,
                                        NewValue = Convert.ToString(currentValue),
                                        IndexNumber = rowIndex,
                                        RecordID = qid
                                    });
                                }
                            }
                            break;
                        //case EntityState.Deleted:
                        //    entityName = ObjectContext.GetObjectType(entry.Entity.GetType()).Name;
                        //    state = entry.State.ToString();
                        //    foreach (string prop in entry.OriginalValues.PropertyNames)
                        //    {
                        //        AuditLogs.Add(new ChangesLog
                        //        {
                        //            TableName = entityName,
                        //            State = state,
                        //            ColumnName = prop,
                        //            OriginalValue = Convert.ToString(entry.OriginalValues[prop]),
                        //            NewValue = string.Empty,
                        //        });

                        //    }
                        //    break;
                        default:
                            break;
                    }
                }
            }
            return AuditLogs;
        }

        private string[] GetMasterEntitiesValue(string masterEntityName, int[] masterIds)
        {
            string formattedEntityName = masterEntityName.Split(".").LastOrDefault();
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>(); ;
            List<string> masterValues = new List<string>();
            switch (formattedEntityName)
            {
                case AuditLogMasterEntity.MasterAllergies:
                    list = context.MasterAllergies.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.value).ToList();
                    break;
                case AuditLogMasterEntity.MasterReaction:
                    list = context.MasterReaction.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.value).ToList();
                    break;
                case AuditLogMasterEntity.GlobalCode:
                    list = context.GlobalCode.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.GlobalCodeValue).ToList();
                    break;
                case AuditLogMasterEntity.MasterRelationship:
                    list = context.MasterRelationship.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.RelationshipName).ToList();
                    break;
                case AuditLogMasterEntity.MasterGender:
                    list = context.MasterGender.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Gender).ToList();
                    break;
                case AuditLogMasterEntity.MasterICD:
                    list = context.MasterICD.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => string.Concat(x.Code, ", ", x.Description)).ToList();
                    break;
                case AuditLogMasterEntity.MasterAdministrationSite:
                    list = context.MasterAdministrationSite.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Description).ToList();
                    break;
                case AuditLogMasterEntity.MasterImmunityStatus:
                    list = context.MasterImmunityStatus.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.ConceptName).ToList();
                    break;
                case AuditLogMasterEntity.MasterImmunization:
                    list = context.MasterImmunization.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.VaccineName).ToList();
                    break;
                case AuditLogMasterEntity.MasterRejectionReason:
                    list = context.MasterRejectionReason.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.ReasonDesc).ToList();
                    break;
                case AuditLogMasterEntity.MasterRouteOfAdministration:
                    list = context.MasterRouteOfAdministration.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Description).ToList();
                    break;
                case AuditLogMasterEntity.Staffs:
                    list = context.Staffs.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => string.Concat(x.FirstName, " ", x.LastName)).ToList();
                    break;
                case AuditLogMasterEntity.MasterActivityUnitType:
                    list = context.MasterActivityUnitType.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.UnitType).ToList();
                    break;
                case AuditLogMasterEntity.MasterFrequencyTypes:
                    list = context.MasterFrequencyTypes.Where(x => masterIds.Contains(x.ID)).ToDictionary(x => x.ID, x => x.Description).ToList();
                    break;
                case AuditLogMasterEntity.Description:
                    list = context.Description.Where(x => masterIds.Contains(x.ID)).ToDictionary(x => x.ID, x => x.Descriptions).ToList();
                    break;
                //case AuditLogMasterEntity.CareGap:
                //    list = context.CareGap.Where(x => masterIds.Contains(x.ID)).ToDictionary(x => x.ID, x => x.GapDescription.GapDescriptions).ToList();
                //    break;
                //case AuditLogMasterEntity.MasterLabTest:
                //    list = context.MasterLabTest.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.LabTestName).ToList();
                //    break;
                case AuditLogMasterEntity.MasterLabTestAnalytes:
                    list = context.MasterLabTestAnalytes.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Analyte).ToList();
                    break;
                //case AuditLogMasterEntity.MasterBarrier:
                //    list = context.MasterBarriers.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.BarrierName).ToList();
                //    break;
                //case AuditLogMasterEntity.MasterMedication:
                //    list = context..Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Analyte).ToList();
                //    break;
                case AuditLogMasterEntity.MasterMedication:
                    list = context.MasterMedication.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => string.Concat(x.DrugName, "")).ToList();//" - ", x.generic_product_identifier //(x => x.Id, x => string.Concat(x.DrugName, " ", x.strength, " ", x.strength_unit_of_measure, " ", x.dosage_form, " ", x.route_of_administration, " (", x.NDCCode, ")")).ToList();
                    break;
                //case AuditLogMasterEntity.MasterTaskTypes:
                //    list = context.MasterTaskTypes.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.TypeName).ToList();
                //    break;
                //case AuditLogMasterEntity.PatientCareGap:
                //    list = context.PatientCareGap.Where(x => masterIds.Contains(x.ID)).Include(x => x.CareGap).Include(x => x.CareGap.GapDescription).ToDictionary(x => x.ID, x => x.CareGap?.GapDescription?.GapDescriptions).ToList();
                //    break;
                case AuditLogMasterEntity.DFA_Document:
                    list = context.DFA_Document.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.DocumentName).ToList();
                    break;
                case AuditLogMasterEntity.PatientPhysician:
                    list = context.PatientPhysician.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => string.Concat(x.FirstName, " ", x.LastName)).ToList();
                    break;
                //case AuditLogMasterEntity.MasterTaxonomyCodes:
                //    list = context.MasterTaxonomyCodes.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Speciality).ToList();
                //    break;
                //case AuditLogMasterEntity.MasterReferrals:
                //    list = context.MasterReferrals.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.ReferralName).ToList();
                //    break;
                case AuditLogMasterEntity.MasterChronicCondition:
                    list = context.MasterChronicCondition.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Condition).ToList();
                    break;
                case AuditLogMasterEntity.DiseaseManagementProgram:
                    list = context.DiseaseManagementProgram.Where(x => masterIds.Contains(x.ID)).ToDictionary(x => x.ID, x => x.Description).ToList();
                    break;
                case AuditLogMasterEntity.MasterCareMetricsQuestionControl:
                    list = context.MasterCareMetricsQuestionControl.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.OptionValue).ToList();
                    break;
                //case AuditLogMasterEntity.MasterImmunizationSubCategary:
                //    list = context.MasterImmunizationSubCategary.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.VaccineSubCategaryName).ToList();
                //    break;
                default:
                    list = new List<KeyValuePair<int, string>>();
                    break;
            }
            foreach (int id in masterIds)
            {
                masterValues.Add(list.Where(x => x.Key == id).FirstOrDefault().Value);
            }
            return masterValues.ToArray();
            //return context.GlobalCode.Where(x => masterIds.Contains(x.Id)).Select(x => x.value).ToArray();
        }

        //public List<ChangesLog> GetChangesLogData(TokenModel tokenModel, PatientDemographicsModel patientDemographicsModel = null, PHIDecryptedModel patients = null)
        //{
        //    List<ChangesLog> AuditLogs = new List<ChangesLog>();
        //    var changeTrack = context.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified).ToList();
        //    int rowIndex = 0;
        //    int qid = 0;
        //    foreach (EntityEntry entry in changeTrack)
        //    {
        //        qid = 0;
        //        rowIndex++;
        //        if (entry.Entity != null)
        //        {
        //            string entityName = GetTableName(entry);
        //            string state = string.Empty;
        //            // Get the all patient encounter linked columns
        //            List<MasterPatientEncLinkedColumn> columns = context.MasterPatientEncLinkedColumn.Where(x => x.MasterPatientEncLinkedEntity.EntityName == entityName && x.MasterPatientEncLinkedEntity.OrganizationId == tokenModel.OrganizationID).ToList();
        //            switch (entry.State)
        //            {
        //                case EntityState.Modified:
        //                    state = entry.State.ToString();

        //                    foreach (IProperty prop in entry.OriginalValues.Properties.ToList())
        //                    {
        //                        if (entityName == "PatientCareMetricsQuestionAnswer" && qid == 0)
        //                            qid = (int)entry.CurrentValues[entry.CurrentValues.Properties.Where(x => x.Name == "QuestionId").FirstOrDefault().Name];

        //                        // check for column exist in MasterPatientEncLinkedColumn
        //                        MasterPatientEncLinkedColumn encLinkedColumn = columns.Where(x => x.ColumnName == prop.Name).FirstOrDefault();
        //                        if (!ReferenceEquals(encLinkedColumn, null))
        //                        {
        //                            object currentValue = entry.CurrentValues[prop.Name];
        //                            object originalValue = entry.OriginalValues[prop.Name];

        //                            if (entityName == DatabaseTables.PatietnTable)
        //                            {
        //                                switch (prop.Name)
        //                                {
        //                                    case PatientAuditLogColumn.FirstName:
        //                                        currentValue = patientDemographicsModel.FirstName;
        //                                        originalValue = patients.FirstName;
        //                                        break;
        //                                    case PatientAuditLogColumn.MiddleName:
        //                                        currentValue = patientDemographicsModel.MiddleName;
        //                                        originalValue = patients.MiddleName;
        //                                        break;
        //                                    case PatientAuditLogColumn.LastName:
        //                                        currentValue = patientDemographicsModel.LastName;
        //                                        originalValue = patients.LastName;
        //                                        break;
        //                                    case PatientAuditLogColumn.DOB:
        //                                        currentValue = patientDemographicsModel.DOB != null ? patientDemographicsModel.DOB.ToString("yyyy-MM-dd HH:mm:ss.fffffff") : null;
        //                                        originalValue = patients.DateOfBirth;
        //                                        break;
        //                                    case PatientAuditLogColumn.Email:
        //                                        currentValue = patientDemographicsModel.Email;
        //                                        originalValue = patients.EmailAddress;
        //                                        break;
        //                                    //case PatientAuditLogColumn.Gender:
        //                                    //    currentValue = Convert.ToString(entry.CurrentValues[prop.Name]).Trim();
        //                                    //    originalValue = Convert.ToString(dbValues[property]).Trim();
        //                                    //    break;
        //                                    case PatientAuditLogColumn.SecondaryEmail:
        //                                        currentValue = patientDemographicsModel.SecondaryEmail;
        //                                        originalValue = patients.SecondaryEmailAddress;
        //                                        break;
        //                                }
        //                            }
        //                            if (!Equals(originalValue, currentValue))
        //                            {
        //                                if (prop.IsForeignKey())
        //                                {
        //                                    IEntityType[] masterEntities = prop.GetContainingForeignKeys().Select(x => x.PrincipalEntityType).ToArray();

        //                                    int[] masterIds = new int[] { (int)(currentValue != null ? currentValue : 0), (int)(originalValue != null ? originalValue : 0) };
        //                                    string[] masterValues = GetMasterEntitiesValue(masterEntities.FirstOrDefault().Name, masterIds);
        //                                    currentValue = masterValues.FirstOrDefault();
        //                                    originalValue = masterValues.LastOrDefault();
        //                                }
        //                                AuditLogs.Add(new ChangesLog
        //                                {
        //                                    TableName = entityName,
        //                                    State = state,
        //                                    ColumnName = prop.Name,
        //                                    ColumnId = encLinkedColumn.Id,
        //                                    //RecordID = prop.GetContainingPrimaryKey()
        //                                    OriginalValue = Convert.ToString(originalValue),
        //                                    NewValue = Convert.ToString(currentValue),
        //                                    IndexNumber = rowIndex,
        //                                    RecordID = qid
        //                                });
        //                            }
        //                        }
        //                    }
        //                    break;
        //                case EntityState.Added:
        //                    state = entry.State.ToString();
        //                    foreach (IProperty prop in entry.CurrentValues.Properties.ToList())
        //                    {
        //                        if (entityName == "PatientCareMetricsQuestionAnswer" && qid==0)
        //                            qid = (int)entry.CurrentValues[entry.CurrentValues.Properties.Where(x => x.Name == "QuestionId").FirstOrDefault().Name];
        //                        MasterPatientEncLinkedColumn encLinkedColumn = columns.Where(x => x.ColumnName == prop.Name).FirstOrDefault();
        //                        object currentValue = entry.CurrentValues[prop.Name];
        //                        if (!ReferenceEquals(encLinkedColumn, null) && !Equals(currentValue, null))
        //                        {
        //                            if (prop.IsForeignKey())
        //                            {
        //                                IEntityType[] masterEntities = prop.GetContainingForeignKeys().Select(x => x.PrincipalEntityType).ToArray();

        //                                int[] masterIds = new int[] { (int)currentValue };
        //                                string[] masterValues = GetMasterEntitiesValue(masterEntities.FirstOrDefault().Name, masterIds);
        //                                currentValue = masterValues.FirstOrDefault();
        //                            }
        //                            AuditLogs.Add(new ChangesLog
        //                            {
        //                                TableName = entityName,
        //                                State = state,
        //                                ColumnName = prop.Name,
        //                                ColumnId = encLinkedColumn.Id,
        //                                OriginalValue = string.Empty,
        //                                NewValue = Convert.ToString(currentValue),
        //                                IndexNumber = rowIndex,
        //                                RecordID = qid
        //                            });
        //                        }
        //                    }
        //                    break;
        //                //case EntityState.Deleted:
        //                //    entityName = ObjectContext.GetObjectType(entry.Entity.GetType()).Name;
        //                //    state = entry.State.ToString();
        //                //    foreach (string prop in entry.OriginalValues.PropertyNames)
        //                //    {
        //                //        AuditLogs.Add(new ChangesLog
        //                //        {
        //                //            TableName = entityName,
        //                //            State = state,
        //                //            ColumnName = prop,
        //                //            OriginalValue = Convert.ToString(entry.OriginalValues[prop]),
        //                //            NewValue = string.Empty,
        //                //        });

        //                //    }
        //                //    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }
        //    return AuditLogs;
        //}
        //private string[] GetMasterEntitiesValue(string masterEntityName, int[] masterIds)
        //{
        //    string formattedEntityName = masterEntityName.Split(".").LastOrDefault();
        //    List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>(); ;
        //    List<string> masterValues = new List<string>(); 
        //    switch (formattedEntityName)
        //    {
        //        case AuditLogMasterEntity.MasterAllergies:
        //            list = context.MasterAllergies.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.value).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterReaction:
        //            list = context.MasterReaction.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.value).ToList();
        //            break;
        //        case AuditLogMasterEntity.GlobalCode:
        //            list = context.GlobalCode.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.GlobalCodeValue).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterRelationship:
        //            list = context.MasterRelationship.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.RelationshipName).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterGender:
        //            list = context.MasterGender.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Gender).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterICD:
        //            list = context.MasterICD.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => string.Concat(x.Code, ", ", x.Description)).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterAdministrationSite:
        //            list = context.MasterAdministrationSite.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Description).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterImmunityStatus:
        //            list = context.MasterImmunityStatus.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.ConceptName).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterImmunization:
        //            list = context.MasterImmunization.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.VaccineName).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterRejectionReason:
        //            list = context.MasterRejectionReason.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.ReasonDesc).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterRouteOfAdministration:
        //            list = context.MasterRouteOfAdministration.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Description).ToList();
        //            break;
        //        case AuditLogMasterEntity.Staffs:
        //            list = context.Staffs.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => string.Concat(x.FirstName, " ", x.LastName)).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterActivityUnitType:
        //            list = context.MasterActivityUnitType.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.UnitType).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterFrequencyTypes:
        //            list = context.MasterFrequencyTypes.Where(x => masterIds.Contains(x.ID)).ToDictionary(x => x.ID, x => x.Description).ToList();
        //            break;
        //        case AuditLogMasterEntity.Description:
        //            list = context.Description.Where(x => masterIds.Contains(x.ID)).ToDictionary(x => x.ID, x => x.Descriptions).ToList();
        //            break;
        //        case AuditLogMasterEntity.CareGap:
        //            list = context.CareGap.Where(x => masterIds.Contains(x.ID)).ToDictionary(x => x.ID, x => x.GapDescription.GapDescriptions).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterLabTest:
        //            list = context.MasterLabTest.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.LabTestName).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterLabTestAnalytes:
        //            list = context.MasterLabTestAnalytes.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Analyte).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterBarrier:
        //            list = context.MasterBarriers.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.BarrierName).ToList();
        //            break;
        //        //case AuditLogMasterEntity.MasterMedication:
        //        //    list = context..Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Analyte).ToList();
        //        //    break;
        //        case AuditLogMasterEntity.MasterMedication:
        //            list = context.MasterMedication.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => string.Concat(x.DrugName, "")).ToList();//" - ", x.generic_product_identifier //(x => x.Id, x => string.Concat(x.DrugName, " ", x.strength, " ", x.strength_unit_of_measure, " ", x.dosage_form, " ", x.route_of_administration, " (", x.NDCCode, ")")).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterTaskTypes:
        //            list = context.MasterTaskTypes.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.TypeName).ToList();
        //            break;
        //        case AuditLogMasterEntity.PatientCareGap:
        //            list = context.PatientCareGap.Where(x => masterIds.Contains(x.ID)).Include(x => x.CareGap).Include(x => x.CareGap.GapDescription).ToDictionary(x => x.ID, x => x.CareGap?.GapDescription?.GapDescriptions).ToList();
        //            break;
        //        case AuditLogMasterEntity.DFA_Document:
        //            list = context.DFA_Document.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.DocumentName).ToList();
        //            break;
        //        case AuditLogMasterEntity.PatientPhysician:
        //            list = context.PatientPhysician.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => string.Concat(x.FirstName, " ", x.LastName)).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterTaxonomyCodes:
        //            list = context.MasterTaxonomyCodes.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Speciality).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterReferrals:
        //            list = context.MasterReferrals.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.ReferralName).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterChronicCondition:
        //            list = context.MasterChronicCondition.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.Condition).ToList();
        //            break;
        //        case AuditLogMasterEntity.DiseaseManagementProgram:
        //            list=context.DiseaseManagementProgram.Where(x => masterIds.Contains(x.ID)).ToDictionary(x => x.ID, x => x.Description).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterCareMetricsQuestionControl:
        //            list = context.MasterCareMetricsQuestionControl.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.OptionValue).ToList();
        //            break;
        //        case AuditLogMasterEntity.MasterImmunizationSubCategary:
        //            list = context.MasterImmunizationSubCategary.Where(x => masterIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x.VaccineSubCategaryName).ToList();
        //            break;
        //        default:
        //            list = new List<KeyValuePair<int, string>>();
        //            break;
        //    }
        //    foreach (int id in masterIds)
        //    {
        //        masterValues.Add(list.Where(x => x.Key == id).FirstOrDefault().Value);
        //    }
        //    return masterValues.ToArray();
        //    //return context.GlobalCode.Where(x => masterIds.Contains(x.Id)).Select(x => x.value).ToArray();
        //}
    }
}
