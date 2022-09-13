using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientAppointment
{
    public class AppointmentAuthModel
    {
        public int PayerServiceCodeId { get; set; }
        public int ServiceCodeId { get; set; }
        public string ServiceCode { get; set; }
        public string Description { get; set; }
        public string AuthorizationNumber { get; set; }
        public Nullable<int> AuthProcedureCPTLinkId { get; set; }
        public bool IsValid { get; set; }
        public bool IsRequiredAuthorization { get; set; }
        public string AuthorizationMessage { get; set; }
        public bool IsAuthorizationMandatory { get; set; }
        public int UnitToBlock { get; set; }
    }
}
