using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterMedication : BaseEntity
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }        
        public string DrugName { get; set; }
        public string NDCCode { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string route_of_administration { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string dosage_form { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string strength { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string strength_unit_of_measure { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string generic_product_identifier { get; set; }
        public long? kdc { get; set; }
        public int? MaintenanceCode { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string GenericName { get; set; }
        [NotMapped]
        public string Value { get { return DrugName; } }
        
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        [Obsolete]
        public virtual Organization Organization { get; set; }
    }
}