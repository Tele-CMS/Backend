using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Payment
{
    public class ManageFeesRefundsModel
    {
        public List<int> Providers { get; set; }
        public decimal F2fFee { get; set; }
        public decimal NewOnlineFee { get; set; }
        public decimal? FolowupFees { get; set; }
        public int? FolowupDays { get; set; }
        public List<CancelationRuleModel> CancelationRules { get; set; }
        public decimal? UrgentcareFee { get; set; }
    }

    public class ProviderFeesModel
    {
        public int StaffId { get; set; }
        public decimal F2fFee { get; set; }
        public decimal NewOnlineFee { get; set; }
        public decimal? UrgentcareFee { get; set; }
    }

    public class CancelationRuleModel
    {
        public int UptoHours { get; set; }
        public int RefundPercentage { get; set; }
        public int StaffId { get; set; }
    }
}
