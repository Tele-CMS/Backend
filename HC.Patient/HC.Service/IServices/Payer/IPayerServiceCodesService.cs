using HC.Model;
using HC.Patient.Model.Payer;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.Payer
{
    public interface IPayerServiceCodesService : IBaseService
    {
        JsonModel AddUpdatePayerServiceCode(PayerServiceCodesModel payerServiceCodes, TokenModel token);
        JsonModel DeletePayerServiceCode(int payerServiceCodeId, TokenModel token);
        JsonModel GetPayerServiceCodeById(int payerServiceCodeId, TokenModel token);
        JsonModel CheckPayerServiceCodeModifierDependency(int payerModifierId, TokenModel token);
        // Custom
        JsonModel SavePayerServiceCode(List<PayerServiceCodesModel> payerServiceCodesModels, TokenModel tokenModel);
        JsonModel PayerOrMasterServiceCodes(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel GetExcludedServiceCodesFromActivity(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel GetPayerActivityServiceCodeDetailsById(int payerAppointmentTypeId, int payerServiceCodeId, TokenModel token);
        JsonModel GetMasterServiceCodeExcludedFromPayerServiceCodes(SearchFilterModel searchFilterModel, TokenModel tokenModel);
    }
}
