using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class UserDocuments : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("UserDocumentId")]
      
        public  int Id { get; set; }
        [Required]
        [RequiredNumber]
      
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        [ForeignKey("MasterDocumentTypes")]
        public int? DocumentTypeId { get; set; }
        [NotMapped]
        public string DocumentTypeName { get { return this.MasterDocumentTypes.Type; } set { this.MasterDocumentTypes.Type = value; } }

      
        [ForeignKey("MasterDocumentTypesStaff")]
        public int? DocumentTypeIdStaff { get; set; }

        [ForeignKey("PatientAppointment")]
        public int? PatientAppointmentId { get; set; }

    
        public bool? IsProviderEducationalDocument { get; set; }

        [NotMapped]
        public string DocumentTypeNameStaff { get { return this.MasterDocumentTypesStaff.Type; } set { this.MasterDocumentTypesStaff.Type = value; } }

        [StringLength(100)]
        public String DocumentName { get; set; }
        [StringLength(50)]
        public String DocumentNumber { get; set; }
        public DateTime? Expiration { get; set; }
        public String OtherDocumentType { get; set; }
        public string UploadPath { get; set; }
        public string Key { get; set; }

        [Obsolete]
        public virtual User User { get; set; }
        public MasterDocumentTypes MasterDocumentTypes { get; set; }

        public MasterDocumentTypesStaff MasterDocumentTypesStaff { get; set; }
        public PatientAppointment PatientAppointment { get; set; }
    }
}