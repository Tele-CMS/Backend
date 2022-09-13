using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.MasterData
{
    public interface IMasterInsuranceTypeService: IBaseService
    {
        JsonModel GetInsuranceTypes(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel SaveInsuranceType(MasterInsuranceTypeModel masterInsuranceTypeModel, TokenModel tokenModel);
        JsonModel GetInsuranceTypeById(int id, TokenModel tokenModel);
        JsonModel DeleteInsuranceType(int id, TokenModel tokenModel);
    }
}
