using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class CustomLabelModel
    {   
        public int Id { get; set; }
        public int? PatientID { get; set; }
        public int CustomLabelID { get; set; }
        public string CustomLabelValue { get; set; }
        public string CustomLabelDataType { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class StaffCustomLabelModel
    {
        public int Id { get; set; }
        public int? StaffID { get; set; }
        public int CustomLabelID { get; set; }
        public string CustomLabelValue { get; set; }
        public string CustomLabelName { get; set; }
        public string CustomLabelDataType { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class MasterCustomLabelModel
    {
        public int Id { get; set; }
        public string CustomLabelName { get; set; }
        public int CustomLabelTypeId { get; set; }
        public bool IsDeleted { get; set; }        
        public int RoleTypeId { get; set; }
        public string RoleTypeName { get; set; }
        public Decimal TotalRecords { get; set; }
    }

    public class PatientsCustomModel
    {
        public List<MasterCustomLabelModel> MasterCustomLabels { get; set; }
        public List<CustomLabelModel> PatientCustomLabels { get; set; }
    }
}
