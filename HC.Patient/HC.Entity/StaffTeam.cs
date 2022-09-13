using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class StaffTeam : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("Staffs")]        
        public int StaffId { get; set; }
                
        [ForeignKey("StaffTeamGroup")]
        public int StaffTeamID { get; set; }        
        
        public virtual Staffs Staffs { get; set; }
        public virtual Staffs StaffTeamGroup { get; set; }    
    }
}