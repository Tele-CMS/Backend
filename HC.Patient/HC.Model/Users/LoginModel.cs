using HC.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace HC.Patient.Model.Users
{
    public class LoginModel : BaseModel
    {
        private const string PASSWORD_PATTERN = "(^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$)";
        public Guid UserId { get; set; }
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public int UserRole { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(PASSWORD_PATTERN, ErrorMessage = "Passwords must contain at least ten characters, including number, uppercase, lowercase and special character letters.")]
        public string Password { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string ResetPassword { get; set; }
        public bool RememberMe { get; set; }
        public string DeviceType { get; set; }
        public string DeviceToken { get; set; }
        public string CurrentPassword { get; set; }
        public class ResetModel
        {
            public string emailId { get; set; }
            public string tokenId { get; set; }
        }
        public class ForgotPassword
        {
            public string EmailId { get; set; }
            public int StatusCode { get; set; }
            public string StatusMessage { get; set; }
        }
        public class ForgetData
        {
            public string EmailId { get; set; }
        }
        public class WardenLoginModel
        {
            public string WardenTeamId { get; set; }
        }
        public class UserLoginModel
        {
            public string UserEmail { get; set; }
            public string UserPassword { get; set; }
        }
    }
}
