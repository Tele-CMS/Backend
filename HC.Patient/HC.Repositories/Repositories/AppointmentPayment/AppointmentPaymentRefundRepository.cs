using HC.Patient.Data;
using HC.Patient.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using HC.Patient.Entity;
using HC.Model;
using System.Linq;
using HC.Repositories;
using System.Data.SqlClient;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories
{
    public class AppointmentPaymentRefundRepository :  RepositoryBase<AppointmentPaymentRefund>, IAppointmentPaymentRefundRepository
    {
        private HCOrganizationContext _context;
        public AppointmentPaymentRefundRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
        public IQueryable<T> GetTotalAppointmentRefund<T>(TokenModel token, int staffId=0) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@organizationId", token.OrganizationID), new SqlParameter("@staffId", staffId) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.ADM_GetTotalAppointmentRefund.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
