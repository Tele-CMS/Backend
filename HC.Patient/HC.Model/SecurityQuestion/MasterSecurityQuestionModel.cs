using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.SecurityQuestion
{
    public class MasterSecurityQuestionModel
    {
        public int Id{ get; set; }
        public string Question { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalRecords { get; set; }
    }
}
