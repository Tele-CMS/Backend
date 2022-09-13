using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.AppConfiguration;
using HC.Patient.Repositories.IRepositories.AppConfiguration;
using HC.Patient.Service.IServices.AppConfiguration;
using HC.Service;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HC.Patient.Service.Services.AppConfiguration
{
    public class AppConfigurationService : BaseService, IAppConfigurationService
    {
        private readonly IAppConfigurationRepository _appConfigurationRepository;
        private readonly HCOrganizationContext _context;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public AppConfigurationService(IAppConfigurationRepository appConfigurationRepository, HCOrganizationContext context)
        {
            _appConfigurationRepository = appConfigurationRepository;
            _context = context;
        }

        public JsonModel GetAppConfigurations(TokenModel tokenModel)
        {
            List<AppConfigurationsModel> appConfigurations = new List<AppConfigurationsModel>();
            List<AppConfigurations> AppConfigurations = _appConfigurationRepository.GetAll(a => a.OrganizationID == tokenModel.OrganizationID && a.IsDeleted == false && a.IsActive==true).ToList();
            if (AppConfigurations != null && AppConfigurations.Count > 0)
            {

                AutoMapper.Mapper.Map(AppConfigurations, appConfigurations);
                response = new JsonModel(appConfigurations, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }

        public JsonModel UpdateAppConfiguration(List<AppConfigurationsModel> appConfigurationsModels, TokenModel tokenModel)
        {
            if (appConfigurationsModels != null && appConfigurationsModels.Count > 0)
            {
                List<AppConfigurations> AppConfigurations = _appConfigurationRepository.GetAll(a => a.OrganizationID == tokenModel.OrganizationID && a.IsDeleted == false).ToList();
                foreach (AppConfigurations item in AppConfigurations)
                {
                    AppConfigurationsModel appConfigurationsModel = appConfigurationsModels.Find(a => a.Id == item.Id);
                    item.Value = appConfigurationsModel.Value;
                    _appConfigurationRepository.Update(item);
                }
                _appConfigurationRepository.SaveChanges();

                //return response
                response = new JsonModel(AppConfigurations, StatusMessage.AppConfigurationUpdated, (int)HttpStatusCode.OK);
            }
            return response;
        }

    }
}
