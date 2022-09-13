using HC.Model;
using HC.Patient.Model.AppConfiguration;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.AppConfiguration
{
    public interface IAppConfigurationService : IBaseService
    {
        JsonModel GetAppConfigurations(TokenModel tokenModel);
        JsonModel UpdateAppConfiguration(List<AppConfigurationsModel> appConfigurationsModels, TokenModel tokenModel);
    }
}
