using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model
{
    public class GlobalCodeModel
    {
        public int Id { get; set; }
        public string GlobalCodeName { get; set; }
        public string GlobalCodeValue { get; set; }
        public int GlobalCodeCategoryID { get; set; }
        public string value { get; set; }
        public int OrganizationID { get; set; }
        public int? DisplayOrder { get;set;}
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public int TotalRecords { get; set; }
        public string SpecialityIcon { get; set; }
        public string PhotoPath { get; set; }
        public string PhotoThumbnailPath { get; set; }





    }
}
