namespace HC.Patient.Model.Image
{
    public class ImageModel
    {
        public string Base64 { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbImageUrl { get; set; }
    }

    public class TypeImageModel
    {
        public byte[] Bytes { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
