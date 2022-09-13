using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Payer;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Payer
{
    public class SymptomatePatientReportRepository : RepositoryBase<SymptomatePatientReport>, ISymptomatePatientReportRepository
    {
        private HCOrganizationContext _context;
        public SymptomatePatientReportRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetSymptomateReportListing<T>(PatientSymptomateFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientFilterModel.PatientId),
                                        new SqlParameter("@PageNumber",patientFilterModel.pageNumber),
                                        new SqlParameter("@PageSize", patientFilterModel.pageSize),
                                        new SqlParameter("@SortColumn", patientFilterModel.sortColumn),
                                        new SqlParameter("@SortOrder ", patientFilterModel.sortOrder)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetSYmptomateReportListing.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
