using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Payer
{
    public class MasterActivitiesModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string DefaultDuration { get; set; }
        public string ColorCode { get; set; }
    }
    public class PayerActivityServiceCodeModel
    {
        public int Id { get; set; }
        public int ServiceCodeId { get; set; }
        public int PayerServiceCodeId { get; set; }
        public string ServiceCode { get; set; }
        public string Description { get; set; }
        public int UnitDuration { get; set; }
        public string UnitTypeName { get; set; }
        public decimal RatePerUnit { get; set; }
        public bool IsBillable { get; set; }
        public bool IsRequiredAuthorization { get; set; }
        public decimal TotalRecords { get; set; }
        public string AttachedModifiers { get; set; }
    }
}
