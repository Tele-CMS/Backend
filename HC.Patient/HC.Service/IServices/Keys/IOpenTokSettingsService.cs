using HC.Model;
using HC.Patient.Model;

namespace HC.Patient.Service.IServices
{
    public interface IOpenTokSettingsService
    {
        /// <summary>
        /// Get Open Tok Settings By OrganizationId
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        OpenTokSettingModel GetOpenTokSettingsByOrganizationId(TokenModel tokenModel);
    }
}
