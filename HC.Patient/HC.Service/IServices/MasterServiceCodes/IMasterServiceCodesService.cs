using HC.Model;
using HC.Patient.Model.MasterServiceCodes;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.MasterServiceCodes
{
    public interface IMasterServiceCodesService : IBaseService
    {
        JsonModel AddUpdateMasterServiceCode(MasterServiceCodesModel masterServiceCodes, TokenModel token);
        JsonModel GetMasterServiceCodes(string searchText, TokenModel token, int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "");
        JsonModel DeleteServiceCode(int serviceCodeId, TokenModel token);
        JsonModel GetMasterServiceCodeById(int serviceCodeId, TokenModel token);
    }
}
