using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Claim;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.Claim
{
    public class ClaimDiagnosisCodeRepository : RepositoryBase<ClaimDiagnosisCode>, IClaimDiagnosisCodeRepository
    {
        private HCOrganizationContext _context;
        public ClaimDiagnosisCodeRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }
    }
}
