using HC.Common.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HC.Patient.Entity
{
    public class Authorization : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AuthorizationId")]        
        public int Id { get; set; }

        [Required]
        [RequiredNumber]        
        [ForeignKey("Patient")]
        public int PatientID { get; set; }

        
        [MaxLength(100)]
        public string AuthorizationTitle { get; set; }

        
        [MaxLength(200)]
        public string AuthorizationNumber { get; set; }

        
        public DateTime StartDate { get; set; }

        
        public DateTime EndDate { get; set; }

        
        public string Notes { get; set; }

        
        [ForeignKey("PatientInsuranceDetails")]
        public int PatientInsuranceId { get; set; }        
        public bool? IsVerified { get; set; }

        
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        
        [Obsolete]
        public virtual Organization Organization { get; set; }

        
        public virtual List<AuthorizationProcedures> AuthorizationProcedures { get; set; }

        
        [NotMapped]
        public dynamic AuthorizationProcedureList
        {
            get
            {
                try
                {
                    return this.AuthorizationProcedures.Where(o => o.IsDeleted == false).Select(p => new
                    {
                        authorizationid = p.AuthorizationId,
                        id = p.Id,
                        isactive = p.IsActive,
                        isdeleted = p.IsDeleted,
                        isverified = p.IsVerified,
                        typeid = p.TypeID,
                        unit = p.Unit,
                        unitconsumed = p.UnitConsumed,
                        unitremain = p.UnitRemain,
                        authprocedurecpt = p.AuthProcedureCPTLink.Where(j => j.IsDeleted == false).Select(i => new
                        {
                            Cptid = i.CPTID,
                            Authorizationproceduresid = i.AuthorizationProceduresId,
                            Id = i.Id,
                            Isdeleted = i.IsDeleted
                        })
                    });
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                //this.AuthProcedureCPTLink2 = AuthProcedureCPTLink;
            }
        }


        
        [Obsolete]
        public virtual Patients Patient { get; set; }
        
        [Obsolete]
        public virtual PatientInsuranceDetails PatientInsuranceDetails { get; set; }
    }
}