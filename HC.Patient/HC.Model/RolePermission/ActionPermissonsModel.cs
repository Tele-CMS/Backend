using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.RolePermission
{
    public class ActionPermissonsModel
    {
        public int ActionPermissionId { get; set; }
        public int ModuleId { get; set; }
        public int RoleId { get; set; }
        public int ScreenId { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public string ActionKey { get; set; }
        public bool Permission { get; set; }

    }
}
