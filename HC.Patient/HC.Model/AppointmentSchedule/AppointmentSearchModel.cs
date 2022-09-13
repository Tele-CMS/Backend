using HC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model
{
    public class AppointmentSearchModel : FilterModel
    {
        public string Date { get; set; }
        //As there might be multiple locations option to select so taken as string
        public string Locations { get; set; }
        //As there might be multiple speciality option to select so taken as string
        public string Taxonomies { get; set; }
        //As there might be multiple gender option to select so taken as string
        public string Gender { get; set; } = "0";
        public string Specialities { get; set; }
        public string Services { get; set; }
        public string Rates { get; set; }
        public string MinRate { get; set; }
        public string ReviewRating { get; set; }
        public string SortType { get; set; }
        public string ProvidersearchText { get; set; }
        public string ProviderId { get; set; }
    }
    public class StaffAvailabilityModel
    {
        public string StaffId { get; set; }
        public string LocationId { get; set; }
        public string AppointmentDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class AppointmentSearchModelForMobile : FilterModel
    {
        public string Date { get; set; }
        //As there might be multiple locations option to select so taken as string
        public string Locations { get; set; }
        //As there might be multiple speciality option to select so taken as string
        public string Taxonomies { get; set; }
        //As there might be multiple gender option to select so taken as string
        public string Gender { get; set; } = "0";
        public string Specialities { get; set; }
        public string Services { get; set; }
        public string Rating { get; set; }
        public string keywords { get; set; }
    }
}
