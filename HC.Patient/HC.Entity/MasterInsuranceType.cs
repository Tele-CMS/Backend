using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterInsuranceType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public string InsuranceType { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string InsuranceCode { get; set; }

        [NotMapped]
        public string value
        {
            get
            {
                try
                {
                    return this.InsuranceType;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }        
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
