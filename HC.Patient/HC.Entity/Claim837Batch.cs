using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Claim837Batch : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [Column(TypeName = "text")]
        public string EDIFileText { get; set; }

        public DateTime SentDate { get; set; }
        public int NoOfClaims { get; set; }

        [ForeignKey("EDIGateway")]
        public int EDIGatewayId { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
                
        [Obsolete]
        public virtual Organization Organization { get; set; }

        [Obsolete]
        public virtual EDIGateway EDIGateway { get; set; }
    }
}
