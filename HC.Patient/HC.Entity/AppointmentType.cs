using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class AppointmentType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AppointmentTypeID")]
        public int Id { get; set; }

        [Required]        
        [StringLength(200)]
        public string Name { get; set; }

        [NotMapped]
        public string value { get { return this.Name; } }

        public string Description { get; set; }
        
        public bool IsBillAble { get; set; }

        [MaxLength(200)]        
        public string DefaultDuration { get; set; }

        
        public bool? AllowMultipleStaff { get; set; }

        [StringLength(100)]
        public string Color { get; set; }

        [StringLength(100)]
        public string FontColor { get; set; }

        
        public bool? IsClientRequired { get; set; }
        
        
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        
        [Obsolete]
        public virtual Organization Organization { get; set; }
        public virtual List<StaffPayrollRateForActivity> StaffPayrollRateForActivity { get; set; }
    }
}