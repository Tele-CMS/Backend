using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.AppointmentTypes
{
    public class AppointmentTypeModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public bool IsBillable { get; set; }
        public bool AllowMultipleStaff { get; set; }
        public bool IsClientRequired { get; set; }
    }
}
