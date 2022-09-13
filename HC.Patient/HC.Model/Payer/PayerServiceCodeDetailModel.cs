using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Payer
{
    public class PayerServiceCodeDetailModel
    {
        public int Id { get; set; }
        public string ServiceCode { get; set; }
        public string Description { get; set; }
        public int UnitDuration { get; set; }
        public int UnitType { get; set; }
        public decimal RatePerUnit { get; set; }
        public bool IsBillable { get; set; }
        public int? Modifier1 { get; set; }
        public int? Modifier2 { get; set; }
        public int? Modifier3 { get; set; }
        public int? Modifier4 { get; set; }
        public List<PayerModifierModel> PayerModifierModel { get; set; }
    }

    public class PayerAppointmentTypesModel
    {
        public int Id { get; set; }
        public decimal? RatePerUnit { get; set; }
        public int? Modifier1 { get; set; }
        public int? Modifier2 { get; set; }
        public int? Modifier3 { get; set; }
        public int? Modifier4 { get; set; }
    }

    public class PayerServiceCodesModel
    {
        public int Id { get; set; }
        public string ServiceCode { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int UnitDuration { get; set; }
        public int UnitType { get; set; }
        public decimal RatePerUnit { get; set; }
        public bool IsBillable { get; set; }
        public int RuleID { get; set; }
        public bool IsRequiredAuthorization { get; set; }
        public string RuleName { get; set; }
        public string UnitTypeName { get; set; }
        public int PayerId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? NewRatePerUnit { get; set; }
        public int ServiceCodeId { get; set; }
        public decimal TotalRecords { get; set; }
        public List<PayerModifierModel> PayerModifierModel { get; set; }
    }

    public class PayerModifierModel
    {
        public int Id { get; set; }

        public int RowNo { get; set; }
        public string Modifier { get; set; }
        public decimal Rate { get; set; }
        public int PayerServiceCodeId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsUsedModifier { get; set; }
    }
    public class PayerOrMasterServiceCodesModel
    {
        public int MasterServiceCodeId { get; set; }
        public int PayerServiceCodeId { get; set; }
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
        public int PayerId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? NewRatePerUnit { get; set; }
        public int ServiceCodeId { get; set; }
        public decimal TotalRecords { get; set; }
        public List<PayerModifierModel> PayerModifierModel { get; set; }
    }
}
