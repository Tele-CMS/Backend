using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.PatientDiseaseManagementProgram;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.PatientDiseaseManagementProgram
{
  public  class PatientDiseaseManagementProgramActivityDiaryRepository : RepositoryBase<DiseaseManagementProgramPatientActivityDiary>, IPatientDiseaseManagementProgramActivityDiaryRepository
    {
        private HCOrganizationContext _context;
        public PatientDiseaseManagementProgramActivityDiaryRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetDMPPatientActivityDiary<T>(int diseaseManagementPlanPatientActivityId, FilterModel filterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@DiseaseManagementPlanPatientActivityId", diseaseManagementPlanPatientActivityId),
                                           new SqlParameter("@PageNumber", filterModel.pageNumber),
                                          new SqlParameter("@PageSize", filterModel.pageSize),
                                          new SqlParameter("@SortOrder", filterModel.sortOrder),
                                          new SqlParameter("@SortColumn", filterModel.sortColumn),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.DMP_GetDiseaseManagmentProgramActivityDiary.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
