using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PayrollBreaktime
{
    public class PayrollBreaktimeDetailsModel
    {
        public int Id { get; set; }
        public decimal StartRange { get; set; }
        public decimal EndRange { get; set; }
        public int NumberOfBreaks { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class PayrollBreaktimeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Duration { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public List<PayrollBreaktimeDetailsModel> PayrollBreaktimeDetails { get; set; }
    }
}
