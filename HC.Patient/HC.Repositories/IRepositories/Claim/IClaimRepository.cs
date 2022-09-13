using HC.Repositories.Interfaces;
using HC.Patient.Entity;
using System.Linq;
using HC.Patient.Model.Claim;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Data.Common;
using System;
using HC.Model;
using System.Xml.Linq;

namespace HC.Patient.Repositories.IRepositories.Claim
{
    public interface IClaimRepository:IRepositoryBase<Claims>
    {
        IQueryable<T> CreateClaim<T>(int patientEncounterId,bool isAdmin,int userId,int organizationId) where T : class, new();
        IQueryable<T> GetClaims<T>(int organizationId, int pageNumber, int pageSize, int? claimId, string lastName, string firstName, string fromDate, string toDate, string payerName, string sortColumn, string sortOrder, Nullable<int> claimStatusId) where T : class, new();
        IQueryable<T> GetClaimServiceLines<T>(int claimId) where T : class, new();
        IQueryable<T> GetOpenChargesForPatient<T>(int patientId, int payerId, TokenModel token) where T : class, new();
        IQueryable<T> GetClaimDetailsById<T>(int claimId) where T : class, new();
        IQueryable<T> DeleteClaim<T>(int claimId, TokenModel token) where T : class, new();
        ClaimsFullDetailModel GetAllClaimsWithServiceLines(int organizationId, int pageNumber, int pageSize, int? claimId, string patientIds, string fromDate, string toDate, string payerName, string sortColumn, string sortOrder, Nullable<int> claimStatusId);
        PaperClaimModel GetBatchPaperClaimInfo(string claimIds, string payerPreference, int organizationId);
        PaperClaimModel GetBatchPaperClaimInfo_Clubbed(string claimIds, string payerPreference, int organizationId);
        PaperClaimModel GetPaperClaimInfo(int claimId, int patientInsuranceId, int organizationId);
        PaperClaimModel GetPaperClaimInfo_Secondary(int claimId, int patientInsuranceId, int organizationId);

        EDI837FileModel GetEDIInfoForSingleClaim(int claimId, int patientInsuranceId,int ediBatchRequestId, int organizationId,int locationId);
        EDI837FileModel GetEDIInfoForBatchClaim(string claimId, string patientPayerPreference, int ediBatchRequestId, int organizationId, int locationId);
        IQueryable<T> SaveClaim837BatchRequestForSingleSubmit<T>(int claimId, int patientInsuranceId, int userId, int organizationId, string payerPreference, string submissionType) where T : class, new();
        IQueryable<T> SaveClaim837BatchRequestForBatchSubmit<T>(string claimIds, int userId, int organizationId, string payerPreference, string submissionType) where T : class, new();
        IQueryable<T> GetClaimsForPatientLedger<T>(int patientId, int pageNumber, int pageSize, string sortColumn, string sortOrder, string claimBalanceStatus, TokenModel token) where T : class, new();

        IQueryable<T> GetClaimServiceLinesForPatientLedger<T>(int claimId,TokenModel token) where T : class, new();
        IQueryable<T> GetClaimHistory<T>(Nullable<int> claimId,int pageNumber, int pageSize, string sortColumn, string sortOrder,TokenModel token) where T : class, new();
        IQueryable<T> SaveClaimHistory<T>(int? claimId, XElement updatedValue, string action, DateTime logDate, TokenModel token) where T : class, new();
        IQueryable<T> GetClaimsForLedger<T>(int pageNumber, int pageSize, string sortColumn, string sortOrder, string patientIds, string payerIds, string tags, string formDate, string toDate, string claimBalanceStatus, TokenModel token) where T : class, new();
        IQueryable<T> GetClaimServiceLinesForLedger<T>(int claimId, TokenModel token) where T : class, new();
        EDI837FileModel GetClaimInfoForResubmission(int claimId, int patientInsuranceId, int ediBatchRequestId, int organizationId, int locationId,string resubmissionReason,string payerControlReferenceNumber);
        EDI837FileModel GetClaimInfoForBatchResubmission(string claimIds, string patientPayerPreference, int ediBatchRequestId, int organizationId, int locationId);
        EDI837FileModel GetEDIInfoForSingleClaim_Secondary(int claimId, int patientInsuranceId, int ediBatchRequestId, int organizationId, int locationId);
        EDI837FileModel GetEDIInfoForBatchClaim_Secondary(string claimId, string patientPayerPreference, int ediBatchRequestId, int organizationId, int locationId);

        EDI837FileModel GetEDIInfoForSingleClaim_Tertiary(int claimId, int patientInsuranceId, int ediBatchRequestId, int organizationId, int locationId);
        EDI837FileModel GetEDIInfoForBatchClaim_Tertiary(string claimIds, string patientPayerPreference, int ediBatchRequestId, int organizationId, int locationId);

        IQueryable<T> GetClaimBalance<T>(int claimId, TokenModel token) where T : class, new();

    }
}
