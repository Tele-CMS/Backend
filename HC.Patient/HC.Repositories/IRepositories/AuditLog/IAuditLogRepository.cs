using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.AuditLog
{
    public interface IAuditLogRepository : IRepositoryBase<AuditLogs>
    {
        void SaveChangesWithAuditLogs(string screenName,string action, Nullable<int> patientId, Nullable<int> userId,string parentInfo,TokenModel token);
        IQueryable<T> GetAuditLogList<T>(string searchText, string createdBy, string patientName, string action, string fromDate, string toDate, int organizationID, int locationID, int pageNumber, int pageSize, string sortColumn, string sortOrder) where T : class, new();
        IQueryable<T> GetLoginLogList<T>(string searchText, string createdBy, string patientName, string action, string fromDate, string toDate, int organizationID, int locationID, int pageNumber, int pageSize, string sortColumn, string sortOrder, int roleID) where T : class, new();
    }
}
