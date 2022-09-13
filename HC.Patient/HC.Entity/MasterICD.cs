using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterICD : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ICDID")]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
        public string CodeType { get; set; }
        [NotMapped]
        public string value
        {
            get
            {
                try
                {
                    
                        string value = "(" + this.Code + ") ";

                        return this.Description.Length > 30 ? value = value + this.Description.Substring(0, 29) + ".." : value = value + this.Description;
                
                    
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public string Description { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}