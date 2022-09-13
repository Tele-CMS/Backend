using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HC.Patient.Entity
{
    public class AuthorizationProcedures : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AuthorizationProcedureId")]        
        public int Id { get; set; }
              
        [ForeignKey("Authorization")]
        public int AuthorizationId { get; set; }       

      
        public int Unit { get; set; }
      
        public int? UnitConsumed { get; set; }
      
        public int? UnitRemain { get; set; }      
        
        [ForeignKey("GlobalCode")]
        public int TypeID { get; set; }
        
        public bool? IsVerified { get; set; }        

        [NotMapped]
        public virtual Authorization Authorization { get; set; }
        public virtual GlobalCode GlobalCode { get; set; }

        public virtual List<AuthProcedureCPT> AuthProcedureCPTLink { get; set; }
      
        [NotMapped]
        public dynamic AuthProcedureCPTLink2
        {
            get
            {
                try
                {
                    return this.AuthProcedureCPTLink.Where(o => IsDeleted == false).Select(p => new
                    {
                        Cptid = p.CPTID,
                        Authorizationproceduresid = p.AuthorizationProceduresId,
                        Id = p.Id,
                        Isdeleted = p.IsDeleted
                    });
                }
                catch(Exception)
                {
                    return null;
                }
            }
            set
            {
                //this.AuthProcedureCPTLink2 = AuthProcedureCPTLink;
            }
        }        
    }
}