using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class QuestionnaireBenchmarkRange:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("MasterBenchmark")]
        public int BenchmarkId { get; set; }
        public decimal MinRange { get; set; }
        public decimal MaxRange { get; set; }
        [ForeignKey("DFA_Document")]
        public int QuestionnaireId { get; set; }
        public virtual DFA_Document DFA_Document { get; set; }
        public virtual MasterBenchmark MasterBenchmark { get; set; }
    }
}
