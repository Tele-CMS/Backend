using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterEncChecklistReviewItems: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int MasterEncounterChecklistId { get; set; }

        [Obsolete]
        public virtual MasterEncounterChecklist MasterEncounterChecklist { get; set; }
    }
}
