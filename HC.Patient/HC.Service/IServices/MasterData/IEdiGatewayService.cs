using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Service.Interfaces;
using Renci.SshNet;

namespace HC.Patient.Service.IServices.MasterData
{
    public interface IEdiGatewayService : IBaseService
    {
        ClearingHouseModel GetActiveClearingHouseDetails(TokenModel token);
        SftpClient CreateConnection(ClearingHouseModel ediGateway);
        
        #region Master EDI Service Methods
        JsonModel GetEDIGateways(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel SaveUpdate(EDIModel eDIModel, TokenModel tokenModel);
        JsonModel GetEDIGateWayById(int Id, TokenModel tokenModel);
        JsonModel DeleteEDIGateway(int id, TokenModel tokenModel);
        #endregion
    }
}
