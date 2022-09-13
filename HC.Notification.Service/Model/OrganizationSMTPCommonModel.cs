using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Notification.Service.Model
{
    public class OrganizationSMTPCommonModel
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string Port { get; set; }
        public string ConnectionSecurity { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public int OrganizationID { get; set; }
        public bool IsDeleted { get; set; }
        public string NetworkUserName { get; set; }
    }
}
