using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Notification.Service.Model
{
    public class FcmResponce
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<Result> results { get; set; }
    }
    public class Result
    {
        public string error { get; set; }
    }
}
