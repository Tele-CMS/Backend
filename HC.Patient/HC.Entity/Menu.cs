using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Menu
    {
        [Key]
        [Column("MenuID")]
        public int MenuID { get; set; }
        
        [Column(TypeName = "varchar(200)")]
        public string MenuName { get; set; }
        
        [Column(TypeName = "varchar(700)")]
        public string MenuIcon { get; set; }

        [Column("DisplayOrder")]
        public int DisplayOrder { get; set; }

        [Column("OrganizationID")]
        public int OrganizationID { get; set; }
    }
}
