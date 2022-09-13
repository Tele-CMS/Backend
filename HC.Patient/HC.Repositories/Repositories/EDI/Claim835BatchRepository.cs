using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.EDI;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using HC.Model;
using System.Linq;
using System.Data.SqlClient;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.EDI
{
    public class Claim835BatchRepository : RepositoryBase<Claim835Batch>, IClaim835BatchRepository
    {
        private HCOrganizationContext _context;
        public Claim835BatchRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable Apply835PaymentsToPatientAccount<T>(string responseClaimIds,TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ResponseClaimIds", responseClaimIds),
                                          new SqlParameter("@UserId", token.UserID),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)

            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_Apply835PaymentsToPatientAccount.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable Apply835ServiceLineAdjustmentsToPatientAccount<T>(string responseClaimServiceLineAdjIds, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ResponseClaimServiceLineAdjIds", responseClaimServiceLineAdjIds),
                                          new SqlParameter("@UserId", token.UserID),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)

            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_Apply835PaymentsToPatientAccount.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable Apply835ServiceLinePaymentsToPatientAccount<T>(string responseClaimServiceLineIds, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ResponseClaimServiceLineIds", responseClaimServiceLineIds),
                                          new SqlParameter("@UserId", token.UserID),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)

            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_Apply835PaymentsToPatientAccount.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public Dictionary<string, object> GetProcessedClaims(int pageNumber, int pageSize, int? claimId, string patientIds, string adjustmentsGroupCodes, string fromDate, string toDate, string payerName, string sortColumn, string sortOrder, TokenModel token)
        {
            SqlParameter[] parameters = { new SqlParameter("@PageNumber", pageNumber),
                                          new SqlParameter("@PageSize", pageSize),
                                          new SqlParameter("@ClaimId",claimId),
                                          new SqlParameter("@PatientIds",patientIds),
                                          new SqlParameter("@AdjustmentsGroupCodes",adjustmentsGroupCodes),
                                          new SqlParameter("@FromDate",fromDate),
                                          new SqlParameter("@ToDate",toDate),
                                          new SqlParameter("@PayerName",payerName),
                                          new SqlParameter("@SortColumn", sortColumn),
                                          new SqlParameter("@SortOrder", sortOrder),
                                          new SqlParameter("@OrganizationId", token.OrganizationID),
                                          new SqlParameter("@UserId", token.UserID)

            };
            return _context.ExecStoredProcedureForProcessedClaims(SQLObjects.CLM_GetProcessedClaims.ToString(), parameters.Length, parameters);
        }

        public IQueryable Save835Response<T>(string responseHeaders, string responseClaims, string responseServiceLine, string responseServiceLineAdj, string fileText, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ResponseHeaders", responseHeaders),
                                          new SqlParameter("@ResponseClaims", responseClaims),
                                          new SqlParameter("@ResponseServiceLine", responseServiceLine),
                                          new SqlParameter("@ResponseServiceLineAdjustments", responseServiceLineAdj),
                                          new SqlParameter("@EDIFileText", fileText),
                                          new SqlParameter("@UserId", token.UserID),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)

            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_SaveEDI835ResponseDetails.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
