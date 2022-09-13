using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
   public class PatientPhysicianModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhysicianName { get; set; }
        public string DEANumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Speciality { get; set; }
        public int? SpecialityId { get; set; }
        public int LinkedEncounterId { get; set; }
        public string NPINumber { get; set; }
        public string Email { get; set; }
        public string FaxNumber { get; set; }

        public int? TaxonomyCodesID { get; set; }

        public string Taxonomy { get; set; }
        public int TotalRecords { get; set; }
    }

    public class PatientProviderModel: PatientPhysicianModel
    {
        public string ProviderFullName { get; set; }
    }

}
