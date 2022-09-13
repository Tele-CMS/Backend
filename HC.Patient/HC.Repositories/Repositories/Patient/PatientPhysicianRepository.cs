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
   public class PatientPhysicianRepository : RepositoryBase<PatientPhysician>, IPatientPhysicianRepository
    {
        private HCOrganizationContext _context;
        public PatientPhysicianRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
        public IQueryable<T> GetPatientPhysicianById<T>(int patientId, FilterModel filterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@PatientId", patientId),
                                          new SqlParameter("@PageNumber", filterModel.pageNumber),
                                          new SqlParameter("@PageSize", filterModel.pageSize),
                                          new SqlParameter("@SortOrder", filterModel.sortOrder),
                                          new SqlParameter("@SortColumn", filterModel.sortColumn),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetPatientPhysicianList.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
