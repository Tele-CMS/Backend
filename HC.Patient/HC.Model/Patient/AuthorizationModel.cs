using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class AuthorizationModel
    {
        public int AuthorizationId { get; set; }
        public string AuthorizationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PayerName { get; set; }
        public string PayerPreference { get; set; }
        public int TotalCount { get; set; }
        public int PatientInsuranceId { get; set; }
        public string AuthorizationTitle { get; set; }
        public string Notes { get; set; }
        public int PayerId { get; set; }
        public bool IsExpired { get; set; }
    }
    public class AuthorizationProceduresModel
    {
        public int AuthorizationId { get; set; }
        public int TotalUnit { get; set; }
        public string GlobalCodeName { get; set; }
        public int AuthorizationProcedureId { get; set; }
        public int ActualAuthProcedureUnitConsumed { get; set; }
        public int TypeId { get; set; }
    }
    public class AuthorizationProceduresCPTModel
    {
        public int AuthorizationProcedureId { get; set; }
        public string ServiceCode { get; set; }
        public int ServiceCodeId { get; set; }
        public int UnitConsumed { get; set; }
        public int BlockedUnit { get; set; }
        public int AuthProcedureCPTLinkId { get; set; }
        public int ActualAuthProcedureAllowedUnit { get; set; }
        public int ActualAuthProcedureUnitConsumed { get; set; }
        public bool IsValid { get; set; }
        public bool IsRequiredAuthorization { get; set; }
        public string AttachedModifiers { get; set; }
    }
    public class AuthorizationValidityModel
    {
        public bool Status { get; set; }
        public StringBuilder AuthorizationMessages { get; set; }
    }

    #region Save Authorization in single request
    public class AuthModel //for save Authorization
    {
        public int Id { get; set; }
        public int PatientID { get; set; }
        public string AuthorizationTitle { get; set; }
        public string AuthorizationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
        public int PatientInsuranceId { get; set; }
        public bool? IsVerified { get; set; }
        public int OrganizationID { get; set; }
        public List<AuthProceduresModel> AuthorizationProcedures { get; set; }
    }
    public class AuthProceduresModel
    {
        public int Id { get; set; }
        public int AuthorizationId { get; set; }
        public int Unit { get; set; }
        public int? UnitConsumed { get; set; }
        public int? UnitRemain { get; set; }
        public int TypeID { get; set; }
        public bool? IsVerified { get; set; }
        public bool IsDeleted { get; set; }
        public List<AuthProcedureCPTModel> AuthProcedureCPT { get; set; }
    }
    public class AuthProcedureCPTModel
    {
        public int Id { get; set; }
        public int AuthorizationProceduresId { get; set; }
        public int CPTID { get; set; }
        public int? UnitConsumed { get; set; }
        public int? BlockedUnit { get; set; }
        public bool IsDeleted { get; set; }

        public List<AuthProcedureCPTModifierModel> AuthProcedureCPTModifiers { get; set; }
    }

    public class AuthProcedureCPTModifierModel
    {
        public int Id { get; set; }
        public int AuthProcedureCPTLinkId { get; set; }
        public string Modifier { get; set; }
        public bool IsDeleted { get; set; }
    }
    #endregion

    public class GetAuthorizationByIdModel
    {
        public AuthModel Authorization { get; set; }
        public List<AuthProceduresModel>  AuthorizationProcedures { get; set; }
        public List<AuthProcedureCPTModel> AuthorizationProcedureCPT { get; set; }
        public List<AuthProcedureCPTModifierModel> AuthorizationProcedureCPTModifiers { get; set; }
    }
}