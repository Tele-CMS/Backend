using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.RolePermission
{
    public class ScreenPermissionsModel
    {
        public int ScreenPermissionId { get; set; }
        public int ModuleId { get; set; }
        public int RoleId { get; set; }
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public string ScreenKey { get; set; }
        public bool Permission { get; set; }
        public int DisplayOrder { get; set; }
        public string NavigationLink { get; set; }
    }
}
