using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.EDI;
using HC.Patient.Repositories.IRepositories.EDI;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.EDI
{
    public class EDI999Repository : RepositoryBase<EDI999AcknowledgementMaster>, IEDI999Repository
    {
        private HCOrganizationContext _context;
        public EDI999Repository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<T> SaveEDI999Acknowledgement<T>(EDI999FileModel edi999FileModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@AcknowledgementType", edi999FileModel.AcknowledgementType),
                                          new SqlParameter("@ControlNumber", edi999FileModel.ControlNumber),
                                          new SqlParameter("@Status", edi999FileModel.Status),
                                          new SqlParameter("@EDIFileText",edi999FileModel.EDIFileText),
                                          new SqlParameter("@UserId", token.UserID),
                                          new SqlParameter("@OrganizationId", token.OrganizationID),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.EDI_SaveEDI999ResponseDetails, parameters.Length, parameters).AsQueryable();
        }
    }
}
