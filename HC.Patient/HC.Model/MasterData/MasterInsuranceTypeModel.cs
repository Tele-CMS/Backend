using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.MasterData
{
    public class MasterInsuranceTypeModel
    {   
        public int Id { get; set; }
        public string InsuranceType { get; set; }
        public string Description { get; set; }
        public string InsuranceCode { get; set; }
        public string value
        {
            get
            {
                try
                {
                    return this.InsuranceType;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public decimal TotalRecords { get; set; }
    }
}
