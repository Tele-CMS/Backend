using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientsPrescriptionModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int? DrugID { get; set; }
        public string DrugName { get; set; }
        public string Dose { get; set; }
        public int? FrequencyID { get; set; }
        public string Strength { get; set; }
        public string Duration { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalRecords { get; set; }
        public string Directions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int? DeletedBy { get; set; }
        public string Patient { get; set; }
        public string Frequency { get; set; }
        public string Prescription { get; set; }
        public string IsActive { get; set; }
        public string DeletedDate { get; set; }
        public string Users { get; set; }
        public string Users1 { get; set; }
        public string Users2 { get; set; }
    }

    public class PatientsPrescriptionEditModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int? DrugID { get; set; }
        public string Dose { get; set; }
        public int? FrequencyID { get; set; }
        public string Strength { get; set; }
        public string Duration { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Notes { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int? DeletedBy { get; set; }
        public string Patient { get; set; }
        public string Frequency { get; set; }
        public string Prescription { get; set; }
        public string IsActive { get; set; }
        public string DeletedDate { get; set; }
        public string Users { get; set; }
        public string Users1 { get; set; }
        public string Users2 { get; set; }
        public string Directions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class PatientsPrescriptionPdfModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int? DrugID { get; set; }
        public string Dose { get; set; }
        public int? FrequencyID { get; set; }
        public string Strength { get; set; }
        public string Duration { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Notes { get; set; }
        public string Directions { get; set; }
        public string DateIssued { get; set; }
        public string Expires { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int? DeletedBy { get; set; }
        public string Patient { get; set; }
        public string Frequency { get; set; }
        public string Prescription { get; set; }
        public string IsActive { get; set; }
        public string DeletedDate { get; set; }
        public string Users { get; set; }
        public string Users1 { get; set; }
        public string Users2 { get; set; }

    }
    public class PatientPrescriptionPdf
    {
        public string PatientName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string MRN { get; set; }
        public List<PatientsPrescriptionPdfModel> PatientsPrescriptionPdfModels { get; set; }
    }

    public class MasterPrescriptionDrugsModel
    {
        public int Id { get; set; }
        public string DrugName { get; set; }
        public decimal TotalRecords { get; set; }
    }

    public class MasterPharmacyModel
    {
        public int Id { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyAddress { get; set; }
        public string PharmacyFaxNumber { get; set; }
        public decimal TotalRecords { get; set; }
    }
    public class PatientsSentPrescriptionModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public DateTime PrescriptionSentDate { get; set; }
        public decimal TotalRecords { get; set; }
        public string ModeOFTransfer { get; set; }
        public string PrescriptionId { get; set; }

    }

}
