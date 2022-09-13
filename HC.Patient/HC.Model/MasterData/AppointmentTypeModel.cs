using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.MasterData
{
    public class AppointmentTypesModel
    {
        public int Id { get; set; }        
        public string Name { get; set; }                
        public string Description { get; set; }        
        public bool IsBillable { get; set; }
        public string DefaultDuration { get; set; }
        public bool? AllowMultipleStaff { get; set; }
        public string Color { get; set; }
        public string FontColor { get; set; }
        public bool? IsClientRequired { get; set; }
        //
        public decimal TotalRecords { get; set; }
    }
}
