using HC.Model;
using HC.Service.Interfaces;
using System.IO;

namespace HC.Patient.Service.IServices.Claim
{
    public interface IPaperClaimService :IBaseService
    {
        MemoryStream GenerateBatchPaperClaims(string claimIds, string payerPreference,bool markSubmitted,int printFormat, TokenModel token);
        MemoryStream GenerateBatchPaperClaims_Clubbed(string claimIds, string payerPreference, bool markSubmitted, int printFormat, TokenModel token);
        MemoryStream GeneratePaperClaim(int claimId, int patientInsuranceId,bool markSubmitted,int printFormat,TokenModel token);
        MemoryStream GeneratePaperClaim_Secondary(int claimId, int patientInsuranceId, int printFormat, TokenModel token);

    }
}
