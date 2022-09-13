using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterNoteType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public string Type { get; set; }

        [NotMapped]
        public string value { get { return this.Type; } set { this.Type = value; } }

        [MaxLength(1000)]
        public string Description { get; set; }

        public bool IsDirectService { get; set; }
        
        [StringLength(100)]
        public string NavigationLink { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        [Obsolete]
        public virtual Organization Organization { get; set; }        
    }
}