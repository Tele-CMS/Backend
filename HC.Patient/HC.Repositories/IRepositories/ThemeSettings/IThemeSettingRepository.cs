using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.ThemeSettings
{
    public interface IThemeSettingRepository: IRepositoryBase<ThemeSetting>

    {
        Entity.ThemeSetting GetSettingByUserID(int UserID);
    }
}
