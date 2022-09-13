using HC.Model;

namespace HC.Patient.Model.Users
{
    public class AuthenticateResultModel : BaseModel
    {
        public string AccessToken { get; set; }
        public string EncryptedAccessToken { get; set; }
        public int ExpireInSeconds { get; set; }
        public long UserId { get; set; }
    }
}
