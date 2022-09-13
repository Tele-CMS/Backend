using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.DiseaseManagementProgram
{
   public class DiseasemanagementProgramActivityListModel
    {
        public int ID { get; set; }
        public string Descriptions { get; set; }
        public string ActivityType { get; set; }
        public int TotalRecords { get; set; }
    }
}
