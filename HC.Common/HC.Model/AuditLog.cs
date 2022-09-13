namespace HC.Model
{
    public class AuditLog
    {
        //public int PrimaryKeyID;

        public string NewValue { get; set; }
        public string OldValue { get; set; }
        public string PropertyName { get; set; }
        public string TableName { get; set; }
        public int? ColumnID { get; set; }
    }
}
