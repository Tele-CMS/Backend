using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterPharmacy : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PharmacyID")]
        public int Id { get; set; }
        [StringLength(200)]
        public string PharmacyName { get; set; }
        public string PharmacyAddress { get; set; }
        public string PharmacyFaxNumber { get; set; }
        [ForeignKey("MasterCity")]
        public int? CityID { get; set; }
        public virtual MasterCity MasterCity { get; set; }

    }
}