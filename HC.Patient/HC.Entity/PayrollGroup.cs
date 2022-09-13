using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PayrollGroup : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string GroupName { get; set; }
        [ForeignKey("PayPeriod")]
        public int PayPeriodId { get; set; }
        [ForeignKey("WorkWeek")]
        public int WorkWeekId { get; set; }
        public decimal DailyLimit { get; set; }
        public decimal WeeklyLimit { get; set; }
        public decimal OverTime { get; set; }
        public decimal DoubleOverTime { get; set; }
        public bool IsCaliforniaRule { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual GlobalCode PayPeriod { get; set; }
        public virtual GlobalCode WorkWeek { get; set; }
        public virtual PayrollBreakTime PayrollBreakTime { get; set; }

        [ForeignKey("PayrollBreakTime")]
        public int PayrollBreakTimeId { get; set; }
    }
}
