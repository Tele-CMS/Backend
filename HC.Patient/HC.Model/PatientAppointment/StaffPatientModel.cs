using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientAppointment
{
    public class StaffPatientModel
    {
        public List<StaffModel> Staff { get; set; }
        public List<PatModel> Patients { get; set; }
    }
    public class StaffModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Value { get; set; }
        public int UserId { get; set; }
    }
    public class PatModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Value { get; set; }
        public int UserId { get; set; }

    }

    public class CareManagerModel
    {
        public int Id { get; set; }
        public int staffID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Language { get; set; }
        public string DegreeName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Value { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public string PhotoThumbnailPath { get; set; }
        public string PhotoPath { get; set; }
        public int TotalRow { get; set; }
        public bool IsOnline { get; set; }
    }
}
