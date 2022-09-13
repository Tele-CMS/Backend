using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientLedger
{
   public class PatientLedgerClaimServiceLineModel
    {
        public int Id { get; set; }
        public string ServiceCode { get; set; }
        public decimal Rate { get; set; }
        public int Quantity { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }

        public decimal InsurancePayments { get; set; }
        public decimal InsuranceAdjustments { get; set; }

        public bool? IsPatientResponsible { get; set; }
        public decimal PatientPayments { get; set; }
        public decimal PatientAdjustments { get; set; }
        public bool IsBillable { get; set; }
        public decimal Balance { get; set;}

    }
}
