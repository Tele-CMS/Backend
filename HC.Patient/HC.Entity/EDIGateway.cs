using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class EDIGateway : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("EDIGatewayID")]
        public int Id { get; set; }

        [MaxLength(200)]
        public string ClearingHouseName { get; set; }

        [MaxLength(60)]
        public string SenderID { get; set; }

        [MaxLength(60)]
        public string ReceiverID { get; set; }

        [MaxLength(10)]
        public string InterchangeQualifier { get; set; }

        [MaxLength(200)]
        public string FTPURL { get; set; }
        [MaxLength(10)]
        public string PortNo { get; set; }
        [MaxLength(200)]
        public string FTPUsername { get; set; }
        [MaxLength(200)]
        public string FTPPassword { get; set; }

        [MaxLength(100)]
        public string Path837 { get; set; }

        [MaxLength(100)]
        public string Path835 { get; set; }


        [MaxLength(100)]
        public string Path276 { get; set; }

        [MaxLength(100)]
        public string Path277 { get; set; }

        [MaxLength(100)]
        public string Path999 { get; set; }

        [MaxLength(100)]
        public string Path270 { get; set; }

        [MaxLength(100)]
        public string Path271 { get; set; }



        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
