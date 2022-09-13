using EDIGenerator.IServices;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.EDI;
using HC.Patient.Service.IServices.EDI;
using HC.Service;
using System.Linq;
using System.Linq.Dynamic.Core;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.EDI
{
    public class EDI270GenerationService : BaseService, IEDI270GenerationService
    {
        private readonly IEDI270Repository _edi270Repository;
        private readonly IEDI270Service _edi270Service;
        private JsonModel response=null;
        public EDI270GenerationService(IEDI270Repository edi270Repository,IEDI270Service edi270Service)
        {
            _edi270Repository = edi270Repository;
            _edi270Service = edi270Service;
        }

        public string Download270(int patientId, int patientInsuranceId, string serviceTypeCodeIds, string serviceCodeIds, TokenModel token)
        {
            string ediText = string.Empty;
            SQLResponseModel saveEnquiryResponse = _edi270Repository.SaveEligibilityEnquiryRequestData<SQLResponseModel>(patientId, patientInsuranceId, serviceTypeCodeIds, serviceCodeIds, token).FirstOrDefault();
            if (saveEnquiryResponse != null && saveEnquiryResponse.StatusCode == 200)
            {
                int eligibilityMasterId = 0;
                int.TryParse(saveEnquiryResponse.ResponseIds, out eligibilityMasterId);
                EDI270FileModel edi270Data = _edi270Repository.GetEDI270RequestInfo(patientId, patientInsuranceId, eligibilityMasterId, token);
                ediText = _edi270Service.GenerateEDI270(edi270Data);
                if (!string.IsNullOrEmpty(ediText))
                {
                    SQLResponseModel updateEligibilityResponse = _edi270Repository.UpdateEligibilityRequest<SQLResponseModel>(eligibilityMasterId, ediText, "update", token).FirstOrDefault();
                    if (updateEligibilityResponse != null && updateEligibilityResponse.StatusCode == 200)
                        response = new JsonModel(null, StatusMessage.EligibilityFileRequestSuccess, (int)HttpStatusCodes.OK, string.Empty);
                    else
                    {
                        _edi270Repository.UpdateEligibilityRequest<SQLResponseModel>(eligibilityMasterId, ediText, "delete", token).FirstOrDefault();
                        response = new JsonModel(null, StatusMessage.EligibilityFileRequestFail, (int)HttpStatusCodes.InternalServerError, string.Empty);
                    }
                }
                else
                {
                    _edi270Repository.UpdateEligibilityRequest<SQLResponseModel>(eligibilityMasterId, ediText, "delete", token).FirstOrDefault();
                    response = new JsonModel(null, StatusMessage.EligibilityFileRequestFail, (int)HttpStatusCodes.InternalServerError, string.Empty);
                }
            }
            return ediText;
        }

        public JsonModel Generate270EligibilityRequestFile(int patientId, int patientInsuranceId, string serviceTypeCodeIds, string serviceCodeIds, TokenModel token)
        {
            SQLResponseModel saveEnquiryResponse = _edi270Repository.SaveEligibilityEnquiryRequestData<SQLResponseModel>(patientId, patientInsuranceId, serviceTypeCodeIds, serviceCodeIds, token).FirstOrDefault();
            if (saveEnquiryResponse != null && saveEnquiryResponse.StatusCode == 200)
            {
                int eligibilityMasterId = 0;
                int.TryParse(saveEnquiryResponse.ResponseIds, out eligibilityMasterId);
                EDI270FileModel edi270Data = _edi270Repository.GetEDI270RequestInfo(patientId, patientInsuranceId, eligibilityMasterId, token);
                string ediText=_edi270Service.GenerateEDI270(edi270Data);
                if (!string.IsNullOrEmpty(ediText))
                {
                    SQLResponseModel updateEligibilityResponse= _edi270Repository.UpdateEligibilityRequest<SQLResponseModel>(eligibilityMasterId, ediText, "update", token).FirstOrDefault();
                    if (updateEligibilityResponse != null && updateEligibilityResponse.StatusCode == 200)
                        response = new JsonModel(null, StatusMessage.EligibilityFileRequestSuccess, (int)HttpStatusCodes.OK, string.Empty);
                    else
                    {
                        _edi270Repository.UpdateEligibilityRequest<SQLResponseModel>(eligibilityMasterId, ediText, "delete", token).FirstOrDefault();
                        response = new JsonModel(null, StatusMessage.EligibilityFileRequestFail, (int)HttpStatusCodes.InternalServerError, string.Empty);
                    }
                }
                else
                {
                    _edi270Repository.UpdateEligibilityRequest<SQLResponseModel>(eligibilityMasterId, ediText, "delete", token).FirstOrDefault();
                    response = new JsonModel(null, StatusMessage.EligibilityFileRequestFail, (int)HttpStatusCodes.InternalServerError, string.Empty);
                }
            }
            return response;
        }

        public JsonModel GetEligibilityEnquiryServiceCodes(TokenModel token)
        {
            return new JsonModel(_edi270Repository.GetEligibilityEnquiryServiceCodes<EligibilityEnquiryServiceTypeMasterModel>(token).ToList(), StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
        }
    }
}
