using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class AppConfigurations : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        
        public int Id { get; set; }

        
        public string Key { get; set; }
        
        public string Label { get; set; }

        
        public string Value { get; set; }

        
        public int ConfigType { get; set; }

        [Required]
        
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        
        [Obsolete]
        public virtual Organization Organization { get; set; }
    }
}
