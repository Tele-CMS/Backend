using HC.Model;
using HC.Patient.Model.ThemeSettings;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.ThemeSettings
{
    public interface IThemeSettingService : IBaseService
    {
        JsonModel CreateSettings(ThemeSettingsModel model, TokenModel token);
        JsonModel GetSettings(int userId);
    }
}
