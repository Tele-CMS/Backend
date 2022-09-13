using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.EDI
{
    public interface IClaim835BatchRepository : IRepositoryBase<Claim835Batch>
    {
        IQueryable Save835Response<T>(string responseHeaders, string responseClaims, string responseServiceLine, string responseServiceLineAdj,string fileText, TokenModel token) where T : class, new();
        IQueryable Apply835PaymentsToPatientAccount<T>(string responseClaimIds,TokenModel token) where T : class, new();
        IQueryable Apply835ServiceLinePaymentsToPatientAccount<T>(string responseClaimServiceLineIds, TokenModel token) where T : class, new();
        IQueryable Apply835ServiceLineAdjustmentsToPatientAccount<T>(string responseClaimServiceLineAdjIds, TokenModel token) where T : class, new();
        Dictionary<string, object> GetProcessedClaims(int pageNumber, int pageSize, int? claimId, string lastName, string firstName, string fromDate, string toDate, string payerName, string sortColumn, string sortOrder, TokenModel token);
    }
}
