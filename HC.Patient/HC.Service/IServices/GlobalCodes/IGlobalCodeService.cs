using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.GlobalCodes
{
    public interface IGlobalCodeService: IBaseService
    {
        /// <summary>
        /// GetGlobalCodeValueId
        /// </summary>
        /// <param name="globalCodeCategoryName"></param>
        /// <param name="globalCodeValue"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        int GetGlobalCodeValueId(string globalCodeCategoryName, string globalCodeValue,TokenModel token);
        /// <summary>
        /// globalCodeCategoryName
        /// </summary>
        /// <param name="globalCodeCategoryName"></param>
        /// <param name="globalCodeValue"></param>
        /// <param name="token"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        int GetGlobalCodeValueId(string globalCodeCategoryName, string globalCodeValue, TokenModel token, bool isActive = true);
        /// <summary>
        /// getGlobalCodeByOrganizationId
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="FilterModel"></param>
        /// <returns></returns>
        JsonModel GetGlobalCodeByOrganizationId(TokenModel tokenModel, GlobalCodeFilterModel FilterModel);

        /// <summary>
        /// getGlobalCodeByOrganizationId
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetGlobalServiceIconName(TokenModel tokenModel, GlobalCodeFilterModel FilterModel);
        /// <summary>
        /// saveUpdateGlobalCode
        /// </summary>
        /// <param name="globalCode"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel SaveUpdateGlobalCode(GlobalCodeModel globalCode, TokenModel tokenModel);

        /// <summary>
        /// CheckGlobalCodeExistance
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>

        JsonModel CheckGlobalCodeExistance(string name, TokenModel tokenModel);
        /// <summary>
        /// getGlobalCodeById
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        JsonModel GetGlobalCodeById(TokenModel tokenModel, string id);
        /// <summary>
        /// DeleteGlobalCode
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        JsonModel DeleteGlobalCode(TokenModel tokenModel, int id);
    }
}
