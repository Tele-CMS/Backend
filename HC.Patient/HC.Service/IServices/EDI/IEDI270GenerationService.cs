using HC.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.EDI
{
    public interface IEDI270GenerationService :IBaseService
    {
        string Download270(int patientId, int patientInsuranceId, string serviceTypeCodeIds, string serviceCodeIds, TokenModel token);
        JsonModel Generate270EligibilityRequestFile(int patientId, int patientInsuranceId, string serviceTypeCodeIds, string serviceCodeIds, TokenModel token);
        JsonModel GetEligibilityEnquiryServiceCodes(TokenModel token);
    }
}
