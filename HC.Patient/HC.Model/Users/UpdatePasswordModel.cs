using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Users
{
    public class UpdatePasswordModel
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string Token { get; set; }
    }

    public class UserInviteModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
