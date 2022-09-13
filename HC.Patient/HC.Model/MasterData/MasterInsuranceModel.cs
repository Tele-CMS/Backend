using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.MasterData
{
    public class MasterInsuranceModel
    {
        
        public  int Id { get; set; }

        public string Name { get; set; }

        public int InsuranceTypeId { get; set; }

        public string InsuranceType { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string CarrierPayerID { get; set; }
        public bool DayClubByProvider { get; set; }
        public string TPLCode { get; set; }
    }
}
