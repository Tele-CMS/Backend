using HC.Model;
using HC.Patient.Model.Payer;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Payer
{
    public interface IPayerActivityService: IBaseService
    {
        JsonModel GetActivities(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel GetPayerActivityServiceCodes(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel GetMasterActivitiesForPayer(SearchFilterModel searchFilterModel, TokenModel token);
        JsonModel UpdatePayerActivityModifiers(PayerAppointmentTypesModel payerAppointmentTypesModel, TokenModel token);
        JsonModel DeletePayerActivityCode(int id, TokenModel tokenModel);
        JsonModel SavePayerActivityCode(PayerActivityCodeModel payerActivityCodeModel, TokenModel tokenModel);
    }
}
