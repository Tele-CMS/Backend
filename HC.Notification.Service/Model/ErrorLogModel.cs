using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Notification.Service.Model
{
    public class ErrorLogModel
    {
        public int ErrorLine { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorNumber { get; set; }
        public DateTime ErrorTime { get; set; }
        public int ErrorLogTypeId { get; set; }
        public int OrganizationId { get; set; }
    }
}
