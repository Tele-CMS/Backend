using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.SpringBPatient
{
    public class SpringB_PatientLabModel
    {
        public DateTime LastDatePerformed { get; set; }
        public string A1c { get; set; }
        public string Body_Fat { get; set; }
        public string Glucose_Sugar { get; set; }
        public string HDL { get; set; }
        public string LDL { get; set; }
        public string Smoking { get; set; }
        public string Total_Cholesterol { get; set; }
        public string Total_HDL { get; set; }
        public string Triglycerides { get; set; }
        public string Waist_Hip_Ratio { get; set; }
        public int TotalRecords { get; set; }
    }
}
