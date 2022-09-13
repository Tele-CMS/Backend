using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.GlobalCodes
{
    public interface IGlobalCodeRepository : IRepositoryBase<GlobalCode>
    {
        /// <summary>
        /// getGlobalCodeByOrganizationId
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="FilterModel"></param>
        /// <returns></returns>
        IQueryable<GlobalCode> getGlobalCodeByOrganizationId(TokenModel tokenModel, GlobalCodeFilterModel FilterModel);

        /// <summary>
        /// getGlobalCodeByOrganizationId
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="FilterModel"></param>
        /// <returns></returns>
        IQueryable<GlobalCode> getGlobalServiceIconName(TokenModel tokenModel, GlobalCodeFilterModel FilterModel);

        /// <summary>
        /// saveUpdateGlobalCode
        /// </summary>
        /// <param name="globalCode"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        GlobalCode saveUpdateGlobalCode(GlobalCode globalCode, TokenModel tokenModel);

        /// <summary>
        /// CheckGlobalCodeExistance
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        GlobalCode CheckGlobalCodeExistance(string name, TokenModel tokenModel);

        /// <summary>
        /// CheckGlobalCodeExistance
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        GlobalCode CheckGlobalCodeExistanceWithPhoto(GlobalCodeModel globalCode, TokenModel tokenModel);

        /// <summary>
        /// getGlobalCodeById
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        GlobalCode getGlobalCodeById(TokenModel tokenModel, int id);
    }
}
