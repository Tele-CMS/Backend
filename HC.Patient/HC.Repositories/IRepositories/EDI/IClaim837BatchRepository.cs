using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.EDI
{
    public interface IClaim837BatchRepository:IRepositoryBase<Claim837Batch>
    {
        void UpdateBatchRequestStatus(int batchId, string ediText, int userId,string claimIds, string action);
        IQueryable<T> GetSubmittedClaimsBatch<T>(int pageNumber, int pageSize, DateTime startDate, DateTime endDate, TokenModel token) where T : class, new();
        Dictionary<string, object> GetSubmittedClaimsBatchDetails(string claim837BatchIds,TokenModel token);
    }
}
