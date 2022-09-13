using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.MasterData
{
    public interface IMasterTagService: IBaseService
    {
        JsonModel GetMasterTag(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel SaveMasterTag(MasterTagModel masterTagModel, TokenModel tokenModel);
        JsonModel GetMasterTagById(int id, TokenModel tokenModel);
        JsonModel DeleteMasterTag(int id, TokenModel tokenModel);
    }
}
