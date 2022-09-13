using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientEncounters
{
    public class PatientEncounterTemplateModel
    {
        public int Id { get; set; }
        public int? PatientEncounterId { get; set; }
        public int? MasterTemplateId { get; set; }
        [Obsolete]
        public string TemplateName { get; set; }
        public string TemplateData { get; set; }
        [Obsolete]
        public string TemplateJson { get; set; }
        public bool IsDeleted { get; set; }
    }
}
