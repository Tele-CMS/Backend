using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class CareChat : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]

        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsSeen { get; set; }

        [ForeignKey("FromUser")]
        public int FromUserId { get; set; }

        [ForeignKey("ToUser")]
        public int? ToUserId { get; set; }

        public DateTime ChatDate { get; set; }
        public int OrganizationID { get; set; }
        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
        public virtual Organization Organization { get; set; }
        public int? RoomId { get; set; }
        public int? MessageType { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}