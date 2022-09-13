using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Notification.Service.Model
{
    public class MasterDatabaseModel
    {
        public int OrganizationID { get; set; }
        public string DatabaseName { get; set; }
        public string Password { get; set; }
        public string ServerName { get; set; }
        public string UserName { get; set; }
    }
}
