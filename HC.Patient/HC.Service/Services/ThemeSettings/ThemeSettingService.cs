using AutoMapper;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.ThemeSettings;
using HC.Patient.Repositories.IRepositories.ThemeSettings;
using HC.Patient.Service.IServices.ThemeSettings;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.ThemeSettings
{
    public class ThemeSettingService : BaseService,IThemeSettingService
    {
        private readonly IThemeSettingRepository _themeSettingRepository;
        private readonly HCOrganizationContext _context;
        public ThemeSettingService(IThemeSettingRepository themeSettingRepository, HCOrganizationContext context)
        {
            _themeSettingRepository = themeSettingRepository;
            _context = context;
        }

        public JsonModel CreateSettings(ThemeSettingsModel model, TokenModel token)
        {
            try
            {
                var existingRecord = _context.ThemeSetting.Where(x => x.UserId == model.UserId).FirstOrDefault();
                if (existingRecord != null)
                {
                    existingRecord.ThemeScheme = model.ThemeScheme;
                    existingRecord.Theme = model.Theme;
                    existingRecord.ThemeLayout = model.ThemeLayout;
                    _themeSettingRepository.Update(existingRecord);
                    _themeSettingRepository.SaveChanges();

                }
                else //insert
                {
                    ThemeSetting themeSettings = new ThemeSetting();

                    Mapper.Map(model, themeSettings);
                    themeSettings.OrganizationID = token.OrganizationID;
                    themeSettings.CreatedBy = token.UserID;
                    themeSettings.CreatedDate = DateTime.UtcNow;
                    themeSettings.IsDeleted = false;
                    themeSettings.IsActive = true;
                    _themeSettingRepository.Create(themeSettings);
                    _themeSettingRepository.SaveChanges();


                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }


        public JsonModel GetSettings(int userId)
        {
            ThemeSetting existingRecord = _themeSettingRepository.GetSettingByUserID(userId);
            ThemeSettingsModel themeSettingsModel = new ThemeSettingsModel();

            if(existingRecord != null)
            {
                themeSettingsModel.ThemeScheme = existingRecord.ThemeScheme;
                themeSettingsModel.ThemeLayout = existingRecord.ThemeLayout;
                themeSettingsModel.Theme = existingRecord.Theme;

                return new JsonModel()
                {
                    data = themeSettingsModel,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
        }
    }
}
