using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Tasks
{
    public class PatientCMTasksModel
    {
        public int Id { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedTo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string TaskType { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public bool? OverallTaskStatus { get; set; }
        public bool? PatientTaskStatus { get; set; }
        public string AssociatedCareGap { get; set; }
        public string AssignedPatientName { get; set; }
        public int? AssignedStaffId { get; set; }
        public DateTime AssignedDate { get; set; }
        public string AssociatedPatient { get; set; }
        public string PatientRisk { get; set; }
        public string CarePlan { get; set; }
        public int MasterTaskTypeId { get; set; }
        public int PriorityId { get; set; }
        public int? PatientId { get; set; }
        public int? AssociatedCareGapId { get; set; }
        public int? AssignedPatientId { get; set; }
        public DateTime? PatientTaskUpdatedDate { get; set; }
        public int? AssignedCareManagerId { get; set; }
        public bool? CareManagerTaskStatus { get; set; }
        public int TotalRecords { get; set; }
        public int LinkedEncounterId { get; set; }
        public string IsAutomated { get; set; }

        public bool? OverallTaskTypeStatus { get; set; }
        public DateTime? UpcomingAppointmentDate { get; set; }
    }

    public class ExportTaskModel
    {
        public string AssociatedPatient { get; set; }
        public string UpcomingAppointmentDate { get; set; }
        public string DueDate { get; set; }
        public string TaskType { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string AssignedBy { get; set; }
        public string OverallTaskStatus { get; set; }
        public string PatientTaskStatus { get; set; }
        public string IsAutomated { get; set; }
    }


    //public class TasksModel
    //{
    //    public int Id { get; set; }
    //    public string AssignedBy { get; set; }
    //    public string AssignedPatientName { get; set; }
    //    public int? AssignedStaffId { get; set; }
    //    public DateTime AssignedDate { get; set; }
    //    public string TaskType { get; set; }
    //    public string AssociatedPatient { get; set; }
    //    public string Description { get; set; }
    //    public string Priority { get; set; }
    //    public string PatientRisk { get; set; }
    //    public string CarePlan { get; set; }
    //    public DateTime DueDate { get; set; }
    //    public int MasterTaskTypeId { get; set; }
    //    public int PriorityId { get; set; }
    //    public int? PatientId { get; set; }
    //    public int? AssociatedCareGapId { get; set; }
    //    public int? AssignedPatientId { get; set; }
    //    public bool? PatientTaskStatus { get; set; }
    //    public DateTime? PatientTaskUpdatedDate { get; set; }
    //    public int? AssignedCareManagerId { get; set; }
    //    public bool? CareManagerTaskStatus { get; set; }
    //    public bool OverallTaskStatus { get; set; }
    //    public int TotalRecords { get; set; }
    //}
}
