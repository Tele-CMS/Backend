using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.ReviewRatings
{
    public class ReviewRatingsModel
    {
        public int Id { get; set; }
        public int PatientAppointmentId { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public int StaffId { get; set; }

    }
}
