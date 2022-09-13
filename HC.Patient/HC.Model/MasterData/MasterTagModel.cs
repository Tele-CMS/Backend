using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.MasterData
{
    public class MasterTagModel
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public string value { get { return this.Tag; } set { this.Tag = value; } }
        public string ColorName { get; set; }
        public string FontColorCode { get; set; }
        public string ColorCode { get; set; }
        public int? RoleTypeID { get; set; }
        public string RoleTypeName { get; set; }
        public string TypeKey { get; set; }
        public int? OrganizationID { get; set; }
        public decimal TotalRecords { get; set; }
    }
}
