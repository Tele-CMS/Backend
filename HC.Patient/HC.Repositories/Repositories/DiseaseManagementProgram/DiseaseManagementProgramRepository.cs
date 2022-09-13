using HC.Model;
using HC.Patient.Data;
using HC.Patient.Repositories.IRepositories.DiseaseManagementProgram;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.DiseaseManagementProgram
{
    public class DiseaseManagementProgramRepository : RepositoryBase<Entity.DiseaseManagementProgram>, IDiseaseManagementProgramRepository
    {
        private HCOrganizationContext _context;
        public DiseaseManagementProgramRepository(HCOrganizationContext context) :base(context)
        {
            _context = context;
        }

        public IQueryable<T> GetDiseaseManagementProgramList<T>(FilterModel filterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { 
                                          new SqlParameter("@OrganizationId", token.OrganizationID),
                                          new SqlParameter("@PageNumber", filterModel.pageNumber),
                                          new SqlParameter("@PageSize", filterModel.pageSize),
                                          new SqlParameter("@SortOrder", string.IsNullOrEmpty(filterModel.sortOrder)?string.Empty:filterModel.sortOrder),
                                          new SqlParameter("@SortColumn", string.IsNullOrEmpty(filterModel.sortColumn)?string.Empty:filterModel.sortColumn)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetDiseaseManagementProgramList.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetDiseaseProgramsListWithEnrollments<T>(TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@OrganizationId", token.OrganizationID),
                                          new SqlParameter("@UserId", token.UserID)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetDiseaseProgramsListWithEnrollments.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetDiseaseConditionsFromProgramIds<T>(string ProgramIds, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@ProgramIds", ProgramIds),
                                          new SqlParameter("@OrganizationId", token.OrganizationID),
                                          new SqlParameter("@UserId", token.UserID)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetDiseaseConditionsFromProgramIds.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
