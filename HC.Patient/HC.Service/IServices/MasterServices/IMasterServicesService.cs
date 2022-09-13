using HC.Model;
using HC.Patient.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices
{
    public interface IMasterServicesService: IBaseService
    {
        /// <summary>
        /// To Get Master Services Based On OrganizatonId
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel getMasterServicesByOrganizationId(TokenModel tokenModel, MasterServiceFilterModel masterServiceFilterModel);


        /// <summary>
        /// To get master service by service id
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        JsonModel getMasterServicesById(TokenModel tokenModel, string id);

        /// <summary>
        /// To Check whether service name exists or not by service name
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        JsonModel ChecServiceNameExistance(TokenModel tokenModel, string name);


        /// <summary>
        /// To save and update master services
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="masterServicesModel"></param>
        /// <returns></returns>
        JsonModel SaveUpdateMasterService(TokenModel tokenModel, MasterServicesModel masterServicesModel);

        /// <summary>
        /// To Delete Master Service By Service Id
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        JsonModel DeleteMasterService(TokenModel tokenModel, string id);
    }
}
