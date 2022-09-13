using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using System;
using System.Data.SqlClient;
using System.Linq;
using static HC.Common.Enums.CommonEnum;
using Microsoft.EntityFrameworkCore;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientGuardianRepository : RepositoryBase<PatientGuardian>, IPatientGuardianRepository
    {
        private HCOrganizationContext _context;
        public PatientGuardianRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetPatientGuardian<T>(PatientGuartdianFilterModel patientGuartdianFilterModel, TokenModel tokenModel) where T : class, new()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@PatientId",patientGuartdianFilterModel.PatientId),
                                              new SqlParameter("@SearchKey", patientGuartdianFilterModel.SearchKey),
                                              new SqlParameter("@OrganizationID",tokenModel.OrganizationID),
                                              new SqlParameter("@SortColumn",patientGuartdianFilterModel.sortColumn),
                                              new SqlParameter("@SortOrder",patientGuartdianFilterModel.sortOrder),
                                              new SqlParameter("@PageNumber",patientGuartdianFilterModel.pageNumber),
                                              new SqlParameter("@PageSize",patientGuartdianFilterModel.pageSize)
                };
                return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetPatientGuardian.ToString(), parameters.Length, parameters).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PatientGuardian GetPatientGuardianById(int id,TokenModel tokenModel)
        {
            PatientGuardian patientGuardian = _context.PatientGuardian.Where(a => a.Id == id && a.IsDeleted == false && a.IsActive == true)
                //.Include(h => h.MasterRelationship)
                .FirstOrDefault();
            return patientGuardian;
        }
    }
}
