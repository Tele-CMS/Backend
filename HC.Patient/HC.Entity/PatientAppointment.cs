using HC.Common.Filters;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HC.Patient.Entity
{
    public class PatientAppointment : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PatientAppointmentId")]
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public int? PatientID { get; set; }

        [RequiredDate]
        public DateTime StartDateTime { get; set; }

        [RequiredDate]
        public DateTime EndDateTime { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        [ForeignKey("AppointmentType")]
        public int? AppointmentTypeID { get; set; }

        [NotMapped]
        public string AppointmentTypeName
        {
            get
            {
                try
                {
                    return AppointmentType == null ? "" : AppointmentType.Name;
                }
                catch (Exception)
                {

                    return "";
                }
            }
        }

        [NotMapped]
        public string LocationOfficeStartHour { get; set; }
        [NotMapped]
        public string LocationOfficeEndHour { get; set; }

        [ForeignKey("PatientAddress")]
        public int? PatientAddressID { get; set; }

        [ForeignKey("Location1")]
        public int? OfficeAddressID { get; set; }

        [ForeignKey("Location")]
        public int? ServiceLocationID { get; set; }

        public string RecurrenceRule { get; set; }

        public bool IsRecurrence { get; set; }
        public bool? IsNotify { get; set; }
        public bool? IsTelehealthAppointment { get; set; }
        public bool? IsUrgentCareAppointment { get; set; }

        [NotMapped]
        public List<Occurrences> Occurrences { get; set; }

        public int? ParentAppointmentID { get; set; }

        public bool IsClientRequired { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string CustomAddress { get; set; }

        [ForeignKey("MasterPatientLocation")]
        public int? CustomAddressID { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [StringLength(20)]
        public string ApartmentNumber { get; set; }

        [NotMapped]
        public int? PatientEncounterID
        {
            get
            {
                try
                {

                    if (PatientEncounter != null && PatientEncounter.Count > 0)
                    {
                        return PatientEncounter.FirstOrDefault().Id;
                    }
                    else
                    {
                        return 0;

                    }

                }
                catch (Exception)
                {

                    return null;
                }
            }
        }

        [NotMapped]
        public int[] StaffIDs
        {
            get
            {
                try
                {
                    return AppointmentStaff.Select(k => k.StaffID).ToArray<int>();
                }
                catch (Exception)
                {

                    return new List<int>().ToArray();
                }
            }
        }

        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        [ForeignKey("CancelType")]
        public int? CancelTypeId { get; set; }

        [ForeignKey("AppointmentStatus")]
        public int? StatusId { get; set; }

        [StringLength(500)]
        public string CancelReason { get; set; }

        public bool IsExcludedFromMileage { get; set; }

        public bool IsDirectService { get; set; }

        public decimal? Mileage { get; set; }

        [ForeignKey("PatientInsuranceDetails")]
        public int? PatientInsuranceId { get; set; }

        [ForeignKey("Authorization")]
        public int? AuthorizationId { get; set; }

        public TimeSpan? DriveTime { get; set; }
        public int? Offset { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual GlobalCode CancelType { get; set; }
        public virtual GlobalCode AppointmentStatus { get; set; }
        public virtual List<PatientEncounter> PatientEncounter { get; set; }
        public virtual List<AppointmentStaff> AppointmentStaff { get; set; }
        [NotMapped]
        public virtual RecurrencePattern RecurrencePattern { get; set; }
        //Foreign key's tables
        public virtual Patients Patient { get; set; }
        public virtual AppointmentType AppointmentType { get; set; }
        public virtual MasterPatientLocation MasterPatientLocation { get; set; }
        public virtual PatientAddress PatientAddress { get; set; }
        public virtual Location Location { get; set; }
        public virtual Location Location1 { get; set; }
        public virtual PatientInsuranceDetails PatientInsuranceDetails { get; set; }
        public virtual Authorization Authorization { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string BookingMode { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string BookingType { get; set; }
        public int? InvitationAppointentId { get; set; }
        public string InvitationAcceptRejectRemarks { get; set; }
    }

    public class Occurrences
    {
        public Period Occurrence { get; set; }
        public int? AppointmentID { get; set; }
        public int? ParentAppointmentID { get; set; }
        public int? PatientEncounterID { get; set; }
    }

}
