using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterLabTestAnalytes : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Analyte { get; set; }
        public string Unit { get; set; }
        public string LOINCCode { get; set; }
        public string SpecimenType { get; set; }
        public decimal? HighValue { get; set; }
        public decimal? LowValue { get; set; }
        public int? SortOrder { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string MasterAnalyteCode { get; set; }
        [ForeignKey("Organization")]
        public int? OrganizationId { get; set; }
        [ForeignKey("MasterMeasureSign")]
        public int? MasterSignId { get; set; }
        public string ReferenceRange { get; set; }
        public int? SearchOrder { get; set; }
        [ForeignKey("MasterGlobalControlType")]
        public int? MasterControlTypeId { get; set; }
        public virtual MasterMeasureSign MasterMeasureSign { get; set; }
        public virtual GlobalCode MasterGlobalControlType { get; set; }
        public virtual Organization Organization { get; set; }

    }

}
