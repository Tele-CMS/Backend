using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.CustomMessage
{
    public class SQLResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string ResponseIds { get; set; }
    }

    public class RecordDependenciesModel {
        public string TableName { get; set; }
        public int TotalCount { get; set; }
    }

}
