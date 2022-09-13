using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model
{
    public class GroupSessionInvitationModel
    {
        public int StaffId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int AppointmentId { get; set; }
        public int SessionId { get; set; }
        public string WebRootUrl { get; set; }
    }
}
