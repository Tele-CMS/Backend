using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterEncChecklistReviewItemsConditionMapping: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("MasterEncChecklistReviewItems")]
        public int MasterEncChecklistReviewItemId { get; set; }
        [ForeignKey("MasterChronicCondition")]
        public int MasterChronicConditionId { get; set; }
        public virtual MasterEncChecklistReviewItems MasterEncChecklistReviewItems { get; set; }
       // public virtual MasterChronicCondition MasterChronicCondition { get; set; }
    }
}
