using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class StaffLeave : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("Staffs")]
        public int StaffId { get; set; }

        [ForeignKey("LeaveType")]
        public int LeaveTypeId { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string OtherLeaveType { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string OtherLeaveReason { get; set; }

        [ForeignKey("LeaveReasonType")]
        public int LeaveReasonId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string Description { get; set; }
        [ForeignKey("LeaveStatus")]
        public int LeaveStatusId { get; set; }
        public DateTime ApprovalDate { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string DeclineReason { get; set; }
        [Obsolete]
        public virtual Staffs Staffs { get; set; }        
        public virtual GlobalCode LeaveType { get; set; }
        public virtual GlobalCode LeaveReasonType { get; set; }
        public virtual GlobalCode LeaveStatus { get; set; }

    }
}
