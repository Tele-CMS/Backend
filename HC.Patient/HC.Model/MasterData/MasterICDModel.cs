using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.MasterData
{
   public class MasterICDModel
    {
        public int Id{ get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal TotalRecords { get; set; }
    }
}
