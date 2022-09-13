using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterCity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CityID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string CityName { get; set; }
        [NotMapped]
        public string value { get { return this.CityName; } set { this.CityName = value; } }
        [ForeignKey("MasterState")]
        public int? StateID { get; set; }
        public virtual MasterState MasterState { get; set; }
    }
}

