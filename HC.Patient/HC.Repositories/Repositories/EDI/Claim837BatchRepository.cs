using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.EDI;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.EDI
{
    public class Claim837BatchRepository : RepositoryBase<Claim837Batch>, IClaim837BatchRepository
    {
        private HCOrganizationContext _context;
        public Claim837BatchRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<T> GetSubmittedClaimsBatch<T>(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PageNumber",pageNumber),
                                          new SqlParameter("@PageSize",pageSize),
                                          new SqlParameter("@OrganizationId",token.OrganizationID),
                                          new SqlParameter("@UserId",token.UserID),
                                          new SqlParameter("@FromDate",fromDate),
                                          new SqlParameter("@ToDate",toDate),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GetSubmittedClaimsHistory.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public Dictionary<string, object> GetSubmittedClaimsBatchDetails(string claim837BatchIds, TokenModel token)
        {
            SqlParameter[] parameters = { new SqlParameter("@Claim837BatchIds",claim837BatchIds)
            };
            return _context.ExecStoredProcedureListWithOutputForSubmittedClaimHistory(SQLObjects.CLM_GetSubmittedClaimsHistoryDetails.ToString(), parameters.Length, parameters);
        }

        public void UpdateBatchRequestStatus(int batchId, string ediText, int userId,string claimIds,string action)
        {
            SqlParameter[] parameters = { new SqlParameter("@Claim837BatchId",batchId),
                                          new SqlParameter("@UserId",userId),
                                          new SqlParameter("@EDIText",ediText),
                                          new SqlParameter("@ClaimIds",claimIds),
                                          new SqlParameter("@Action",action)
            };
            _context.ExecStoredProcedureListWithoutOutput(SQLObjects.CLM_UpdateBatchRequestStatus.ToString(), parameters.Length, parameters);
        }
    }
}
