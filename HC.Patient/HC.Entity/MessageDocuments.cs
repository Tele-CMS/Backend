namespace HC.Patient.Entity
{
    public class MessageDocuments
    {
        public int MessageDocumentID { get; set; }
        public int MessageID { get; set; }
        public string Name { get; set; }                

        public virtual Message Message { get; set; }
    }
}
