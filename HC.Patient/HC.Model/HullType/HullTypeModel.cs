using HC.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace HC.Patient.Model.HullType
{
    public  class HullTypeModel : BaseModel
    {
        [Key]
        public System.Guid HullTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }

        public System.DateTime CreatedDateTime { get; set; }

        public Nullable<System.DateTime> ModifiedDateTime { get; set; }

        public Nullable<bool> IsDeleted { get; set; }

        public Nullable<System.Guid> DeletedBy { get; set; }

        public Nullable<System.DateTime> DeletedDateTime { get; set; }



    }
}
