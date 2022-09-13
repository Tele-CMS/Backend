using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.MasterServiceCodes
{
    public class MasterServiceCodesModel
    {   
        public int Id { get; set; }
        public string ServiceCode { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int UnitDuration { get; set; }
        public int UnitTypeID { get; set; }
        public decimal RatePerUnit { get; set; }
        public bool IsBillable { get; set; }
        public int RuleID { get; set; }
        public bool IsRequiredAuthorization { get; set; }
        public string RuleName { get; set; }
        public string UnitTypeName { get; set; }
        public decimal TotalRecords { get; set; }
        public List<ModifierModel> ModifierModel { get; set; }
    }

    public class ModifierModel
    {
        public int ModifierID { get; set; }
        public string Modifier { get; set; }
        public decimal Rate { get; set; }
        public int ServiceCodeId { get; set; }
        public string Key { get; set; }
        public bool IsUsedModifier { get; set; }
        
    }
}
