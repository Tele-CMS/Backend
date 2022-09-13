using HC.Patient.Data;
using HC.Patient.Repositories.IRepositories.ThemeSettings;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.Repositories.ThemeSettings
{
    public class ThemeSettingRepository: RepositoryBase<Entity.ThemeSetting>, IThemeSettingRepository
    {
        private HCOrganizationContext _context;
    public ThemeSettingRepository(HCOrganizationContext context) : base(context)
    {
        this._context = context;
    }

        public Entity.ThemeSetting GetSettingByUserID(int UserID)
        {
            try
            {
                return _context.ThemeSetting.Where(x => x.UserId == UserID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
