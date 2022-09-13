using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class OrganizationSubscriptionPlan : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(100)]
        public string PlanName { get; set; }

        public DateTime StartDate { get; set; }

        public int PlanType { get; set; }

        public decimal AmountPerClient { get; set; }

        public int TotalNumberOfClients { get; set; }        

        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        //relationship tables
        [Obsolete]
        public virtual Organization Organization { get; set; }
    }
}

