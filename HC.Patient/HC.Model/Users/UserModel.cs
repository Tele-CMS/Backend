using HC.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace HC.Patient.Model.Users
{
    public class UserModel : BaseModel
    {
        private const string PASSWORD_PATTERN = "(^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$)";
        public string ClientId { get; set; }
        [Required]
        public string UserName { get; set; }
       
        public string[] UserRole { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string emailAddress { get; set; }
        
        public string Password { get; set; }
        public System.Guid UserId { get; set; }
        public string Prefix { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
       // [Required]
        public string TelMainOne { get; set; }
        public string TelMainTwo { get; set; }
        public string TelMobile { get; set; }
        public string Telex { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> LastLoginDateTime { get; set; }        
        public System.DateTime CreatedDateTime { get; set; }        
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? OfficeId { get; set; }
        public Nullable<System.Guid> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDateTime { get; set; }
        public string ImageType { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public Guid? EntityId { get; set; }
    }

    public class UserDocumentsModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Dictionary<string, string> Base64 { get; set; }
        public List<IFormFile> UploadFile { get; set; }
        public string DocumentTitle { get; set; }
        public string Url { get; set; }
        public int? DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public int? DocumentTypeIdStaff { get; set; }
        public string DocumentTypeNameStaff { get; set; }
        public DateTime? Expiration { get; set; }
        public string Extenstion { get; set; }        
        public string OtherDocumentType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Key { get; set; }
        public int? PatientAppointmentId { get; set; }
        public bool IsActive { get; set; }
        public bool IsProviderEducationalDocument { get; set; }
    }
    public class UserDocumentsResponseModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Base64 { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentTypeNameStaff { get; set; }
        public DateTime? Expiration { get; set; }
        public String OtherDocumentType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public MemoryStream File { get; set; }
        public string Extenstion { get; set; }
        public string FileName { get; set; }
    }
}
