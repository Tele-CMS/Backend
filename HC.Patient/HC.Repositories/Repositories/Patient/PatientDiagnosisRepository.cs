using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientDiagnosisRepository :RepositoryBase<PatientDiagnosis> , IPatientDiagnosisRepository
    {
        private HCOrganizationContext _context;
        public PatientDiagnosisRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<T> GetDiagnosis<T>(PatientFilterModel filterModel, TokenModel tokenModel) where T : class, new()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@PatientId", filterModel.PatientId),
                                              new SqlParameter("@PageNumber", filterModel.pageNumber),
                                              new SqlParameter("@PageSize", filterModel.pageSize)};
                return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetDiagnosisWithPagination.ToString(), parameters.Length, parameters).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
