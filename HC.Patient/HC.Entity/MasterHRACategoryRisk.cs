using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterHRACategoryRisk : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ReferralLinks { get; set; }
        [ForeignKey("Organization")]
        public int? OrganizationId { get; set; }
        public int? SortOrder { get; set; }
        [ForeignKey("GlobalCode")]
        public int? HRAGenderCriteria { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual GlobalCode GlobalCode { get; set; }

        public string ExecutiveDescription { get; set; }
    }
}
