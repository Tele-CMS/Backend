using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Payer
{
    public class ServiceCodeModifiersSearchModel
    {
        public int PayerModifierId { get; set; }
        public int PayerServiceCodeId { get; set; }        
        public string Modifier { get; set; }
        public decimal Rate { get; set; }
    }
}
