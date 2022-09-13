using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientAllergyModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int AllergyTypeId { get; set; }
        public string AllergyType { get; set; }
        public string Allergen { get; set; }
        public string Note { get; set; }
        public int ReactionID { get; set; }
        public string Reaction { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LinkedEncounterId { get; set; }
        public decimal TotalRecords { get; set; }
    }
}
