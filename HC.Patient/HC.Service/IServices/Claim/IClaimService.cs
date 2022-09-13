using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Claim;
using HC.Service.Interfaces;
using System;
using System.Xml.Linq;

namespace HC.Patient.Service.IServices.Claim
{
    public interface IClaimService  : IBaseService
    {
        JsonModel GetClaims(int organizationId, int pageNumber, int pageSize, int? claimId, string lastName, string firstName, string fromDate, string toDate, string payerName, string sortColumn, string sortOrder,Nullable<int> claimStatusId);
        JsonModel GetClaimServiceLines(int claimId);
        JsonModel GetOpenChargesForPatient(int patientId, int payerId, TokenModel token);
        JsonModel CreateClaim(int patientEncounterId,bool isAdmin,TokenModel token);
        JsonModel SaveClaimServiceLine(int claimId, ClaimServiceLineModel requestObj, TokenModel token);
        XElement GetClaimServiceLineModifications(ClaimServiceLine claimServiceLine, ClaimServiceLineModel claimServiceLineModel);
        JsonModel GetClaimServiceLineDetails(int claimId,int serviceLineId);
        JsonModel DeleteClaimServiceLine(int serviceLineId,TokenModel token);
        JsonModel DeleteClaim(int claimId, TokenModel token);
        JsonModel GetClaimDetailsById(int claimId);
        JsonModel UpdateClaimDetails(ClaimModel claimModel,TokenModel token);
        JsonModel GetAllClaimsWithServiceLines(int pageNumber, int pageSize, int? claimId, string patientIds, string fromDate, string toDate, string payerName, string sortColumn, string sortOrder, Nullable<int> claimStatusId,TokenModel token);

        JsonModel GetClaimsForPatientLedger(int patientId, int pageNumber, int pageSize, string sortColumn, string sortOrder, string claimBalanceStatus, TokenModel token);
        JsonModel GetClaimServiceLinesForPatientLedger(int claimId,TokenModel token);
        JsonModel GetClaimHistory(Nullable<int> claimId,int pageNumber,int pageSize,string sortColumn,string sortOrder,TokenModel token);
        JsonModel GetClaimsForLedger(int pageNumber, int pageSize, string sortColumn, string sortOrder, string patientIds, string payerIds, string tags, string fromDate, string endDate, string claimBalanceStatus, TokenModel token);
        JsonModel GetClaimServiceLinesForLedger(int claimId, TokenModel token);
        JsonModel UpdatePaymentStatus(Nullable<int> claimId, int? paymentStatusId, TokenModel token);
        

    }
}
