using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientImmunizationModel
    {
        public int Id { get; set; }
        public int PatientID { get; set; }
        public int? OrderBy { get; set; }
        public int? VFCID { get; set; }
        public DateTime AdministeredDate { get; set; }
        public int Immunization { get; set; }
        public int? AmountAdministered { get; set; }
        public int? ManufactureID { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string VaccineLotNumber { get; set; }
        public int? AdministrationSiteID { get; set; }
        public int? RouteOfAdministrationID { get; set; }
        public int? AdministeredBy { get; set; }
        public int ImmunityStatusID { get; set; }
        public bool RejectedImmunization { get; set; }
        public int? RejectionReasonID { get; set; }
        public string RejectionReasonNote { get; set; }
        public bool IsDeleted { get; set; }

        //
        public string VaccineName { get; set; }
        public string RouteOfAdministration { get; set; }
        public string AdministrationSite { get; set; }
        public string ConceptName { get; set; }
        public int LinkedEncounterId { get; set; }

        public int TotalRecords { get; set; }

    }
}
