using HC.Model;
using HC.Patient.Model.Claim;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.EDI
{
    public interface IEDI837GenerationService :IBaseService
    {
        JsonModel GenerateSingleEDI837(int claimId, int patientInsuranceId,TokenModel token);
        JsonModel GenerateBatchEDI837(string claimIds, TokenModel token);
        JsonModel ResubmitClaim(int claimId, int patientInsuranceId, string resubmissionReason, string payerControlReferenceNumber, TokenModel token);
        JsonModel ResubmitBatchClaim(List<ResubmitInputModel> claimInfo, TokenModel token);
        JsonModel GenerateSingleEDI837_Secondary(int claimId, int patientInsuranceId, TokenModel token);
        JsonModel GenerateBatchEDI837_Secondary(string claimIds, TokenModel token);

        JsonModel GenerateSingleEDI837_Tertiary(int claimId, int patientInsuranceId, TokenModel token);
        JsonModel GenerateBatchEDI837_Tertiary(string claimIds, TokenModel token);

        string DownloadSingleEDI837(int claimId, int patientInsuranceId, int locationId, TokenModel token);
        string DownloadBatchEDI837(string claimIds, int locationId, string path, TokenModel token);
        JsonModel SubmitClaimsForNonEDI(string claimIds,TokenModel token);
        JsonModel GetSubmittedClaimsBatch(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, TokenModel token);
        JsonModel GetSubmittedClaimsBatchDetails(string claim837BatchIds,TokenModel token);
        JsonModel GetEDIInfo(int claimId, int patientInsuranceId, TokenModel token);
    }
}
