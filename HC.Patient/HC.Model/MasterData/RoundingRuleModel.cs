using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.MasterData
{
    public class RoundingRuleModel
    {
        public int Id { get; set; }
        public string RuleName { get; set; }
        public bool IsDeleted { get; set; }
        public int OrganizationID { get; set; }
        public int RowNumber { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public List<RoundingRuleDetailsModel> RoundingRuleDetail { get; set; }
    }

    public class RoundingRuleModelList
    {
        public int Id { get; set; }
        public string RuleName { get; set; }
        public int OrganizationID { get; set; }
        public long? RowNumber { get; set; }
        public int? TotalRecords { get; set; }
        public double? TotalPages { get; set; }
    }

    public class RoundingRuleDetailsModel
    {
        public int Id { get; set; }
        public decimal StartRange { get; set; }
        public decimal EndRange { get; set; }
        public int Unit { get; set; }
        public bool IsDeleted { get; set; }
    }
}
