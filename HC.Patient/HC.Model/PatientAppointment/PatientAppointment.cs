using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Model.PatientAppointment
{

    public class PatientAppointments: PatientAppointmentsModel
    {
        [NotMapped]
        public int[] StaffIDs { get; set; }
    }
    public class PatientAppointmentsModel
    {
        public int? PatientAppointmentId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Notes { get; set; }
        public int? StaffID { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? PatientID { get; set; }
        public DateTime? StartDateTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? AppointmentTypeID { get; set; }
        public string RecurrenceRule { get; set; }
        public int? PatientLocationID { get; set; }
        public int? ServiceAddressID { get; set; }
        public int? ServiceID { get; set; }
        public int? ServiceLocationID { get; set; }
        public int? ParentAppointmentID { get; set; }
        public bool? IsClientRequired { get; set; }
        public bool IsTelehealthAppointment { get; set; }
        //public int CancelTypeId { get; set; }
    }


    public class PatientAppointmentFilter
    {
        public List<PatientAppointments> PatientAppointment { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsInsert { get; set; }
    }

    public class OfficeHours
    {
        public int Id { get; set; }     
        public string OfficeStartHour { get; set; }        
        public string OfficeEndHour { get; set; }
    }
}
