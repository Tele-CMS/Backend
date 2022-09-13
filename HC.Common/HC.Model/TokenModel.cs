using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Model
{
    public class TokenModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int RoleID { get; set; }
        public int OrganizationID { get; set; }
        public int LocationID { get; set; }
        public int StaffID { get; set; }
        public string Timezone { get; set; }
        public string IPAddress { get; set; }
        public string DomainName { get; set; }
        public string MacAddress { get; set; }
        public int PateintID { get; set; }
        public int OffSet { get; set; }
        public string AccessToken { get; set; }
        public HttpContext Request { get; set; }
    }
    public class DomainToken
    {
        public string BusinessToken { get; set; }
        public int OrganizationId { get; set; }
        public string HostName { get; set; }
        //public int organizationID { get; set; }

        
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public object Organization { get; set; }
    }
}
