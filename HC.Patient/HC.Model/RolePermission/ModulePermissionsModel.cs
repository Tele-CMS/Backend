using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.RolePermission
{
    public class ModulePermissionsModel
    {
        public int ModulePermissionId { get; set; }
        public int ModuleId { get; set;}
        public string ModuleName { get; set; }
        public string ModuleKey { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Permission { get; set; }
        public int DisplayOrder { get; set; }
        public string ModuleIcon { get; set; }
        public string NavigationLink { get; set; }
    }
}
