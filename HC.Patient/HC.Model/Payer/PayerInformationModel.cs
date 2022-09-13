using HC.Patient.Entity;
using HC.Patient.Model.MasterServiceCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HC.Patient.Model.Payer
{

    public class PayerAppTypeModel
    {
        public int PayerServiceCodeID { get; set; }
        public int ServiceCodeID { get; set; }
        public string CPTCode { get; set; }
        public string Description { get; set; }
        public int UnitDuration { get; set; }
        public bool IsBillable { get; set; }
        public decimal RatePerUnit { get; set; }
        public int UnitTypeID { get; set; }
        public string UnitTypeName { get; set; }
        public string Type { get; set; }
        public int PayerID { get; set; }
        public int PayerAppointmentTypeID { get; set; }
        public bool IsDeleted { get; set; }
        public int? AppointmentTypeID { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public long? RowNumber { get; set; }
        public int? TotalRecords { get; set; }
        public double? TotalPages { get; set; }
        public bool IsRequiredAuthorization { get; set; }
        public decimal? NewRatePerUnit { get; set; }
        public int RuleID { get; set; }
        public string RuleName { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }

    public class PayerInformationModel: MasterServiceCodeModel
    {
        public PayerInformationModel()
        {
            this.CreatedBy = 1;
        }
        //public int PayerID { get; set; }
        public string PayerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string InsuranceType { get; set; }
        public string Status { get; set; }
        //public bool IsActive { get; set; }
        public int InsuranceTypeID { get; set; }
        public string TPLCode { get; set; }
        public string City { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public string StateName { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
        public string Fax { get; set; }
       // public long? RowNumber { get; set; }
       // public int? TotalRecords{ get; set; }
        public int CreatedBy { get; set; }
        public string CarrierPayerID { get; set; }
        public bool DayClubByProvider { get; set; }
        public bool IsEDIPayer { get; set; }
        public string AdditionalClaimInfo { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ApartmentNumber { get; set; }
        public bool IsPractitionerIsRenderingProvider { get; set; }
        public int Form1500PrintFormat { get; set; }

        //public int RuleID { get; set; }
        //public string RuleName { get; set; }

        //public int TotalPages { get; set; }
        public List<ModifierModel> ModifierModel { get; set; }
    }


    public class MasterServiceCodeModel : PayerAppointmentTypeModel
    {
        //public int ServiceCodeID { get; set; }
        //public string CPTCode { get; set; }
        //public string Description { get; set; }
        //public int UnitDuration { get; set; }
        //public bool IsBillable { get; set; }
        //public decimal RatePerUnit { get; set; }
        //public int UnitTypeID { get; set; }
        //public string UnitTypeName { get; set; }
        //public string Type { get; set; }
        //public long? RowNumber { get; set; }
        //public int? TotalRecords { get; set; }
        //public int TotalPages { get; set; }
    }


    public class PayerAppointmentTypeModel : PayerServiceCodeModel
    {
        //public int PayerAppointmentTypeID { get; set; }
        public string AppointmentTypeName { get; set; }
        public int AppointmentTypeID { get; set; }
        public bool IsActive { get; set; }
        // public int PayerID { get; set; }
        //public int PayerServiceCodeID { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        // public long? RowNumber { get; set; }
        // public int? TotalRecords { get; set; }
        // public int TotalPages { get; set; }
    }


    public class PayerServiceCodeModel
    {
        public int PayerServiceCodeID { get; set; }
        public int ServiceCodeID { get; set; }
        public string CPTCode { get; set; }
        public string Description { get; set; }
        public int UnitDuration { get; set; }
        public bool IsBillable { get; set; }
        public decimal RatePerUnit { get; set; }
        public int UnitTypeID { get; set; }
        public string UnitTypeName { get; set; }
        public string Type { get; set; }
        public int PayerID { get; set; }
        public long? RowNumber { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalPages { get; set; }
        public int PayerAppointmentTypeID { get; set; }

        public bool IsRequiredAuthorization { get; set; }

        public decimal? NewRatePerUnit { get; set; }
        public int RuleID { get; set; }
        public string RuleName { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }

    public class PayerInfoUpdateModel
    {
        public List<PayerServiceCode> PayerServiceCodesList { get; set; }
        public List<PayerAppointmentType> PayerAppointmentTypeList { get; set; }
        public List<PayerInformation> PayerInformationList { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsInsert { get; set; }

    }

    public class PayerAppointmentType
    {
        public PayerAppointmentType()
        {
            this.CreatedDate = DateTime.UtcNow;
            this.CreatedBy = 1;
            this.UpdatedDate = null;
            this.DeletedDate = null;
        }
        public int? Id { get; set; }
        public int? AppointmentTypeId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? PayerId { get; set; }
        public int? PayerServiceCodeId { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public decimal? RatePerUnit { get; set; }
    }
    public class PayerInformation
    {
        public PayerInformation()
        {
            this.CreatedDate = DateTime.UtcNow;
            this.CreatedBy = 1;
            this.UpdatedDate = null;
            this.DeletedDate = null;
        }
        public int? Id { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int? DeletedBy { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? OrganizationID { get; set; }
        public string CarrierPayerID { get; set; }
        public bool DayClubByProvider { get; set; }
        public string TPLCode { get; set; }
        public int? InsuranceTypeId { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string City { get; set; }
        public string Fax { get; set; }
        public int? StateID { get; set; }
        public int? CountryID { get; set; }
        public string Zip { get; set; }
    }


    public class PayerServiceCode
    {
        public PayerServiceCode()
        {
            this.CreatedDate = DateTime.UtcNow;
            this.CreatedBy = 1;
            this.UpdatedDate = null;
            this.DeletedDate = null;
            //this.IsDeleted = false;
        }
        public int? Id { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsBillable { get; set; }
        public bool? IsDeleted { get; set; }
        public int? PayerId { get; set; }
        public decimal? RatePerUnit { get; set; }
        public int? UnitDuration { get; set; }
        public int? UnitType { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ServiceCodeId { get; set; }
        public bool IsRequiredAuthorization { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? NewRatePerUnit { get; set; }
        public int RuleID { get; set; }

    }

    public class PayerInfoDropDownModel
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public string PayerPreference { get; set; }
        public int InsuranceCompanyId { get; set; }
        public int PatientInsuranceId { get; set; }
        public bool IsEDIPayer { get; set; }
        public bool IsPractitionerIsRenderingProvider { get; set; }
        public int Form1500PrintFormat { get; set; }
    }
}


