using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model
{
    public class ApplicationUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsValid { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public string BusinessToken { get; set; }
        public string DeviceToken { get; set; }
        public string UniqueDeviceId { get; set; }
        public string NPINumber { get; set; }
        public string JwtToken { get; set; }
    }
}
