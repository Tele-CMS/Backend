using System;
using System.Collections.Generic;
using System.Text;
using HC.Repositories;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Data;
using System.Linq;
using System.Data.SqlClient;
using HC.Patient.Repositories.IRepositories.Claim;
using HC.Patient.Model.Claim;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Data.Common;
using static HC.Common.Enums.CommonEnum;
using HC.Model;
using System.Xml.Linq;

namespace HC.Patient.Repositories.Repositories
{
    public class ClaimRepository : RepositoryBase<Claims>, IClaimRepository
    {
        private HCOrganizationContext _context;
        public ClaimRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> CreateClaim<T>(int patientEncounterId, bool isAdmin, int userId, int organizationId) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientEncounterId", patientEncounterId),
                                          new SqlParameter("@UserId", userId),
                                          new SqlParameter("@OrganizationId", organizationId),
                                          new SqlParameter("@CreatedDate", DateTime.UtcNow),
                                          new SqlParameter("@IsAdmin",isAdmin)
            };
            return _context.ExecStoredProcedureListWithSingleOutput<T>(SQLObjects.CLM_CreateClaim.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> DeleteClaim<T>(int claimId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId", claimId),
                                          new SqlParameter("@UserId", token.UserID),
            };

            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_DeleteClaim.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public ClaimsFullDetailModel GetAllClaimsWithServiceLines(int organizationId, int pageNumber, int pageSize, int? claimId, string patientIds, string fromDate, string toDate, string payerName, string sortColumn, string sortOrder, Nullable<int> claimStatusId)
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId",organizationId),
                                          new SqlParameter("@PageNumber",pageNumber),
                                          new SqlParameter("@PageSize",pageSize),
                                          new SqlParameter("@ClaimId",claimId),
                                          new SqlParameter("@PatientIds",patientIds),
                                          new SqlParameter("@FromDate",fromDate),
                                          new SqlParameter("@ToDate",toDate),
                                          new SqlParameter("@PayerName",payerName),
                                           new SqlParameter("@SortColumn",sortColumn),
                                          new SqlParameter("@SortOrder",sortOrder),
                                          new SqlParameter("@ClaimStatusId",claimStatusId)
            };
            return _context.ExecStoredProcedureForAllClaimDetailsWithServiceLine(SQLObjects.CLM_GetAllClaimsWithServiceLines.ToString(), parameters.Length, parameters);
        }

        public IQueryable<T> GetClaimDetailsById<T>(int claimId) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId", claimId) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GetClaimDetailsById.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetClaims<T>(int organizationId, int pageNumber, int pageSize, int? claimId, string lastName, string firstName, string fromDate, string toDate, string payerName, string sortColumn, string sortOrder, Nullable<int> claimStatusId) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId",organizationId),
                                          new SqlParameter("@PageNumber",pageNumber),
                                          new SqlParameter("@PageSize",pageSize),
                                          new SqlParameter("@ClaimId",claimId),
                                          new SqlParameter("@LastName",lastName),
                                          new SqlParameter("@FirstName",firstName),
                                          new SqlParameter("@FromDate",fromDate),
                                          new SqlParameter("@ToDate",toDate),
                                          new SqlParameter("@PayerName",payerName),
                                           new SqlParameter("@SortColumn",sortColumn),
                                          new SqlParameter("@SortOrder",sortOrder),
                                          new SqlParameter("@ClaimStatusId",claimStatusId)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GetClaims.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetClaimServiceLines<T>(int claimId) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId", claimId) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GetClaimServiceLines.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetOpenChargesForPatient<T>(int patientId, int payerId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId",patientId),
                                          new SqlParameter("@PayerId", payerId),
                                          new SqlParameter("@OrganizationId", token.OrganizationID),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GetOpenChargesForPatient.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public PaperClaimModel GetBatchPaperClaimInfo(string claimIds, string payerPreference, int organizationId)
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimIds",claimIds),
                                          new SqlParameter("@PayerPreference",payerPreference),
                                          new SqlParameter("@OrganizationId",organizationId)
            };
            return _context.ExecStoredProcedureForPaperClaimInfo(SQLObjects.CLM_GetBatchPaperClaimInfo, parameters.Length, parameters);
        }

        public PaperClaimModel GetBatchPaperClaimInfo_Clubbed(string claimIds, string payerPreference, int organizationId)
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimIds",claimIds),
                                          new SqlParameter("@PayerPreference",payerPreference),
                                          new SqlParameter("@OrganizationId",organizationId)
            };
            return _context.ExecStoredProcedureForPaperClaimInfo(SQLObjects.CLM_GetBatchPaperClaimInfo_Clubbed, parameters.Length, parameters);
        }

        public PaperClaimModel GetPaperClaimInfo(int claimId, int patientInsuranceId, int organizationId)
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId",claimId),
                                          new SqlParameter("@PatientInsuranceId",patientInsuranceId),
                                          new SqlParameter("@OrganizationId",organizationId)
            };
            return _context.ExecStoredProcedureForPaperClaimInfo(SQLObjects.CLM_GetPaperClaimInfo.ToString(), parameters.Length, parameters);
        }
        public PaperClaimModel GetPaperClaimInfo_Secondary(int claimId, int patientInsuranceId, int organizationId)
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId",claimId),
                                          new SqlParameter("@PatientInsuranceId",patientInsuranceId),
                                          new SqlParameter("@OrganizationId",organizationId)
            };
            return _context.ExecStoredProcedureForPaperClaimInfo(SQLObjects.CLM_GetPaperClaimInfo_Secondary.ToString(), parameters.Length, parameters);
        }

        public EDI837FileModel GetEDIInfoForSingleClaim(int claimId, int patientInsuranceId, int edibatchRequestId, int organizationId, int locationId)
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId",claimId),
                                          new SqlParameter("@PatientInsuranceId",patientInsuranceId),
                                          new SqlParameter("@EdiBatchRequestId",edibatchRequestId),
                                          new SqlParameter("@OrganizationId",organizationId),
                                          new SqlParameter("@LocationId",locationId)
            };
            return _context.ExecStoredProcedureForEDIInfo(SQLObjects.CLM_GetEDIInfo.ToString(), parameters.Length, parameters);
        }

        public EDI837FileModel GetEDIInfoForBatchClaim(string claimIds, string PatientPayerPreference, int edibatchRequestId, int organizationId, int locationId)
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimIds",claimIds),
                                          new SqlParameter("@PatientPayerPreference",PatientPayerPreference),
                                          new SqlParameter("@EdiBatchRequestId",edibatchRequestId),
                                          new SqlParameter("@OrganizationId",organizationId),
                                          new SqlParameter("@LocationId",locationId)
            };
            return _context.ExecStoredProcedureForEDIInfo(SQLObjects.CLM_GetBatchEDIInfo.ToString(), parameters.Length, parameters);
        }

        public IQueryable<T> SaveClaim837BatchRequestForSingleSubmit<T>(int claimId, int patientInsuranceId, int userId, int organizationId, string payerPreference, string submissionType) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId", claimId),
                                        new SqlParameter("@PatientInsuranceId", patientInsuranceId),
                                        new SqlParameter("@UserId", userId),
                                        new SqlParameter("@OrganizationId", organizationId),
                                        new SqlParameter("@PayerPreference", payerPreference),
                                        new SqlParameter("@SubmissionType", submissionType)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GenerateClaim837BatchIdForSingleSubmit.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> SaveClaim837BatchRequestForBatchSubmit<T>(string claimIds, int userId, int organizationId, string payerPreference, string submissionType) where T : class, new()
        {

            SqlParameter[] parameters = { new SqlParameter("@ClaimIds", claimIds),
                                        new SqlParameter("@UserId", userId),
                                        new SqlParameter("@OrganizationId", organizationId),
                                        new SqlParameter("@PayerPreference", payerPreference),
                                        new SqlParameter("@SubmissionType", submissionType)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GenerateClaim837BatchIdForBatchSubmit.ToString(), parameters.Length, parameters).AsQueryable();

        }

        public IQueryable<T> GetClaimsForPatientLedger<T>(int patientId, int pageNumber, int pageSize, string sortColumn, string sortOrder, string claimBalanceStatus, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId", token.OrganizationID),
                                        new SqlParameter("@PageNumber", pageNumber),
                                        new SqlParameter("@PageSize", pageSize),
                                        new SqlParameter("@SortColumn", sortColumn),
                                        new SqlParameter("@SortOrder", sortOrder),
                                        new SqlParameter("@PatientId", patientId),
                                        new SqlParameter("@ClaimBalanceStatus", claimBalanceStatus),
                                        
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GetClaimsForPatientLedger.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetClaimServiceLinesForPatientLedger<T>(int claimId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId", claimId) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GetClaimServiceLinesForPatientLedger.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetClaimHistory<T>(Nullable<int> claimId, int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId", token.OrganizationID),
                                            new SqlParameter("@PageNumber", pageNumber),
                                            new SqlParameter("@PageSize", pageSize),
                                            new SqlParameter("@SortColumn", sortColumn),
                                            new SqlParameter("@SortOrder", sortOrder),
                                             new SqlParameter("@ClaimId", claimId)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GetClaimHistory.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> SaveClaimHistory<T>(int? claimId, XElement inputXML, string action, DateTime logDate, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId", claimId),
                                        new SqlParameter("@InputXML", inputXML.ToString()),
                                        new SqlParameter("@Action", action),
                                        new SqlParameter("@LogDate", logDate),
                                        new SqlParameter("@UserId", token.UserID),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_InsertClaimHistory.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetClaimsForLedger<T>(int pageNumber, int pageSize, string sortColumn, string sortOrder, string patientIds, string payerIds, string tags, string fromDate, string toDate, string claimBalanceStatus, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId", token.OrganizationID),
                                        new SqlParameter("@PageNumber", pageNumber),
                                        new SqlParameter("@PageSize", pageSize),
                                        new SqlParameter("@SortColumn", sortColumn),
                                        new SqlParameter("@SortOrder", sortOrder),
                                        new SqlParameter("@PatientIds", patientIds),
                                        new SqlParameter("@PayerIds", payerIds),
                                        new SqlParameter("@Tags", tags),
                                        new SqlParameter("@FromDate", fromDate),
                                        new SqlParameter("@ToDate", toDate),
                                        new SqlParameter("@ClaimBalanceStatus", claimBalanceStatus),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GetClaimsForLedger.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetClaimServiceLinesForLedger<T>(int claimId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId", claimId) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GetClaimServiceLinesForLedger.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public EDI837FileModel GetClaimInfoForResubmission(int claimId, int patientInsuranceId, int ediBatchRequestId, int organizationId, int locationId, string resubmissionReason,string payerControlReferenceNumber)
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId",claimId),
                                          new SqlParameter("@PatientInsuranceId",patientInsuranceId),
                                          new SqlParameter("@EdiBatchRequestId",ediBatchRequestId),
                                          new SqlParameter("@OrganizationId",organizationId),
                                          new SqlParameter("@LocationId",locationId),
                                          new SqlParameter("@ResubmissionReason",resubmissionReason),
                                          new SqlParameter("@PayerControlReferenceNumber",payerControlReferenceNumber),
            };
            return _context.ExecStoredProcedureForEDIInfo(SQLObjects.CLM_GetEDIInfoResubmit.ToString(), parameters.Length, parameters);
        }
        public EDI837FileModel GetEDIInfoForSingleClaim_Secondary(int claimId, int patientInsuranceId, int edibatchRequestId, int organizationId, int locationId)
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId",claimId),
                                          new SqlParameter("@PatientInsuranceId",patientInsuranceId),
                                          new SqlParameter("@EdiBatchRequestId",edibatchRequestId),
                                          new SqlParameter("@OrganizationId",organizationId),
                                          new SqlParameter("@LocationId",locationId)
            };
            return _context.ExecStoredProcedureForEDIInfo(SQLObjects.CLM_GetEDIInfo_SecondaryPayer.ToString(), parameters.Length, parameters);
        }

        public EDI837FileModel GetEDIInfoForBatchClaim_Secondary(string claimIds, string patientPayerPreference, int ediBatchRequestId, int organizationId, int locationId)
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimIds",claimIds),
                                          new SqlParameter("@PatientPayerPreference",patientPayerPreference),
                                          new SqlParameter("@EdiBatchRequestId",ediBatchRequestId),
                                          new SqlParameter("@OrganizationId",organizationId),
                                          new SqlParameter("@LocationId",locationId)
            };
            return _context.ExecStoredProcedureForEDIInfo(SQLObjects.CLM_GetBatchEDIInfo_SecondaryPayer.ToString(), parameters.Length, parameters);
        }

        public EDI837FileModel GetClaimInfoForBatchResubmission(string claimIds, string patientPayerPreference, int ediBatchRequestId, int organizationId, int locationId)
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimsXML",claimIds),
                                          new SqlParameter("@EdiBatchRequestId",ediBatchRequestId),
                                          new SqlParameter("@OrganizationId",organizationId),
                                          new SqlParameter("@LocationId",locationId)
            };
            return _context.ExecStoredProcedureForEDIInfo(SQLObjects.CLM_GetBatchEDIInfoResubmit.ToString(), parameters.Length, parameters);
        }

        public IQueryable<T> GetClaimBalance<T>(int claimId, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId",claimId)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GetClaimBalance.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public EDI837FileModel GetEDIInfoForSingleClaim_Tertiary(int claimId, int patientInsuranceId, int ediBatchRequestId, int organizationId, int locationId)
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimId",claimId),
                                          new SqlParameter("@PatientInsuranceId",patientInsuranceId),
                                          new SqlParameter("@EdiBatchRequestId",ediBatchRequestId),
                                          new SqlParameter("@OrganizationId",organizationId),
                                          new SqlParameter("@LocationId",locationId)
            };
            return _context.ExecStoredProcedureForEDIInfo(SQLObjects.CLM_GetEDIInfo_TertiaryPayer.ToString(), parameters.Length, parameters);
        }

        public EDI837FileModel GetEDIInfoForBatchClaim_Tertiary(string claimIds, string patientPayerPreference, int ediBatchRequestId, int organizationId, int locationId)
        {
            SqlParameter[] parameters = { new SqlParameter("@ClaimIds",claimIds),
                                          new SqlParameter("@PatientPayerPreference",patientPayerPreference),
                                          new SqlParameter("@EdiBatchRequestId",ediBatchRequestId),
                                          new SqlParameter("@OrganizationId",organizationId),
                                          new SqlParameter("@LocationId",locationId)
            };
            return _context.ExecStoredProcedureForEDIInfo(SQLObjects.CLM_GetBatchEDIInfo_TertiaryPayer.ToString(), parameters.Length, parameters);
        }
    }
}
