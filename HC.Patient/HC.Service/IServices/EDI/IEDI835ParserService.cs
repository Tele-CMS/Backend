using HC.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.EDI
{
    public interface IEDI835ParserService :IBaseService
    {
        JsonModel ReadEDI835(TokenModel token);
        JsonModel Get835SegmentData(TokenModel token);
        JsonModel GetProcessedClaims(int pageNumber, int pageSize, int? claimId, string patientIds, string adjustmentsGroupCodes, string fromDate, string toDate, string payerName, string sortColumn, string sortOrder, TokenModel token);
        JsonModel Apply835PaymentsToPatientAccount(string responseClaimIds, TokenModel token);
        JsonModel Apply835ServiceLinePaymentsToPatientAccount(string responseClaimServiceLineIds, TokenModel token);
        JsonModel Apply835ServiceLineAdjustmentsToPatientAccount(string responseClaimServiceLineAdjIds, TokenModel token);
    }
}
