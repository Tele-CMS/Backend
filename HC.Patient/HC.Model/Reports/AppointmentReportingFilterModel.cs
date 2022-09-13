using HC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Reports
{
    public class AppointmentReportingFilterModel: FilterModel
    {
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime? CurrentDate { get; set; }
        public int? CancellationType { get; set; }
        public string MemberName { get; set; }
        public int? Status { get; set; }
        public bool? NextAppointmentPresent { get; set; }
        public string CareManagerIds { get; set; }
        public string ProgramTypes { get; set; }
        public string EnrolledPrograms { get; set; }
        public string XmlProgramTypes { get; set; }
        public string XmlEnrolledPrograms { get; set; }
        public bool? IsEligible { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

}
