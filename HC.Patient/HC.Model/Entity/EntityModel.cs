using HC.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace HC.Patient.Model.Entity
{
    public class EntityModel: BaseModel
    {
        [Key]
        public Guid EntityId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageType { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public string BankDetails { get; set; }
    }
}
