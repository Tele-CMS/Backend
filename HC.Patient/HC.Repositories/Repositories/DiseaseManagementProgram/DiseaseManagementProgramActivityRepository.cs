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
    public class DiseaseManagementProgramActivityRepository : RepositoryBase<Entity.DiseaseManagementProgram>, IDiseaseManagementProgramActivityRepository
    {
        private HCOrganizationContext _context;
        public DiseaseManagementProgramActivityRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<T> GetDiseaseManagementProgramActivitiesList<T>(int diseaseManagementProgramId, FilterModel filterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@DiseaseManagementProgramId", diseaseManagementProgramId),
                                          new SqlParameter("@OrganizationId", token.OrganizationID),
                                          new SqlParameter("@PageNumber", filterModel.pageNumber),
                                          new SqlParameter("@PageSize", filterModel.pageSize),
                                           new SqlParameter("@SortOrder", string.IsNullOrEmpty(filterModel.sortOrder)?string.Empty:filterModel.sortOrder),
                                          new SqlParameter("@SortColumn", string.IsNullOrEmpty(filterModel.sortColumn)?string.Empty:filterModel.sortColumn)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetDiseaseManagementProgramActivitiesList.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
