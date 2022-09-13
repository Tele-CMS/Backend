namespace HC.Patient.Model.Payer
{
    public class PayerActivityModel
    {
        public int AppointmentTypeID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalRecords { get; set; }
    }

    public class PayerActivityCodeModel
    {
        public int Id { get; set; }
        public int? AppointmentTypeID { get; set; }
        public int PayerID { get; set; }
        public int PayerServiceCodeID { get; set; }
        public int? Modifier1 { get; set; }
        public int? Modifier2 { get; set; }
        public int? Modifier3 { get; set; }
        public int? Modifier4 { get; set; }
        public decimal RatePerUnit { get; set; }
    }
}
