using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MachineLoginLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string IpAddress { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string MacAddress { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public int OrganizationID { get; set; }
        public string UniqueDeviceId { get; set; }
        public DateTime LoginDate { get; set; }

        public virtual Organization Organization { get; set; }        
        public virtual User User { get; set; }
    }
}
