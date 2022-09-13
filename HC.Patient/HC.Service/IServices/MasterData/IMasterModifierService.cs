using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.MasterData
{
    public interface IMasterModifierService : IBaseService
    {
       JsonModel GetMasterModifiersList(int pageNumber,int pageSize,string modifier,TokenModel token);
       JsonModel CreateMasterModifiers(MasterModifierModel masterModifierModel, TokenModel token);
       JsonModel GetMasterModifierDetail(int modifierId);
       JsonModel DeleteMasterModifier(int modifierId, TokenModel token);

    }
}
