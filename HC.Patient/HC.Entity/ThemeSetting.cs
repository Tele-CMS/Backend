using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class ThemeSetting : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Theme { get; set; }

        public int ThemeScheme { get; set; }

        public int ThemeLayout { get; set; }
        public int OrganizationID { get; set; }
    }
}
