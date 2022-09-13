using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class UserFavourites
    {
        public UserFavourites()
        {
            this.CreatedDate = DateTime.UtcNow;
            this.CreatedBy = 1;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("FavouritesID")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }

        [Required]
        [ForeignKey("Users")]
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        [ForeignKey("Users1")]
        public int CreatedBy { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        [ForeignKey("Users2")]
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        [Required]
        public int OrganizationID { get; set; }

        public virtual Patients Patient { get; set; }

        public virtual User Users { get; set; }

        public virtual User Users1 { get; set; }

        public virtual User Users2 { get; set; }
    }
}

