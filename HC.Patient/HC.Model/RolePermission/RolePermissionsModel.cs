using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.RolePermission
{
    public class RolePermissionsModel
    {
        public List<ModulePermissionsModel> ModulePermissions { get; set; }
        public List<ScreenPermissionsModel> ScreenPermissions { get; set; }
        public List<ActionPermissonsModel> ActionPermissions { get; set; }
    }
}
