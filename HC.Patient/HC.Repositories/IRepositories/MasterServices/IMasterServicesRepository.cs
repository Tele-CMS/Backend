using HC.Model;
using HC.Patient.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories
{
    public interface IMasterServicesRepository
    {
        /// <summary>
        /// To Get All Master Servives Based On OrganizationId
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        IQueryable<MasterServices> getMasterServicesByOrganizationId(TokenModel tokenModel, MasterServiceFilterModel masterServiceFilterModel);

        /// <summary>
        /// To Save And Update Particular Service
        /// </summary>
        /// <param name="masterServices"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        MasterServices saveUpdateMasterServices(MasterServices masterServices, TokenModel tokenModel);

        /// <summary>
        /// To Check whether service name exists or not
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        MasterServices CheckServiceNameExistance(string name, TokenModel tokenModel);

        /// <summary>
        /// To Get Master Service By Service Id
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="id">Int Id</param>
        /// <returns></returns>
        MasterServices getMasterServicesById(TokenModel tokenModel, int id);
    }
}
