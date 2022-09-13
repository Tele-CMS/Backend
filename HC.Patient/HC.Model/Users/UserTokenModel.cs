using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Users
{
    public class UserTokenModel
    {
        public string ApnToken { get; set; }
        public string DeviceToken { get; set; }
        public string Name { get; set; }
    }
}
